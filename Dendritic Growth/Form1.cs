using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
namespace Dendritic_Growth
{
    public partial class Form1 : Form
    {
        public double Ta, TD, C0, beta, gamma, Me, kke, D, a, a0, QL, Khi, VD, VDl, r, tD, hD, Q, epsilone, Angle0, Angle1, GibsTompsonAlloy, GibsTompsonInocula, Mn, Si, Ni, Mo, Ti, Al, InoculaVOL, gammaInocula, Wa;  // Параметры сплава.
        public double V0, noise, t, n0, TConst, deltaTFull, h, V0VD, KShod; // Параметры модели. 
        public int  X, Y, SolidPixel, Seeds, SeedRadius, DFj; // Размеры расчетной области и количество и параметры начальной твердой фазы.
        public bool StartPressed = false, epsiloneOK = true;        
        public int CFmaxJ;
        int i, j;
        int qq;        
        public struct Cell // Элементарная ячейка расчетной области. 
        {   public double G, C, K, Jx, Jy, V, b, m, kk, Cs, Gn, kkn, Cn, Jxn, Jyn, dT, GibsTompson; // Переменные расчитываемые на каждом шаге.
            public byte CF, DF, WasDF;        }
        
        double[,] Mw = new double[7, 7] {{ 0.0, 0.351, 0.841, 0.988, 0.841, 0.351, 0.0  },
                                        {0.351, 0.999,   1.0,   1.0,   1.0, 0.999, 0.351},
                                        {0.841,   1.0,   1.0,   1.0,   1.0,   1.0, 0.841},
                                        {0.988,   1.0,   1.0,   1.0,   1.0,   1.0, 0.988},
                                        {0.841,   1.0,   1.0,   1.0,   1.0,   1.0, 0.841},
                                        {0.351, 0.999,   1.0,   1.0,   1.0, 0.999, 0.351},
                                        {  0.0, 0.351, 0.841, 0.988, 0.841, 0.351, 0.0  }};
        double S = 38.484;
        double A = 15.754;
        double B = 6.976;

        private Form2 form2 = new Form2();
         
        public Cell[,] CellsArea;
        public int[] CFIndex;

        public Form1()
        {
            InitializeComponent();
        }

        public void GandC(Cell[,] UserCells) // Расчет Прироста твердой фазы, концентрации, потоков и тд.
        {

            //for (i = 3; i <= CellsArea.GetLength(0) - 4; i++) 
            //{
            //for (j = 3; j <= CellsArea.GetLength(1) - 4; j++)
            //{


            Parallel.For(3, CellsArea.GetLength(0) - 3, i =>
            {
                Parallel.For(3, CellsArea.GetLength(1) - 3, j =>
                 {

                     UserCells[i, j].DF = 0;

                });
            });

            Random random2 = new Random(); // Экземпляр класс Random, для генерирования псевдослучайныйх чисел.

            Parallel.For(3, CellsArea.GetLength(0) - 3, i =>
            {
                Parallel.For(3, CFmaxJ + 10, j =>
                {
                    UserCells[i, j].DF = 0;

                    if (UserCells[i, j].CF == 1)
                    {
                        double rand2 = random2.Next(-100, 100);
                        rand2 = rand2 / 100;

                        UserCells[i, j].b = 1 + noise * rand2; // Учет случайных флуктуаций на границе раздела фаз. (безразмерная)

                        UserCells[i, j].K = CurvatureK(UserCells, i, j); // Расчет сумы долей твердой фазы в круге диаметром 9h, для оценки кривизны поверхности.

                        Opti(UserCells, deltaTFull, i, j); // Численный поиск V, kk, m. Newton Method. + GibbsTompson*K

                        UserCells[i, j].G = UserCells[i, j].Gn + (t / h) * UserCells[i, j].V; // Рсчет прироста твердой фазы в ячейке.                            

                        if (UserCells[i, j].G >= 1)
                        {
                            UserCells[i, j].G = 1;
                            UserCells[i, j].CF = 0;
                            UserCells[i, j].C = UserCells[i, j].Cs;

                            UserCells[i, j].Jx = 0;
                            UserCells[i, j].Jy = 0;

                        }
                    }
                });
            });

            DiffusionFrontIni(CellsArea);

            Parallel.For(3, CellsArea.GetLength(0) - 3, i =>
            {
                Parallel.For(3, CellsArea.GetLength(1) - 3, j =>
                {
                    if (UserCells[i, j].G != 1 && UserCells[i, j].WasDF == 1)
                    {
                        UserCells[i, j].C = UserCells[i, j].Cn
                            + (1 / (1 - (1 - UserCells[i, j].kkn) * UserCells[i, j].Gn)) *
                            (((1 - UserCells[i, j].kkn) * (UserCells[i, j].G - UserCells[i, j].Gn) * UserCells[i, j].Cn - (UserCells[i, j].kk - UserCells[i, j].kkn) * UserCells[i, j].Gn * UserCells[i, j].Cn)
                                 - ((t / h) * (UserCells[i, j].Jxn - UserCells[i - 1, j].Jxn + UserCells[i, j].Jyn - UserCells[i, j - 1].Jyn)));

                        UserCells[i, j].Cs = UserCells[i, j].kk * UserCells[i, j].C;
                    }
                });
            });


            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //for (int i = 3; i <= CellsArea.GetLength(0) - 4; i++) // Расчет концентраций и потоков.
            //  {
            // for (int j = 3; j <= CellsArea.GetLength(1) - 4; j++)
            //     {

            Parallel.For(3, CellsArea.GetLength(0) - 3, i =>
            {
                Parallel.For(3, CellsArea.GetLength(1) - 3, j =>
                {

                    if (UserCells[i, j].G != 1 && UserCells[i, j].WasDF == 1)
                    {
                        // Контакт диффузионного фронта с расплавом

                        if (UserCells[i, j].DF == 1)
                        {
                            // По Х

                            if (UserCells[i + 1, j].G == 1)
                            {
                                UserCells[i, j].Jx = 0;
                            }

                            if (UserCells[i + 1, j].WasDF == 1 && UserCells[i + 1, j].G != 1)
                            {
                                UserCells[i, j].Jx = (1 - t) * UserCells[i, j].Jxn - 1 * (t / Q * h) * (1 - Math.Max(UserCells[i + 1, j].Gn, UserCells[i, j].Gn)) * (UserCells[i + 1, j].Cn - UserCells[i, j].Cn);
                            }

                            if (UserCells[i + 1, j].WasDF == 0 && UserCells[i + 1, j].G != 1)
                            {
                                UserCells[i, j].Jx = -1 * Math.Exp(-1 * Math.Sqrt((KShod * t) / 4)) * (UserCells[i + 1, j].Cn - UserCells[i, j].Cn);
                            }

                            // По Y

                            if (UserCells[i, j + 1].G == 1)
                            {
                                UserCells[i, j].Jy = 0;
                            }

                            if (UserCells[i, j + 1].WasDF == 1 && UserCells[i, j + 1].G != 1)
                            {
                                UserCells[i, j].Jy = (1 - t) * UserCells[i, j].Jyn - 1 * (t / Q * h) * (1 - Math.Max(UserCells[i, j + 1].Gn, UserCells[i, j].Gn)) * (UserCells[i, j + 1].Cn - UserCells[i, j].Cn);
                            }

                            if (UserCells[i, j + 1].WasDF == 0 && UserCells[i, j + 1].G != 1)
                            {
                                UserCells[i, j].Jy = -1 * Math.Exp(-1 * Math.Sqrt((KShod * t) / 4)) * (UserCells[i, j + 1].Cn - UserCells[i, j].Cn);
                            }
                        }
                        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                        // Контакт Диффузионного фронта с диффузионным фронтом

                        if (UserCells[i, j].DF == 0)
                        {
                            // По Х

                            if (UserCells[i + 1, j].G == 1)
                            {
                                UserCells[i, j].Jx = 0;
                            }

                            if (UserCells[i + 1, j].G != 1)
                            {
                                UserCells[i, j].Jx = (1 - t) * UserCells[i, j].Jxn - 1 * (t / Q * h) * (1 - Math.Max(UserCells[i + 1, j].Gn, UserCells[i, j].Gn)) * (UserCells[i + 1, j].Cn - UserCells[i, j].Cn);
                            }

                            // По Y

                            if (UserCells[i, j + 1].G == 1)
                            {
                                UserCells[i, j].Jy = 0;
                            }

                            if (UserCells[i, j + 1].G != 1)
                            {
                                UserCells[i, j].Jy = (1 - t) * UserCells[i, j].Jyn - 1 * (t / Q * h) * (1 - Math.Max(UserCells[i, j + 1].Gn, UserCells[i, j].Gn)) * (UserCells[i, j + 1].Cn - UserCells[i, j].Cn);
                            }
                        }


                        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }

                    if (UserCells[i, j].G == 1)
                    {
                        UserCells[i, j].Jx = 0;
                        UserCells[i, j].Jy = 0;
                    }

                });
            });

            CrystallizationFrontIni(UserCells); // CF - фронт кристаллизации

        }

