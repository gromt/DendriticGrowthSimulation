
#pragma warning disable 219
#pragma warning disable 162
using System;
public class MainTest
{

    public static bool doc_test_bool(bool val, bool test_val)
    { return (val && test_val) || (!val && !test_val); }

    public static bool doc_test_int(int val, int test_val)
    { return val==test_val; }

    public static bool doc_test_real(double val, double test_val, double _threshold)
    {
        double s = _threshold>=0 ? 1.0 : Math.Abs(test_val); 
        double threshold = Math.Abs(_threshold);
        return Math.Abs(val-test_val)/s<=threshold; 
    }

    public static bool doc_test_complex(alglib.complex val, alglib.complex test_val, double _threshold)
    { 
        double s = _threshold>=0 ? 1.0 : alglib.math.abscomplex(test_val);
        double threshold = Math.Abs(_threshold);
        return alglib.math.abscomplex(val-test_val)/s<=threshold;
    }


    public static bool doc_test_bool_vector(bool[] val, bool[] test_val)
    {
        int i;
        if( alglib.ap.len(val)!=alglib.ap.len(test_val) )
            return false;
        for(i=0; i<alglib.ap.len(val); i++)
            if( val[i]!=test_val[i] )
                return false;
        return true;
    }

    public static bool doc_test_bool_matrix(bool[,] val, bool[,] test_val)
    {
        int i, j;
        if( alglib.ap.rows(val)!=alglib.ap.rows(test_val) )
            return false;
        if( alglib.ap.cols(val)!=alglib.ap.cols(test_val) )
            return false;
        for(i=0; i<alglib.ap.rows(val); i++)
            for(j=0; j<alglib.ap.cols(val); j++)
                if( val[i,j]!=test_val[i,j] )
                    return false;
        return true;
    }

    public static bool doc_test_int_vector(int[] val, int[] test_val)
    {
        int i;
        if( alglib.ap.len(val)!=alglib.ap.len(test_val) )
            return false;
        for(i=0; i<alglib.ap.len(val); i++)
            if( val[i]!=test_val[i] )
                return false;
        return true;
    }

    public static bool doc_test_int_matrix(int[,] val, int[,] test_val)
    {
        int i, j;
        if( alglib.ap.rows(val)!=alglib.ap.rows(test_val) )
            return false;
        if( alglib.ap.cols(val)!=alglib.ap.cols(test_val) )
            return false;
        for(i=0; i<alglib.ap.rows(val); i++)
            for(j=0; j<alglib.ap.cols(val); j++)
                if( val[i,j]!=test_val[i,j] )
                    return false;
        return true;
    }

    public static bool doc_test_real_vector(double[] val, double[] test_val, double _threshold)
    {
        int i;
        if( alglib.ap.len(val)!=alglib.ap.len(test_val) )
            return false;
        for(i=0; i<alglib.ap.len(val); i++)
        {
            double s = _threshold>=0 ? 1.0 : Math.Abs(test_val[i]); 
            double threshold = Math.Abs(_threshold);
            if( Math.Abs(val[i]-test_val[i])/s>threshold )
                return false;
        }
        return true;
    }

    public static bool doc_test_real_matrix(double[,] val, double[,] test_val, double _threshold)
    {
        int i, j;
        if( alglib.ap.rows(val)!=alglib.ap.rows(test_val) )
            return false;
        if( alglib.ap.cols(val)!=alglib.ap.cols(test_val) )
            return false;
        for(i=0; i<alglib.ap.rows(val); i++)
            for(j=0; j<alglib.ap.cols(val); j++)
            {
                double s = _threshold>=0 ? 1.0 : Math.Abs(test_val[i,j]);
                double threshold = Math.Abs(_threshold);
                if( Math.Abs(val[i,j]-test_val[i,j])/s>threshold )
                    return false;
            }
        return true;
    }

    public static bool doc_test_complex_vector(alglib.complex[] val, alglib.complex[] test_val, double _threshold)
    {
        int i;
        if( alglib.ap.len(val)!=alglib.ap.len(test_val) )
            return false;
        for(i=0; i<alglib.ap.len(val); i++)
        {
            double s = _threshold>=0 ? 1.0 : alglib.math.abscomplex(test_val[i]);
            double threshold = Math.Abs(_threshold);
            if( alglib.math.abscomplex(val[i]-test_val[i])/s>threshold )
                return false;
        }
        return true;
    }

    public static bool doc_test_complex_matrix(alglib.complex[,] val, alglib.complex[,] test_val, double _threshold)
    {
        int i, j;
        if( alglib.ap.rows(val)!=alglib.ap.rows(test_val) )
            return false;
        if( alglib.ap.cols(val)!=alglib.ap.cols(test_val) )
            return false;
        for(i=0; i<alglib.ap.rows(val); i++)
            for(j=0; j<alglib.ap.cols(val); j++)
            {
                double s = _threshold>=0 ? 1.0 : alglib.math.abscomplex(test_val[i,j]);
                double threshold = Math.Abs(_threshold);
                if( alglib.math.abscomplex(val[i,j]-test_val[i,j])/s>threshold )
                    return false;
            }
        return true;
    }

    public static void spoil_vector_by_adding_element<T>(ref T[] x) where T : new()
    {
        int i;
        T[] y = x;
        x = new T[y.Length+1];
        for(i=0; i<y.Length; i++)
            x[i] = y[i];
        x[y.Length] = new T();
    }

    public static void spoil_vector_by_deleting_element<T>(ref T[] x) where T : new()
    {
        int i;
        T[] y = x;
        x = new T[y.Length-1];
        for(i=0; i<y.Length-1; i++)
            x[i] = y[i];
    }

    public static void spoil_matrix_by_adding_row<T>(ref T[,] x) where T : new()
    {
        int i, j;
        T[,] y = x;
        x = new T[y.GetLength(0)+1,y.GetLength(1)];
        for(i=0; i<y.GetLength(0); i++)
            for(j=0; j<y.GetLength(1); j++)
                x[i,j] = y[i,j];
        for(j=0; j<y.GetLength(1); j++)
            x[y.GetLength(0),j] = new T();
    }

    public static void spoil_matrix_by_deleting_row<T>(ref T[,] x) where T : new()
    {
        int i, j;
        T[,] y = x;
        x = new T[y.GetLength(0)-1,y.GetLength(1)];
        for(i=0; i<y.GetLength(0)-1; i++)
            for(j=0; j<y.GetLength(1); j++)
                x[i,j] = y[i,j];
    }

    public static void spoil_matrix_by_adding_col<T>(ref T[,] x) where T : new()
    {
        int i, j;
        T[,] y = x;
        x = new T[y.GetLength(0), y.GetLength(1)+1];
        for(i=0; i<y.GetLength(0); i++)
            for(j=0; j<y.GetLength(1); j++)
                x[i,j] = y[i,j];
        for(i=0; i<y.GetLength(0); i++)
            x[i,y.GetLength(1)] = new T();
    }

    public static void spoil_matrix_by_deleting_col<T>(ref T[,] x) where T : new()
    {
        int i, j;
        T[,] y = x;
        x = new T[y.GetLength(0), y.GetLength(1)-1];
        for(i=0; i<y.GetLength(0); i++)
            for(j=0; j<y.GetLength(1)-1; j++)
                x[i,j] = y[i,j];
    }

    public static void spoil_vector_by_value<T>(ref T[] x, T val)
    {
        if( x.Length!=0 )
            x[alglib.math.randominteger(x.Length)] = val;
    }
    public static void spoil_matrix_by_value<T>(ref T[,] x, T val)
    {
        if( x.GetLength(0)!=0 && x.GetLength(1)!=0 )
            x[alglib.math.randominteger(x.GetLength(0)),alglib.math.randominteger(x.GetLength(1))] = val;
    }

    public static void function1_func(double[] x, ref double func, object obj)
    {
        // this callback calculates f(x0,x1) = 100*(x0+3)^4 + (x1-3)^4
        func = 100*System.Math.Pow(x[0]+3,4) + System.Math.Pow(x[1]-3,4);
    }
    public static void function1_grad(double[] x, ref double func, double[] grad, object obj)
    {
        // this callback calculates f(x0,x1) = 100*(x0+3)^4 + (x1-3)^4
        // and its derivatives df/d0 and df/dx1
        func = 100*System.Math.Pow(x[0]+3,4) + System.Math.Pow(x[1]-3,4);
        grad[0] = 400*System.Math.Pow(x[0]+3,3);
        grad[1] = 4*System.Math.Pow(x[1]-3,3);
    }
    public static void function1_hess(double[] x, ref double func, double[] grad, double[,] hess, object obj)
    {
        // this callback calculates f(x0,x1) = 100*(x0+3)^4 + (x1-3)^4
        // its derivatives df/d0 and df/dx1
        // and its Hessian.
        func = 100*System.Math.Pow(x[0]+3,4) + System.Math.Pow(x[1]-3,4);
        grad[0] = 400*System.Math.Pow(x[0]+3,3);
        grad[1] = 4*System.Math.Pow(x[1]-3,3);
        hess[0,0] = 1200*System.Math.Pow(x[0]+3,2);
        hess[0,1] = 0;
        hess[1,0] = 0;
        hess[1,1] = 12*System.Math.Pow(x[1]-3,2);
    }
    public static void  function1_fvec(double[] x, double[] fi, object obj)
    {
        //
        // this callback calculates
        // f0(x0,x1) = 100*(x0+3)^4,
        // f1(x0,x1) = (x1-3)^4
        //
        fi[0] = 10*System.Math.Pow(x[0]+3,2);
        fi[1] = System.Math.Pow(x[1]-3,2);
    }
    public static void  function1_jac(double[] x, double[] fi, double[,] jac, object obj)
    {
        // this callback calculates
        // f0(x0,x1) = 100*(x0+3)^4,
        // f1(x0,x1) = (x1-3)^4
        // and Jacobian matrix J = [dfi/dxj]
        fi[0] = 10*System.Math.Pow(x[0]+3,2);
        fi[1] = System.Math.Pow(x[1]-3,2);
        jac[0,0] = 20*(x[0]+3);
        jac[0,1] = 0;
        jac[1,0] = 0;
        jac[1,1] = 2*(x[1]-3);
    }
    public static void function2_func(double[] x, ref double func, object obj)
    {
        //
        // this callback calculates f(x0,x1) = (x0^2+1)^2 + (x1-1)^2
        //
        func = System.Math.Pow(x[0]*x[0]+1,2) + System.Math.Pow(x[1]-1,2);
    }
    public static void function2_grad(double[] x, ref double func, double[] grad, object obj)
    {
        //
        // this callback calculates f(x0,x1) = (x0^2+1)^2 + (x1-1)^2
        // and its derivatives df/d0 and df/dx1
        //
        func = System.Math.Pow(x[0]*x[0]+1,2) + System.Math.Pow(x[1]-1,2);
        grad[0] = 4*(x[0]*x[0]+1)*x[0];
        grad[1] = 2*(x[1]-1);
    }
    public static void function2_hess(double[] x, ref double func, double[] grad, double[,] hess, object obj)
    {
        //
        // this callback calculates f(x0,x1) = (x0^2+1)^2 + (x1-1)^2
        // its gradient and Hessian
        //
        func = System.Math.Pow(x[0]*x[0]+1,2) + System.Math.Pow(x[1]-1,2);
        grad[0] = 4*(x[0]*x[0]+1)*x[0];
        grad[1] = 2*(x[1]-1);
        hess[0,0] = 12*x[0]*x[0]+4;
        hess[0,1] = 0;
        hess[1,0] = 0;
        hess[1,1] = 2;
    }
    public static void  function2_fvec(double[] x, double[] fi, object obj)
    {
        //
        // this callback calculates
        // f0(x0,x1) = 100*(x0+3)^4,
        // f1(x0,x1) = (x1-3)^4
        //
        fi[0] = x[0]*x[0]+1;
        fi[1] = x[1]-1;
    }
    public static void  function2_jac(double[] x, double[] fi, double[,] jac, object obj)
    {
        //
        // this callback calculates
        // f0(x0,x1) = x0^2+1
        // f1(x0,x1) = x1-1
        // and Jacobian matrix J = [dfi/dxj]
        //
        fi[0] = x[0]*x[0]+1;
        fi[1] = x[1]-1;
        jac[0,0] = 2*x[0];
        jac[0,1] = 0;
        jac[1,0] = 0;
        jac[1,1] = 1;
    }
    public static void bad_func(double[] x, ref double func, object obj)
    {
        //
        // this callback calculates 'bad' function,
        // i.e. function with incorrectly calculated derivatives
        //
        func = 100*System.Math.Pow(x[0]+3,4) + System.Math.Pow(x[1]-3,4);
    }
    public static void bad_grad(double[] x, ref double func, double[] grad, object obj)
    {
        //
        // this callback calculates 'bad' function,
        // i.e. function with incorrectly calculated derivatives
        //
        func = 100*System.Math.Pow(x[0]+3,4) + System.Math.Pow(x[1]-3,4);
        grad[0] = 40*System.Math.Pow(x[0]+3,3);
        grad[1] = 40*System.Math.Pow(x[1]-3,3);
    }
    public static void bad_hess(double[] x, ref double func, double[] grad, double[,] hess, object obj)
    {
        //
        // this callback calculates 'bad' function,
        // i.e. function with incorrectly calculated derivatives
        //
        func = 100*System.Math.Pow(x[0]+3,4) + System.Math.Pow(x[1]-3,4);
        grad[0] = 40*System.Math.Pow(x[0]+3,3);
        grad[1] = 40*System.Math.Pow(x[1]-3,3);
        hess[0,0] = 120*System.Math.Pow(x[0]+3,2);
        hess[0,1] = 1;
        hess[1,0] = 1;
        hess[1,1] = 120*System.Math.Pow(x[1]-3,2);
    }
    public static void  bad_fvec(double[] x, double[] fi, object obj)
    {
        //
        // this callback calculates 'bad' function,
        // i.e. function with incorrectly calculated derivatives
        //
        fi[0] = 10*System.Math.Pow(x[0]+3,2);
        fi[1] = System.Math.Pow(x[1]-3,2);
    }
    public static void  bad_jac(double[] x, double[] fi, double[,] jac, object obj)
    {
        //
        // this callback calculates 'bad' function,
        // i.e. function with incorrectly calculated derivatives
        //
        fi[0] = 10*System.Math.Pow(x[0]+3,2);
        fi[1] = System.Math.Pow(x[1]-3,2);
        jac[0,0] = 20*(x[0]+3);
        jac[0,1] = 0;
        jac[1,0] = 1;
        jac[1,1] = 20*(x[1]-3);
    }
    public static void function_cx_1_func(double[] c, double[] x, ref double func, object obj)
    {
        // this callback calculates f(c,x)=exp(-c0*sqr(x0))
        // where x is a position on X-axis and c is adjustable parameter
        func = System.Math.Exp(-c[0]*x[0]*x[0]);
    }
    public static void function_cx_1_grad(double[] c, double[] x, ref double func, double[] grad, object obj)
    {
        // this callback calculates f(c,x)=exp(-c0*sqr(x0)) and gradient G={df/dc[i]}
        // where x is a position on X-axis and c is adjustable parameter.
        // IMPORTANT: gradient is calculated with respect to C, not to X
        func = System.Math.Exp(-c[0]*System.Math.Pow(x[0],2));
        grad[0] = -System.Math.Pow(x[0],2)*func;
    }
    public static void function_cx_1_hess(double[] c, double[] x, ref double func, double[] grad, double[,] hess, object obj)
    {
        // this callback calculates f(c,x)=exp(-c0*sqr(x0)), gradient G={df/dc[i]} and Hessian H={d2f/(dc[i]*dc[j])}
        // where x is a position on X-axis and c is adjustable parameter.
        // IMPORTANT: gradient/Hessian are calculated with respect to C, not to X
        func = System.Math.Exp(-c[0]*System.Math.Pow(x[0],2));
        grad[0] = -System.Math.Pow(x[0],2)*func;
        hess[0,0] = System.Math.Pow(x[0],4)*func;
    }
    public static void ode_function_1_diff(double[] y, double x, double[] dy, object obj)
    {
        // this callback calculates f(y[],x)=-y[0]
        dy[0] = -y[0];
    }
    public static void int_function_1_func(double x, double xminusa, double bminusx, ref double y, object obj)
    {
        // this callback calculates f(x)=exp(x)
        y = Math.Exp(x);
    }
    public static void function_debt_func(double[] c, double[] x, ref double func, object obj)
    {
        //
        // this callback calculates f(c,x)=c[0]*(1+c[1]*(pow(x[0]-1999,c[2])-1))
        //
        func = c[0]*(1+c[1]*(System.Math.Pow(x[0]-1999,c[2])-1));
    }
    public static void s1_grad(double[] x, ref double func, double[] grad, object obj)
    {
        //
        // this callback calculates f(x) = (1+x)^(-0.2) + (1-x)^(-0.3) + 1000*x and its gradient.
        //
        // function is trimmed when we calculate it near the singular points or outside of the [-1,+1].
        // Note that we do NOT calculate gradient in this case.
        //
        if( (x[0]<=-0.999999999999) || (x[0]>=+0.999999999999) )
        {
            func = 1.0E+300;
            return;
        }
        func = System.Math.Pow(1+x[0],-0.2) + System.Math.Pow(1-x[0],-0.3) + 1000*x[0];
        grad[0] = -0.2*System.Math.Pow(1+x[0],-1.2) +0.3*System.Math.Pow(1-x[0],-1.3) + 1000;
    }

