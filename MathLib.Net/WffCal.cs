using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace MathLib.Net
{
    public static class WffCal
    {
        #region C++ 动态链接库接口
        private const string DllName = "MathLib.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void SaveMat1(double[] data, int len, string fileName);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void SaveIMat1(int[] data, int len, string fileName);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void SaveMat2(double[] data, int row, int col, string fileName);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void SaveIMat2(int[] data, int row, int col, string fileName);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern double Random(int flag);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void RandomU2(double[] input, int row, int col);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void RandomU1(double[] input, int len);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void RandomN2(double[] input, int row, int col);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void RandomN1(double[] input, int len);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern int RandomInt(int maxi);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void RandomInt2(int[] input, int maxi, int row, int col);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void RandomInt1(int[] input, int maxi, int len);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern double Stddev1(double[] input, int len);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Stddev2(double[] input, int row, int col, int flag, double[] result);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern double Var1(double[] input, int len);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Var2(double[] input, int row, int col, int flag, double[] result);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern int Rank1(double[] input, int row, int col);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern int Rank2(double[] input, int row, int col, double tolerance);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Kron(double[] input1, int row1, int col1, double[] input2, int row2, int col2, double[] result);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Svd(double[] input, int row, int col, double[] u, double[] s, double[] v);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Interp1(double[] x, double[] y, int len1, double[] xi, int len2, int flag, double[] yi);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Sort(double[] input, int len, int flag, double[] result);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void SortIndex(double[] input, int len, int flag, int[] result);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Fft(double[] x, double[] y, int len, double[] xx, double[] yy, double[] amp);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Ifft(double[] x, double[] y, int len, double[] xx, double[] yy, double[] amp);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Polyfit(double[] x, double[] y, int len, int N, double[] p);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Polyval(double[] x, int len1, double[] p, int len2, double[] y);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Solve(double[] A, int m, int n, double[] y, double[] x);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Lsqnonneg(double[] A, int m, int n, double[] y, double[] x);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void ExpFitting(double[] x, double[] y, int len, int N, double[] p);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void GaussFitting(double[] x, double[] y, int M, double[] p);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Conv2(double[] input1, int row1, int col1, double[] input2, int row2, int col2, double[] output, int row3, int col3);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Wden(double[] Input, double[] Output, int signal_size);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Wdbn(double[] Input, double[] Output, int signal_size, int scale, int dbn, bool isHard);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Wdbn2(double[] Input, double[] Output, int height, int width, int scale, int dbn);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void WHaar(double[] Input, double[] Output, int signal_size, int scale);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Nlm(double[] Input, double[] Output, int signal_size, int window_width, int patch_width, double sigma);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void Nlm2(double[] Input, double[] Output, int height, int width, int window_width, int patch_width, double sigma);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void InvSIRT(double[] TArray, double[] MeaAmp, int M, int N, int iters, double[] T2Dist);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void InvBRD(double[] TArray, double[] MeaAmp, int M, int N, double a0, double[] T2Dist);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void InvCG(double[] TArray, double[] MeaAmp, int M, int N, int iters, double[] T2Dist);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)] private static extern void InvSVD(double[] TArray, double[] MeaAmp, int M, int N, double a0, double[] T2Dist);


        //_declspec(dllexport) void InvT1T2(double* EH, double* tau1, int Nw, double* tau2, int NECH, double Tmin, double Tmax, double alpha, int Nt, int invModel, double* T2T1Dist, double* EHfit, double* T, double* T1Dist, double* T2Dist, double& ReError);
        //_declspec(dllexport) void InvDT2(double* EH, double* Gk2, int Ng, double* tau, int NECH, double Dmin, double Dmax, int Nd, double Tmin, double Tmax, double alpha, int Nt, double Delta1, double delta2, double* T2DDist, double* EHfit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError);
        //_declspec(dllexport) void InvDT2_Gcst(double* SEQtau2, double* SEQsig, int* NECH, int N_echo, double* TE, double Gcst, double Dmin, double Dmax, double Tmin, double Tmax, int nPreset, double alpha, double* T2DDist, double* d_fit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError);
        #endregion


        public const double EPS = 2.2204e-16;

        #region cs 外部调用接口
        /// <summary>
        /// int --> double
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double[] Double(int[] data)
        {
            int len = data.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = data[i];
            }
            return result;
        }

        /// <summary>
        /// int --> double
        /// </summary>
        /// <returns></returns>
        public static double[,] Double(int[,] data)
        {
            int row = data.GetLength(0);
            int col = data.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = data[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// double --> int
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int[] Int(double[] data)
        {
            int len = data.Length;
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = (int)data[i];
            }
            return result;
        }

        /// <summary>
        /// double --> int
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static int[,] Int(double[,] data)
        {
            int row = data.GetLength(0);
            int col = data.GetLength(1);
            int[,] result = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = (int)data[i, j];
                }
            }
            return result;
        }



        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Save(this double[] data, string fileName)
        {
            SaveMat1(data, data.Length, fileName);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Save(this int[] data, string fileName)
        {
            SaveIMat1(data, data.Length, fileName);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Save(this double[,] data, string fileName)
        {
            int row = data.GetLength(0);
            int col = data.GetLength(1);
            double[] mData = Reshape(data);
            SaveMat2(mData, row, col, fileName);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fileName"></param>
        public static void Save(this int[,] data, string fileName)
        {
            int row = data.GetLength(0);
            int col = data.GetLength(1);
            int[] mData = Reshape(data);
            SaveIMat2(mData, row, col, fileName);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static double[] LoadVec(string fileName)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            List<string> listLines = new List<string>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    listLines.Add(reader.ReadLine());
                }
            }
            file.Close();
            int len = listLines.Count;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                string[] strs = new Regex("[\\s]+").Replace(listLines[i].Trim(), " ").Split(' ');
                result[i] = double.Parse(strs[0]);
            }
            return result;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int[] LoadIVec(string fileName)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            List<string> listLines = new List<string>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    listLines.Add(reader.ReadLine());
                }
            }
            file.Close();
            int len = listLines.Count;
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                string[] strs = new Regex("[\\s]+").Replace(listLines[i].Trim(), " ").Split(' ');
                result[i] = int.Parse(strs[0]);
            }
            return result;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static double[,] LoadMat(string fileName)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            List<string> listLines = new List<string>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    listLines.Add(reader.ReadLine());
                }
            }
            file.Close();
            int row = listLines.Count;
            string[] strs = new Regex("[\\s]+").Replace(listLines[0].Trim(), " ").Split(' ');
            int col = strs.Length;
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                strs = new Regex("[\\s]+").Replace(listLines[i].Trim(), " ").Split(' ');
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = double.Parse(strs[j]);
                }
            }
            return result;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static int[,] LoadIMat(string fileName)
        {
            FileStream file = File.Open(fileName, FileMode.Open);
            List<string> listLines = new List<string>();
            using (StreamReader reader = new StreamReader(file))
            {
                while (!reader.EndOfStream)
                {
                    listLines.Add(reader.ReadLine());
                }
            }
            file.Close();
            int row = listLines.Count;
            string[] strs = new Regex("[\\s]+").Replace(listLines[0].Trim(), " ").Split(' ');
            int col = strs.Length;
            int[,] result = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                strs = new Regex("[\\s]+").Replace(listLines[i].Trim(), " ").Split(' ');
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = int.Parse(strs[j]);
                }
            }
            return result;
        }



        /// <summary>
        /// 均匀分布随机数
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double Rand()
        {
            return Random(0);
        }

        /// <summary>
        /// 均匀分布随机数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double[,] Rand(int row, int col)
        {
            double[] mat = new double[row * col];
            RandomU2(mat, row, col);
            double[,] result = mat.Reshape(row, col);
            return result;
        }

        /// <summary>
        /// 均匀分布随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[] Rand(int len)
        {
            double[] result = new double[len];
            RandomU1(result, len);
            return result;
        }

        /// <summary>
        /// 高斯分布随机数
        /// </summary>
        /// <returns></returns>
        public static double Randn()
        {
            return Random(1);
        }

        /// <summary>
        /// 高斯分布随机数
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double[,] Randn(int row, int col)
        {
            double[] mat = new double[row * col];
            RandomN2(mat, row, col);
            double[,] result = mat.Reshape(row, col);
            return result;
        }

        /// <summary>
        /// 高斯分布随机数
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[] Randn(int len)
        {
            double[] result = new double[len];
            RandomN1(result, len);
            return result;
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <param name="maxi"></param>
        /// <returns></returns>
        public static int Randi(int maxi)
        {
            return RandomInt(maxi);
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <param name="maxi"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int[,] Randi(int maxi, int row, int col)
        {
            int[] imat = new int[row * col];
            RandomInt2(imat, maxi, row, col);
            int[,] result = imat.Reshape(row, col);
            return result;
        }

        /// <summary>
        /// 随机整数
        /// </summary>
        /// <param name="maxi"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int[] Randi(int maxi, int len)
        {
            int[] result = new int[len];
            RandomInt1(result, maxi, len);
            return result;
        }



        /// <summary>
        /// 均值：按列、按行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[] Mean(this double[,] input, int flag = 0)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] result;
            if (flag == 0)
            {
                result = new double[col];
                for (int j = 0; j < col; j++)
                {
                    double sumi = 0;
                    for (int i = 0; i < row; i++)
                    {
                        sumi += input[i, j];
                    }
                    result[j] = sumi / row;
                }
            }
            else
            {
                result = new double[row];
                for (int i = 0; i < row; i++)
                {
                    double sumi = 0;
                    for (int j = 0; j < col; j++)
                    {
                        sumi += input[i, j];
                    }
                    result[i] = sumi / col;
                }
            }
            return result;
        }

        /// <summary>
        /// 均值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Mean(this double[] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result += input[i];
            }
            result /= input.Length;
            return result;
        }

        /// <summary>
        /// 标准差
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Std(this double[] input)
        {
            return Stddev1(input, input.Length);
        }

        /// <summary>
        /// 标准差：按列、按行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[] Std(this double[,] input, int flag = 0)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] result;
            if (flag == 0)
            {
                result = new double[col];
            }
            else
            {
                flag = 1;
                result = new double[row];
            }
            double[] data = input.Reshape();
            Stddev2(data, row, col, flag, result);
            return result;
        }

        /// <summary>
        /// 方差
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Var(this double[] input)
        {
            return Var1(input, input.Length);
        }

        /// <summary>
        /// 方差：按列、按行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[] Var(this double[,] input, int flag = 0)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] result;
            if (flag == 0)
            {
                result = new double[col];
            }
            else
            {
                flag = 1;
                result = new double[row];
            }
            double[] data = input.Reshape();
            Var2(data, row, col, flag, result);
            return result;
        }



        /// <summary>
        /// 矩阵的秩
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int Rank(this double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] data = input.Reshape();
            int r = Rank1(data, row, col);
            return r;
        }

        /// <summary>
        /// 矩阵的秩
        /// </summary>
        /// <param name="input"></param>
        /// <param name="tolerance"></param>
        /// <returns></returns>
        public static int Rank(this double[,] input, double tolerance)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] data = input.Reshape();
            int r = Rank2(data, row, col, tolerance);
            return r;
        }

        /// <summary>
        /// kron 乘积
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] Kron(double[,] mat1, double[,] input)
        {
            int row1 = mat1.GetLength(0);
            int col1 = mat1.GetLength(1);
            int row2 = input.GetLength(0);
            int col2 = input.GetLength(1);
            double[] input1 = mat1.Reshape();
            double[] input2 = input.Reshape();
            int row = row1 * row2;
            int col = col1 * col2;
            double[] output = new double[row * col];
            Kron(input1, row1, col1, input2, row2, col2, output);
            double[,] result = output.Reshape(row, col);
            return result;
        }

        /// <summary>
        /// SVD 奇异值分解
        /// </summary>
        /// <param name="input"></param>
        /// <param name="u"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        public static void Svd(double[,] input, out double[,] u, out double[,] s, out double[,] v)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            int len = Math.Min(row, col);
            double[] data = input.Reshape();
            double[] u1 = new double[row * row];
            double[] s1 = new double[len];
            double[] v1 = new double[col * col];
            Svd(data, row, col, u1, s1, v1);
            u = u1.Reshape(row, row);
            s = new double[row, col];
            for (int i = 0; i < len; i++)
            {
                s[i, i] = s1[i];
            }
            v = v1.Reshape(col, col);
        }

        /// <summary>
        /// 一维插值：最近邻(nearest)、线性(linear)、快速最近邻(*nearest)、快速线性(*linear)、样条(spline)
        /// </summary>
        /// <param name="xi"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static double[] Interp1(double[] xi, double[] x, double[] y, string method = "linear")
        {
            int len2 = xi.Length;
            int len1 = x.Length;
            int flag = 2;
            if (method == "nearest")
            {
                flag = 1;
            }
            else if (method == "*nearest")
            {
                flag = 3;
            }
            else if (method == "*linear")
            {
                flag = 4;
            }
            else if (method == "spline")
            {
                flag = 5;
            }
            double[] result = new double[len2];
            Interp1(x, y, len1, xi, len2, flag, result);
            return result;
        }

        /// <summary>
        /// 排序：升序、降序
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static double[] Sort(double[] input, string sortDirection = "ascend")
        {
            int len = input.Length;
            double[] result = new double[len];
            int flag = 1;
            if (sortDirection == "descend")
            {
                flag = 2;
            }
            Sort(input, len, flag, result);
            return result;
        }

        /// <summary>
        /// 排序：升序小标、降序下标
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static int[] SortIndex(double[] input, string sortDirection = "ascend")
        {
            int len = input.Length;
            int[] result = new int[len];
            int flag = 1;
            if (sortDirection == "descend")
            {
                flag = 2;
            }
            SortIndex(input, len, flag, result);
            return result;
        }



        /// <summary>
        /// fft 傅里叶变换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="realPart"></param>
        /// <param name="imagPart"></param>
        /// <param name="moduPart"></param>
        public static void FFT(double[] x, double[] y, out double[] realPart, out double[] imagPart, out double[] moduPart)
        {
            int len = x.Length;
            realPart = new double[len];
            imagPart = new double[len];
            moduPart = new double[len];
            Fft(x, y, len, realPart, imagPart, moduPart);
        }

        /// <summary>
        /// ifft 傅里叶逆变换
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="realPart"></param>
        /// <param name="imagPart"></param>
        /// <param name="moduPart"></param>
        public static void IFFT(double[] x, double[] y, out double[] realPart, out double[] imagPart, out double[] moduPart)
        {
            int len = x.Length;
            realPart = new double[len];
            imagPart = new double[len];
            moduPart = new double[len];
            Ifft(x, y, len, realPart, imagPart, moduPart);
        }

        /// <summary>
        /// fftshift 将零频分量移至中心
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] FFTShift(this double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            int mid = (len + 1) / 2;
            for (int i = 0; i < len - mid; i++)
            {
                result[i] = input[mid + i];
            }
            for (int i = len - mid; i < len; i++)
            {
                result[i] = input[i + mid - len];
            }
            return result;
        }

        /// <summary>
        /// ifftshift 将 fftshift 还原
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] IFFTShift(this double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            int mid = (len + 1) / 2;
            for (int i = 0; i < mid; i++)
            {
                result[i] = input[len - mid + i];
            }
            for (int i = mid; i < len; i++)
            {
                result[i] = input[i - mid];
            }
            return result;
        }



        /// <summary>
        /// 多项式拟合
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static double[] Polyfit(double[] x, double[] y, int N)
        {
            int len = x.Length;
            double[] result = new double[N + 1];
            Polyfit(x, y, len, N, result);
            return result;
        }

        /// <summary>
        /// 多项式函数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double[] Polyval(double[] x, double[] p)
        {
            int len1 = x.Length;
            int len2 = p.Length;
            double[] result = new double[len1];
            Polyval(x, len1, p, len2, result);
            return result;
        }

        /// <summary>
        /// 左除：用 SVD 解线性方程组
        /// </summary>
        /// <param name="A"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double[] Solve(double[,] A, double[] y)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            double[] data = A.Reshape();
            double[] x = new double[n];
            Solve(data, m, n, y, x);
            return x;
        }

        /// <summary>
        /// 非负最小二乘
        /// </summary>
        /// <param name="C"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static double[] Lsqnonneg(double[,] C, double[] d)
        {
            int row = C.GetLength(0);
            int col = C.GetLength(1);
            int len = d.Length;
            double[] x = new double[col];
            if (row == len)
            {
                double[] input = C.Reshape();
                Lsqnonneg(input, row, col, d, x);
            }
            return x;
        }

        /// <summary>
        /// 指数衰减拟合
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static double[] ExpFitting(double[] x, double[] y, int N)
        {
            double[] result = new double[2 * N + 1];
            if (N <= 3)
            {
                ExpFitting(x, y, x.Length, N, result);
            }
            return result;
        }

        /// <summary>
        /// 指数衰减函数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static double[] ExpFitting(double[] x, double[] p)
        {
            int len = x.Length;
            int N = p.Length / 2;
            double[] result = new double[len];
            if (N <= 3)
            {
                for (int i = 0; i < len; i++)
                {
                    if (N == 1)
                    {
                        result[i] = p[0] * Math.Exp(-x[i] / p[1]) + p[2];
                    }
                    else if (N == 2)
                    {
                        result[i] = p[0] * Math.Exp(-x[i] / p[1]) + p[2] * Math.Exp(-x[i] / p[3]) + p[4];
                    }
                    else
                    {
                        result[i] = p[0] * Math.Exp(-x[i] / p[1]) + p[2] * Math.Exp(-x[i] / p[3]) + p[4] * Math.Exp(-x[i] / p[5]) + p[6];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 高斯拟合
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double[] GaussFitting(double[] x, double[] y)
        {
            double[] result = new double[3];
            GaussFitting(x, y, x.Length, result);
            return result;
        }

        /// <summary>
        /// 高斯函数值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static double[] GaussFitting(double[] x, double a, double b, double c)
        {
            int len = x.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Math.Pow(a, 2) * Math.Exp(-Math.Pow(x[i] - b, 2) / Math.Pow(c, 2));
            }
            return result;
        }



        /// <summary>
        /// 卷积
        /// </summary>
        /// <param name="x"></param>
        /// <param name="flag"></param>
        /// <param name="shape"></param>
        /// <returns></returns>
        public static double[,] Conv2(double[,] x, double[,] flag, string shape = "full")
        {
            int row1 = x.GetLength(0);
            int col1 = x.GetLength(1);
            int row2 = flag.GetLength(0);
            int col2 = flag.GetLength(1);
            int row3 = row1 + row2 - 1;
            int col3 = col1 + col2 - 1;
            if (shape == "same")
            {
                row3 = row1;
                col3 = col1;
            }
            double[] input1 = x.Reshape();
            double[] input2 = flag.Reshape();
            double[] output = new double[row3 * col3];
            Conv2(input1, row1, col1, input2, row2, col2, output, row3, col3);
            double[,] result = output.Reshape(row3, row3);
            return result;
        }

        /// <summary>
        /// 一维 sym8 小波滤波函数，四层全局软阈值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Wden(double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            Wden(input, result, len);
            return result;
        }

        /// <summary>
        /// 一维 db 小波滤波函数，多层级
        /// </summary>
        /// <param name="input"></param>
        /// <param name="scale"></param>
        /// <param name="dbn"></param>
        /// <param name="isHard"></param>
        /// <returns></returns>
        public static double[] Wdbn(double[] input, int scale, int dbn, bool isHard = true)
        {
            int len = input.Length;
            double[] result = new double[len];
            Wdbn(input, result, len, scale, dbn, isHard);
            return result;
        }

        /// <summary>
        /// 图像 db 小波滤波函数，多层级，全局软阈值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="scale"></param>
        /// <param name="dbn"></param>
        /// <returns></returns>
        public static double[,] Wdbn2(double[,] input, int scale, int dbn)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] data = input.Reshape();
            double[] result = new double[row * col];
            Wdbn2(data, result, row, col, scale, dbn);
            return result.Reshape(row, col);
        }

        /// <summary>
        /// 平移不变小波滤波，多层级
        /// length(signal) = N * 2^L;
        /// </summary>
        public static double[] WHaar(double[] input, int signal_size, int scale)
        {
            int len = input.Length;
            double[] result = new double[len];
            WHaar(input, result, signal_size, scale);
            return result;
        }

        /// <summary>
        /// 一维非局部均值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="windowWidth"></param>
        /// <param name="patchWidth"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double[] Nlm(double[] input, int windowWidth, int patchWidth, double sigma)
        {
            int len = input.Length;
            double[] result = new double[len];
            Nlm(input, result, len, windowWidth, patchWidth, sigma);
            return result;
        }

        /// <summary>
        /// 非局部均值图像滤波
        /// </summary>
        /// <param name="input"></param>
        /// <param name="windowWidth"></param>
        /// <param name="patchWidth"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public static double[,] Nlm2(double[,] input, int windowWidth, int patchWidth, double sigma)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] data = input.Reshape();
            double[] result = new double[row * col];
            Nlm2(data, result, row, col, windowWidth, patchWidth, sigma);
            return result.Reshape(row, col);
        }



        /// <summary>
        /// 矩阵维度变换 不改变总长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Reshape(this double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[] result = new double[row * col];
            for (int i = 0; i < row * col; i++)
            {
                result[i] = input[i / col, i % col];
            }
            return result;
        }

        /// <summary>
        /// 矩阵维度变换 不改变总长度
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[] Reshape(this int[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            int[] result = new int[row * col];
            for (int i = 0; i < row * col; i++)
            {
                result[i] = input[i / col, i % col];
            }
            return result;
        }

        /// <summary>
        /// 矩阵维度变换 不改变总长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double[,] Reshape(this double[] input, int row, int col)
        {
            double[,] output = new double[row, col];
            for (int i = 0; i < row * col; i++)
            {
                output[i / col, i % col] = input[i];
            }
            return output;
        }

        /// <summary>
        /// 矩阵维度变换 不改变总长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int[,] Reshape(this int[] input, int row, int col)
        {
            int[,] output = new int[row, col];
            for (int i = 0; i < row * col; i++)
            {
                output[i / col, i % col] = input[i];
            }
            return output;
        }

        /// <summary>
        /// 矩阵维度变换 不改变总长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double[,] Reshape(this double[,] input, int row, int col)
        {
            double[] val = Reshape(input);
            double[,] result = Reshape(val, row, col);
            return result;
        }

        /// <summary>
        /// 矩阵维度变换 不改变总长度
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int[,] Reshape(this int[,] input, int row, int col)
        {
            int[] val = Reshape(input);
            int[,] result = Reshape(val, row, col);
            return result;
        }



        /// <summary>
        /// sirt 迭代算法
        /// </summary>
        /// <param name="C"></param>
        /// <param name="d"></param>
        /// <param name="iter"></param>
        /// <returns></returns>
        public static double[] SIRT(double[,] C, double[] d, int iter)
        {
            int row = C.GetLength(0);
            int col = C.GetLength(1);
            double[] result = new double[col];
            double[] input = C.Reshape();
            InvSIRT(input, d, row, col, iter, result);
            return result;
        }

        /// <summary>
        /// 罚函数 brd
        /// </summary>
        /// <param name="C"></param>
        /// <param name="d"></param>
        /// <param name="a0"></param>
        /// <returns></returns>
        public static double[] BRD(double[,] C, double[] d, double a0)
        {
            int row = C.GetLength(0);
            int col = C.GetLength(1);
            double[] result = new double[col];
            double[] input = C.Reshape();
            InvBRD(input, d, row, col, a0, result);
            return result;
        }

        /// <summary>
        /// 共轭梯度 cg
        /// </summary>
        /// <param name="C"></param>
        /// <param name="d"></param>
        /// <param name="iter"></param>
        /// <returns></returns>
        public static double[] CG(double[,] C, double[] d, int iter)
        {
            int row = C.GetLength(0);
            int col = C.GetLength(1);
            double[] result = new double[col];
            double[] input = C.Reshape();
            InvCG(input, d, row, col, iter, result);
            return result;
        }

        /// <summary>
        /// 奇异值分解 svd
        /// </summary>
        /// <param name="C"></param>
        /// <param name="d"></param>
        /// <param name="a0"></param>
        /// <returns></returns>
        public static double[] TSVD(double[,] C, double[] d, double a0)
        {
            int row = C.GetLength(0);
            int col = C.GetLength(1);
            double[] result = new double[col];
            double[] input = C.Reshape();
            InvSVD(input, d, row, col, a0, result);
            return result;
        }



        /// <summary>
        /// 所有元素求和
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Accu(this double[] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result += input[i];
            }
            return result;
        }

        /// <summary>
        /// 所有元素求和
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Accu(this double[,] input)
        {
            double result = 0;
            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    result += input[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 按列、按行求和
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[] Sum(this double[,] input, int flag = 0)
        {
            double[] result;
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            // 按列求和
            if (flag == 0)
            {
                result = new double[col];
                for (int j = 0; j < col; j++)
                {
                    result[j] = 0;
                    for (int i = 0; i < row; i++)
                    {
                        result[j] = result[j] + input[i, j];
                    }
                }
            }
            else
            {
                result = new double[row];
                for (int i = 0; i < row; i++)
                {
                    result[i] = 0;
                    for (int j = 0; j < col; j++)
                    {
                        result[i] = result[i] + input[i, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 按列、按行最大值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[] Max(this double[,] input, int flag = 0)
        {
            double[] result;
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            // 按列最大值
            if (flag == 0)
            {
                result = new double[col];
                for (int j = 0; j < col; j++)
                {
                    double maxi = input[0, j];
                    for (int i = 1; i < row; i++)
                    {
                        if (maxi < input[i, j])
                        {
                            maxi = input[i, j];
                        }
                    }
                    result[j] = maxi;
                }
            }
            else
            {
                result = new double[row];
                for (int i = 0; i < row; i++)
                {
                    double maxi = input[i, 0];
                    for (int j = 1; j < col; j++)
                    {
                        if (maxi < input[i, j])
                        {
                            maxi = input[i, j];
                        }
                    }
                    result[i] = maxi;
                }
            }
            return result;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Max(this double[] data)
        {
            int len = data.Length;
            double result = data[0];
            for (int i = 1; i < len; i++)
            {
                if (result < data[i])
                {
                    result = data[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double PeakValue(this double[,] data)
        {
            int row = data.GetLength(0);
            int col = data.GetLength(1);
            double result = data[0, 0];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (result < data[i, j])
                    {
                        result = data[i, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 按列、按行最小值
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[] Min(this double[,] input, int flag = 0)
        {
            double[] result;
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            // 按列最大值
            if (flag == 0)
            {
                result = new double[col];
                for (int j = 0; j < col; j++)
                {
                    double mini = input[0, j];
                    for (int i = 1; i < row; i++)
                    {
                        if (mini > input[i, j])
                        {
                            mini = input[i, j];
                        }
                    }
                    result[j] = mini;
                }
            }
            else
            {
                result = new double[row];
                for (int i = 0; i < row; i++)
                {
                    double mini = input[i, 0];
                    for (int j = 1; j < col; j++)
                    {
                        if (mini > input[i, j])
                        {
                            mini = input[i, j];
                        }
                    }
                    result[i] = mini;
                }
            }
            return result;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double Min(this double[] data)
        {
            int len = data.Length;
            double result = data[0];
            for (int i = 1; i < len; i++)
            {
                if (result > data[i])
                {
                    result = data[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double MinValue(this double[,] data)
        {
            int row = data.GetLength(0);
            int col = data.GetLength(1);
            double result = data[0, 0];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (result > data[i, j])
                    {
                        result = data[i, j];
                    }
                }
            }
            return result;
        }


        /// <summary>
        /// 按列、按行最大值下标
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int[] MaxIndex(this double[,] input, int flag = 0)
        {
            int[] result;
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            // 按列最大值下标
            if (flag == 0)
            {
                result = new int[col];
                for (int j = 0; j < col; j++)
                {
                    double maxi = input[0, j];
                    int index = 0;
                    for (int i = 1; i < row; i++)
                    {
                        if (maxi < input[i, j])
                        {
                            maxi = input[i, j];
                            index = i;
                        }
                    }
                    result[j] = index;
                }
            }
            else
            {
                result = new int[row];
                for (int i = 0; i < row; i++)
                {
                    double maxi = input[i, 0];
                    int index = 0;
                    for (int j = 1; j < col; j++)
                    {
                        if (maxi < input[i, j])
                        {
                            maxi = input[i, j];
                            index = j;
                        }
                    }
                    result[i] = index;
                }
            }
            return result;
        }

        /// <summary>
        /// 按列、按行最小值下标
        /// </summary>
        /// <param name="input"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int[] MinIndex(this double[,] input, int flag = 0)
        {
            int[] result;
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            // 按列最大值下标
            if (flag == 0)
            {
                result = new int[col];
                for (int j = 0; j < col; j++)
                {
                    double mini = input[0, j];
                    int index = 0;
                    for (int i = 1; i < row; i++)
                    {
                        if (mini > input[i, j])
                        {
                            mini = input[i, j];
                            index = i;
                        }
                    }
                    result[j] = index;
                }
            }
            else
            {
                result = new int[row];
                for (int i = 0; i < row; i++)
                {
                    double mini = input[i, 0];
                    int index = 0;
                    for (int j = 1; j < col; j++)
                    {
                        if (mini > input[i, j])
                        {
                            mini = input[i, j];
                            index = j;
                        }
                    }
                    result[i] = index;
                }
            }
            return result;
        }

        /// <summary>
        /// 最大值下标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int MaxIndex(this double[] input)
        {
            int result = 0;
            double maxi = input[0];
            for (int i = 1; i < input.Length; i++)
            {
                if (maxi < input[i])
                {
                    maxi = input[i];
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// 最小值下标
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int MinIndex(this double[] input)
        {
            int result = 0;
            double mini = input[0];
            for (int i = 1; i < input.Length; i++)
            {
                if (mini > input[i])
                {
                    mini = input[i];
                    result = i;
                }
            }
            return result;
        }

        /// <summary>
        /// 单位矩阵
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[,] Eye(int len)
        {
            double[,] result = new double[len, len];
            for (int i = 0; i < len; i++)
            {
                result[i, i] = 1.0;
            }
            return result;
        }

        /// <summary>
        /// 单位矩阵
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int[,] EyeInt(int len)
        {
            int[,] result = new int[len, len];
            for (int i = 0; i < len; i++)
            {
                result[i, i] = 1;
            }
            return result;
        }

        /// <summary>
        /// 矩阵初始化为 1
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static double[,] Ones(int row, int col)
        {
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵初始化为 1
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[] Ones(int len)
        {
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = 1;
            }
            return result;
        }

        /// <summary>
        /// 矩阵初始化为 1
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static int[,] OnesInt(int row, int col)
        {
            int[,] result = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = 1;
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵初始化为 1
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static int[] OnesInt(int len)
        {
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = 1;
            }
            return result;
        }

        /// <summary>
        /// 对角阵
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] Diag(double[] input)
        {
            int len = input.Length;
            double[,] result = new double[len, len];
            for (int i = 0; i < len; i++)
            {
                result[i, i] = input[i];
            }
            return result;
        }

        /// <summary>
        /// 对角阵
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Diag(double[,] input)
        {
            int len = Math.Min(input.GetLength(0), input.GetLength(1));
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = input[i, i];
            }
            return result;
        }

        /// <summary>
        /// 对角阵
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[,] Diag(int[] input)
        {
            int len = input.Length;
            int[,] result = new int[len, len];
            for (int i = 0; i < len; i++)
            {
                result[i, i] = input[i];
            }
            return result;
        }

        /// <summary>
        /// 对角阵
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[] Diag(int[,] input)
        {
            int len = Math.Min(input.GetLength(0), input.GetLength(1));
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = input[i, i];
            }
            return result;
        }

        /// <summary>
        /// 线性等分
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[] Linspace(double a, double b, int len)
        {
            double[] result = new double[len];
            if (len > 1)
            {
                for (int i = 0; i < len; i++)
                {
                    result[i] = ((b - a) * i / (len - 1)) + a;
                }
            }
            return result;
        }

        /// <summary>
        /// 对数等分
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="len"></param>
        /// <returns></returns>
        public static double[] Logspace(double a, double b, int len)
        {
            double[] result = Linspace(a, b, len);
            for (int i = 0; i < len; i++)
            {
                result[i] = Math.Pow(10, result[i]);
            }
            return result;
        }

        /// <summary>
        /// 子矩阵
        /// </summary>
        /// <param name="input"></param>
        /// <param name="row1"></param>
        /// <param name="row2"></param>
        /// <param name="col1"></param>
        /// <param name="col2"></param>
        /// <returns></returns>
        public static double[,] SubMat(double[,] input, int row1, int row2, int col1, int col2)
        {
            double[,] result = new double[row2 - row1 + 1, col2 - col1 + 1];
            for (int i = row1; i <= row2; i++)
            {
                for (int j = col1; j <= col2; j++)
                {
                    result[i - row1, j - col1] = input[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 子矩阵
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        /// <returns></returns>
        public static double[] SubVec(double[] input, int index1, int index2)
        {
            double[] result = new double[index2 - index1 + 1];
            for (int i = index1; i <= index2; i++)
            {
                result[i - index1] = input[i];
            }
            return result;
        }

        /// <summary>
        /// 某行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static double[] Row(double[,] input, int index)
        {
            int col = input.GetLength(1);
            double[] result = new double[col];
            for (int j = 0; j < col; j++)
            {
                result[j] = input[index, j];
            }
            return result;
        }

        /// <summary>
        /// 某列
        /// </summary>
        /// <param name="input"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static double[] Col(this double[,] input, int index)
        {
            int row = input.GetLength(0);
            double[] result = new double[row];
            for (int i = 0; i < row; i++)
            {
                result[i] = input[i, index];
            }
            return result;
        }

        /// <summary>
        /// bool 矩阵
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool[] True(int len)
        {
            bool[] result = new bool[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = true;
            }
            return result;
        }

        /// <summary>
        /// bool 矩阵
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool[,] True(int row, int col)
        {
            bool[,] result = new bool[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = true;
                }
            }
            return result;
        }

        /// <summary>
        /// bool 矩阵
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static bool[] False(int len)
        {
            bool[] result = new bool[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = false;
            }
            return result;
        }

        /// <summary>
        /// bool 矩阵
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        public static bool[,] False(int row, int col)
        {
            bool[,] result = new bool[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = false;
                }
            }
            return result;
        }



        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Copy(double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = input[i];
            }
            return result;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[] Copy(int[] input)
        {
            int len = input.Length;
            int[] result = new int[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = input[i];
            }
            return result;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] Copy(double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = input[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[,] Copy(int[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            int[,] result = new int[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = input[i, j];
                }
            }
            return result;
        }



        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[] MatMul(double[] input, double k)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = k * input[i];
            }
            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[,] MatMul(double[,] input, double k)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = input[i, j] * k;
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] MatMul(double[] vec1, double[] input)
        {
            int len1 = vec1.Length;
            int len2 = input.Length;
            double[] result = new double[len1];
            if (len1 == len2)
            {
                for (int i = 0; i < len1; i++)
                {
                    result[i] = vec1[i] * input[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] MatMul(double[,] mat1, double[] input)
        {
            int row = mat1.GetLength(0);
            int col = mat1.GetLength(1);
            double[] result = new double[row];
            for (int i = 0; i < row; i++)
            {
                result[i] = 0;
                for (int j = 0; j < col; j++)
                {
                    result[i] = result[i] + (mat1[i, j] * input[j]);
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵乘法
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] MatMul(double[,] mat1, double[,] input)
        {
            int row = mat1.GetLength(0);
            int col1 = mat1.GetLength(1);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = 0;
                    for (int k = 0; k < col1; k++)
                    {
                        result[i, j] = result[i, j] + mat1[i, k] * input[k, j];
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[] MatPlus(double[] input, double k)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = input[i] + k;
            }
            return result;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[,] MatPlus(double[,] input, double k)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = input[i, j] + k;
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="vec1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] MatPlus(double[] vec1, double[] input)
        {
            int len1 = vec1.Length;
            int len2 = input.Length;
            double[] result = new double[len1];
            if (len1 == len2)
            {
                for (int i = 0; i < len1; i++)
                {
                    result[i] = vec1[i] + input[i];
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵加法
        /// </summary>
        /// <param name="mat1"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] MatPlus(double[,] mat1, double[,] input)
        {
            int row = mat1.GetLength(0);
            int col = mat1.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = mat1[i, j] + input[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 矩阵转置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] MatTra(this double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[col, row];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[j, i] = input[i, j];
                }
            }
            return result;
        }

        /// <summary>
        /// 模
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double Norm(this double[] input)
        {
            double result = 0;
            for (int i = 0; i < input.Length; i++)
            {
                result += input[i] * input[i];
            }
            return Math.Sqrt(result);
        }

        /// <summary>
        /// 矩阵翻转：上下、左右
        /// </summary>
        /// <param name="x"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[,] Flip(this double[,] x, int flag = 1)
        {
            int row = x.GetLength(0);
            int col = x.GetLength(1);
            double[,] y = new double[row, col];
            // 上下翻转
            if (flag == 1)
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        y[i, j] = x[row - 1 - i, j];
                    }
                }
            }
            // 左右翻转
            else if (flag == 2)
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        y[i, j] = x[i, col - 1 - j];
                    }
                }
            }
            // 不翻转
            else
            {
                for (int i = 0; i < row; i++)
                {
                    for (int j = 0; j < col; j++)
                    {
                        y[i, j] = x[i, j];
                    }
                }
            }
            return y;
        }

        /// <summary>
        /// 矩阵旋转：90 180 270
        /// </summary>
        /// <param name="x"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static double[,] Rot90(this double[,] x, int flag = 1)
        {
            double[,] y = Copy(x);
            if (flag == 1)
            {
                //B = flip(A, 2);
                //B = permute(B,[2 1]);
                double[,] xx = x.Flip(2);
                y = xx.MatTra();
            }
            else if (flag == 2)
            {
                //B = flip(flip(A, 1), 2);
                double[,] xx = x.Flip();
                y = xx.Flip(2);
            }
            else if (flag == 3)
            {
                //B = permute(A,[2 1]);
                //B = flip(B, 2);
                double[,] xx = x.MatTra();
                y = xx.Flip(2);
            }
            return y;
        }

        /// <summary>
        /// number --> string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string Num2Str(this double[] input, int digits = 4)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
            {
                result = result + Math.Round(input[i], digits) + "  ";
            }
            return result;
        }

        /// <summary>
        /// number --> string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static string[] Num2Str(this double[,] input, int digits = 4)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            string[] result = new string[row];
            for (int i = 0; i < row; i++)
            {
                result[i] = "";
                for (int j = 0; j < col; j++)
                {
                    result[i] = result[i] + Math.Round(input[i, j], digits) + "  ";
                }
            }
            return result;
        }

        /// <summary>
        /// number --> string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Num2Str(this int[] input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
            {
                result = result + input[i] + "  ";
            }
            return result;
        }

        /// <summary>
        /// number --> string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] Num2Str(this int[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            string[] result = new string[row];
            for (int i = 0; i < row; i++)
            {
                result[i] = "";
                for (int j = 0; j < col; j++)
                {
                    result[i] = result[i] + input[i, j] + "  ";
                }
            }
            return result;
        }

        /// <summary>
        /// bool --> string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Bool2Str(this bool[] input)
        {
            int len = input.Length;
            string result = "";
            for (int i = 0; i < len; i++)
            {
                result = result + input[i].ToString() + "  ";
            }
            return result;
        }

        /// <summary>
        /// bool --> string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] Bool2Str(this bool[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            string[] result = new string[row];
            for (int i = 0; i < row; i++)
            {
                result[i] = "";
                for (int j = 0; j < col; j++)
                {
                    result[i] = result[i] + input[i, j].ToString() + "  ";
                }
            }
            return result;
        }

        /// <summary>
        /// 相关系数
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        public static double Corrcoef(double[] d1, double[] d2)
        {
            double x = 0;
            double y = 0;
            double xy = 0;
            double xSum = 0;
            double ySum = 0;
            int len = d1.Length;
            for (int i = 0; i < len; i++)
            {
                xSum += d1[i];
                ySum += d2[i];
            }
            for (int i = 0; i < len; i++)
            {
                x += ((len * d1[i]) - xSum) * ((len * d1[i]) - xSum);
                y += ((len * d2[i]) - ySum) * ((len * d2[i]) - ySum);
                xy += ((len * d1[i]) - xSum) * ((len * d2[i]) - ySum);
            }
            double result = Math.Abs(xy) / Math.Sqrt(x * y);
            return result;
        }



        /// <summary>
        /// 指数函数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[] Pow(double[] input, double k)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Math.Pow(input[i], k);
            }
            return result;
        }

        /// <summary>
        /// 指数函数
        /// </summary>
        /// <param name="input"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static double[,] Pow(double[,] input, double k)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = Math.Pow(input[i, j], k);
                }
            }
            return result;
        }

        /// <summary>
        /// 绝对值函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Abs(double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Math.Abs(input[i]);
            }
            return result;
        }

        /// <summary>
        /// 绝对值函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] Abs(double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = Math.Abs(input[i, j]);
                }
            }
            return result;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Sin(double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Math.Sin(input[i]);
            }
            return result;
        }

        /// <summary>
        /// 正弦函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] Sin(double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = Math.Sin(input[i, j]);
                }
            }
            return result;
        }

        /// <summary>
        /// 余弦函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[] Cos(double[] input)
        {
            int len = input.Length;
            double[] result = new double[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = Math.Cos(input[i]);
            }
            return result;
        }

        /// <summary>
        /// 余弦函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double[,] Cos(double[,] input)
        {
            int row = input.GetLength(0);
            int col = input.GetLength(1);
            double[,] result = new double[row, col];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    result[i, j] = Math.Cos(input[i, j]);
                }
            }
            return result;
        }

        /// <summary>
        /// colormap
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static int[,] Colormap(EnumColormap flag)
        {
            int[,] data = ColorMap.Parula;
            if (flag == EnumColormap.Gray)
            {
                data = ColorMap.Gray;
            }
            else if (flag == EnumColormap.Jet)
            {
                data = ColorMap.Jet;
            }
            else if (flag == EnumColormap.Bone)
            {
                data = ColorMap.Bone;
            }
            else if (flag == EnumColormap.Hot)
            {
                data = ColorMap.Hot;
            }
            return Copy(data);
        }

        /// <summary>
        /// 矩阵标准化
        /// </summary>
        /// <param name="x"></param>
        /// <param name="unitFlag"></param>
        /// <returns></returns>
        public static int[,] MatUnit(double[,] x, int unitFlag = 255)
        {
            int row = x.GetLength(0);
            int col = x.GetLength(1);
            int[,] y = new int[row, col];
            double maxi = PeakValue(x);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    y[i, j] = (int)Math.Round(x[i, j] / maxi * unitFlag);
                }
            }
            return y;
        }

        /// <summary>
        /// 最大值位置
        /// </summary>
        /// <param name="x"></param>
        /// <param name="rowIndex"></param>
        /// <param name="colIndex"></param>
        public static void PeakPosition(this double[,] x, out int rowIndex, out int colIndex)
        {
            int row = x.GetLength(0);
            int col = x.GetLength(1);
            rowIndex = 0;
            colIndex = 0;
            double maxi = PeakValue(x);
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    if (Math.Abs(x[i, j] - maxi) < EPS)
                    {
                        rowIndex = i;
                        colIndex = j;
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 区域生长
        /// </summary>
        /// <param name="Img"></param>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="regMaxDist"></param>
        /// <returns></returns>
        public static int[,] RegionGrowing(this double[,] Img0, int x0, int y0, int regMaxDist)
        {
            int[,] Img = Int(Img0);
            int row = Img.GetLength(0);
            int col = Img.GetLength(1);
            int[,] Jm = new int[row, col];
            double regMean = Img[x0, y0];
            Jm[x0, y0] = 1;    // 种子点
            double regSum = regMean;  // 符合条件总和
            int regNum = 1;   // 符合条件个数
            int count = 1;    // 周围八个点符合条件个数
            int[,] regChoose = new int[row * col, 2];
            regChoose[regNum - 1, 0] = x0;
            regChoose[regNum - 1, 1] = y0;
            int num = 1;   // 第一个点
            double sTemp;  // 周围八个点符合条件总和
            while (count > 0)
            {
                sTemp = 0;
                count = 0;
                // 对新增的每个点遍历，避免重复
                for (int k = 0; k < num; k++)
                {
                    int i = regChoose[regNum - num + k, 0];
                    int j = regChoose[regNum - num + k, 1];
                    // 已确定且不是边界上的点
                    if ((Jm[i, j] == 1) && (i > 0) && (i < row - 1) && (j > 0) && (j < col - 1))
                    {
                        // 八邻域
                        for (int u = -1; u <= 1; u++)
                        {
                            for (int v = -1; v <= 1; v++)
                            {
                                // 未处理且满足条件
                                if ((Jm[i + u, j + v] == 0) && (Math.Abs(Img[i + u, j + v] - regMean) <= regMaxDist))
                                {
                                    Jm[i + u, j + v] = 1;   // 对应点设置为1
                                    count = count + 1;
                                    regChoose[regNum + count - 1, 0] = i + u;
                                    regChoose[regNum + count - 1, 1] = j + v;
                                    // 灰度值存入 s_temp 中
                                    sTemp = sTemp + Img[i + u, j + v];
                                }
                            }
                        }
                    }
                }
                num = count;   // 新增的点
                regNum = regNum + count;  // 区域内总点数
                regSum = regSum + sTemp;  // 区域内总和
            }
            // 在外面再加一圈 1
            int[,] JmCopy = Copy(Jm);
            for (int i = 1; i < row - 1; i++)
            {
                for (int j = 1; j < col - 1; j++)
                {
                    if (JmCopy[i, j] == 1)
                    {
                        Jm[i - 1, j - 1] = 1;
                        Jm[i - 1, j] = 1;
                        Jm[i - 1, j + 1] = 1;
                        Jm[i - 1, j] = 1;
                        Jm[i - 1, j + 1] = 1;
                        Jm[i + 1, j - 1] = 1;
                        Jm[i + 1, j] = 1;
                        Jm[i + 1, j + 1] = 1;
                    }
                }
            }
            return Jm;
        }

        /// <summary>
        /// 区域边界
        /// </summary>
        /// <param name="Jm"></param>
        /// <returns></returns>
        public static int[,] RegionBoundary(this int[,] Jm)
        {
            int row = Jm.GetLength(0);
            int col = Jm.GetLength(1);
            // 防止出现边界问题
            int[,] Je = new int[row + 2, col + 2];
            int i, j;
            for (i = 1; i <= row; i++)
            {
                for (j = 1; j <= col; j++)
                {
                    Je[i, j] = Jm[i - 1, j - 1];
                }
            }
            // 标记 Je 中已选择的边界点
            int[,] F = new int[row + 2, col + 2];
            // 临时边界点
            int[,] P = new int[(row + 2) * (col + 2), 2];
            // 确定第一个边界点
            int flag = 0;
            int m = 0;
            int n = 0;
            for (i = 1; i < row + 2; i++)
            {
                for (j = 1; j < col + 2; j++)
                {
                    if (Je[i, j] == 1)
                    {
                        // 初始点
                        m = i;
                        n = j;
                        flag = 1;
                        break;
                    }
                }
                if (flag == 1)
                {
                    break;
                }
            }
            // 初始化
            i = m;
            j = n;
            int dir = 7;
            flag = 1;
            int count = 0;
            // 从初始点开始遍历，直到满足退出条件
            int xx, yy, xxx, yyy;
            int p = 0;
            int q = 0;
            while (true)
            {
                xx = i;
                yy = j;
                if (dir % 2 == 0)
                {
                    dir = (dir + 7) % 8;
                }
                else
                {
                    dir = (dir + 6) % 8;
                }
                switch (dir)
                {
                    case 0:
                        j = j + 1;
                        break;
                    case 1:
                        i = i - 1;
                        j = j + 1;
                        break;
                    case 2:
                        i = i - 1;
                        break;
                    case 3:
                        i = i - 1;
                        j = j - 1;
                        break;
                    case 4:
                        j = j - 1;
                        break;
                    case 5:
                        i = i + 1;
                        j = j - 1;
                        break;
                    case 6:
                        i = i + 1;
                        break;
                    case 7:
                        i = i + 1;
                        j = j + 1;
                        break;
                }
                while (Math.Abs(Je[i, j] - 1) > 0.1)
                {
                    dir = (dir + 1) % 8;
                    i = xx;
                    j = yy;
                    switch (dir)
                    {
                        case 0:
                            j = j + 1;
                            break;
                        case 1:
                            i = i - 1;
                            j = j + 1;
                            break;
                        case 2:
                            i = i - 1;
                            break;
                        case 3:
                            i = i - 1;
                            j = j - 1;
                            break;
                        case 4:
                            j = j - 1;
                            break;
                        case 5:
                            i = i + 1;
                            j = j - 1;
                            break;
                        case 6:
                            i = i + 1;
                            break;
                        case 7:
                            i = i + 1;
                            j = j + 1;
                            break;
                    }
                }
                if (flag == 1)
                {
                    p = i;
                    q = j;
                    xx = i;
                    yy = j;
                    flag = 0;
                }
                F[i, j] = 1;
                xxx = i;
                yyy = j;
                count = count + 1;
                P[count - 1, 0] = xxx;
                P[count - 1, 1] = yyy;
                if ((m == xx) && (n == yy) && (p == xxx) && (q == yyy))
                {
                    break;
                }
            }
            // 输出
            List<List<int>> tempResult = new List<List<int>>();
            for (i = 0; i < (row + 2) * (col + 2); i++)
            {
                if (P[i, 1] > 0)
                {
                    tempResult.Add(new List<int> { P[i, 0], P[i, 1] });
                }
                else
                {
                    break;
                }
            }
            int len = tempResult.Count;
            int[,] result = new int[len, 2];
            for (i = 0; i < len; i++)
            {
                result[i, 0] = tempResult[i][1];
                result[i, 1] = tempResult[i][0];
            }
            return result;
        }
        #endregion
    }
}