        public void Opti(Cell[,] UserCells, double TConst, int i, int j) // Поиск решений.
        {
            /////////////////////////////////////////////////////////////////////////////////////////

            //// Вариант №1 /// Предполагаем, что V >= 1. Тогда k = 1.

            double V, LastV, f1, f2;
            bool stop = false;
            bool f1f2 = false;
            bool f2f1 = false;
            double RangeBorder1, RangeBorder2;
            epsilone = 0.001;

            TConst = TConst + (UserCells[i, j].GibsTompson * UserCells[i, j].K * (1 - 15 * Math.Cos(4 * (Angle1 - Angle0)))) / TD - Me * C0 / TD; // All const. Не соответствует Павлыку, без учета случайных флуктуаций.
            UserCells[i, j].dT = TConst * TD;

            V = UserCells[i, j].b * (TConst - UserCells[i, j].C * C0 * (Me / (1 - kke)) * Math.Log(kke) / TD);

            if (V >= 1)
            {
                UserCells[i, j].V = V;
                UserCells[i, j].kk = 1;
                UserCells[i, j].m = (1 / (1 - kke)) * (Me * (1 - CellsArea[i, j].kk + Math.Log(CellsArea[i, j].kk / kke) + (1 - CellsArea[i, j].kk) * (1 - CellsArea[i, j].kk) * V));
            }

            //// Вариант №2 /// Когда, что V < 1.

            if (UserCells[i, j].V < 1)
            {
                f2f1 = true;

                RangeBorder1 = 5 - Math.Sqrt(26);
                RangeBorder2 = 1;

                V = RangeBorder1 + epsilone;

                f1 = Math.Log(((1 - V * V) * kke + V * r) / ((1 - V * V + V * r) * kke));
                f2 = (V - TConst) * (1 - kke) / (UserCells[i, j].b * UserCells[i, j].C * C0 * Me / TD) - ((1 - ((1 - V * V) * kke + V * r) / (1 - V * V + V * r)) + (1 - ((1 - V * V) * kke + V * r) / (1 - V * V + V * r)) * (1 - ((1 - V * V) * kke + V * r) / (1 - V * V + V * r)) * V);

                if (f1 > f2) { f1f2 = true; }
                if (f2 > f1) { f2f1 = true; }

                while (stop == false)
                {
                    if (V > RangeBorder2) { stop = true; }

                    if (f1 == f2) // А вдруг так :)
                    {
                        stop = true;
                        UserCells[i, j].V = V;
                        CellsArea[i, j].kk = ((1 - V * V) * kke + V * r) / (1 - V * V + V * r);
                        UserCells[i, j].m = (1 / (1 - kke)) * (Me * (1 - CellsArea[i, j].kk + Math.Log(CellsArea[i, j].kk / kke) + (1 - CellsArea[i, j].kk) * (1 - CellsArea[i, j].kk) * V));
                    }

                    else
                    {

                        LastV = V;
                        V = V + epsilone;

                        f1 = Math.Log(((1 - V * V) * kke + V * r) / ((1 - V * V + V * r) * kke));
                        f2 = (V - TConst) * (1 - kke) / (UserCells[i, j].b * UserCells[i, j].C * C0 * Me / TD) - ((1 - ((1 - V * V) * kke + V * r) / (1 - V * V + V * r)) + (1 - ((1 - V * V) * kke + V * r) / (1 - V * V + V * r)) * (1 - ((1 - V * V) * kke + V * r) / (1 - V * V + V * r)) * V);

                        if (f1 > f2) { f1f2 = true; }
                        if (f2 > f1) { f2f1 = true; }

                        if (f1f2 == f2f1)
                        {
                            stop = true;
                            UserCells[i, j].V = (LastV + V) / 2;
                            CellsArea[i, j].kk = ((1 - V * V) * kke + V * r) / (1 - V * V + V * r);
                            UserCells[i, j].m = (1 / (1 - kke)) * (Me * (1 - CellsArea[i, j].kk + Math.Log(CellsArea[i, j].kk / kke) + (1 - CellsArea[i, j].kk) * (1 - CellsArea[i, j].kk) * V));
                        }
                    }

                }

                if (UserCells[i, j].V < 0)
                {
                    UserCells[i, j].V = 0;
                }
                /////////////////////////////////////////////////////////////////////////////////////
            }
        }// Поиск оптимальных решений///

        public void CopyBorder(Cell[,] UserCells)  //Перенос граничных условий. Закрытие расчетной области в цилиндр.
        {
            Parallel.For(0, CellsArea.GetLength(1) - 3, j =>
            {
                UserCells[0, j] = UserCells[UserCells.GetLength(0) - 6, j];
                UserCells[1, j] = UserCells[UserCells.GetLength(0) - 5, j];
                UserCells[2, j] = UserCells[UserCells.GetLength(0) - 4, j];

                UserCells[UserCells.GetLength(0) - 1, j] = UserCells[5, j];
                UserCells[UserCells.GetLength(0) - 2, j] = UserCells[4, j];
                UserCells[UserCells.GetLength(0) - 3, j] = UserCells[3, j];

            });

        }