    public static int Main(string[] args)
    {
        bool _TotalResult = true;
        bool _TestResult;
        int _spoil_scenario;
        System.Console.WriteLine("C# interface tests. Please wait...");
        try
        {
            //
            // TEST nneighbor_d_1
            //      Nearest neighbor search, KNN queries
            //
            System.Console.WriteLine("0/91");
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    double[,] a = new double[,]{{0,0},{0,1},{1,0},{1,1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    int nx = 2;
                    int ny = 0;
                    int normtype = 2;
                    alglib.kdtree kdt;
                    double[] x;
                    double[,] r = new double[0,0];
                    int k;
                    alglib.kdtreebuild(a, nx, ny, normtype, out kdt);
                    x = new double[]{-1,0};
                    k = alglib.kdtreequeryknn(kdt, x, 1);
                    _TestResult = _TestResult && doc_test_int(k, 1);
                    alglib.kdtreequeryresultsx(kdt, ref r);
                    _TestResult = _TestResult && doc_test_real_matrix(r, new double[,]{{0,0}}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "nneighbor_d_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST nneighbor_t_2
            //      Subsequent queries; buffered functions must use previously allocated storage (if large enough), so buffer may contain some info from previous call
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    double[,] a = new double[,]{{0,0},{0,1},{1,0},{1,1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    int nx = 2;
                    int ny = 0;
                    int normtype = 2;
                    alglib.kdtree kdt;
                    double[] x;
                    double[,] rx = new double[0,0];
                    int k;
                    alglib.kdtreebuild(a, nx, ny, normtype, out kdt);
                    x = new double[]{+2,0};
                    k = alglib.kdtreequeryknn(kdt, x, 2, true);
                    _TestResult = _TestResult && doc_test_int(k, 2);
                    alglib.kdtreequeryresultsx(kdt, ref rx);
                    _TestResult = _TestResult && doc_test_real_matrix(rx, new double[,]{{1,0},{1,1}}, 0.05);
                    x = new double[]{-2,0};
                    k = alglib.kdtreequeryknn(kdt, x, 1, true);
                    _TestResult = _TestResult && doc_test_int(k, 1);
                    alglib.kdtreequeryresultsx(kdt, ref rx);
                    _TestResult = _TestResult && doc_test_real_matrix(rx, new double[,]{{0,0},{1,1}}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "nneighbor_t_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST nneighbor_d_2
            //      Serialization of KD-trees
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    double[,] a = new double[,]{{0,0},{0,1},{1,0},{1,1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    int nx = 2;
                    int ny = 0;
                    int normtype = 2;
                    alglib.kdtree kdt0;
                    alglib.kdtree kdt1;
                    string s;
                    double[] x;
                    double[,] r0 = new double[0,0];
                    double[,] r1 = new double[0,0];

                    //
                    // Build tree and serialize it
                    //
                    alglib.kdtreebuild(a, nx, ny, normtype, out kdt0);
                    alglib.kdtreeserialize(kdt0, out s);
                    alglib.kdtreeunserialize(s, out kdt1);

                    //
                    // Compare results from KNN queries
                    //
                    x = new double[]{-1,0};
                    alglib.kdtreequeryknn(kdt0, x, 1);
                    alglib.kdtreequeryresultsx(kdt0, ref r0);
                    alglib.kdtreequeryknn(kdt1, x, 1);
                    alglib.kdtreequeryresultsx(kdt1, ref r1);
                    _TestResult = _TestResult && doc_test_real_matrix(r0, new double[,]{{0,0}}, 0.05);
                    _TestResult = _TestResult && doc_test_real_matrix(r1, new double[,]{{0,0}}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "nneighbor_d_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST basestat_d_base
            //      Basic functionality (moments, adev, median, percentile)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double mean;
                    double variance;
                    double skewness;
                    double kurtosis;
                    double adev;
                    double p;
                    double v;

                    //
                    // Here we demonstrate calculation of sample moments
                    // (mean, variance, skewness, kurtosis)
                    //
                    alglib.samplemoments(x, out mean, out variance, out skewness, out kurtosis);
                    _TestResult = _TestResult && doc_test_real(mean, 28.5, 0.01);
                    _TestResult = _TestResult && doc_test_real(variance, 801.1667, 0.01);
                    _TestResult = _TestResult && doc_test_real(skewness, 0.5751, 0.01);
                    _TestResult = _TestResult && doc_test_real(kurtosis, -1.2666, 0.01);

                    //
                    // Average deviation
                    //
                    alglib.sampleadev(x, out adev);
                    _TestResult = _TestResult && doc_test_real(adev, 23.2, 0.01);

                    //
                    // Median and percentile
                    //
                    alglib.samplemedian(x, out v);
                    _TestResult = _TestResult && doc_test_real(v, 20.5, 0.01);
                    p = 0.5;
                    if( _spoil_scenario==3 )
                        p = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        p = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        p = (double)System.Double.NegativeInfinity;
                    alglib.samplepercentile(x, p, out v);
                    _TestResult = _TestResult && doc_test_real(v, 20.5, 0.01);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "basestat_d_base");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST basestat_d_c2
            //      Correlation (covariance) between two random variables
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<10; _spoil_scenario++)
            {
                try
                {
                    //
                    // We have two samples - x and y, and want to measure dependency between them
                    //
                    double[] x = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double v;

                    //
                    // Three dependency measures are calculated:
                    // * covariation
                    // * Pearson correlation
                    // * Spearman rank correlation
                    //
                    v = alglib.cov2(x, y);
                    _TestResult = _TestResult && doc_test_real(v, 82.5, 0.001);
                    v = alglib.pearsoncorr2(x, y);
                    _TestResult = _TestResult && doc_test_real(v, 0.9627, 0.001);
                    v = alglib.spearmancorr2(x, y);
                    _TestResult = _TestResult && doc_test_real(v, 1.000, 0.001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "basestat_d_c2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST basestat_d_cm
            //      Correlation (covariance) between components of random vector
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    //
                    // X is a sample matrix:
                    // * I-th row corresponds to I-th observation
                    // * J-th column corresponds to J-th variable
                    //
                    double[,] x = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double[,] c;

                    //
                    // Three dependency measures are calculated:
                    // * covariation
                    // * Pearson correlation
                    // * Spearman rank correlation
                    //
                    // Result is stored into C, with C[i,j] equal to correlation
                    // (covariance) between I-th and J-th variables of X.
                    //
                    alglib.covm(x, out c);
                    _TestResult = _TestResult && doc_test_real_matrix(c, new double[,]{{1.80,0.60,-1.40},{0.60,0.70,-0.80},{-1.40,-0.80,14.70}}, 0.01);
                    alglib.pearsoncorrm(x, out c);
                    _TestResult = _TestResult && doc_test_real_matrix(c, new double[,]{{1.000,0.535,-0.272},{0.535,1.000,-0.249},{-0.272,-0.249,1.000}}, 0.01);
                    alglib.spearmancorrm(x, out c);
                    _TestResult = _TestResult && doc_test_real_matrix(c, new double[,]{{1.000,0.556,-0.306},{0.556,1.000,-0.750},{-0.306,-0.750,1.000}}, 0.01);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "basestat_d_cm");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST basestat_d_cm2
            //      Correlation (covariance) between two random vectors
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    //
                    // X and Y are sample matrices:
                    // * I-th row corresponds to I-th observation
                    // * J-th column corresponds to J-th variable
                    //
                    double[,] x = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double[,] y = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double[,] c;

                    //
                    // Three dependency measures are calculated:
                    // * covariation
                    // * Pearson correlation
                    // * Spearman rank correlation
                    //
                    // Result is stored into C, with C[i,j] equal to correlation
                    // (covariance) between I-th variable of X and J-th variable of Y.
                    //
                    alglib.covm2(x, y, out c);
                    _TestResult = _TestResult && doc_test_real_matrix(c, new double[,]{{4.100,-3.250},{2.450,-1.500},{13.450,-5.750}}, 0.01);
                    alglib.pearsoncorrm2(x, y, out c);
                    _TestResult = _TestResult && doc_test_real_matrix(c, new double[,]{{0.519,-0.699},{0.497,-0.518},{0.596,-0.433}}, 0.01);
                    alglib.spearmancorrm2(x, y, out c);
                    _TestResult = _TestResult && doc_test_real_matrix(c, new double[,]{{0.541,-0.649},{0.216,-0.433},{0.433,-0.135}}, 0.01);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "basestat_d_cm2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST basestat_t_base
            //      Tests ability to detect errors in inputs
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<34; _spoil_scenario++)
            {
                try
                {
                    double mean;
                    double variance;
                    double skewness;
                    double kurtosis;
                    double adev;
                    double p;
                    double v;

                    //
                    // first, we test short form of functions
                    //
                    double[] x1 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x1, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x1, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x1, (double)System.Double.NegativeInfinity);
                    alglib.samplemoments(x1, out mean, out variance, out skewness, out kurtosis);
                    double[] x2 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==3 )
                        spoil_vector_by_value(ref x2, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref x2, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref x2, (double)System.Double.NegativeInfinity);
                    alglib.sampleadev(x2, out adev);
                    double[] x3 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref x3, (double)System.Double.NaN);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref x3, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_value(ref x3, (double)System.Double.NegativeInfinity);
                    alglib.samplemedian(x3, out v);
                    double[] x4 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref x4, (double)System.Double.NaN);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref x4, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref x4, (double)System.Double.NegativeInfinity);
                    p = 0.5;
                    if( _spoil_scenario==12 )
                        p = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        p = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        p = (double)System.Double.NegativeInfinity;
                    alglib.samplepercentile(x4, p, out v);

                    //
                    // and then we test full form
                    //
                    double[] x5 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref x5, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref x5, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref x5, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==18 )
                        spoil_vector_by_deleting_element(ref x5);
                    alglib.samplemoments(x5, 10, out mean, out variance, out skewness, out kurtosis);
                    double[] x6 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==19 )
                        spoil_vector_by_value(ref x6, (double)System.Double.NaN);
                    if( _spoil_scenario==20 )
                        spoil_vector_by_value(ref x6, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==21 )
                        spoil_vector_by_value(ref x6, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==22 )
                        spoil_vector_by_deleting_element(ref x6);
                    alglib.sampleadev(x6, 10, out adev);
                    double[] x7 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==23 )
                        spoil_vector_by_value(ref x7, (double)System.Double.NaN);
                    if( _spoil_scenario==24 )
                        spoil_vector_by_value(ref x7, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==25 )
                        spoil_vector_by_value(ref x7, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==26 )
                        spoil_vector_by_deleting_element(ref x7);
                    alglib.samplemedian(x7, 10, out v);
                    double[] x8 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==27 )
                        spoil_vector_by_value(ref x8, (double)System.Double.NaN);
                    if( _spoil_scenario==28 )
                        spoil_vector_by_value(ref x8, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==29 )
                        spoil_vector_by_value(ref x8, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==30 )
                        spoil_vector_by_deleting_element(ref x8);
                    p = 0.5;
                    if( _spoil_scenario==31 )
                        p = (double)System.Double.NaN;
                    if( _spoil_scenario==32 )
                        p = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==33 )
                        p = (double)System.Double.NegativeInfinity;
                    alglib.samplepercentile(x8, 10, p, out v);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "basestat_t_base");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST basestat_t_covcorr
            //      Tests ability to detect errors in inputs
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<126; _spoil_scenario++)
            {
                try
                {
                    double v;
                    double[,] c;

                    //
                    // 2-sample short-form cov/corr are tested
                    //
                    double[] x1 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x1, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x1, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x1, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x1);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x1);
                    double[] y1 = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y1, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y1, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y1, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y1);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y1);
                    v = alglib.cov2(x1, y1);
                    double[] x2 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref x2, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref x2, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref x2, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==13 )
                        spoil_vector_by_adding_element(ref x2);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_deleting_element(ref x2);
                    double[] y2 = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref y2, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref y2, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref y2, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==18 )
                        spoil_vector_by_adding_element(ref y2);
                    if( _spoil_scenario==19 )
                        spoil_vector_by_deleting_element(ref y2);
                    v = alglib.pearsoncorr2(x2, y2);
                    double[] x3 = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==20 )
                        spoil_vector_by_value(ref x3, (double)System.Double.NaN);
                    if( _spoil_scenario==21 )
                        spoil_vector_by_value(ref x3, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==22 )
                        spoil_vector_by_value(ref x3, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==23 )
                        spoil_vector_by_adding_element(ref x3);
                    if( _spoil_scenario==24 )
                        spoil_vector_by_deleting_element(ref x3);
                    double[] y3 = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==25 )
                        spoil_vector_by_value(ref y3, (double)System.Double.NaN);
                    if( _spoil_scenario==26 )
                        spoil_vector_by_value(ref y3, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==27 )
                        spoil_vector_by_value(ref y3, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==28 )
                        spoil_vector_by_adding_element(ref y3);
                    if( _spoil_scenario==29 )
                        spoil_vector_by_deleting_element(ref y3);
                    v = alglib.spearmancorr2(x3, y3);

                    //
                    // 2-sample full-form cov/corr are tested
                    //
                    double[] x1a = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==30 )
                        spoil_vector_by_value(ref x1a, (double)System.Double.NaN);
                    if( _spoil_scenario==31 )
                        spoil_vector_by_value(ref x1a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==32 )
                        spoil_vector_by_value(ref x1a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==33 )
                        spoil_vector_by_deleting_element(ref x1a);
                    double[] y1a = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==34 )
                        spoil_vector_by_value(ref y1a, (double)System.Double.NaN);
                    if( _spoil_scenario==35 )
                        spoil_vector_by_value(ref y1a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==36 )
                        spoil_vector_by_value(ref y1a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==37 )
                        spoil_vector_by_deleting_element(ref y1a);
                    v = alglib.cov2(x1a, y1a, 10);
                    double[] x2a = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==38 )
                        spoil_vector_by_value(ref x2a, (double)System.Double.NaN);
                    if( _spoil_scenario==39 )
                        spoil_vector_by_value(ref x2a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==40 )
                        spoil_vector_by_value(ref x2a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==41 )
                        spoil_vector_by_deleting_element(ref x2a);
                    double[] y2a = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==42 )
                        spoil_vector_by_value(ref y2a, (double)System.Double.NaN);
                    if( _spoil_scenario==43 )
                        spoil_vector_by_value(ref y2a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==44 )
                        spoil_vector_by_value(ref y2a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==45 )
                        spoil_vector_by_deleting_element(ref y2a);
                    v = alglib.pearsoncorr2(x2a, y2a, 10);
                    double[] x3a = new double[]{0,1,4,9,16,25,36,49,64,81};
                    if( _spoil_scenario==46 )
                        spoil_vector_by_value(ref x3a, (double)System.Double.NaN);
                    if( _spoil_scenario==47 )
                        spoil_vector_by_value(ref x3a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==48 )
                        spoil_vector_by_value(ref x3a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==49 )
                        spoil_vector_by_deleting_element(ref x3a);
                    double[] y3a = new double[]{0,1,2,3,4,5,6,7,8,9};
                    if( _spoil_scenario==50 )
                        spoil_vector_by_value(ref y3a, (double)System.Double.NaN);
                    if( _spoil_scenario==51 )
                        spoil_vector_by_value(ref y3a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==52 )
                        spoil_vector_by_value(ref y3a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==53 )
                        spoil_vector_by_deleting_element(ref y3a);
                    v = alglib.spearmancorr2(x3a, y3a, 10);

                    //
                    // vector short-form cov/corr are tested.
                    //
                    double[,] x4 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==54 )
                        spoil_matrix_by_value(ref x4, (double)System.Double.NaN);
                    if( _spoil_scenario==55 )
                        spoil_matrix_by_value(ref x4, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==56 )
                        spoil_matrix_by_value(ref x4, (double)System.Double.NegativeInfinity);
                    alglib.covm(x4, out c);
                    double[,] x5 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==57 )
                        spoil_matrix_by_value(ref x5, (double)System.Double.NaN);
                    if( _spoil_scenario==58 )
                        spoil_matrix_by_value(ref x5, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==59 )
                        spoil_matrix_by_value(ref x5, (double)System.Double.NegativeInfinity);
                    alglib.pearsoncorrm(x5, out c);
                    double[,] x6 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==60 )
                        spoil_matrix_by_value(ref x6, (double)System.Double.NaN);
                    if( _spoil_scenario==61 )
                        spoil_matrix_by_value(ref x6, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==62 )
                        spoil_matrix_by_value(ref x6, (double)System.Double.NegativeInfinity);
                    alglib.spearmancorrm(x6, out c);

                    //
                    // vector full-form cov/corr are tested.
                    //
                    double[,] x7 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==63 )
                        spoil_matrix_by_value(ref x7, (double)System.Double.NaN);
                    if( _spoil_scenario==64 )
                        spoil_matrix_by_value(ref x7, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==65 )
                        spoil_matrix_by_value(ref x7, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==66 )
                        spoil_matrix_by_deleting_row(ref x7);
                    if( _spoil_scenario==67 )
                        spoil_matrix_by_deleting_col(ref x7);
                    alglib.covm(x7, 5, 3, out c);
                    double[,] x8 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==68 )
                        spoil_matrix_by_value(ref x8, (double)System.Double.NaN);
                    if( _spoil_scenario==69 )
                        spoil_matrix_by_value(ref x8, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==70 )
                        spoil_matrix_by_value(ref x8, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==71 )
                        spoil_matrix_by_deleting_row(ref x8);
                    if( _spoil_scenario==72 )
                        spoil_matrix_by_deleting_col(ref x8);
                    alglib.pearsoncorrm(x8, 5, 3, out c);
                    double[,] x9 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==73 )
                        spoil_matrix_by_value(ref x9, (double)System.Double.NaN);
                    if( _spoil_scenario==74 )
                        spoil_matrix_by_value(ref x9, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==75 )
                        spoil_matrix_by_value(ref x9, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==76 )
                        spoil_matrix_by_deleting_row(ref x9);
                    if( _spoil_scenario==77 )
                        spoil_matrix_by_deleting_col(ref x9);
                    alglib.spearmancorrm(x9, 5, 3, out c);

                    //
                    // cross-vector short-form cov/corr are tested.
                    //
                    double[,] x10 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==78 )
                        spoil_matrix_by_value(ref x10, (double)System.Double.NaN);
                    if( _spoil_scenario==79 )
                        spoil_matrix_by_value(ref x10, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==80 )
                        spoil_matrix_by_value(ref x10, (double)System.Double.NegativeInfinity);
                    double[,] y10 = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==81 )
                        spoil_matrix_by_value(ref y10, (double)System.Double.NaN);
                    if( _spoil_scenario==82 )
                        spoil_matrix_by_value(ref y10, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==83 )
                        spoil_matrix_by_value(ref y10, (double)System.Double.NegativeInfinity);
                    alglib.covm2(x10, y10, out c);
                    double[,] x11 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==84 )
                        spoil_matrix_by_value(ref x11, (double)System.Double.NaN);
                    if( _spoil_scenario==85 )
                        spoil_matrix_by_value(ref x11, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==86 )
                        spoil_matrix_by_value(ref x11, (double)System.Double.NegativeInfinity);
                    double[,] y11 = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==87 )
                        spoil_matrix_by_value(ref y11, (double)System.Double.NaN);
                    if( _spoil_scenario==88 )
                        spoil_matrix_by_value(ref y11, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==89 )
                        spoil_matrix_by_value(ref y11, (double)System.Double.NegativeInfinity);
                    alglib.pearsoncorrm2(x11, y11, out c);
                    double[,] x12 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==90 )
                        spoil_matrix_by_value(ref x12, (double)System.Double.NaN);
                    if( _spoil_scenario==91 )
                        spoil_matrix_by_value(ref x12, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==92 )
                        spoil_matrix_by_value(ref x12, (double)System.Double.NegativeInfinity);
                    double[,] y12 = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==93 )
                        spoil_matrix_by_value(ref y12, (double)System.Double.NaN);
                    if( _spoil_scenario==94 )
                        spoil_matrix_by_value(ref y12, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==95 )
                        spoil_matrix_by_value(ref y12, (double)System.Double.NegativeInfinity);
                    alglib.spearmancorrm2(x12, y12, out c);

                    //
                    // cross-vector full-form cov/corr are tested.
                    //
                    double[,] x13 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==96 )
                        spoil_matrix_by_value(ref x13, (double)System.Double.NaN);
                    if( _spoil_scenario==97 )
                        spoil_matrix_by_value(ref x13, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==98 )
                        spoil_matrix_by_value(ref x13, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==99 )
                        spoil_matrix_by_deleting_row(ref x13);
                    if( _spoil_scenario==100 )
                        spoil_matrix_by_deleting_col(ref x13);
                    double[,] y13 = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==101 )
                        spoil_matrix_by_value(ref y13, (double)System.Double.NaN);
                    if( _spoil_scenario==102 )
                        spoil_matrix_by_value(ref y13, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==103 )
                        spoil_matrix_by_value(ref y13, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==104 )
                        spoil_matrix_by_deleting_row(ref y13);
                    if( _spoil_scenario==105 )
                        spoil_matrix_by_deleting_col(ref y13);
                    alglib.covm2(x13, y13, 5, 3, 2, out c);
                    double[,] x14 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==106 )
                        spoil_matrix_by_value(ref x14, (double)System.Double.NaN);
                    if( _spoil_scenario==107 )
                        spoil_matrix_by_value(ref x14, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==108 )
                        spoil_matrix_by_value(ref x14, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==109 )
                        spoil_matrix_by_deleting_row(ref x14);
                    if( _spoil_scenario==110 )
                        spoil_matrix_by_deleting_col(ref x14);
                    double[,] y14 = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==111 )
                        spoil_matrix_by_value(ref y14, (double)System.Double.NaN);
                    if( _spoil_scenario==112 )
                        spoil_matrix_by_value(ref y14, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==113 )
                        spoil_matrix_by_value(ref y14, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==114 )
                        spoil_matrix_by_deleting_row(ref y14);
                    if( _spoil_scenario==115 )
                        spoil_matrix_by_deleting_col(ref y14);
                    alglib.pearsoncorrm2(x14, y14, 5, 3, 2, out c);
                    double[,] x15 = new double[,]{{1,0,1},{1,1,0},{-1,1,0},{-2,-1,1},{-1,0,9}};
                    if( _spoil_scenario==116 )
                        spoil_matrix_by_value(ref x15, (double)System.Double.NaN);
                    if( _spoil_scenario==117 )
                        spoil_matrix_by_value(ref x15, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==118 )
                        spoil_matrix_by_value(ref x15, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==119 )
                        spoil_matrix_by_deleting_row(ref x15);
                    if( _spoil_scenario==120 )
                        spoil_matrix_by_deleting_col(ref x15);
                    double[,] y15 = new double[,]{{2,3},{2,1},{-1,6},{-9,9},{7,1}};
                    if( _spoil_scenario==121 )
                        spoil_matrix_by_value(ref y15, (double)System.Double.NaN);
                    if( _spoil_scenario==122 )
                        spoil_matrix_by_value(ref y15, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==123 )
                        spoil_matrix_by_value(ref y15, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==124 )
                        spoil_matrix_by_deleting_row(ref y15);
                    if( _spoil_scenario==125 )
                        spoil_matrix_by_deleting_col(ref y15);
                    alglib.spearmancorrm2(x15, y15, 5, 3, 2, out c);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "basestat_t_covcorr");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_d_r1
            //      Real matrix inverse
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    double[,] a = new double[,]{{1,-1},{1,1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref a);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref a);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref a);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref a);
                    int info;
                    alglib.matinvreport rep;
                    alglib.rmatrixinverse(ref a, out info, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_real_matrix(a, new double[,]{{0.5,0.5},{-0.5,0.5}}, 0.00005);
                    _TestResult = _TestResult && doc_test_real(rep.r1, 0.5, 0.00005);
                    _TestResult = _TestResult && doc_test_real(rep.rinf, 0.5, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_d_r1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_d_c1
            //      Complex matrix inverse
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    alglib.complex[,] a = new alglib.complex[,]{{new alglib.complex(0,1),-1},{new alglib.complex(0,1),1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref a);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref a);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref a);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref a);
                    int info;
                    alglib.matinvreport rep;
                    alglib.cmatrixinverse(ref a, out info, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_complex_matrix(a, new alglib.complex[,]{{new alglib.complex(0,-0.5),new alglib.complex(0,-0.5)},{-0.5,0.5}}, 0.00005);
                    _TestResult = _TestResult && doc_test_real(rep.r1, 0.5, 0.00005);
                    _TestResult = _TestResult && doc_test_real(rep.rinf, 0.5, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_d_c1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_d_spd1
            //      SPD matrix inverse
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    double[,] a = new double[,]{{2,1},{1,2}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref a);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref a);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref a);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref a);
                    int info;
                    alglib.matinvreport rep;
                    alglib.spdmatrixinverse(ref a, out info, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_real_matrix(a, new double[,]{{0.666666,-0.333333},{-0.333333,0.666666}}, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_d_spd1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_d_hpd1
            //      HPD matrix inverse
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    alglib.complex[,] a = new alglib.complex[,]{{2,1},{1,2}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref a);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref a);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref a);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref a);
                    int info;
                    alglib.matinvreport rep;
                    alglib.hpdmatrixinverse(ref a, out info, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_complex_matrix(a, new alglib.complex[,]{{0.666666,-0.333333},{-0.333333,0.666666}}, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_d_hpd1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_t_r1
            //      Real matrix inverse: singular matrix
            //
            _TestResult = true;
            try
            {
                double[,] a = new double[,]{{1,-1},{-2,2}};
                int info;
                alglib.matinvreport rep;
                alglib.rmatrixinverse(ref a, out info, out rep);
                _TestResult = _TestResult && doc_test_int(info, -3);
                _TestResult = _TestResult && doc_test_real(rep.r1, 0.0, 0.00005);
                _TestResult = _TestResult && doc_test_real(rep.rinf, 0.0, 0.00005);
            }
            catch(alglib.alglibexception)
            { _TestResult = false; }
            catch
            { throw; }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_t_r1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_t_c1
            //      Complex matrix inverse: singular matrix
            //
            _TestResult = true;
            try
            {
                alglib.complex[,] a = new alglib.complex[,]{{new alglib.complex(0,1),new alglib.complex(0,-1)},{-2,2}};
                int info;
                alglib.matinvreport rep;
                alglib.cmatrixinverse(ref a, out info, out rep);
                _TestResult = _TestResult && doc_test_int(info, -3);
                _TestResult = _TestResult && doc_test_real(rep.r1, 0.0, 0.00005);
                _TestResult = _TestResult && doc_test_real(rep.rinf, 0.0, 0.00005);
            }
            catch(alglib.alglibexception)
            { _TestResult = false; }
            catch
            { throw; }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_t_c1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_e_spd1
            //      Attempt to use SPD function on nonsymmetrix matrix
            //
            _TestResult = true;
            try
            {
                double[,] a = new double[,]{{1,0},{1,1}};
                int info;
                alglib.matinvreport rep;
                alglib.spdmatrixinverse(ref a, out info, out rep);
                _TestResult = false;
            }
            catch(alglib.alglibexception)
            {}
            catch
            { throw; }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_e_spd1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matinv_e_hpd1
            //      Attempt to use SPD function on nonsymmetrix matrix
            //
            _TestResult = true;
            try
            {
                alglib.complex[,] a = new alglib.complex[,]{{1,0},{1,1}};
                int info;
                alglib.matinvreport rep;
                alglib.hpdmatrixinverse(ref a, out info, out rep);
                _TestResult = false;
            }
            catch(alglib.alglibexception)
            {}
            catch
            { throw; }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matinv_e_hpd1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST mincg_d_1
            //      Nonlinear optimization by CG
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // with nonlinear conjugate gradient method.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.mincgstate state;
                    alglib.mincgreport rep;

                    alglib.mincgcreate(x, out state);
                    alglib.mincgsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.mincgoptimize(state, function1_grad, null, null);
                    alglib.mincgresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "mincg_d_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST mincg_d_2
            //      Nonlinear optimization with additional settings and restarts
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<18; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // with nonlinear conjugate gradient method.
                    //
                    // Several advanced techniques are demonstrated:
                    // * upper limit on step size
                    // * restart from new point
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    double stpmax = 0.1;
                    if( _spoil_scenario==12 )
                        stpmax = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        stpmax = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        stpmax = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.mincgstate state;
                    alglib.mincgreport rep;

                    // first run
                    alglib.mincgcreate(x, out state);
                    alglib.mincgsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.mincgsetstpmax(state, stpmax);
                    alglib.mincgoptimize(state, function1_grad, null, null);
                    alglib.mincgresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);

                    // second run - algorithm is restarted with mincgrestartfrom()
                    x = new double[]{10,10};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    alglib.mincgrestartfrom(state, x);
                    alglib.mincgoptimize(state, function1_grad, null, null);
                    alglib.mincgresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "mincg_d_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST mincg_numdiff
            //      Nonlinear optimization by CG with numerical differentiation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<15; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // using numerical differentiation to calculate gradient.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    double diffstep = 1.0e-6;
                    if( _spoil_scenario==12 )
                        diffstep = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        diffstep = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        diffstep = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.mincgstate state;
                    alglib.mincgreport rep;

                    alglib.mincgcreatef(x, diffstep, out state);
                    alglib.mincgsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.mincgoptimize(state, function1_func, null, null);
                    alglib.mincgresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "mincg_numdiff");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST mincg_ftrim
            //      Nonlinear optimization by CG, function with singularities
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x) = (1+x)^(-0.2) + (1-x)^(-0.3) + 1000*x.
                    // This function has singularities at the boundary of the [-1,+1], but technique called
                    // "function trimming" allows us to solve this optimization problem.
                    //
                    // See http://www.alglib.net/optimization/tipsandtricks.php#ftrimming for more information
                    // on this subject.
                    //
                    double[] x = new double[]{0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 1.0e-6;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.mincgstate state;
                    alglib.mincgreport rep;

                    alglib.mincgcreate(x, out state);
                    alglib.mincgsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.mincgoptimize(state, s1_grad, null, null);
                    alglib.mincgresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-0.99917305}, 0.000005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "mincg_ftrim");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minbleic_d_1
            //      Nonlinear optimization with bound constraints
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<22; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // subject to bound constraints -1<=x<=+1, -1<=y<=+1, using BLEIC optimizer.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double[] bndl = new double[]{-1,-1};
                    if( _spoil_scenario==3 )
                        spoil_vector_by_value(ref bndl, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref bndl);
                    double[] bndu = new double[]{+1,+1};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref bndu, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_deleting_element(ref bndu);
                    alglib.minbleicstate state;
                    alglib.minbleicreport rep;

                    //
                    // These variables define stopping conditions for the underlying CG algorithm.
                    // They should be stringent enough in order to guarantee overall stability
                    // of the outer iterations.
                    //
                    // We use very simple condition - |g|<=epsg
                    //
                    double epsg = 0.000001;
                    if( _spoil_scenario==7 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==8 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==10 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==11 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==12 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==13 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsx = (double)System.Double.NegativeInfinity;

                    //
                    // These variables define stopping conditions for the outer iterations:
                    // * epso controls convergence of outer iterations; algorithm will stop
                    //   when difference between solutions of subsequent unconstrained problems
                    //   will be less than 0.0001
                    // * epsi controls amount of infeasibility allowed in the final solution
                    //
                    double epso = 0.00001;
                    if( _spoil_scenario==16 )
                        epso = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        epso = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        epso = (double)System.Double.NegativeInfinity;
                    double epsi = 0.00001;
                    if( _spoil_scenario==19 )
                        epsi = (double)System.Double.NaN;
                    if( _spoil_scenario==20 )
                        epsi = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==21 )
                        epsi = (double)System.Double.NegativeInfinity;

                    //
                    // Now we are ready to actually optimize something:
                    // * first we create optimizer
                    // * we add boundary constraints
                    // * we tune stopping conditions
                    // * and, finally, optimize and obtain results...
                    //
                    alglib.minbleiccreate(x, out state);
                    alglib.minbleicsetbc(state, bndl, bndu);
                    alglib.minbleicsetinnercond(state, epsg, epsf, epsx);
                    alglib.minbleicsetoutercond(state, epso, epsi);
                    alglib.minbleicoptimize(state, function1_grad, null, null);
                    alglib.minbleicresults(state, out x, out rep);

                    //
                    // ...and evaluate these results
                    //
                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-1,1}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minbleic_d_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minbleic_d_2
            //      Nonlinear optimization with linear inequality constraints
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<24; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // subject to inequality constraints:
                    // * x>=2 (posed as general linear constraint),
                    // * x+y>=6
                    // using BLEIC optimizer.
                    //
                    double[] x = new double[]{5,5};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double[,] c = new double[,]{{1,0,2},{1,1,6}};
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_value(ref c, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_value(ref c, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_value(ref c, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_row(ref c);
                    if( _spoil_scenario==7 )
                        spoil_matrix_by_deleting_col(ref c);
                    int[] ct = new int[]{1,1};
                    if( _spoil_scenario==8 )
                        spoil_vector_by_deleting_element(ref ct);
                    alglib.minbleicstate state;
                    alglib.minbleicreport rep;

                    //
                    // These variables define stopping conditions for the underlying CG algorithm.
                    // They should be stringent enough in order to guarantee overall stability
                    // of the outer iterations.
                    //
                    // We use very simple condition - |g|<=epsg
                    //
                    double epsg = 0.000001;
                    if( _spoil_scenario==9 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==12 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==15 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==16 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==17 )
                        epsx = (double)System.Double.NegativeInfinity;

                    //
                    // These variables define stopping conditions for the outer iterations:
                    // * epso controls convergence of outer iterations; algorithm will stop
                    //   when difference between solutions of subsequent unconstrained problems
                    //   will be less than 0.0001
                    // * epsi controls amount of infeasibility allowed in the final solution
                    //
                    double epso = 0.00001;
                    if( _spoil_scenario==18 )
                        epso = (double)System.Double.NaN;
                    if( _spoil_scenario==19 )
                        epso = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==20 )
                        epso = (double)System.Double.NegativeInfinity;
                    double epsi = 0.00001;
                    if( _spoil_scenario==21 )
                        epsi = (double)System.Double.NaN;
                    if( _spoil_scenario==22 )
                        epsi = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==23 )
                        epsi = (double)System.Double.NegativeInfinity;

                    //
                    // Now we are ready to actually optimize something:
                    // * first we create optimizer
                    // * we add linear constraints
                    // * we tune stopping conditions
                    // * and, finally, optimize and obtain results...
                    //
                    alglib.minbleiccreate(x, out state);
                    alglib.minbleicsetlc(state, c, ct);
                    alglib.minbleicsetinnercond(state, epsg, epsf, epsx);
                    alglib.minbleicsetoutercond(state, epso, epsi);
                    alglib.minbleicoptimize(state, function1_grad, null, null);
                    alglib.minbleicresults(state, out x, out rep);

                    //
                    // ...and evaluate these results
                    //
                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{2,4}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minbleic_d_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minbleic_numdiff
            //      Nonlinear optimization with bound constraints and numerical differentiation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<25; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // subject to bound constraints -1<=x<=+1, -1<=y<=+1, using BLEIC optimizer.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double[] bndl = new double[]{-1,-1};
                    if( _spoil_scenario==3 )
                        spoil_vector_by_value(ref bndl, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref bndl);
                    double[] bndu = new double[]{+1,+1};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref bndu, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_deleting_element(ref bndu);
                    alglib.minbleicstate state;
                    alglib.minbleicreport rep;

                    //
                    // These variables define stopping conditions for the underlying CG algorithm.
                    // They should be stringent enough in order to guarantee overall stability
                    // of the outer iterations.
                    //
                    // We use very simple condition - |g|<=epsg
                    //
                    double epsg = 0.000001;
                    if( _spoil_scenario==7 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==8 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==10 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==11 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==12 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==13 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsx = (double)System.Double.NegativeInfinity;

                    //
                    // These variables define stopping conditions for the outer iterations:
                    // * epso controls convergence of outer iterations; algorithm will stop
                    //   when difference between solutions of subsequent unconstrained problems
                    //   will be less than 0.0001
                    // * epsi controls amount of infeasibility allowed in the final solution
                    //
                    double epso = 0.00001;
                    if( _spoil_scenario==16 )
                        epso = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        epso = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        epso = (double)System.Double.NegativeInfinity;
                    double epsi = 0.00001;
                    if( _spoil_scenario==19 )
                        epsi = (double)System.Double.NaN;
                    if( _spoil_scenario==20 )
                        epsi = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==21 )
                        epsi = (double)System.Double.NegativeInfinity;

                    //
                    // This variable contains differentiation step
                    //
                    double diffstep = 1.0e-6;
                    if( _spoil_scenario==22 )
                        diffstep = (double)System.Double.NaN;
                    if( _spoil_scenario==23 )
                        diffstep = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==24 )
                        diffstep = (double)System.Double.NegativeInfinity;

                    //
                    // Now we are ready to actually optimize something:
                    // * first we create optimizer
                    // * we add boundary constraints
                    // * we tune stopping conditions
                    // * and, finally, optimize and obtain results...
                    //
                    alglib.minbleiccreatef(x, diffstep, out state);
                    alglib.minbleicsetbc(state, bndl, bndu);
                    alglib.minbleicsetinnercond(state, epsg, epsf, epsx);
                    alglib.minbleicsetoutercond(state, epso, epsi);
                    alglib.minbleicoptimize(state, function1_func, null, null);
                    alglib.minbleicresults(state, out x, out rep);

                    //
                    // ...and evaluate these results
                    //
                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-1,1}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minbleic_numdiff");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minbleic_ftrim
            //      Nonlinear optimization by BLEIC, function with singularities
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<18; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x) = (1+x)^(-0.2) + (1-x)^(-0.3) + 1000*x.
                    //
                    // This function is undefined outside of (-1,+1) and has singularities at x=-1 and x=+1.
                    // Special technique called "function trimming" allows us to solve this optimization problem 
                    // - without using boundary constraints!
                    //
                    // See http://www.alglib.net/optimization/tipsandtricks.php#ftrimming for more information
                    // on this subject.
                    //
                    double[] x = new double[]{0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 1.0e-6;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    double epso = 1.0e-6;
                    if( _spoil_scenario==12 )
                        epso = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        epso = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        epso = (double)System.Double.NegativeInfinity;
                    double epsi = 1.0e-6;
                    if( _spoil_scenario==15 )
                        epsi = (double)System.Double.NaN;
                    if( _spoil_scenario==16 )
                        epsi = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==17 )
                        epsi = (double)System.Double.NegativeInfinity;
                    alglib.minbleicstate state;
                    alglib.minbleicreport rep;

                    alglib.minbleiccreate(x, out state);
                    alglib.minbleicsetinnercond(state, epsg, epsf, epsx);
                    alglib.minbleicsetoutercond(state, epso, epsi);
                    alglib.minbleicoptimize(state, s1_grad, null, null);
                    alglib.minbleicresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-0.99917305}, 0.000005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minbleic_ftrim");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST mcpd_simple1
            //      Simple unconstrained MCPD model (no entry/exit states)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    //
                    // The very simple MCPD example
                    //
                    // We have a loan portfolio. Our loans can be in one of two states:
                    // * normal loans ("good" ones)
                    // * past due loans ("bad" ones)
                    //
                    // We assume that:
                    // * loans can transition from any state to any other state. In 
                    //   particular, past due loan can become "good" one at any moment 
                    //   with same (fixed) probability. Not realistic, but it is toy example :)
                    // * portfolio size does not change over time
                    //
                    // Thus, we have following model
                    //     state_new = P*state_old
                    // where
                    //         ( p00  p01 )
                    //     P = (          )
                    //         ( p10  p11 )
                    //
                    // We want to model transitions between these two states using MCPD
                    // approach (Markov Chains for Proportional/Population Data), i.e.
                    // to restore hidden transition matrix P using actual portfolio data.
                    // We have:
                    // * poportional data, i.e. proportion of loans in the normal and past 
                    //   due states (not portfolio size measured in some currency, although 
                    //   it is possible to work with population data too)
                    // * two tracks, i.e. two sequences which describe portfolio
                    //   evolution from two different starting states: [1,0] (all loans 
                    //   are "good") and [0.8,0.2] (only 80% of portfolio is in the "good"
                    //   state)
                    //
                    alglib.mcpdstate s;
                    alglib.mcpdreport rep;
                    double[,] p;
                    double[,] track0 = new double[,]{{1.00000,0.00000},{0.95000,0.05000},{0.92750,0.07250},{0.91738,0.08263},{0.91282,0.08718}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref track0, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref track0, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref track0, (double)System.Double.NegativeInfinity);
                    double[,] track1 = new double[,]{{0.80000,0.20000},{0.86000,0.14000},{0.88700,0.11300},{0.89915,0.10085}};
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_value(ref track1, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_value(ref track1, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_value(ref track1, (double)System.Double.NegativeInfinity);

                    alglib.mcpdcreate(2, out s);
                    alglib.mcpdaddtrack(s, track0);
                    alglib.mcpdaddtrack(s, track1);
                    alglib.mcpdsolve(s);
                    alglib.mcpdresults(s, out p, out rep);

                    //
                    // Hidden matrix P is equal to
                    //         ( 0.95  0.50 )
                    //         (            )
                    //         ( 0.05  0.50 )
                    // which means that "good" loans can become "bad" with 5% probability, 
                    // while "bad" loans will return to good state with 50% probability.
                    //
                    _TestResult = _TestResult && doc_test_real_matrix(p, new double[,]{{0.95,0.50},{0.05,0.50}}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "mcpd_simple1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST mcpd_simple2
            //      Simple MCPD model (no entry/exit states) with equality constraints
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    //
                    // Simple MCPD example
                    //
                    // We have a loan portfolio. Our loans can be in one of three states:
                    // * normal loans
                    // * past due loans
                    // * charged off loans
                    //
                    // We assume that:
                    // * normal loan can stay normal or become past due (but not charged off)
                    // * past due loan can stay past due, become normal or charged off
                    // * charged off loan will stay charged off for the rest of eternity
                    // * portfolio size does not change over time
                    // Not realistic, but it is toy example :)
                    //
                    // Thus, we have following model
                    //     state_new = P*state_old
                    // where
                    //         ( p00  p01    )
                    //     P = ( p10  p11    )
                    //         (      p21  1 )
                    // i.e. four elements of P are known a priori.
                    //
                    // Although it is possible (given enough data) to In order to enforce 
                    // this property we set equality constraints on these elements.
                    //
                    // We want to model transitions between these two states using MCPD
                    // approach (Markov Chains for Proportional/Population Data), i.e.
                    // to restore hidden transition matrix P using actual portfolio data.
                    // We have:
                    // * poportional data, i.e. proportion of loans in the current and past 
                    //   due states (not portfolio size measured in some currency, although 
                    //   it is possible to work with population data too)
                    // * two tracks, i.e. two sequences which describe portfolio
                    //   evolution from two different starting states: [1,0,0] (all loans 
                    //   are "good") and [0.8,0.2,0.0] (only 80% of portfolio is in the "good"
                    //   state)
                    //
                    alglib.mcpdstate s;
                    alglib.mcpdreport rep;
                    double[,] p;
                    double[,] track0 = new double[,]{{1.000000,0.000000,0.000000},{0.950000,0.050000,0.000000},{0.927500,0.060000,0.012500},{0.911125,0.061375,0.027500},{0.896256,0.060900,0.042844}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref track0, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref track0, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref track0, (double)System.Double.NegativeInfinity);
                    double[,] track1 = new double[,]{{0.800000,0.200000,0.000000},{0.860000,0.090000,0.050000},{0.862000,0.065500,0.072500},{0.851650,0.059475,0.088875},{0.838805,0.057451,0.103744}};
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_value(ref track1, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_value(ref track1, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_value(ref track1, (double)System.Double.NegativeInfinity);

                    alglib.mcpdcreate(3, out s);
                    alglib.mcpdaddtrack(s, track0);
                    alglib.mcpdaddtrack(s, track1);
                    alglib.mcpdaddec(s, 0, 2, 0.0);
                    alglib.mcpdaddec(s, 1, 2, 0.0);
                    alglib.mcpdaddec(s, 2, 2, 1.0);
                    alglib.mcpdaddec(s, 2, 0, 0.0);
                    alglib.mcpdsolve(s);
                    alglib.mcpdresults(s, out p, out rep);

                    //
                    // Hidden matrix P is equal to
                    //         ( 0.95 0.50      )
                    //         ( 0.05 0.25      )
                    //         (      0.25 1.00 ) 
                    // which means that "good" loans can become past due with 5% probability, 
                    // while past due loans will become charged off with 25% probability or
                    // return back to normal state with 50% probability.
                    //
                    _TestResult = _TestResult && doc_test_real_matrix(p, new double[,]{{0.95,0.50,0.00},{0.05,0.25,0.00},{0.00,0.25,1.00}}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "mcpd_simple2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlbfgs_d_1
            //      Nonlinear optimization by L-BFGS
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // using LBFGS method.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlbfgsstate state;
                    alglib.minlbfgsreport rep;

                    alglib.minlbfgscreate(1, x, out state);
                    alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlbfgsoptimize(state, function1_grad, null, null);
                    alglib.minlbfgsresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlbfgs_d_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlbfgs_d_2
            //      Nonlinear optimization with additional settings and restarts
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<18; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // using LBFGS method.
                    //
                    // Several advanced techniques are demonstrated:
                    // * upper limit on step size
                    // * restart from new point
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    double stpmax = 0.1;
                    if( _spoil_scenario==12 )
                        stpmax = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        stpmax = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        stpmax = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlbfgsstate state;
                    alglib.minlbfgsreport rep;

                    // first run
                    alglib.minlbfgscreate(1, x, out state);
                    alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlbfgssetstpmax(state, stpmax);
                    alglib.minlbfgsoptimize(state, function1_grad, null, null);
                    alglib.minlbfgsresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);

                    // second run - algorithm is restarted
                    x = new double[]{10,10};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    alglib.minlbfgsrestartfrom(state, x);
                    alglib.minlbfgsoptimize(state, function1_grad, null, null);
                    alglib.minlbfgsresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlbfgs_d_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlbfgs_numdiff
            //      Nonlinear optimization by L-BFGS with numerical differentiation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<15; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x,y) = 100*(x+3)^4+(y-3)^4
                    // using numerical differentiation to calculate gradient.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    double diffstep = 1.0e-6;
                    if( _spoil_scenario==12 )
                        diffstep = (double)System.Double.NaN;
                    if( _spoil_scenario==13 )
                        diffstep = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==14 )
                        diffstep = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlbfgsstate state;
                    alglib.minlbfgsreport rep;

                    alglib.minlbfgscreatef(1, x, diffstep, out state);
                    alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlbfgsoptimize(state, function1_func, null, null);
                    alglib.minlbfgsresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlbfgs_numdiff");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlbfgs_ftrim
            //      Nonlinear optimization by LBFGS, function with singularities
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of f(x) = (1+x)^(-0.2) + (1-x)^(-0.3) + 1000*x.
                    // This function has singularities at the boundary of the [-1,+1], but technique called
                    // "function trimming" allows us to solve this optimization problem.
                    //
                    // See http://www.alglib.net/optimization/tipsandtricks.php#ftrimming for more information
                    // on this subject.
                    //
                    double[] x = new double[]{0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 1.0e-6;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlbfgsstate state;
                    alglib.minlbfgsreport rep;

                    alglib.minlbfgscreate(1, x, out state);
                    alglib.minlbfgssetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlbfgsoptimize(state, s1_grad, null, null);
                    alglib.minlbfgsresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-0.99917305}, 0.000005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlbfgs_ftrim");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST odesolver_d1
            //      Solving y'=-y with ODE solver
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<13; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{1};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] x = new double[]{0,1,2,3};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double eps = 0.00001;
                    if( _spoil_scenario==7 )
                        eps = (double)System.Double.NaN;
                    if( _spoil_scenario==8 )
                        eps = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        eps = (double)System.Double.NegativeInfinity;
                    double h = 0;
                    if( _spoil_scenario==10 )
                        h = (double)System.Double.NaN;
                    if( _spoil_scenario==11 )
                        h = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==12 )
                        h = (double)System.Double.NegativeInfinity;
                    alglib.odesolverstate s;
                    int m;
                    double[] xtbl;
                    double[,] ytbl;
                    alglib.odesolverreport rep;
                    alglib.odesolverrkck(y, x, eps, h, out s);
                    alglib.odesolversolve(s, ode_function_1_diff, null);
                    alglib.odesolverresults(s, out m, out xtbl, out ytbl, out rep);
                    _TestResult = _TestResult && doc_test_int(m, 4);
                    _TestResult = _TestResult && doc_test_real_vector(xtbl, new double[]{0,1,2,3}, 0.005);
                    _TestResult = _TestResult && doc_test_real_matrix(ytbl, new double[,]{{1},{0.367},{0.135},{0.050}}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "odesolver_d1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST fft_complex_d1
            //      Complex FFT: simple example
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    //
                    // first we demonstrate forward FFT:
                    // [1i,1i,1i,1i] is converted to [4i, 0, 0, 0]
                    //
                    alglib.complex[] z = new alglib.complex[]{new alglib.complex(0,1),new alglib.complex(0,1),new alglib.complex(0,1),new alglib.complex(0,1)};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.NegativeInfinity);
                    alglib.fftc1d(ref z);
                    _TestResult = _TestResult && doc_test_complex_vector(z, new alglib.complex[]{new alglib.complex(0,4),0,0,0}, 0.0001);

                    //
                    // now we convert [4i, 0, 0, 0] back to [1i,1i,1i,1i]
                    // with backward FFT
                    //
                    alglib.fftc1dinv(ref z);
                    _TestResult = _TestResult && doc_test_complex_vector(z, new alglib.complex[]{new alglib.complex(0,1),new alglib.complex(0,1),new alglib.complex(0,1),new alglib.complex(0,1)}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "fft_complex_d1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST fft_complex_d2
            //      Complex FFT: advanced example
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    //
                    // first we demonstrate forward FFT:
                    // [0,1,0,1i] is converted to [1+1i, -1-1i, -1-1i, 1+1i]
                    //
                    alglib.complex[] z = new alglib.complex[]{0,1,0,new alglib.complex(0,1)};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.NegativeInfinity);
                    alglib.fftc1d(ref z);
                    _TestResult = _TestResult && doc_test_complex_vector(z, new alglib.complex[]{new alglib.complex(1,+1),new alglib.complex(-1,-1),new alglib.complex(-1,-1),new alglib.complex(1,+1)}, 0.0001);

                    //
                    // now we convert result back with backward FFT
                    //
                    alglib.fftc1dinv(ref z);
                    _TestResult = _TestResult && doc_test_complex_vector(z, new alglib.complex[]{0,1,0,new alglib.complex(0,1)}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "fft_complex_d2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST fft_real_d1
            //      Real FFT: simple example
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    //
                    // first we demonstrate forward FFT:
                    // [1,1,1,1] is converted to [4, 0, 0, 0]
                    //
                    double[] x = new double[]{1,1,1,1};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    alglib.complex[] f;
                    double[] x2;
                    alglib.fftr1d(x, out f);
                    _TestResult = _TestResult && doc_test_complex_vector(f, new alglib.complex[]{4,0,0,0}, 0.0001);

                    //
                    // now we convert [4, 0, 0, 0] back to [1,1,1,1]
                    // with backward FFT
                    //
                    alglib.fftr1dinv(f, out x2);
                    _TestResult = _TestResult && doc_test_real_vector(x2, new double[]{1,1,1,1}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "fft_real_d1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST fft_real_d2
            //      Real FFT: advanced example
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    //
                    // first we demonstrate forward FFT:
                    // [1,2,3,4] is converted to [10, -2+2i, -2, -2-2i]
                    //
                    // note that output array is self-adjoint:
                    // * f[0] = conj(f[0])
                    // * f[1] = conj(f[3])
                    // * f[2] = conj(f[2])
                    //
                    double[] x = new double[]{1,2,3,4};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    alglib.complex[] f;
                    double[] x2;
                    alglib.fftr1d(x, out f);
                    _TestResult = _TestResult && doc_test_complex_vector(f, new alglib.complex[]{10,new alglib.complex(-2,+2),-2,new alglib.complex(-2,-2)}, 0.0001);

                    //
                    // now we convert [10, -2+2i, -2, -2-2i] back to [1,2,3,4]
                    //
                    alglib.fftr1dinv(f, out x2);
                    _TestResult = _TestResult && doc_test_real_vector(x2, new double[]{1,2,3,4}, 0.0001);

                    //
                    // remember that F is self-adjoint? It means that we can pass just half
                    // (slightly larger than half) of F to inverse real FFT and still get our result.
                    //
                    // I.e. instead [10, -2+2i, -2, -2-2i] we pass just [10, -2+2i, -2] and everything works!
                    //
                    // NOTE: in this case we should explicitly pass array length (which is 4) to ALGLIB;
                    // if not, it will automatically use array length to determine FFT size and
                    // will erroneously make half-length FFT.
                    //
                    f = new alglib.complex[]{10,new alglib.complex(-2,+2),-2};
                    alglib.fftr1dinv(f, 4, out x2);
                    _TestResult = _TestResult && doc_test_real_vector(x2, new double[]{1,2,3,4}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "fft_real_d2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST fft_complex_e1
            //      error detection in backward FFT
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<3; _spoil_scenario++)
            {
                try
                {
                    alglib.complex[] z = new alglib.complex[]{0,2,0,-2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref z, (alglib.complex)System.Double.NegativeInfinity);
                    alglib.fftc1dinv(ref z);
                    _TestResult = _TestResult && doc_test_complex_vector(z, new alglib.complex[]{0,new alglib.complex(0,1),0,new alglib.complex(0,-1)}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "fft_complex_e1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST autogk_d1
            //      Integrating f=exp(x) by adaptive integrator
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates integration of f=exp(x) on [0,1]:
                    // * first, autogkstate is initialized
                    // * then we call integration function
                    // * and finally we obtain results with autogkresults() call
                    //
                    double a = 0;
                    if( _spoil_scenario==0 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==1 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==2 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = 1;
                    if( _spoil_scenario==3 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        b = (double)System.Double.NegativeInfinity;
                    alglib.autogkstate s;
                    double v;
                    alglib.autogkreport rep;

                    alglib.autogksmooth(a, b, out s);
                    alglib.autogkintegrate(s, int_function_1_func, null);
                    alglib.autogkresults(s, out v, out rep);

                    _TestResult = _TestResult && doc_test_real(v, 1.7182, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "autogk_d1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_d_calcdiff
            //      Interpolation and differentiation using barycentric representation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // Here we demonstrate polynomial interpolation and differentiation
                    // of y=x^2-x sampled at [0,1,2]. Barycentric representation of polynomial is used.
                    //
                    double[] x = new double[]{0,1,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -1;
                    if( _spoil_scenario==10 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        t = (double)System.Double.NegativeInfinity;
                    double v;
                    double dv;
                    double d2v;
                    alglib.barycentricinterpolant p;

                    // barycentric model is created
                    alglib.polynomialbuild(x, y, out p);

                    // barycentric interpolation is demonstrated
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);

                    // barycentric differentation is demonstrated
                    alglib.barycentricdiff1(p, t, out v, out dv);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && doc_test_real(dv, -3.0, 0.00005);

                    // second derivatives with barycentric representation
                    alglib.barycentricdiff1(p, t, out v, out dv);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && doc_test_real(dv, -3.0, 0.00005);
                    alglib.barycentricdiff2(p, t, out v, out dv, out d2v);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && doc_test_real(dv, -3.0, 0.00005);
                    _TestResult = _TestResult && doc_test_real(d2v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_d_calcdiff");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_d_conv
            //      Conversion between power basis and barycentric representation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    //
                    // Here we demonstrate conversion of y=x^2-x
                    // between power basis and barycentric representation.
                    //
                    double[] a = new double[]{0,-1,+1};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref a, (double)System.Double.NegativeInfinity);
                    double t = 2;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    double[] a2;
                    double v;
                    alglib.barycentricinterpolant p;

                    //
                    // a=[0,-1,+1] is decomposition of y=x^2-x in the power basis:
                    //
                    //     y = 0 - 1*x + 1*x^2
                    //
                    // We convert it to the barycentric form.
                    //
                    alglib.polynomialpow2bar(a, out p);

                    // now we have barycentric interpolation; we can use it for interpolation
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.005);

                    // we can also convert back from barycentric representation to power basis
                    alglib.polynomialbar2pow(p, out a2);
                    _TestResult = _TestResult && doc_test_real_vector(a2, new double[]{0,-1,+1}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_d_conv");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_d_spec
            //      Polynomial interpolation on special grids (equidistant, Chebyshev I/II)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<11; _spoil_scenario++)
            {
                try
                {
                    //
                    // Temporaries:
                    // * values of y=x^2-x sampled at three special grids:
                    //   * equdistant grid spanning [0,2],     x[i] = 2*i/(N-1), i=0..N-1
                    //   * Chebyshev-I grid spanning [-1,+1],  x[i] = 1 + Cos(PI*(2*i+1)/(2*n)), i=0..N-1
                    //   * Chebyshev-II grid spanning [-1,+1], x[i] = 1 + Cos(PI*i/(n-1)), i=0..N-1
                    // * barycentric interpolants for these three grids
                    // * vectors to store coefficients of quadratic representation
                    //
                    double[] y_eqdist = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y_eqdist, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y_eqdist, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y_eqdist, (double)System.Double.NegativeInfinity);
                    double[] y_cheb1 = new double[]{-0.116025,0.000000,1.616025};
                    if( _spoil_scenario==3 )
                        spoil_vector_by_value(ref y_cheb1, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y_cheb1, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y_cheb1, (double)System.Double.NegativeInfinity);
                    double[] y_cheb2 = new double[]{0,0,2};
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y_cheb2, (double)System.Double.NaN);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y_cheb2, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_value(ref y_cheb2, (double)System.Double.NegativeInfinity);
                    alglib.barycentricinterpolant p_eqdist;
                    alglib.barycentricinterpolant p_cheb1;
                    alglib.barycentricinterpolant p_cheb2;
                    double[] a_eqdist;
                    double[] a_cheb1;
                    double[] a_cheb2;

                    //
                    // First, we demonstrate construction of barycentric interpolants on
                    // special grids. We unpack power representation to ensure that
                    // interpolant was built correctly.
                    //
                    // In all three cases we should get same quadratic function.
                    //
                    alglib.polynomialbuildeqdist(0.0, 2.0, y_eqdist, out p_eqdist);
                    alglib.polynomialbar2pow(p_eqdist, out a_eqdist);
                    _TestResult = _TestResult && doc_test_real_vector(a_eqdist, new double[]{0,-1,+1}, 0.00005);

                    alglib.polynomialbuildcheb1(-1, +1, y_cheb1, out p_cheb1);
                    alglib.polynomialbar2pow(p_cheb1, out a_cheb1);
                    _TestResult = _TestResult && doc_test_real_vector(a_cheb1, new double[]{0,-1,+1}, 0.00005);

                    alglib.polynomialbuildcheb2(-1, +1, y_cheb2, out p_cheb2);
                    alglib.polynomialbar2pow(p_cheb2, out a_cheb2);
                    _TestResult = _TestResult && doc_test_real_vector(a_cheb2, new double[]{0,-1,+1}, 0.00005);

                    //
                    // Now we demonstrate polynomial interpolation without construction 
                    // of the barycentricinterpolant structure.
                    //
                    // We calculate interpolant value at x=-2.
                    // In all three cases we should get same f=6
                    //
                    double t = -2;
                    if( _spoil_scenario==9 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==10 )
                        t = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalceqdist(0.0, 2.0, y_eqdist, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);

                    v = alglib.polynomialcalccheb1(-1, +1, y_cheb1, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);

                    v = alglib.polynomialcalccheb2(-1, +1, y_cheb2, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_d_spec");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_1
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<10; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{0,1,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -1;
                    if( _spoil_scenario==8 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        t = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuild(x, y, 3, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_2
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -1;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        t = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuildeqdist(0.0, 2.0, y, 3, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_3
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{-0.116025,0.000000,1.616025};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -1;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        t = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuildcheb1(-1.0, +1.0, y, 3, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_3");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_4
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -2;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        b = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuildcheb2(a, b, y, 3, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_4");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_5
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -1;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        t = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalceqdist(0.0, 2.0, y, 3, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_5");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_6
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{-0.116025,0.000000,1.616025};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -1;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        b = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalccheb1(a, b, y, 3, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_6");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_7
            //      Polynomial interpolation, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = -2;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        b = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalccheb2(a, b, y, 3, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_7");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_8
            //      Polynomial interpolation: y=x^2-x, equidistant grid, barycentric form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double t = -1;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuildeqdist(0.0, 2.0, y, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_8");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_9
            //      Polynomial interpolation: y=x^2-x, Chebyshev grid (first kind), barycentric form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<11; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{-0.116025,0.000000,1.616025};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double t = -1;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==5 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==8 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuildcheb1(a, b, y, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_9");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_10
            //      Polynomial interpolation: y=x^2-x, Chebyshev grid (second kind), barycentric form
            //
            System.Console.WriteLine("50/91");
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<11; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double t = -2;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==5 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==8 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.NegativeInfinity;
                    alglib.barycentricinterpolant p;
                    double v;
                    alglib.polynomialbuildcheb2(a, b, y, out p);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_10");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_11
            //      Polynomial interpolation: y=x^2-x, equidistant grid
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double t = -1;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalceqdist(0.0, 2.0, y, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_11");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_12
            //      Polynomial interpolation: y=x^2-x, Chebyshev grid (first kind)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<11; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{-0.116025,0.000000,1.616025};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double t = -1;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==5 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==8 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalccheb1(a, b, y, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_12");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST polint_t_13
            //      Polynomial interpolation: y=x^2-x, Chebyshev grid (second kind)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<11; _spoil_scenario++)
            {
                try
                {
                    double[] y = new double[]{0,0,2};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    double t = -2;
                    if( _spoil_scenario==3 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==4 )
                        t = (double)System.Double.NegativeInfinity;
                    double a = -1;
                    if( _spoil_scenario==5 )
                        a = (double)System.Double.NaN;
                    if( _spoil_scenario==6 )
                        a = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==7 )
                        a = (double)System.Double.NegativeInfinity;
                    double b = +1;
                    if( _spoil_scenario==8 )
                        b = (double)System.Double.NaN;
                    if( _spoil_scenario==9 )
                        b = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==10 )
                        b = (double)System.Double.NegativeInfinity;
                    double v;
                    v = alglib.polynomialcalccheb2(a, b, y, t);
                    _TestResult = _TestResult && doc_test_real(v, 6.0, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "polint_t_13");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST spline1d_d_linear
            //      Piecewise linear spline interpolation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // We use piecewise linear spline to interpolate f(x)=x^2 sampled 
                    // at 5 equidistant nodes on [-1,+1].
                    //
                    double[] x = new double[]{-1.0,-0.5,0.0,+0.5,+1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{+1.0,0.25,0.0,0.25,+1.0};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = 0.25;
                    if( _spoil_scenario==10 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        t = (double)System.Double.NegativeInfinity;
                    double v;
                    alglib.spline1dinterpolant s;

                    // build spline
                    alglib.spline1dbuildlinear(x, y, out s);

                    // calculate S(0.25) - it is quite different from 0.25^2=0.0625
                    v = alglib.spline1dcalc(s, t);
                    _TestResult = _TestResult && doc_test_real(v, 0.125, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "spline1d_d_linear");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST spline1d_d_cubic
            //      Cubic spline interpolation
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<10; _spoil_scenario++)
            {
                try
                {
                    //
                    // We use cubic spline to interpolate f(x)=x^2 sampled 
                    // at 5 equidistant nodes on [-1,+1].
                    //
                    // First, we use default boundary conditions ("parabolically terminated
                    // spline") because cubic spline built with such boundary conditions 
                    // will exactly reproduce any quadratic f(x).
                    //
                    // Then we try to use natural boundary conditions
                    //     d2S(-1)/dx^2 = 0.0
                    //     d2S(+1)/dx^2 = 0.0
                    // and see that such spline interpolated f(x) with small error.
                    //
                    double[] x = new double[]{-1.0,-0.5,0.0,+0.5,+1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{+1.0,0.25,0.0,0.25,+1.0};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y);
                    double t = 0.25;
                    if( _spoil_scenario==8 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        t = (double)System.Double.NegativeInfinity;
                    double v;
                    alglib.spline1dinterpolant s;
                    int natural_bound_type = 2;
                    //
                    // Test exact boundary conditions: build S(x), calculare S(0.25)
                    // (almost same as original function)
                    //
                    alglib.spline1dbuildcubic(x, y, out s);
                    v = alglib.spline1dcalc(s, t);
                    _TestResult = _TestResult && doc_test_real(v, 0.0625, 0.00001);

                    //
                    // Test natural boundary conditions: build S(x), calculare S(0.25)
                    // (small interpolation error)
                    //
                    alglib.spline1dbuildcubic(x, y, 5, natural_bound_type, 0.0, natural_bound_type, 0.0, out s);
                    v = alglib.spline1dcalc(s, t);
                    _TestResult = _TestResult && doc_test_real(v, 0.0580, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "spline1d_d_cubic");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST spline1d_d_griddiff
            //      Differentiation on the grid using cubic splines
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<10; _spoil_scenario++)
            {
                try
                {
                    //
                    // We use cubic spline to do grid differentiation, i.e. having
                    // values of f(x)=x^2 sampled at 5 equidistant nodes on [-1,+1]
                    // we calculate derivatives of cubic spline at nodes WITHOUT
                    // CONSTRUCTION OF SPLINE OBJECT.
                    //
                    // There are efficient functions spline1dgriddiffcubic() and
                    // spline1dgriddiff2cubic() for such calculations.
                    //
                    // We use default boundary conditions ("parabolically terminated
                    // spline") because cubic spline built with such boundary conditions 
                    // will exactly reproduce any quadratic f(x).
                    //
                    // Actually, we could use natural conditions, but we feel that 
                    // spline which exactly reproduces f() will show us more 
                    // understandable results.
                    //
                    double[] x = new double[]{-1.0,-0.5,0.0,+0.5,+1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{+1.0,0.25,0.0,0.25,+1.0};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] d1;
                    double[] d2;

                    //
                    // We calculate first derivatives: they must be equal to 2*x
                    //
                    alglib.spline1dgriddiffcubic(x, y, out d1);
                    _TestResult = _TestResult && doc_test_real_vector(d1, new double[]{-2.0,-1.0,0.0,+1.0,+2.0}, 0.0001);

                    //
                    // Now test griddiff2, which returns first AND second derivatives.
                    // First derivative is 2*x, second is equal to 2.0
                    //
                    alglib.spline1dgriddiff2cubic(x, y, out d1, out d2);
                    _TestResult = _TestResult && doc_test_real_vector(d1, new double[]{-2.0,-1.0,0.0,+1.0,+2.0}, 0.0001);
                    _TestResult = _TestResult && doc_test_real_vector(d2, new double[]{2.0,2.0,2.0,2.0,2.0}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "spline1d_d_griddiff");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST spline1d_d_convdiff
            //      Resampling using cubic splines
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<11; _spoil_scenario++)
            {
                try
                {
                    //
                    // We use cubic spline to do resampling, i.e. having
                    // values of f(x)=x^2 sampled at 5 equidistant nodes on [-1,+1]
                    // we calculate values/derivatives of cubic spline on 
                    // another grid (equidistant with 9 nodes on [-1,+1])
                    // WITHOUT CONSTRUCTION OF SPLINE OBJECT.
                    //
                    // There are efficient functions spline1dconvcubic(),
                    // spline1dconvdiffcubic() and spline1dconvdiff2cubic() 
                    // for such calculations.
                    //
                    // We use default boundary conditions ("parabolically terminated
                    // spline") because cubic spline built with such boundary conditions 
                    // will exactly reproduce any quadratic f(x).
                    //
                    // Actually, we could use natural conditions, but we feel that 
                    // spline which exactly reproduces f() will show us more 
                    // understandable results.
                    //
                    double[] x_old = new double[]{-1.0,-0.5,0.0,+0.5,+1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x_old, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x_old, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x_old, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref x_old);
                    double[] y_old = new double[]{+1.0,0.25,0.0,0.25,+1.0};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y_old, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y_old, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y_old, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y_old);
                    double[] x_new = new double[]{-1.00,-0.75,-0.50,-0.25,0.00,+0.25,+0.50,+0.75,+1.00};
                    if( _spoil_scenario==8 )
                        spoil_vector_by_value(ref x_new, (double)System.Double.NaN);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref x_new, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref x_new, (double)System.Double.NegativeInfinity);
                    double[] y_new;
                    double[] d1_new;
                    double[] d2_new;

                    //
                    // First, conversion without differentiation.
                    //
                    //
                    alglib.spline1dconvcubic(x_old, y_old, x_new, out y_new);
                    _TestResult = _TestResult && doc_test_real_vector(y_new, new double[]{1.0000,0.5625,0.2500,0.0625,0.0000,0.0625,0.2500,0.5625,1.0000}, 0.0001);

                    //
                    // Then, conversion with differentiation (first derivatives only)
                    //
                    //
                    alglib.spline1dconvdiffcubic(x_old, y_old, x_new, out y_new, out d1_new);
                    _TestResult = _TestResult && doc_test_real_vector(y_new, new double[]{1.0000,0.5625,0.2500,0.0625,0.0000,0.0625,0.2500,0.5625,1.0000}, 0.0001);
                    _TestResult = _TestResult && doc_test_real_vector(d1_new, new double[]{-2.0,-1.5,-1.0,-0.5,0.0,0.5,1.0,1.5,2.0}, 0.0001);

                    //
                    // Finally, conversion with first and second derivatives
                    //
                    //
                    alglib.spline1dconvdiff2cubic(x_old, y_old, x_new, out y_new, out d1_new, out d2_new);
                    _TestResult = _TestResult && doc_test_real_vector(y_new, new double[]{1.0000,0.5625,0.2500,0.0625,0.0000,0.0625,0.2500,0.5625,1.0000}, 0.0001);
                    _TestResult = _TestResult && doc_test_real_vector(d1_new, new double[]{-2.0,-1.5,-1.0,-0.5,0.0,0.5,1.0,1.5,2.0}, 0.0001);
                    _TestResult = _TestResult && doc_test_real_vector(d2_new, new double[]{2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0,2.0}, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "spline1d_d_convdiff");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minqp_d_u1
            //      Unconstrained dense quadratic programming
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<13; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = x0^2 + x1^2 -6*x0 - 4*x1
                    //
                    // Exact solution is [x0,x1] = [3,2]
                    //
                    // We provide algorithm with starting point, although in this case
                    // (dense matrix, no constraints) it can work without such information.
                    //
                    // IMPORTANT: this solver minimizes  following  function:
                    //     f(x) = 0.5*x'*A*x + b'*x.
                    // Note that quadratic term has 0.5 before it. So if you want to minimize
                    // quadratic function, you should rewrite it in such way that quadratic term
                    // is multiplied by 0.5 too.
                    // For example, our function is f(x)=x0^2+x1^2+..., but we rewrite it as 
                    //     f(x) = 0.5*(2*x0^2+2*x1^2) + ....
                    // and pass diag(2,2) as quadratic term - NOT diag(1,1)!
                    //
                    double[,] a = new double[,]{{2,0},{0,2}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref a);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref a);
                    double[] b = new double[]{-6,-4};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_deleting_element(ref b);
                    double[] x0 = new double[]{0,1};
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref x0, (double)System.Double.NaN);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref x0, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref x0, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_deleting_element(ref x0);
                    double[] x;
                    alglib.minqpstate state;
                    alglib.minqpreport rep;

                    alglib.minqpcreate(2, out state);
                    alglib.minqpsetquadraticterm(state, a);
                    alglib.minqpsetlinearterm(state, b);
                    alglib.minqpsetstartingpoint(state, x0);
                    alglib.minqpoptimize(state);
                    alglib.minqpresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{3,2}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minqp_d_u1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minqp_d_bc1
            //      Constrained dense quadratic programming
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<17; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = x0^2 + x1^2 -6*x0 - 4*x1
                    // subject to bound constraints 0<=x0<=2.5, 0<=x1<=2.5
                    //
                    // Exact solution is [x0,x1] = [2.5,2]
                    //
                    // We provide algorithm with starting point. With such small problem good starting
                    // point is not really necessary, but with high-dimensional problem it can save us
                    // a lot of time.
                    //
                    // IMPORTANT: this solver minimizes  following  function:
                    //     f(x) = 0.5*x'*A*x + b'*x.
                    // Note that quadratic term has 0.5 before it. So if you want to minimize
                    // quadratic function, you should rewrite it in such way that quadratic term
                    // is multiplied by 0.5 too.
                    // For example, our function is f(x)=x0^2+x1^2+..., but we rewrite it as 
                    //     f(x) = 0.5*(2*x0^2+2*x1^2) + ....
                    // and pass diag(2,2) as quadratic term - NOT diag(1,1)!
                    //
                    double[,] a = new double[,]{{2,0},{0,2}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref a, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref a, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref a);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref a);
                    double[] b = new double[]{-6,-4};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_deleting_element(ref b);
                    double[] x0 = new double[]{0,1};
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref x0, (double)System.Double.NaN);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref x0, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref x0, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_deleting_element(ref x0);
                    double[] bndl = new double[]{0.0,0.0};
                    if( _spoil_scenario==13 )
                        spoil_vector_by_value(ref bndl, (double)System.Double.NaN);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_deleting_element(ref bndl);
                    double[] bndu = new double[]{2.5,2.5};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref bndu, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_deleting_element(ref bndu);
                    double[] x;
                    alglib.minqpstate state;
                    alglib.minqpreport rep;

                    alglib.minqpcreate(2, out state);
                    alglib.minqpsetquadraticterm(state, a);
                    alglib.minqpsetlinearterm(state, b);
                    alglib.minqpsetstartingpoint(state, x0);
                    alglib.minqpsetbc(state, bndl, bndu);
                    alglib.minqpoptimize(state);
                    alglib.minqpresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{2.5,2}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minqp_d_bc1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_d_v
            //      Nonlinear least squares optimization using function vector only
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = f0^2+f1^2, where 
                    //
                    //     f0(x0,x1) = 10*(x0+3)^2
                    //     f1(x0,x1) = (x1-3)^2
                    //
                    // using "V" mode of the Levenberg-Marquardt optimizer.
                    //
                    // Optimization algorithm uses:
                    // * function vector f[] = {f1,f2}
                    //
                    // No other information (Jacobian, gradient, etc.) is needed.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;

                    alglib.minlmcreatev(2, x, 0.0001, out state);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_fvec, null, null);
                    alglib.minlmresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,+3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_d_v");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_d_vj
            //      Nonlinear least squares optimization using function vector and Jacobian
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = f0^2+f1^2, where 
                    //
                    //     f0(x0,x1) = 10*(x0+3)^2
                    //     f1(x0,x1) = (x1-3)^2
                    //
                    // using "VJ" mode of the Levenberg-Marquardt optimizer.
                    //
                    // Optimization algorithm uses:
                    // * function vector f[] = {f1,f2}
                    // * Jacobian matrix J = {dfi/dxj}.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;

                    alglib.minlmcreatevj(2, x, out state);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_fvec, function1_jac, null, null);
                    alglib.minlmresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,+3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_d_vj");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_d_fgh
            //      Nonlinear Hessian-based optimization for general functions
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = 100*(x0+3)^4+(x1-3)^4
                    // using "FGH" mode of the Levenberg-Marquardt optimizer.
                    //
                    // F is treated like a monolitic function without internal structure,
                    // i.e. we do NOT represent it as a sum of squares.
                    //
                    // Optimization algorithm uses:
                    // * function value F(x0,x1)
                    // * gradient G={dF/dxi}
                    // * Hessian H={d2F/(dxi*dxj)}
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;

                    alglib.minlmcreatefgh(x, out state);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_func, function1_grad, function1_hess, null, null);
                    alglib.minlmresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,+3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_d_fgh");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_d_vb
            //      Bound constrained nonlinear least squares optimization
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<16; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = f0^2+f1^2, where 
                    //
                    //     f0(x0,x1) = 10*(x0+3)^2
                    //     f1(x0,x1) = (x1-3)^2
                    //
                    // with boundary constraints
                    //
                    //     -1 <= x0 <= +1
                    //     -1 <= x1 <= +1
                    //
                    // using "V" mode of the Levenberg-Marquardt optimizer.
                    //
                    // Optimization algorithm uses:
                    // * function vector f[] = {f1,f2}
                    //
                    // No other information (Jacobian, gradient, etc.) is needed.
                    //
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double[] bndl = new double[]{-1,-1};
                    if( _spoil_scenario==3 )
                        spoil_vector_by_value(ref bndl, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref bndl);
                    double[] bndu = new double[]{+1,+1};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref bndu, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_deleting_element(ref bndu);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==7 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==8 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==10 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==11 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==12 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==13 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;

                    alglib.minlmcreatev(2, x, 0.0001, out state);
                    alglib.minlmsetbc(state, bndl, bndu);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_fvec, null, null);
                    alglib.minlmresults(state, out x, out rep);

                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-1,+1}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_d_vb");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_d_restarts
            //      Efficient restarts of LM optimizer
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<15; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates minimization of F(x0,x1) = f0^2+f1^2, where 
                    //
                    //     f0(x0,x1) = 10*(x0+3)^2
                    //     f1(x0,x1) = (x1-3)^2
                    //
                    // using several starting points and efficient restarts.
                    //
                    double[] x;
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==0 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==1 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==2 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==3 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==6 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;

                    //
                    // create optimizer using minlmcreatev()
                    //
                    x = new double[]{10,10};
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    alglib.minlmcreatev(2, x, 0.0001, out state);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_fvec, null, null);
                    alglib.minlmresults(state, out x, out rep);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,+3}, 0.005);

                    //
                    // restart optimizer using minlmrestartfrom()
                    //
                    // we can use different starting point, different function,
                    // different stopping conditions, but problem size
                    // must remain unchanged.
                    //
                    x = new double[]{4,4};
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==13 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    alglib.minlmrestartfrom(state, x);
                    alglib.minlmoptimize(state, function2_fvec, null, null);
                    alglib.minlmresults(state, out x, out rep);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{0,1}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_d_restarts");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_t_1
            //      Nonlinear least squares optimization, FJ scheme (obsolete, but supported)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;
                    alglib.minlmcreatefj(2, x, out state);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_func, function1_jac, null, null);
                    alglib.minlmresults(state, out x, out rep);
                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,+3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_t_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST minlm_t_2
            //      Nonlinear least squares optimization, FGJ scheme (obsolete, but supported)
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<12; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{0,0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    double epsg = 0.0000000001;
                    if( _spoil_scenario==3 )
                        epsg = (double)System.Double.NaN;
                    if( _spoil_scenario==4 )
                        epsg = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==5 )
                        epsg = (double)System.Double.NegativeInfinity;
                    double epsf = 0;
                    if( _spoil_scenario==6 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==7 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==8 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0;
                    if( _spoil_scenario==9 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==10 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    alglib.minlmstate state;
                    alglib.minlmreport rep;
                    alglib.minlmcreatefgj(2, x, out state);
                    alglib.minlmsetcond(state, epsg, epsf, epsx, maxits);
                    alglib.minlmoptimize(state, function1_func, function1_grad, function1_jac, null, null);
                    alglib.minlmresults(state, out x, out rep);
                    _TestResult = _TestResult && doc_test_int(rep.terminationtype, 4);
                    _TestResult = _TestResult && doc_test_real_vector(x, new double[]{-3,+3}, 0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "minlm_t_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_nlf
            //      Nonlinear fitting using function value only
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<27; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate exponential fitting
                    // by f(x) = exp(-c*x^2)
                    // using function value only.
                    //
                    // Gradient is estimated using combination of numerical differences
                    // and secant updates. diffstep variable stores differentiation step 
                    // (we have to tell algorithm what step to use).
                    //
                    double[,] x = new double[,]{{-1},{-0.8},{-0.6},{-0.4},{-0.2},{0},{0.2},{0.4},{0.6},{0.8},{1.0}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref x);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref x);
                    double[] y = new double[]{0.223130,0.382893,0.582748,0.786628,0.941765,1.000000,0.941765,0.786628,0.582748,0.382893,0.223130};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] c = new double[]{0.3};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref c, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref c, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref c, (double)System.Double.NegativeInfinity);
                    double epsf = 0;
                    if( _spoil_scenario==13 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0.000001;
                    if( _spoil_scenario==16 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    int info;
                    alglib.lsfitstate state;
                    alglib.lsfitreport rep;
                    double diffstep = 0.0001;
                    if( _spoil_scenario==19 )
                        diffstep = (double)System.Double.NaN;
                    if( _spoil_scenario==20 )
                        diffstep = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==21 )
                        diffstep = (double)System.Double.NegativeInfinity;

                    //
                    // Fitting without weights
                    //
                    alglib.lsfitcreatef(x, y, c, diffstep, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.5}, 0.05);

                    //
                    // Fitting with weights
                    // (you can change weights and see how it changes result)
                    //
                    double[] w = new double[]{1,1,1,1,1,1,1,1,1,1,1};
                    if( _spoil_scenario==22 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==23 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==24 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==25 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==26 )
                        spoil_vector_by_deleting_element(ref w);
                    alglib.lsfitcreatewf(x, y, w, c, diffstep, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.5}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_nlf");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_nlfg
            //      Nonlinear fitting using gradient
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<24; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate exponential fitting
                    // by f(x) = exp(-c*x^2)
                    // using function value and gradient (with respect to c).
                    //
                    double[,] x = new double[,]{{-1},{-0.8},{-0.6},{-0.4},{-0.2},{0},{0.2},{0.4},{0.6},{0.8},{1.0}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref x);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref x);
                    double[] y = new double[]{0.223130,0.382893,0.582748,0.786628,0.941765,1.000000,0.941765,0.786628,0.582748,0.382893,0.223130};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] c = new double[]{0.3};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref c, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref c, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref c, (double)System.Double.NegativeInfinity);
                    double epsf = 0;
                    if( _spoil_scenario==13 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0.000001;
                    if( _spoil_scenario==16 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    int info;
                    alglib.lsfitstate state;
                    alglib.lsfitreport rep;

                    //
                    // Fitting without weights
                    //
                    alglib.lsfitcreatefg(x, y, c, true, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, function_cx_1_grad, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.5}, 0.05);

                    //
                    // Fitting with weights
                    // (you can change weights and see how it changes result)
                    //
                    double[] w = new double[]{1,1,1,1,1,1,1,1,1,1,1};
                    if( _spoil_scenario==19 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==20 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==21 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==22 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==23 )
                        spoil_vector_by_deleting_element(ref w);
                    alglib.lsfitcreatewfg(x, y, w, c, true, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, function_cx_1_grad, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.5}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_nlfg");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_nlfgh
            //      Nonlinear fitting using gradient and Hessian
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<24; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate exponential fitting
                    // by f(x) = exp(-c*x^2)
                    // using function value, gradient and Hessian (with respect to c)
                    //
                    double[,] x = new double[,]{{-1},{-0.8},{-0.6},{-0.4},{-0.2},{0},{0.2},{0.4},{0.6},{0.8},{1.0}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref x);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref x);
                    double[] y = new double[]{0.223130,0.382893,0.582748,0.786628,0.941765,1.000000,0.941765,0.786628,0.582748,0.382893,0.223130};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] c = new double[]{0.3};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref c, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref c, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref c, (double)System.Double.NegativeInfinity);
                    double epsf = 0;
                    if( _spoil_scenario==13 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0.000001;
                    if( _spoil_scenario==16 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    int info;
                    alglib.lsfitstate state;
                    alglib.lsfitreport rep;

                    //
                    // Fitting without weights
                    //
                    alglib.lsfitcreatefgh(x, y, c, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, function_cx_1_grad, function_cx_1_hess, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.5}, 0.05);

                    //
                    // Fitting with weights
                    // (you can change weights and see how it changes result)
                    //
                    double[] w = new double[]{1,1,1,1,1,1,1,1,1,1,1};
                    if( _spoil_scenario==19 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==20 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==21 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==22 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==23 )
                        spoil_vector_by_deleting_element(ref w);
                    alglib.lsfitcreatewfgh(x, y, w, c, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, function_cx_1_grad, function_cx_1_hess, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.5}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_nlfgh");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_nlfb
            //      Bound contstrained nonlinear fitting using function value only
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<26; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate exponential fitting by
                    //     f(x) = exp(-c*x^2)
                    // subject to bound constraints
                    //     0.0 <= c <= 1.0
                    // using function value only.
                    //
                    // Gradient is estimated using combination of numerical differences
                    // and secant updates. diffstep variable stores differentiation step 
                    // (we have to tell algorithm what step to use).
                    //
                    // Unconstrained solution is c=1.5, but because of constraints we should
                    // get c=1.0 (at the boundary).
                    //
                    double[,] x = new double[,]{{-1},{-0.8},{-0.6},{-0.4},{-0.2},{0},{0.2},{0.4},{0.6},{0.8},{1.0}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref x);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref x);
                    double[] y = new double[]{0.223130,0.382893,0.582748,0.786628,0.941765,1.000000,0.941765,0.786628,0.582748,0.382893,0.223130};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] c = new double[]{0.3};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref c, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref c, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref c, (double)System.Double.NegativeInfinity);
                    double[] bndl = new double[]{0.0};
                    if( _spoil_scenario==13 )
                        spoil_vector_by_value(ref bndl, (double)System.Double.NaN);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_deleting_element(ref bndl);
                    double[] bndu = new double[]{1.0};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref bndu, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_deleting_element(ref bndu);
                    double epsf = 0;
                    if( _spoil_scenario==17 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==18 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==19 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 0.000001;
                    if( _spoil_scenario==20 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==21 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==22 )
                        epsx = (double)System.Double.NegativeInfinity;
                    int maxits = 0;
                    int info;
                    alglib.lsfitstate state;
                    alglib.lsfitreport rep;
                    double diffstep = 0.0001;
                    if( _spoil_scenario==23 )
                        diffstep = (double)System.Double.NaN;
                    if( _spoil_scenario==24 )
                        diffstep = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==25 )
                        diffstep = (double)System.Double.NegativeInfinity;

                    alglib.lsfitcreatef(x, y, c, diffstep, out state);
                    alglib.lsfitsetbc(state, bndl, bndu);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitfit(state, function_cx_1_func, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.0}, 0.05);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_nlfb");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_nlscale
            //      Nonlinear fitting with custom scaling and bound constraints
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<30; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate fitting by
                    //     f(x) = c[0]*(1+c[1]*((x-1999)^c[2]-1))
                    // subject to bound constraints
                    //     -INF  < c[0] < +INF
                    //      -10 <= c[1] <= +10
                    //      0.1 <= c[2] <= 2.0
                    // Data we want to fit are time series of Japan national debt
                    // collected from 2000 to 2008 measured in USD (dollars, not
                    // millions of dollars).
                    //
                    // Our variables are:
                    //     c[0] - debt value at initial moment (2000),
                    //     c[1] - direction coefficient (growth or decrease),
                    //     c[2] - curvature coefficient.
                    // You may see that our variables are badly scaled - first one 
                    // is order of 10^12, and next two are somewhere about 1 in 
                    // magnitude. Such problem is difficult to solve without some
                    // kind of scaling.
                    // That is exactly where lsfitsetscale() function can be used.
                    // We set scale of our variables to [1.0E12, 1, 1], which allows
                    // us to easily solve this problem.
                    //
                    // You can try commenting out lsfitsetscale() call - and you will 
                    // see that algorithm will fail to converge.
                    //
                    double[,] x = new double[,]{{2000},{2001},{2002},{2003},{2004},{2005},{2006},{2007},{2008}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref x);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref x);
                    double[] y = new double[]{4323239600000.0,4560913100000.0,5564091500000.0,6743189300000.0,7284064600000.0,7050129600000.0,7092221500000.0,8483907600000.0,8625804400000.0};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] c = new double[]{1.0e+13,1,1};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref c, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref c, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref c, (double)System.Double.NegativeInfinity);
                    double epsf = 0;
                    if( _spoil_scenario==13 )
                        epsf = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        epsf = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        epsf = (double)System.Double.NegativeInfinity;
                    double epsx = 1.0e-5;
                    if( _spoil_scenario==16 )
                        epsx = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        epsx = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        epsx = (double)System.Double.NegativeInfinity;
                    double[] bndl = new double[]{-System.Double.PositiveInfinity,-10,0.1};
                    if( _spoil_scenario==19 )
                        spoil_vector_by_value(ref bndl, (double)System.Double.NaN);
                    if( _spoil_scenario==20 )
                        spoil_vector_by_deleting_element(ref bndl);
                    double[] bndu = new double[]{System.Double.PositiveInfinity,+10,2.0};
                    if( _spoil_scenario==21 )
                        spoil_vector_by_value(ref bndu, (double)System.Double.NaN);
                    if( _spoil_scenario==22 )
                        spoil_vector_by_deleting_element(ref bndu);
                    double[] s = new double[]{1.0e+12,1,1};
                    if( _spoil_scenario==23 )
                        spoil_vector_by_value(ref s, (double)System.Double.NaN);
                    if( _spoil_scenario==24 )
                        spoil_vector_by_value(ref s, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==25 )
                        spoil_vector_by_value(ref s, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==26 )
                        spoil_vector_by_deleting_element(ref s);
                    int maxits = 0;
                    int info;
                    alglib.lsfitstate state;
                    alglib.lsfitreport rep;
                    double diffstep = 1.0e-5;
                    if( _spoil_scenario==27 )
                        diffstep = (double)System.Double.NaN;
                    if( _spoil_scenario==28 )
                        diffstep = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==29 )
                        diffstep = (double)System.Double.NegativeInfinity;

                    alglib.lsfitcreatef(x, y, c, diffstep, out state);
                    alglib.lsfitsetcond(state, epsf, epsx, maxits);
                    alglib.lsfitsetbc(state, bndl, bndu);
                    alglib.lsfitsetscale(state, s);
                    alglib.lsfitfit(state, function_debt_func, null, null);
                    alglib.lsfitresults(state, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 2);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{4.142560e+12,0.434240,0.565376}, -0.005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_nlscale");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_lin
            //      Unconstrained (general) linear least squares fitting with and without weights
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<13; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate linear fitting by f(x|a) = a*exp(0.5*x).
                    //
                    // We have:
                    // * y - vector of experimental data
                    // * fmatrix -  matrix of basis functions calculated at sample points
                    //              Actually, we have only one basis function F0 = exp(0.5*x).
                    //
                    double[,] fmatrix = new double[,]{{0.606531},{0.670320},{0.740818},{0.818731},{0.904837},{1.000000},{1.105171},{1.221403},{1.349859},{1.491825},{1.648721}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref fmatrix, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref fmatrix, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref fmatrix, (double)System.Double.NegativeInfinity);
                    double[] y = new double[]{1.133719,1.306522,1.504604,1.554663,1.884638,2.072436,2.257285,2.534068,2.622017,2.897713,3.219371};
                    if( _spoil_scenario==3 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y);
                    int info;
                    double[] c;
                    alglib.lsfitreport rep;

                    //
                    // Linear fitting without weights
                    //
                    alglib.lsfitlinear(y, fmatrix, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.98650}, 0.00005);

                    //
                    // Linear fitting with individual weights.
                    // Slightly different result is returned.
                    //
                    double[] w = new double[]{1.414213,1,1,1,1,1,1,1,1,1,1};
                    if( _spoil_scenario==8 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_deleting_element(ref w);
                    alglib.lsfitlinearw(y, w, fmatrix, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{1.983354}, 0.00005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_lin");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_linc
            //      Constrained (general) linear least squares fitting with and without weights
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<20; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate linear fitting by f(x|a,b) = a*x+b
                    // with simple constraint f(0)=0.
                    //
                    // We have:
                    // * y - vector of experimental data
                    // * fmatrix -  matrix of basis functions sampled at [0,1] with step 0.2:
                    //                  [ 1.0   0.0 ]
                    //                  [ 1.0   0.2 ]
                    //                  [ 1.0   0.4 ]
                    //                  [ 1.0   0.6 ]
                    //                  [ 1.0   0.8 ]
                    //                  [ 1.0   1.0 ]
                    //              first column contains value of first basis function (constant term)
                    //              second column contains second basis function (linear term)
                    // * cmatrix -  matrix of linear constraints:
                    //                  [ 1.0  0.0  0.0 ]
                    //              first two columns contain coefficients before basis functions,
                    //              last column contains desired value of their sum.
                    //              So [1,0,0] means "1*constant_term + 0*linear_term = 0" 
                    //
                    double[] y = new double[]{0.072436,0.246944,0.491263,0.522300,0.714064,0.921929};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref y);
                    double[,] fmatrix = new double[,]{{1,0.0},{1,0.2},{1,0.4},{1,0.6},{1,0.8},{1,1.0}};
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_value(ref fmatrix, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_value(ref fmatrix, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_matrix_by_value(ref fmatrix, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_matrix_by_adding_row(ref fmatrix);
                    if( _spoil_scenario==9 )
                        spoil_matrix_by_adding_col(ref fmatrix);
                    if( _spoil_scenario==10 )
                        spoil_matrix_by_deleting_row(ref fmatrix);
                    if( _spoil_scenario==11 )
                        spoil_matrix_by_deleting_col(ref fmatrix);
                    double[,] cmatrix = new double[,]{{1,0,0}};
                    if( _spoil_scenario==12 )
                        spoil_matrix_by_value(ref cmatrix, (double)System.Double.NaN);
                    if( _spoil_scenario==13 )
                        spoil_matrix_by_value(ref cmatrix, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==14 )
                        spoil_matrix_by_value(ref cmatrix, (double)System.Double.NegativeInfinity);
                    int info;
                    double[] c;
                    alglib.lsfitreport rep;

                    //
                    // Constrained fitting without weights
                    //
                    alglib.lsfitlinearc(y, fmatrix, cmatrix, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{0,0.932933}, 0.0005);

                    //
                    // Constrained fitting with individual weights
                    //
                    double[] w = new double[]{1,1.414213,1,1,1,1};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==18 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==19 )
                        spoil_vector_by_deleting_element(ref w);
                    alglib.lsfitlinearwc(y, w, fmatrix, cmatrix, out info, out c, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && doc_test_real_vector(c, new double[]{0,0.938322}, 0.0005);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_linc");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_pol
            //      Unconstrained polynomial fitting
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<20; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates polynomial fitting.
                    //
                    // Fitting is done by two (M=2) functions from polynomial basis:
                    //     f0 = 1
                    //     f1 = x
                    // Basically, it just a linear fit; more complex polynomials may be used
                    // (e.g. parabolas with M=3, cubic with M=4), but even such simple fit allows
                    // us to demonstrate polynomialfit() function in action.
                    //
                    // We have:
                    // * x      set of abscissas
                    // * y      experimental data
                    //
                    // Additionally we demonstrate weighted fitting, where second point has
                    // more weight than other ones.
                    //
                    double[] x = new double[]{0.0,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0.00,0.05,0.26,0.32,0.33,0.43,0.60,0.60,0.77,0.98,1.02};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    int m = 2;
                    double t = 2;
                    if( _spoil_scenario==10 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==11 )
                        t = (double)System.Double.NegativeInfinity;
                    int info;
                    alglib.barycentricinterpolant p;
                    alglib.polynomialfitreport rep;
                    double v;

                    //
                    // Fitting without individual weights
                    //
                    // NOTE: result is returned as barycentricinterpolant structure.
                    //       if you want to get representation in the power basis,
                    //       you can use barycentricbar2pow() function to convert
                    //       from barycentric to power representation (see docs for 
                    //       POLINT subpackage for more info).
                    //
                    alglib.polynomialfit(x, y, m, out info, out p, out rep);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.011, 0.002);

                    //
                    // Fitting with individual weights
                    //
                    // NOTE: slightly different result is returned
                    //
                    double[] w = new double[]{1,1.414213562,1,1,1,1,1,1,1,1,1};
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==13 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==15 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_deleting_element(ref w);
                    double[] xc = new double[0];
                    if( _spoil_scenario==17 )
                        spoil_vector_by_adding_element(ref xc);
                    double[] yc = new double[0];
                    if( _spoil_scenario==18 )
                        spoil_vector_by_adding_element(ref yc);
                    int[] dc = new int[0];
                    if( _spoil_scenario==19 )
                        spoil_vector_by_adding_element(ref dc);
                    alglib.polynomialfitwc(x, y, w, xc, yc, dc, m, out info, out p, out rep);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.023, 0.002);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_pol");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_polc
            //      Constrained polynomial fitting
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<29; _spoil_scenario++)
            {
                try
                {
                    //
                    // This example demonstrates polynomial fitting.
                    //
                    // Fitting is done by two (M=2) functions from polynomial basis:
                    //     f0 = 1
                    //     f1 = x
                    // with simple constraint on function value
                    //     f(0) = 0
                    // Basically, it just a linear fit; more complex polynomials may be used
                    // (e.g. parabolas with M=3, cubic with M=4), but even such simple fit allows
                    // us to demonstrate polynomialfit() function in action.
                    //
                    // We have:
                    // * x      set of abscissas
                    // * y      experimental data
                    // * xc     points where constraints are placed
                    // * yc     constraints on derivatives
                    // * dc     derivative indices
                    //          (0 means function itself, 1 means first derivative)
                    //
                    double[] x = new double[]{1.0,1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0.9,1.1};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] w = new double[]{1,1};
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==13 )
                        spoil_vector_by_adding_element(ref w);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_deleting_element(ref w);
                    double[] xc = new double[]{0};
                    if( _spoil_scenario==15 )
                        spoil_vector_by_value(ref xc, (double)System.Double.NaN);
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref xc, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref xc, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==18 )
                        spoil_vector_by_adding_element(ref xc);
                    if( _spoil_scenario==19 )
                        spoil_vector_by_deleting_element(ref xc);
                    double[] yc = new double[]{0};
                    if( _spoil_scenario==20 )
                        spoil_vector_by_value(ref yc, (double)System.Double.NaN);
                    if( _spoil_scenario==21 )
                        spoil_vector_by_value(ref yc, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==22 )
                        spoil_vector_by_value(ref yc, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==23 )
                        spoil_vector_by_adding_element(ref yc);
                    if( _spoil_scenario==24 )
                        spoil_vector_by_deleting_element(ref yc);
                    int[] dc = new int[]{0};
                    if( _spoil_scenario==25 )
                        spoil_vector_by_adding_element(ref dc);
                    if( _spoil_scenario==26 )
                        spoil_vector_by_deleting_element(ref dc);
                    double t = 2;
                    if( _spoil_scenario==27 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==28 )
                        t = (double)System.Double.NegativeInfinity;
                    int m = 2;
                    int info;
                    alglib.barycentricinterpolant p;
                    alglib.polynomialfitreport rep;
                    double v;

                    alglib.polynomialfitwc(x, y, w, xc, yc, dc, m, out info, out p, out rep);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.000, 0.001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_polc");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_d_spline
            //      Unconstrained fitting by penalized regression spline
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<19; _spoil_scenario++)
            {
                try
                {
                    //
                    // In this example we demonstrate penalized spline fitting of noisy data
                    //
                    // We have:
                    // * x - abscissas
                    // * y - vector of experimental data, straight line with small noise
                    //
                    double[] x = new double[]{0.00,0.10,0.20,0.30,0.40,0.50,0.60,0.70,0.80,0.90};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_adding_element(ref x);
                    if( _spoil_scenario==4 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0.10,0.00,0.30,0.40,0.30,0.40,0.62,0.68,0.75,0.95};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_adding_element(ref y);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_deleting_element(ref y);
                    int info;
                    double v;
                    alglib.spline1dinterpolant s;
                    alglib.spline1dfitreport rep;
                    double rho;

                    //
                    // Fit with VERY small amount of smoothing (rho = -5.0)
                    // and large number of basis functions (M=50).
                    //
                    // With such small regularization penalized spline almost fully reproduces function values
                    //
                    rho = -5.0;
                    if( _spoil_scenario==10 )
                        rho = (double)System.Double.NaN;
                    if( _spoil_scenario==11 )
                        rho = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==12 )
                        rho = (double)System.Double.NegativeInfinity;
                    alglib.spline1dfitpenalized(x, y, 50, rho, out info, out s, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    v = alglib.spline1dcalc(s, 0.0);
                    _TestResult = _TestResult && doc_test_real(v, 0.10, 0.01);

                    //
                    // Fit with VERY large amount of smoothing (rho = 10.0)
                    // and large number of basis functions (M=50).
                    //
                    // With such regularization our spline should become close to the straight line fit.
                    // We will compare its value in x=1.0 with results obtained from such fit.
                    //
                    rho = +10.0;
                    if( _spoil_scenario==13 )
                        rho = (double)System.Double.NaN;
                    if( _spoil_scenario==14 )
                        rho = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==15 )
                        rho = (double)System.Double.NegativeInfinity;
                    alglib.spline1dfitpenalized(x, y, 50, rho, out info, out s, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    v = alglib.spline1dcalc(s, 1.0);
                    _TestResult = _TestResult && doc_test_real(v, 0.969, 0.001);

                    //
                    // In real life applications you may need some moderate degree of fitting,
                    // so we try to fit once more with rho=3.0.
                    //
                    rho = +3.0;
                    if( _spoil_scenario==16 )
                        rho = (double)System.Double.NaN;
                    if( _spoil_scenario==17 )
                        rho = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==18 )
                        rho = (double)System.Double.NegativeInfinity;
                    alglib.spline1dfitpenalized(x, y, 50, rho, out info, out s, out rep);
                    _TestResult = _TestResult && doc_test_int(info, 1);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_d_spline");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_t_polfit_1
            //      Polynomial fitting, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<10; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{0.0,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0.00,0.05,0.26,0.32,0.33,0.43,0.60,0.60,0.77,0.98,1.02};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y);
                    int m = 2;
                    double t = 2;
                    if( _spoil_scenario==8 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==9 )
                        t = (double)System.Double.NegativeInfinity;
                    int info;
                    alglib.barycentricinterpolant p;
                    alglib.polynomialfitreport rep;
                    double v;
                    alglib.polynomialfit(x, y, 11, m, out info, out p, out rep);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.011, 0.002);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_t_polfit_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_t_polfit_2
            //      Polynomial fitting, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<14; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{0.0,0.1,0.2,0.3,0.4,0.5,0.6,0.7,0.8,0.9,1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0.00,0.05,0.26,0.32,0.33,0.43,0.60,0.60,0.77,0.98,1.02};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] w = new double[]{1,1.414213562,1,1,1,1,1,1,1,1,1};
                    if( _spoil_scenario==8 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_deleting_element(ref w);
                    double[] xc = new double[0];
                    double[] yc = new double[0];
                    int[] dc = new int[0];
                    int m = 2;
                    double t = 2;
                    if( _spoil_scenario==12 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==13 )
                        t = (double)System.Double.NegativeInfinity;
                    int info;
                    alglib.barycentricinterpolant p;
                    alglib.polynomialfitreport rep;
                    double v;
                    alglib.polynomialfitwc(x, y, w, 11, xc, yc, dc, 0, m, out info, out p, out rep);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.023, 0.002);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_t_polfit_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST lsfit_t_polfit_3
            //      Polynomial fitting, full list of parameters.
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<23; _spoil_scenario++)
            {
                try
                {
                    double[] x = new double[]{1.0,1.0};
                    if( _spoil_scenario==0 )
                        spoil_vector_by_value(ref x, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_vector_by_value(ref x, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_vector_by_value(ref x, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_vector_by_deleting_element(ref x);
                    double[] y = new double[]{0.9,1.1};
                    if( _spoil_scenario==4 )
                        spoil_vector_by_value(ref y, (double)System.Double.NaN);
                    if( _spoil_scenario==5 )
                        spoil_vector_by_value(ref y, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==6 )
                        spoil_vector_by_value(ref y, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==7 )
                        spoil_vector_by_deleting_element(ref y);
                    double[] w = new double[]{1,1};
                    if( _spoil_scenario==8 )
                        spoil_vector_by_value(ref w, (double)System.Double.NaN);
                    if( _spoil_scenario==9 )
                        spoil_vector_by_value(ref w, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==10 )
                        spoil_vector_by_value(ref w, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==11 )
                        spoil_vector_by_deleting_element(ref w);
                    double[] xc = new double[]{0};
                    if( _spoil_scenario==12 )
                        spoil_vector_by_value(ref xc, (double)System.Double.NaN);
                    if( _spoil_scenario==13 )
                        spoil_vector_by_value(ref xc, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==14 )
                        spoil_vector_by_value(ref xc, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==15 )
                        spoil_vector_by_deleting_element(ref xc);
                    double[] yc = new double[]{0};
                    if( _spoil_scenario==16 )
                        spoil_vector_by_value(ref yc, (double)System.Double.NaN);
                    if( _spoil_scenario==17 )
                        spoil_vector_by_value(ref yc, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==18 )
                        spoil_vector_by_value(ref yc, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==19 )
                        spoil_vector_by_deleting_element(ref yc);
                    int[] dc = new int[]{0};
                    if( _spoil_scenario==20 )
                        spoil_vector_by_deleting_element(ref dc);
                    int m = 2;
                    double t = 2;
                    if( _spoil_scenario==21 )
                        t = (double)System.Double.PositiveInfinity;
                    if( _spoil_scenario==22 )
                        t = (double)System.Double.NegativeInfinity;
                    int info;
                    alglib.barycentricinterpolant p;
                    alglib.polynomialfitreport rep;
                    double v;
                    alglib.polynomialfitwc(x, y, w, 2, xc, yc, dc, 1, m, out info, out p, out rep);
                    v = alglib.barycentriccalc(p, t);
                    _TestResult = _TestResult && doc_test_real(v, 2.000, 0.001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "lsfit_t_polfit_3");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_d_1
            //      Determinant calculation, real matrix, short form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    double[,] b = new double[,]{{1,2},{2,1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref b);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref b);
                    double a;
                    a = alglib.rmatrixdet(b);
                    _TestResult = _TestResult && doc_test_real(a, -3, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_d_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_d_2
            //      Determinant calculation, real matrix, full form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    double[,] b = new double[,]{{5,4},{4,5}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref b);
                    double a;
                    a = alglib.rmatrixdet(b, 2);
                    _TestResult = _TestResult && doc_test_real(a, 9, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_d_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_d_3
            //      Determinant calculation, complex matrix, short form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    alglib.complex[,] b = new alglib.complex[,]{{new alglib.complex(1,+1),2},{2,new alglib.complex(1,-1)}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref b);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref b);
                    alglib.complex a;
                    a = alglib.cmatrixdet(b);
                    _TestResult = _TestResult && doc_test_complex(a, -2, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_d_3");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_d_4
            //      Determinant calculation, complex matrix, full form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    alglib.complex a;
                    alglib.complex[,] b = new alglib.complex[,]{{new alglib.complex(0,5),4},{new alglib.complex(0,4),5}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref b);
                    a = alglib.cmatrixdet(b, 2);
                    _TestResult = _TestResult && doc_test_complex(a, new alglib.complex(0,9), 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_d_4");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_d_5
            //      Determinant calculation, complex matrix with zero imaginary part, short form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<7; _spoil_scenario++)
            {
                try
                {
                    alglib.complex a;
                    alglib.complex[,] b = new alglib.complex[,]{{9,1},{2,1}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref b);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref b);
                    a = alglib.cmatrixdet(b);
                    _TestResult = _TestResult && doc_test_complex(a, 7, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_d_5");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_t_0
            //      Determinant calculation, real matrix, full form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    double a;
                    double[,] b = new double[,]{{3,4},{-4,3}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref b);
                    a = alglib.rmatrixdet(b, 2);
                    _TestResult = _TestResult && doc_test_real(a, 25, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_t_0");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_t_1
            //      Determinant calculation, real matrix, LU, short form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<9; _spoil_scenario++)
            {
                try
                {
                    double a;
                    double[,] b = new double[,]{{1,2},{2,5}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref b);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref b);
                    int[] p = new int[]{1,1};
                    if( _spoil_scenario==7 )
                        spoil_vector_by_adding_element(ref p);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_deleting_element(ref p);
                    a = alglib.rmatrixludet(b, p);
                    _TestResult = _TestResult && doc_test_real(a, -5, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_t_1");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_t_2
            //      Determinant calculation, real matrix, LU, full form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    double a;
                    double[,] b = new double[,]{{5,4},{4,5}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (double)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (double)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref b);
                    int[] p = new int[]{0,1};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_deleting_element(ref p);
                    a = alglib.rmatrixludet(b, p, 2);
                    _TestResult = _TestResult && doc_test_real(a, 25, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_t_2");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_t_3
            //      Determinant calculation, complex matrix, full form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<5; _spoil_scenario++)
            {
                try
                {
                    alglib.complex a;
                    alglib.complex[,] b = new alglib.complex[,]{{new alglib.complex(0,5),4},{-4,new alglib.complex(0,5)}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref b);
                    a = alglib.cmatrixdet(b, 2);
                    _TestResult = _TestResult && doc_test_complex(a, -9, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_t_3");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_t_4
            //      Determinant calculation, complex matrix, LU, short form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<9; _spoil_scenario++)
            {
                try
                {
                    alglib.complex a;
                    alglib.complex[,] b = new alglib.complex[,]{{1,2},{2,new alglib.complex(0,5)}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_adding_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_adding_col(ref b);
                    if( _spoil_scenario==5 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==6 )
                        spoil_matrix_by_deleting_col(ref b);
                    int[] p = new int[]{1,1};
                    if( _spoil_scenario==7 )
                        spoil_vector_by_adding_element(ref p);
                    if( _spoil_scenario==8 )
                        spoil_vector_by_deleting_element(ref p);
                    a = alglib.cmatrixludet(b, p);
                    _TestResult = _TestResult && doc_test_complex(a, new alglib.complex(0,-5), 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_t_4");
            _TotalResult = _TotalResult && _TestResult;


            //
            // TEST matdet_t_5
            //      Determinant calculation, complex matrix, LU, full form
            //
            _TestResult = true;
            for(_spoil_scenario=-1; _spoil_scenario<6; _spoil_scenario++)
            {
                try
                {
                    alglib.complex a;
                    alglib.complex[,] b = new alglib.complex[,]{{5,new alglib.complex(0,4)},{4,5}};
                    if( _spoil_scenario==0 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NaN);
                    if( _spoil_scenario==1 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.PositiveInfinity);
                    if( _spoil_scenario==2 )
                        spoil_matrix_by_value(ref b, (alglib.complex)System.Double.NegativeInfinity);
                    if( _spoil_scenario==3 )
                        spoil_matrix_by_deleting_row(ref b);
                    if( _spoil_scenario==4 )
                        spoil_matrix_by_deleting_col(ref b);
                    int[] p = new int[]{0,1};
                    if( _spoil_scenario==5 )
                        spoil_vector_by_deleting_element(ref p);
                    a = alglib.cmatrixludet(b, p, 2);
                    _TestResult = _TestResult && doc_test_complex(a, 25, 0.0001);
                    _TestResult = _TestResult && (_spoil_scenario==-1);
                }
                catch(alglib.alglibexception)
                { _TestResult = _TestResult && (_spoil_scenario!=-1); }
                catch
                { throw; }
            }
            if( !_TestResult)
                System.Console.WriteLine("{0,-32} FAILED", "matdet_t_5");
            _TotalResult = _TotalResult && _TestResult;


            System.Console.WriteLine("91/91");
        }
        catch
        {
            System.Console.WriteLine("Unhandled exception was raised!");
            return 1;
        }
        return _TotalResult ? 0 : 1;
    }
}