        public void CrystallizationFrontIni(Cell[,] UserCells) // CF - фронт кристаллизации
        {
            //for (int i = 3; i <= UserCells.GetLength(0) - 4; i++) 
            //{
            //for (int j = 3; j <= UserCells.GetLength(1) - 4; j++)
            //{

            CFmaxJ = 0;

            Parallel.For(3, CellsArea.GetLength(0) - 3, i =>
            {
                Parallel.For(3, CellsArea.GetLength(1) - 3, j =>
                {
                    UserCells[i, j].CF = 0;

                    if (UserCells[i, j].G < 1 &&
                        (
                        //UserCells[i - 1, j - 1].G == 1 || 
                         UserCells[i, j - 1].G == 1 ||
                        //UserCells[i + 1, j - 1].G == 1 || 
                         UserCells[i - 1, j].G == 1 ||
                         UserCells[i + 1, j].G == 1 ||
                        //UserCells[i - 1, j + 1].G == 1 || 
                         UserCells[i, j + 1].G == 1// || 
                        //UserCells[i + 1, j + 1].G == 1
                         ))
                    {
                        UserCells[i, j].CF = 1;

                        if (CFmaxJ < j)
                        {
                            CFmaxJ = j;
                        }
                    }
                });
            });

        }// Инициализация фронта кристаллизации.

        public void DiffusionFrontIni(Cell[,] UserCells) // DF - фронт диффузии
        {
            for (int i = 3; i <= UserCells.GetLength(0) - 4; i++)
            {
                int Delta = Convert.ToInt32(Math.Truncate(qq * t / h));

                if (CFIndex[i] + Delta <= UserCells.GetLength(1) - 1)
                {
                    UserCells[i, CFIndex[i] + Delta].DF = 1;
                    UserCells[i, CFIndex[i] + Delta].WasDF = 1;
                }
            }
        }

        public double CurvatureK(Cell[,] UserCells, int i, int j) // Расчет коефициента кривизны поверхности в точке.
        {
            // Расчет сумы долей твердой фазы в круге диаметром 3h, для оценки кривизны поверхности. По Павлыку.

            double Fs = 0; // Вектор кристаллизации

            for (int n = -3; n <= 3; n++) // Расчет вектора кристаллизаци
            {
                for (int m = -3; m <= 3; m++)
                {
                    Fs = Fs + UserCells[i + n, j + m].Gn * Mw[3 + n, 3 + m];
                }
            }

            double deltaX = 0;

            for (int n = -3; n <= 3; n++) // Расчет deltaX
            {
                for (int m = -3; m <= 3; m++)
                {
                    deltaX = deltaX + n * UserCells[i + n, j + m].Gn * Mw[3 + n, 3 + m];
                }
            }

            double deltaY = 0;

            for (int n = -3; n <= 3; n++) // Расчет deltaY
            {
                for (int m = -3; m <= 3; m++)
                {
                    deltaY = deltaY + m * UserCells[i + n, j + m].Gn * Mw[3 + n, 3 + m];
                }
            }

            Angle1 = Math.Atan(deltaX / deltaY); // Угол нормального вектора к направлению кристаллизации.

            return (A + B * UserCells[i, j].Gn - Fs) * 2 / (S * h * hD); // Коефициент кривизны (Должен быть размерным 1/м).

            // double  Nf = 3 + 3*UserCells[i, j].G, Ns = 0; 

            // for (int m = -1; m <= 1; m++)
            //   {
            //         for (int n = -1; n <= 1; n++)
            //        {
            //             Ns = Ns + UserCells[i + n, j + m].G;
            //          }
            //     }

            //     return (Ns - Nf) / (9 * 3 * h);  // 2R = 3h, 9 - колличество ячеек в круге 3h

        }// Вычисление коефициента кривизны в точке.

        public void Saven(Cell[,] UserCells)
        {
            for (int i = 3; i <= CellsArea.GetLength(0) - 4; i++) // Расчет концентраций и потоков.
            {
                for (int j = 3; j <= CellsArea.GetLength(1) - 4; j++)
                {
                    UserCells[i, j].kkn = UserCells[i, j].kk;
                    UserCells[i, j].Cn = UserCells[i, j].C;   // Для сохранения значений с предыдущего шага.
                    UserCells[i, j].Jxn = UserCells[i, j].Jx;
                    UserCells[i, j].Jyn = UserCells[i, j].Jy;
                    UserCells[i, j].Gn = UserCells[i, j].G;   // Для сохранения значений с предыдущего шага.
                }
            }
        }

        public void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) // Вся основная физика.
        {

            // Определение начального полного переохлаждения в отсутствии начального градиента концентрации, исходя из начальной скорости кристаллизации

            //int p = 2775;     
            //h = a / p; // Размер ячейки вычислительной схемы, вычесляется как часть размера зерна основного металла.

            //KShod = (h * h) / (D * t);

            //hD = a0*n0;
            //tD = hD / VD;

            // пересчитать исходя из Q h t KShod.            
            //tD = D / (VD * VD); 
            //hD = tD * VD;
    
            
            //h = h / hD;
            //t = t / tD;
            //Q = (hD*hD) / (D*tD);
            
            r = VD / VDl;

            int Kshod0 = 1140; // 1140
            
            h = 10; // 10
            t = 1;

            Q = Kshod0 * t / (h * h);

            KShod = (Q*h*h)/(t);

            hD = (KShod * D) / (h * h * t * VD);
            tD = hD / VD;
            TD = VD / beta;

            V0 = V0VD;

            if (KShod < 8)
            {   
                epsiloneOK = false;
                form2.ShowDialog();
            }

            if (epsiloneOK == true)    // Расчет коефициента неравновестного распределения примеси.
            {
                if (V0 >= 1) 

                 {
                     CellsArea[0, 0].kk = 1;
                 }

                 else { CellsArea[0, 0].kk = ((1 - V0 * V0) * kke + V0 * r) / (1 - V0 * V0 + V0 * r); }
    
                 ////////////////////////////////////////////////////////////////////////////////////
    
                 // Расчет наклона кинетического ликвидуса для расчета deltaTn

                CellsArea[0, 0].m = (1 / (1 - kke)) * (Me * (1 - CellsArea[0, 0].kk + Math.Log(CellsArea[0, 0].kk / kke) + (1 - CellsArea[0, 0].kk) * (1 - CellsArea[0, 0].kk) * V0));
                 
                /////////////////////////////////////////////////////////////
               // Расчет полного начального переохлаждения в ситеме в отсутствии начального градиента концентрации, исходя из начальной скорости кристаллизации

                double T0 = Ta + CellsArea[0, 0].m * CellsArea[0, 0].Cn * C0 - V0 * VD / beta;

                deltaTFull = Ta + Me * C0 - T0;  
                deltaTFull = deltaTFull / TD;

                /////////////////////////////////////////////////////////////

                double[] F = {   Math.Pow(2.0, 1873 / (1873 - deltaTFull * TD)),  // C
                                 Math.Pow(5.0, 1873 / (1873 - deltaTFull * TD)),  // Mn
                                 Math.Pow(2.2, 1873 / (1873 - deltaTFull * TD)),  // Si
                                 Math.Pow(1.4, 1873  / (1873 - deltaTFull * TD)),  // Ni
                                 Math.Pow(0.45, 1873 / (1873 - deltaTFull * TD)), // Mo
                                 Math.Pow(0.12, 1873 / (1873 - deltaTFull * TD))  // Ti
                             };

                 double[] ni = { C0/12, Mn/55, Si/28, Ni/58.7, Mo/96, Ti/47.9};

                 double niSUM = 0;

                 for (i = 0; i <= 5; i++)
                 { 
                    niSUM = niSUM + ni[i];
                 }

                  double[] Xi = new double[6];

                 for (i = 0; i <= 5; i++)
                 {
                    Xi[i] = ni[i] / niSUM;
                 }

                double SUM = 0;

                for (i = 0; i <= 5; i++)
                {
                    SUM = SUM + Xi[i] * F[i];
                }

                gamma = gamma - 0.2 * Math.Log10(SUM);

                /////////////////////////////////////////////////////////////

                GibsTompsonAlloy = (gamma * Ta) / QL;
                GibsTompsonInocula = ((gammaInocula + gamma - Wa) * Ta) / QL;

                /////////////////////////////////////////////////////  

                //for (i = 0; i <= CellsArea.GetLength(0) - 1; i++) // Переприсвоение для задания начальных приближений m, k, V во всех ячейках.
                //{
                //   for (j = 0; j <= CellsArea.GetLength(1) - 1; j++)
                //    {
                //        CellsArea[i, j].kk = CellsArea[0, 0].kk; // CellsArea[0, 0].kk
                //        CellsArea[i, j].m = CellsArea[0, 0].m;

                //         CellsArea[i, j].V = 0;//V0
                //   }
                // }
                
                CrystallizationFrontIni(CellsArea); // CF - фронт кристаллизации
                
                CFIndex = new int[CellsArea.GetLength(0)];
                
                Random random1 = new Random(); // Экземпляр класс Random, для генерирования псевдослучайныйх чисел.

                for (i = 3; i <= CellsArea.GetLength(0) - 4; i++)
                {
                    for (j = 3; j <= CellsArea.GetLength(1) - 4; j++)
                    {
                        double rand1 = random1.NextDouble();

                        if (rand1 < InoculaVOL)
                        {
                            CellsArea[i, j].GibsTompson = GibsTompsonInocula;
                        }

                        else
                        {
                            CellsArea[i, j].GibsTompson = GibsTompsonAlloy;
                        }
                    }
                }

                Parallel.For(3, CellsArea.GetLength(0) - 3, i =>
                {
                    Parallel.For(3, CellsArea.GetLength(1) - 3, j =>
                        {
                            if (CellsArea[i, j].CF == 1)
                            {
                                CellsArea[i, j].DF = 1;
                                CFIndex[i] = j;
                            }
                        });
                });

                CopyBorder(CellsArea);

                ExcelOut(CellsArea, 0);
                Draw(CellsArea, 0);

                qq = 1; // Начало шага n+1
                
                while (StartPressed != false)
                {
                    Saven(CellsArea);

                    GandC(CellsArea);      // Расчет Прироста твердой фазы, концентрации, потоков и тд.

                    CopyBorder(CellsArea); // Перенос граничных условий. Закрытие расчетной области в цилиндр.
                               
                    if (qq % 1000 == 0)
                      {
                           Draw(CellsArea, qq);
                           if (qq % 1000 == 0)
                           { ExcelOut(CellsArea, qq); }
                       }

                    qq++;
                }
            }
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;

            // Параметры сплава

            textBox1.Text = Convert.ToString(1809);  // Температура затвердевания основного компонента Та (К)
            textBox2.Text = Convert.ToString(0.05);   // Концентрация приместного компонента С0 (вес. %)
            textBox3.Text = Convert.ToString(0.4);   // Кинетический коефициент роста β (м/(с∙К))

            textBox4.Text = Convert.ToString(0.1860);   // Коефициент поверхностного натяжения СПЛАВА γ (Дж/м2)

            textBox29.Text = Convert.ToString(0.1780);  // Коефициент поверхностного натяжения ИНОКУЛЯНТА γ (Дж/м2)
            textBox30.Text = Convert.ToString(0.2);     // Количество инокулянтов, (доли единицы)
            textBox31.Text = Convert.ToString(0.0760);   // Работа адгезии, Wa

            textBox5.Text = Convert.ToString(-80);   // Тангенс угла наклона равновестного ликвидуса Ме (К/вес.%) (Одинаков для всех концентраций углерода, зависит от других примесей)
            textBox6.Text = Convert.ToString(0.1);   // Равновестный коефициент распределения Ке (-)
            textBox7.Text = Convert.ToString(6E-8);  // Коефициент диффузии D (м2/с)

            textBox9.Text = Convert.ToString(1E+9);  // Скрытая теплота кристаллизации QL (Дж/м3)
            textBox10.Text = Convert.ToString(5E+6); // Теплоемкость χ (Дж/(м3∙К))

            textBox11.Text = Convert.ToString( 17);   // Скорость диффузии в объеме VD (м/с)
            textBox12.Text = Convert.ToString( 17);   // Скорость диффузии на границе раздела фаз VDl (м/с)
            textBox23.Text = Convert.ToString( 0);    //Угол наклона градиента температуры к начальному фронту кристаллизации.
            
            // Концентрация других приместных элементов для расчера коефициента поверхностного натяжения.

            textBox15.Text = Convert.ToString(1.300);    //Mn (HH 0)
            textBox24.Text = Convert.ToString(0.301);    //Si
            textBox25.Text = Convert.ToString(2.500);    //Ni
            textBox26.Text = Convert.ToString(0.270);    //Mo
            textBox27.Text = Convert.ToString(0.003);    //Ti
            textBox28.Text = Convert.ToString(0.043);    //Al

            Mn = Convert.ToDouble(textBox15.Text);
            Si = Convert.ToDouble(textBox24.Text);
            Ni = Convert.ToDouble(textBox25.Text);
            Mo = Convert.ToDouble(textBox26.Text);
            Ti = Convert.ToDouble(textBox27.Text);
            Al = Convert.ToDouble(textBox28.Text);
            
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            Ta = Convert.ToDouble(textBox1.Text);    // Температура затвердевания основного компонента Та (К)
            C0 = Convert.ToDouble(textBox2.Text);    // Концентрация приместного компонента С0 (вес. %)
            beta = Convert.ToDouble(textBox3.Text);  // Кинетический коефициент роста β (м/(с∙К))

            gamma = Convert.ToDouble(textBox4.Text); // Коефициент поверхностного натяжения Сплава γ (Дж/м2)
            gammaInocula = Convert.ToDouble(textBox29.Text); // Коефициент поверхностного натяжения Инокулянта γ (Дж/м2)
            InoculaVOL =  Convert.ToDouble(textBox30.Text);
            Wa = Convert.ToDouble(textBox31.Text);

            Me = Convert.ToDouble(textBox5.Text);    // Тангенс угла наклона равновестного ликвидуса Ме (К/вес.%)
            kke = Convert.ToDouble(textBox6.Text);    // Равновестный коефициент распределения Ке (-)
            D = Convert.ToDouble(textBox7.Text);     // Коефициент диффузии D (м2/с)
            QL = Convert.ToDouble(textBox9.Text);    // Скрытая теплота кристаллизации QL (Дж/м3)
            Khi = Convert.ToDouble(textBox10.Text);  // Теплоемкость χ (Дж/(м3∙К))
            VD = Convert.ToDouble(textBox11.Text);   // Скорость диффузии в объеве VD (м/с)
            VDl = Convert.ToDouble(textBox12.Text);  // Скорость диффузии на границе раздела фаз VDl (м/с)
            Angle0 = Convert.ToDouble(textBox23.Text); //Угол наклона градиента температуры к начальному фронту кристаллизации.

            //
            // Параметры модели

            textBox8.Text = Convert.ToString(30E-6);  // Размер зерна базового металла a (м)
            textBox22.Text = Convert.ToString(3E-10); // Межатомное расстояние a0

            textBox19.Text = Convert.ToString(1);    // Шаг по времени t
            
            textBox13.Text = Convert.ToString(1000); // 1000      // Количество ячеек в расчетной побрасти по X
            textBox14.Text = Convert.ToString(2000); // 2000      // Количество ячеек в расчетной побрасти по Y
            textBox16.Text = Convert.ToString(4);       // Размер твердой подложки
            textBox17.Text = Convert.ToString(1);        // Количсетво зародышей
            textBox18.Text = Convert.ToString(3);        // Радиус зародышей

            textBox20.Text = Convert.ToString(0.50);     // Начальная скорость кристаллизации, определяющая полное начальное переохлаждение V0
            textBox21.Text = Convert.ToString(0.07);     // Амплитуда шума noise

            X = Convert.ToInt32(textBox13.Text);           // Количество ячеек в расчетной побрасти по X
            Y = Convert.ToInt32(textBox14.Text);           // Количество ячеек в расчетной побрасти по Y
            SolidPixel = Convert.ToInt32(textBox16.Text);  // Размер твердой подложки
            Seeds = Convert.ToInt32(textBox17.Text);       // Количсетво зародышей
            SeedRadius = Convert.ToInt32(textBox18.Text);  // Радиус зародышей
            t = Convert.ToDouble(textBox19.Text);          // Шаг по времени
            V0VD = Convert.ToDouble(textBox20.Text);       // Начальная скорость кристаллизации, определяющая полное начальное переохлаждение
            noise = Convert.ToDouble(textBox21.Text);      // Амплитуда шума
            a = Convert.ToDouble(textBox8.Text);           // Размер зерна базового металла a (м)
            a0 = Convert.ToDouble(textBox22.Text);         // Межатомное расстояние a0
            
            //

        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (StartPressed == false)
            {
                // Обновления значения констант, согласно с введенными данными.
                // Параметры сплава

                Ta    = Convert.ToDouble(textBox1.Text);    // Температура затвердевания основного компонента Та (К)
                C0    = Convert.ToDouble(textBox2.Text);    // Концентрация приместного компонента С0 (вес. %)
                beta  = Convert.ToDouble(textBox3.Text);    // Кинетический коефициент роста β (м/(с∙К))
                gamma = Convert.ToDouble(textBox4.Text);    // Коефициент поверхностного натяжения γ (Дж/м2)
                Me    = Convert.ToDouble(textBox5.Text);    // Тангенс угла наклона равновестного ликвидуса Ме (К/вес.%)
                kke   = Convert.ToDouble(textBox6.Text);    // Равновестный коефициент распределения Ке (-)
                D     = Convert.ToDouble(textBox7.Text);    // Коефициент диффузии D (м2/с)
                a     = Convert.ToDouble(textBox8.Text);    // Размер зерна базового металла a (м)
                QL    = Convert.ToDouble(textBox9.Text);    // Скрытая теплота кристаллизации QL (Дж/м3)
                Khi   = Convert.ToDouble(textBox10.Text);   // Теплоемкость χ (Дж/(м3∙К))
                VD    = Convert.ToDouble(textBox11.Text);   // Скорость диффузии в объеве VD (м/с)
                VDl   = Convert.ToDouble(textBox12.Text);   // Скорость диффузии на границе раздела фаз VDl (м/с)
               
                Mn = Convert.ToDouble(textBox15.Text);
                Si = Convert.ToDouble(textBox24.Text);
                Ni = Convert.ToDouble(textBox25.Text);
                Mo = Convert.ToDouble(textBox26.Text);
                Ti = Convert.ToDouble(textBox27.Text);
                Al = Convert.ToDouble(textBox28.Text);
                
                //
                // Параметры модели

                X = Convert.ToInt32(textBox13.Text);          // Количество ячеек в расчетной побрасти по X
                Y = Convert.ToInt32(textBox14.Text);          // Количество ячеек в расчетной побрасти по Y
                SolidPixel = Convert.ToInt32(textBox16.Text); // Размер твердой подложки
                Seeds = Convert.ToInt32(textBox17.Text);       // Количсетво зародышей
                SeedRadius = Convert.ToInt32(textBox18.Text);  // Радиус зародышей
                t = Convert.ToDouble(textBox19.Text);          // Шаг по времени
                V0 = Convert.ToDouble(textBox20.Text);         // Начальная скорость кристаллизации, определяющая полное начальное переохлаждение
                noise = Convert.ToDouble(textBox21.Text);      // Амплитуда шума

                //

                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CloseProcess();
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Начало расчета. 

            StartPressed = true;      // Устранение конфликтов при нажатии кнопок START, STOP, Refresh.
            button2.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = true;

            saveFileDialog1.Filter = "Bitmap Image|*.png";
            saveFileDialog1.Title = "Save Image Files";
            saveFileDialog1.InitialDirectory = "C:/Users/Gromt/MyDocuments/DENSIM";
            saveFileDialog1.FileName = "Project_0";
            saveFileDialog1.ShowDialog();

            if (StartPressed == true)
            {
                CellsArea = new Cell[X + 6, Y + 3]; // Присвоение размера массиву ячеек.

                // Присваеваем всем клеткам области симуляции начальное состояние (Все жидкость) //

                for (i = 0; i <= CellsArea.GetLength(0) - 1; i++)
                {
                    for (j = 0; j <= CellsArea.GetLength(1) - 1; j++)
                    {
                        CellsArea[i, j].G = 0;
                        CellsArea[i, j].Gn = 0;
                        CellsArea[i, j].C = C0/C0; 
                        CellsArea[i, j].Cn = C0/C0;
                        CellsArea[i, j].Jx = 0;
                        CellsArea[i, j].Jy = 0;
                    }
                }

                // Конец присвоения //

                // Создаем твердую подложку толщиной SolidPixel

                for (i = 0; i <= CellsArea.GetLength(0) - 1; i++)
                {
                    for (j = 0; j <= SolidPixel - 1; j++)
                    {
                        CellsArea[i, j].G = 1;
                        CellsArea[i, j].Gn = 1;
                    }
                }

                //  Конец создания твердой подложки толщиной SolidPixel


                // Создаем центры зарождения твердой фазы (Seeds) //

                for (i = 0; i <= Seeds - 1; i++) // Придание центрам радиуса (2D размер).
                {
                    for (j = 0; j <= SeedRadius - 1; j++)
                    {
                        for (int k = -SeedRadius + j; k <= SeedRadius - j; k++)
                        {
                           CellsArea[(X / (2 * Seeds)) + i * (X / Seeds) - 1 + k + 3, j + SolidPixel].G = 1;
                           CellsArea[(X / (2 * Seeds)) + i * (X / Seeds) - 1 + k + 3, j + SolidPixel].Gn = 1;
                        }
                    }
                }

            }

            backgroundWorker1.RunWorkerAsync();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Останавливаем расчет.

            button2.Enabled = false;
            button3.Enabled = true;
            button4.Enabled = false;
            StartPressed = false;
        }

        private void Draw(Cell[,] UserCells, int userK)    // Вывод в форму массив клеток прямоугольниками
        {

            Graphics GDIp = CreateGraphics();

            //Bitmap MyBitMap = new Bitmap(UserCells.GetLength(0), UserCells.GetLength(1)); // Создаем точечное изображение
            //Bitmap MyBitMapC = new Bitmap(UserCells.GetLength(0), UserCells.GetLength(1)); // Создаем точечное изображение
            Bitmap MyBitMapCF = new Bitmap(UserCells.GetLength(0), UserCells.GetLength(1)); // Создаем точечное изображение


            for (i = 3; i <= UserCells.GetLength(0) - 4; i++) // Заполняем MyBitMap.
            {
                for (j = 0; j <= UserCells.GetLength(1) - 4; j++)
                {

         //// РАСПРЕДЕЛЕНИЕ ЦВЕТОВ КЛЕТКАМ С РАЗНЫМ СОСТОЯНИЕМ //// 

         //if (UserCells[i, j].G == 0)
         //{
         //    MyBitMap.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 0, 0)); // Liquid
         //}

         //if (UserCells[i, j].G == 1)
         //{
          //   MyBitMap.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 0, 255)); // Solid
         //}

         //         if (UserCells[i, j].G < 1 && UserCells[i, j].G > 0)
         //{
          //   MyBitMap.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 60, 175)); // front
         //}

         //// РАСПРЕДЕЛЕНИЕ ЦВЕТОВ КЛЕТКАМ по концентрации //// 

                    if (UserCells[i, j].C < 0)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 255, 255));
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.0 && UserCells[i, j].C < 0.01)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 10, 255));
                    }

                    if (UserCells[i, j].C >= 0.01 && UserCells[i, j].C < 0.02)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 15, 255));
                    }

                    if (UserCells[i, j].C >= 0.02 && UserCells[i, j].C < 0.03)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 20, 255));
                    }

                    if (UserCells[i, j].C >= 0.03 && UserCells[i, j].C < 0.04)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 25, 255));
                    }

                    if (UserCells[i, j].C >= 0.04 && UserCells[i, j].C < 0.05)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 30, 255));
                    }

                    if (UserCells[i, j].C >= 0.05 && UserCells[i, j].C < 0.06)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 35, 255));
                    }

                    if (UserCells[i, j].C >= 0.06 && UserCells[i, j].C < 0.07)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 40, 255));
                    }

                    if (UserCells[i, j].C >= 0.07 && UserCells[i, j].C < 0.08)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 45, 255));
                    }

                    if (UserCells[i, j].C >= 0.08 && UserCells[i, j].C < 0.09)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 50, 255));
                    }

                    if (UserCells[i, j].C >= 0.09 && UserCells[i, j].C < 0.1)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 55, 255));
                    }

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.1 && UserCells[i, j].C < 0.11)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 60, 255));
                    }

                    if (UserCells[i, j].C >= 0.11 && UserCells[i, j].C < 0.12)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 65, 255));
                    }

                    if (UserCells[i, j].C >= 0.12 && UserCells[i, j].C < 0.13)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 70, 255));
                    }
                    if (UserCells[i, j].C >= 0.13 && UserCells[i, j].C < 0.14)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 75, 255));
                    }
                    if (UserCells[i, j].C >= 0.14 && UserCells[i, j].C < 0.15)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 80, 255));
                    }
                    if (UserCells[i, j].C >= 0.15 && UserCells[i, j].C < 0.16)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 85, 255));
                    }
                    if (UserCells[i, j].C >= 0.16 && UserCells[i, j].C < 0.17)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 90, 255));
                    }
                    if (UserCells[i, j].C >= 0.17 && UserCells[i, j].C < 0.18)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 95, 255));
                    }
                    if (UserCells[i, j].C >= 0.18 && UserCells[i, j].C < 0.19)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 100, 255));
                    }
                    if (UserCells[i, j].C >= 0.19 && UserCells[i, j].C < 0.2)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 105, 255));
                    }

//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.2 && UserCells[i, j].C < 0.21)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 110, 255));
                    }
                    if (UserCells[i, j].C >= 0.21 && UserCells[i, j].C < 0.22)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 115, 255));
                    }
                    if (UserCells[i, j].C >= 0.22 && UserCells[i, j].C < 0.23)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 120, 255));
                    }
                    if (UserCells[i, j].C >= 0.23 && UserCells[i, j].C < 0.24)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 125, 255));
                    }
                    if (UserCells[i, j].C >= 0.24 && UserCells[i, j].C < 0.25)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 130, 255));
                    }
                    if (UserCells[i, j].C >= 0.25 && UserCells[i, j].C < 0.26)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 10, 135, 255));
                    }
                    if (UserCells[i, j].C >= 0.26 && UserCells[i, j].C < 0.27)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 20, 140, 255));
                    }
                    if (UserCells[i, j].C >= 0.27 && UserCells[i, j].C < 0.28)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 30, 145, 255));
                    }
                    if (UserCells[i, j].C >= 0.28 && UserCells[i, j].C < 0.29)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 40, 150, 255));
                    }
                    if (UserCells[i, j].C >= 0.29 && UserCells[i, j].C < 0.3)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 50, 155, 255));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (UserCells[i, j].C >= 0.3 && UserCells[i, j].C < 0.31)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 60, 160, 255));
                    }
                    if (UserCells[i, j].C >= 0.31 && UserCells[i, j].C < 0.32)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 70, 165, 255));
                    }
                    if (UserCells[i, j].C >= 0.32 && UserCells[i, j].C < 0.33)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 80, 170, 255));
                    }
                    if (UserCells[i, j].C >= 0.33 && UserCells[i, j].C < 0.34)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 90, 175, 255));
                    }
                    if (UserCells[i, j].C >= 0.34 && UserCells[i, j].C < 0.35)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 100, 180, 255));
                    }
                    if (UserCells[i, j].C >= 0.35 && UserCells[i, j].C < 0.36)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 110, 185, 255));
                    }
                    if (UserCells[i, j].C >= 0.36 && UserCells[i, j].C < 0.37)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 120, 190, 255));
                    }
                    if (UserCells[i, j].C >= 0.37 && UserCells[i, j].C < 0.38)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 130, 195, 255));
                    }
                    if (UserCells[i, j].C >= 0.38 && UserCells[i, j].C < 0.39)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 140, 200, 255));
                    }
                    if (UserCells[i, j].C >= 0.39 && UserCells[i, j].C < 0.4)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 150, 205, 255));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.4 && UserCells[i, j].C < 0.41)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 160, 210, 255));
                    }
                    if (UserCells[i, j].C >= 0.41 && UserCells[i, j].C < 0.42)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 170, 215, 255));
                    }
                    if (UserCells[i, j].C >= 0.42 && UserCells[i, j].C < 0.43)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 180, 220, 255));
                    }
                    if (UserCells[i, j].C >= 0.43 && UserCells[i, j].C < 0.44)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 190, 225, 255));
                    }
                    if (UserCells[i, j].C >= 0.44 && UserCells[i, j].C < 0.45)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 200, 230, 255));
                    }
                    if (UserCells[i, j].C >= 0.45 && UserCells[i, j].C < 0.46)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 210, 235, 255));
                    }
                    if (UserCells[i, j].C >= 0.46 && UserCells[i, j].C < 0.47)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 220, 240, 255));
                    }
                    if (UserCells[i, j].C >= 0.47 && UserCells[i, j].C < 0.48)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 230, 245, 255));
                    }
                    if (UserCells[i, j].C >= 0.48 && UserCells[i, j].C < 0.49)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 240, 250, 255));
                    }
                    if (UserCells[i, j].C >= 0.49 && UserCells[i, j].C < 0.5)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 250, 255, 255));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (UserCells[i, j].C >= 0.5 && UserCells[i, j].C < 0.51)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 240, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.51 && UserCells[i, j].C < 0.52)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 230, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.52 && UserCells[i, j].C < 0.53)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 220, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.53 && UserCells[i, j].C < 0.54)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 210, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.54 && UserCells[i, j].C < 0.55)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 200, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.55 && UserCells[i, j].C < 0.56)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 190, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.56 && UserCells[i, j].C < 0.57)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 180, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.57 && UserCells[i, j].C < 0.58)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 170, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.58 && UserCells[i, j].C < 0.59)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 160, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.59 && UserCells[i, j].C < 0.6)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 150, 255, 255));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.6 && UserCells[i, j].C < 0.61)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 140, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.61 && UserCells[i, j].C < 0.62)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 130, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.62 && UserCells[i, j].C < 0.63)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 120, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.63 && UserCells[i, j].C < 0.64)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 110, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.64 && UserCells[i, j].C < 0.65)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 100, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.65 && UserCells[i, j].C < 0.66)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 90, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.66 && UserCells[i, j].C < 0.67)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 80, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.67 && UserCells[i, j].C < 0.68)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 70, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.68 && UserCells[i, j].C < 0.69)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 60, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.69 && UserCells[i, j].C < 0.7)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 50, 255, 255));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    if (UserCells[i, j].C >= 0.7 && UserCells[i, j].C < 0.71)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 40, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.71 && UserCells[i, j].C < 0.72)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 30, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.72 && UserCells[i, j].C < 0.73)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 20, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.73 && UserCells[i, j].C < 0.74)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 10, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.74 && UserCells[i, j].C < 0.75)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 255));
                    }
                    if (UserCells[i, j].C >= 0.75 && UserCells[i, j].C < 0.76)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 250));
                    }
                    if (UserCells[i, j].C >= 0.76 && UserCells[i, j].C < 0.77)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 240));
                    }
                    if (UserCells[i, j].C >= 0.77 && UserCells[i, j].C < 0.78)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 230));
                    }
                    if (UserCells[i, j].C >= 0.78 && UserCells[i, j].C < 0.79)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 220));
                    }
                    if (UserCells[i, j].C >= 0.79 && UserCells[i, j].C < 0.8)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 210));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.8 && UserCells[i, j].C < 0.81)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 200));
                    }
                    if (UserCells[i, j].C >= 0.81 && UserCells[i, j].C < 0.82)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 190));
                    }
                    if (UserCells[i, j].C >= 0.82 && UserCells[i, j].C < 0.83)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 180));
                    }
                    if (UserCells[i, j].C >= 0.83 && UserCells[i, j].C < 0.84)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 170));
                    }
                    if (UserCells[i, j].C >= 0.84 && UserCells[i, j].C < 0.85)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 160));
                    }
                    if (UserCells[i, j].C >= 0.85 && UserCells[i, j].C < 0.86)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 150));
                    }
                    if (UserCells[i, j].C >= 0.86 && UserCells[i, j].C < 0.87)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 140));
                    }
                    if (UserCells[i, j].C >= 0.87 && UserCells[i, j].C < 0.88)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 130));
                    }
                    if (UserCells[i, j].C >= 0.88 && UserCells[i, j].C < 0.89)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 120));
                    }
                    if (UserCells[i, j].C >= 0.89 && UserCells[i, j].C < 0.9)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 110));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 0.9 && UserCells[i, j].C < 0.91)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 100));
                    }
                    if (UserCells[i, j].C >= 0.91 && UserCells[i, j].C < 0.92)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 90));
                    }
                    if (UserCells[i, j].C >= 0.92 && UserCells[i, j].C < 0.93)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 80));
                    }
                    if (UserCells[i, j].C >= 0.93 && UserCells[i, j].C < 0.94)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 70));
                    }
                    if (UserCells[i, j].C >= 0.94 && UserCells[i, j].C < 0.95)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 60));
                    }
                    if (UserCells[i, j].C >= 0.95 && UserCells[i, j].C < 0.96)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 50));
                    }
                    if (UserCells[i, j].C >= 0.96 && UserCells[i, j].C < 0.97)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 40));
                    }
                    if (UserCells[i, j].C >= 0.97 && UserCells[i, j].C < 0.98)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 30));
                    }
                    if (UserCells[i, j].C >= 0.98 && UserCells[i, j].C < 0.99)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 20));
                    }
                    if (UserCells[i, j].C >= 0.99 && UserCells[i, j].C < 1.0)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 10));
                    }

                    //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 1.0 && UserCells[i, j].C < 1.1)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.1 && UserCells[i, j].C < 1.2)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 25, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.2 && UserCells[i, j].C < 1.3)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 50, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.3 && UserCells[i, j].C < 1.4)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 75, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.4 && UserCells[i, j].C < 1.5)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 100, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.5 && UserCells[i, j].C < 1.6)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 125, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.6 && UserCells[i, j].C < 1.7)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 150, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.7 && UserCells[i, j].C < 1.8)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 175, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.8 && UserCells[i, j].C < 1.9)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 200, 255, 0));
                    }

                    if (UserCells[i, j].C >= 1.9 && UserCells[i, j].C < 2.0)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 225, 255, 0));
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 2.0 && UserCells[i, j].C < 2.1)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 250, 250, 0));
                    }

                    if (UserCells[i, j].C >= 2.1 && UserCells[i, j].C < 2.2)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 225, 0));
                    }

                    if (UserCells[i, j].C >= 2.2 && UserCells[i, j].C < 2.3)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 200, 0));
                    }

                    if (UserCells[i, j].C >= 2.3 && UserCells[i, j].C < 2.4)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 175, 0));
                    }

                    if (UserCells[i, j].C >= 2.4 && UserCells[i, j].C < 2.5)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 150, 0));
                    }

                    if (UserCells[i, j].C >= 2.5 && UserCells[i, j].C < 2.6)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 125, 0));
                    }

                    if (UserCells[i, j].C >= 2.6 && UserCells[i, j].C < 2.7)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 100, 0));
                    }

                    if (UserCells[i, j].C >= 2.7 && UserCells[i, j].C < 2.8)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 75, 0));
                    }

                    if (UserCells[i, j].C >= 2.8 && UserCells[i, j].C < 2.9)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 50, 0));
                    }

                    if (UserCells[i, j].C >= 2.9 && UserCells[i, j].C < 3.0)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 25, 0));
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 3.0 && UserCells[i, j].C < 3.1)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 250, 0, 0));
                    }

                    if (UserCells[i, j].C >= 3.1 && UserCells[i, j].C < 3.2)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 240, 0, 10));
                    }

                    if (UserCells[i, j].C >= 3.2 && UserCells[i, j].C < 3.3)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 230, 0, 20));
                    }

                    if (UserCells[i, j].C >= 3.3 && UserCells[i, j].C < 3.4)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 220, 0, 30));
                    }

                    if (UserCells[i, j].C >= 3.4 && UserCells[i, j].C < 3.5)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 210, 0, 40));
                    }

                    if (UserCells[i, j].C >= 3.5 && UserCells[i, j].C < 3.6)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 200, 0, 50));
                    }

                    if (UserCells[i, j].C >= 3.6 && UserCells[i, j].C < 3.7)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 190, 0, 60));
                    }

                    if (UserCells[i, j].C >= 3.7 && UserCells[i, j].C < 3.8)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 180, 0, 70));
                    }

                    if (UserCells[i, j].C >= 3.8 && UserCells[i, j].C < 3.9)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 170, 0, 80));
                    }

                    if (UserCells[i, j].C >= 3.9 && UserCells[i, j].C < 4.0)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 160, 0, 90));
                    }

                    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                    if (UserCells[i, j].C >= 4.0 && UserCells[i, j].C < 4.1)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 140, 0, 100));
                    }

                    if (UserCells[i, j].C >= 4.1 && UserCells[i, j].C < 4.2)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 130, 0, 110));
                    }

                    if (UserCells[i, j].C >= 4.2 && UserCells[i, j].C < 4.3)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 120, 0, 120));
                    }

                    if (UserCells[i, j].C >= 4.3 && UserCells[i, j].C < 4.4)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 110, 0, 130));
                    }

                    if (UserCells[i, j].C >= 4.4 && UserCells[i, j].C < 4.5)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 100, 0, 140));
                    }

                    if (UserCells[i, j].C >= 4.5 && UserCells[i, j].C < 4.6)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 90, 0, 150));
                    }

                    if (UserCells[i, j].C >= 4.6 && UserCells[i, j].C < 4.7)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 80, 0, 160));
                    }

                    if (UserCells[i, j].C >= 4.7 && UserCells[i, j].C < 4.8)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 70, 0, 170));
                    }

                    if (UserCells[i, j].C >= 4.8 && UserCells[i, j].C < 4.9)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 60, 0, 180));
                    }

                    if (UserCells[i, j].C >= 4.9 && UserCells[i, j].C < 5.0)
                    {
                        MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 50, 0, 190));
                    }

         /////////////////////////////////////////////////////////////////////////////////////////////////////////////

          if (UserCells[i, j].C > 5.0)
         {
             MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 40, 0, 200));
         }


         if (UserCells[i, j].CF == 1)
         {
             MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 0, 0));
         }

         if (UserCells[i, j].C < 0)
         {
             MyBitMapCF.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 255, 255, 255));
         }

         //if (UserCells[i, j].DF == 1)
         //{
            // MyBitMapC.SetPixel(i, UserCells.GetLength(1) - 1 - j, Color.FromArgb(255, 0, 0, 0));
         //}
     }
}

            if (StartPressed == true) // Сохранение рисунков только после нажатия кнопки Start.
            {
                //MyBitMap.Save(saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.Length - 4) + "_G_" + userK + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp); // Сохранение рисунка (одного кадра).
                //MyBitMapC.Save(saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.Length - 4) + "_C_" + userK + ".bmp", System.Drawing.Imaging.ImageFormat.Bmp); // Сохранение рисунка (одного кадра).
                MyBitMapCF.Save(saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.Length - 4) + "_CF_" + userK + ".png", System.Drawing.Imaging.ImageFormat.Png); // Сохранение рисунка (одного кадра).
            }

            //GDIp.DrawImage(MyBitMap, 500, 10); // Выводим MyBitMap.
            //GDIp.DrawImage(MyBitMapC, 501 + X+6, 10); // Выводим MyBitMapC.
           
            int width = MyBitMapCF.Width;
            int height = MyBitMapCF.Height;
            RectangleF destinationRect = new RectangleF(
                900,
                5,
                0.25f * width,
                0.25f * height);

            GDIp.DrawImage(MyBitMapCF, destinationRect); // Выводим MyBitMapCF.

        }

        private void ExcelOut(Cell[,] UserCells, int userK)
        {
            // Вывод данных в таблицу Excel.

            Excel.Application excel = new Excel.Application(); //создаем COM-объект Excel
            excel.SheetsInNewWorkbook = 3;//количество листов в книге
            excel.Workbooks.Add(); //добавляем книгу
            Excel.Workbook workbook = excel.Workbooks[1]; //получам ссылку на первую открытую книгу
            Excel.Worksheet sheetC  = workbook.Worksheets.get_Item(1); //получаем ссылку на первый лист
            Excel.Worksheet sheetG = workbook.Worksheets.get_Item(2); //получаем ссылку на первый лист
            Excel.Worksheet sheetgamma = workbook.Worksheets.get_Item(3); //получаем ссылку на первый лист

            sheetC.Name = "Concentration";
            sheetG.Name = "G";
            sheetgamma.Name = "GibsTomphson";

            object[,] dataExportC = new object[UserCells.GetLength(0), UserCells.GetLength(1)];
            object[,] dataExportG = new object[UserCells.GetLength(0), UserCells.GetLength(1)];
            object[,] dataExportgamma = new object[UserCells.GetLength(0), UserCells.GetLength(1)];
            
            //выводим результаты расчетов.

            for (int m = 0; m <= CellsArea.GetLength(0) - 1; m++) 
              {
             for (int k = 0; k <= CellsArea.GetLength(1) - 4; k++)
                 {
                      dataExportC[m, k] = UserCells[m, k].C;
                      dataExportG[m, k] = UserCells[m, k].G;
                      dataExportgamma[m, k] = UserCells[m, k].GibsTompson;
                 }
             }

            Excel.Range rgC = sheetC.Range[sheetC.Cells[1, 1], sheetC.Cells[UserCells.GetLength(0), UserCells.GetLength(1)]];
            rgC.set_Value(Excel.XlRangeValueDataType.xlRangeValueDefault, dataExportC);
            
            Excel.Range rgG = sheetG.Range[sheetG.Cells[1, 1], sheetG.Cells[UserCells.GetLength(0), UserCells.GetLength(1)]];
            rgG.set_Value(Excel.XlRangeValueDataType.xlRangeValueDefault, dataExportG);

            Excel.Range rgdT = sheetgamma.Range[sheetgamma.Cells[1, 1], sheetgamma.Cells[UserCells.GetLength(0), UserCells.GetLength(1)]];
            rgdT.set_Value(Excel.XlRangeValueDataType.xlRangeValueDefault, dataExportgamma);
            

            //excel.Visible = 1; //делаем объект видимым
            excel.ActiveWorkbook.CheckCompatibility = false;
            excel.ActiveWorkbook.SaveAs(saveFileDialog1.FileName.Substring(0, saveFileDialog1.FileName.Length - 4) + userK, Excel.XlFileFormat.xlWorkbookDefault, null, null, 0, null, Excel.XlSaveAsAccessMode.xlShared);
            excel.Quit();
        }
        public void CloseProcess()
        {
            Process[] List;
            List = Process.GetProcessesByName("EXCEL");
            foreach (Process proc in List)
            {
                proc.Kill();
            }
        }
    }
}
