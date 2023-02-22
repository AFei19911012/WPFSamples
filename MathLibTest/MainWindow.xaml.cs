using System.Windows;

namespace MathLibTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitFunctions();
        }

        private void InitFunctions()
        {
            _ = ListBox_Functions.Items.Add("数据保存和加载");
            _ = ListBox_Functions.Items.Add("随机数");
            _ = ListBox_Functions.Items.Add("一维插值");
            _ = ListBox_Functions.Items.Add("多项式拟合");
            _ = ListBox_Functions.Items.Add("左除解方程");
            _ = ListBox_Functions.Items.Add("非负最小二乘解方程");
            _ = ListBox_Functions.Items.Add("指数拟合");
            _ = ListBox_Functions.Items.Add("高斯拟合");
            _ = ListBox_Functions.Items.Add("饱和度反演");
        }

        private void ListBox_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ListView_Result.Items.Clear();
            int idx = ListBox_Functions.SelectedIndex;
            if (idx == 0)
            {
                double[] data = new double[5] { 1, 2, 3, 4, 5};
                ListView_Result.Items.Add("Save");
                WffCal.Save(data, "vec.txt");
                ListView_Result.Items.Add(WffCal.Num2Str(data));
                ListView_Result.Items.Add("");

                ListView_Result.Items.Add("LoadVec");
                data = WffCal.LoadVec("vec.txt");
                ListView_Result.Items.Add(WffCal.Num2Str(data));
                ListView_Result.Items.Add("");

                double[,] mat = WffCal.Ones(5, 5);
                WffCal.Save(mat, "mat.txt");
                ListView_Result.Items.Add("LoadMat");
                mat = WffCal.LoadMat("mat.txt");
                string[] strs = WffCal.Num2Str(mat);
                for (int i = 0; i < strs.Length; i++)
                {
                    ListView_Result.Items.Add(strs[i]);
                }
            }
            else if (idx == 1)
            {
                double[,] mat = WffCal.Rand(5, 5);
                ListView_Result.Items.Add("Rand");
                string[] strs = WffCal.Num2Str(mat);
                for (int i = 0; i < strs.Length; i++)
                {
                    ListView_Result.Items.Add(strs[i]);
                }
                ListView_Result.Items.Add("");

                int[,] imat = WffCal.Randi(10, 5, 5);
                ListView_Result.Items.Add("Randi");
                strs = WffCal.Num2Str(imat);
                for (int i = 0; i < strs.Length; i++)
                {
                    ListView_Result.Items.Add(strs[i]);
                }
            }
            else if (idx == 2)
            {
                double[] x = WffCal.Linspace(1, 10, 10);
                double[] y = WffCal.MatPlus(x, 1);
                ListView_Result.Items.Add("x = ");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("y = ");
                ListView_Result.Items.Add(y.Num2Str());
                ListView_Result.Items.Add("");

                ListView_Result.Items.Add("linear: x = 2.5  3.5");
                double[] xi = new double[2] { 2.5, 3.5 };
                double[] yi = WffCal.Interp1(xi, x, y, "linear");
                ListView_Result.Items.Add("linear: y = " + yi[0].ToString() + "  " + yi[1].ToString());

                yi = WffCal.Interp1(xi, x, y, "spline");
                ListView_Result.Items.Add("spline: y = " + yi[0].ToString() + "  " + yi[1].ToString());
            }
            else if (idx == 3)
            {
                double[] x = new double[10];
                double[] y = new double[10];
                for (int i = 0; i < 10; i++)
                {
                    x[i] = i;
                    y[i] = 2 * x[i] * x[i] + 3 * x[i] + 2;
                }
                double[] p = WffCal.Polyfit(x, y, 2);
                double[] yfit = WffCal.Polyval(x, p);

                ListView_Result.Items.Add("x = ");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("y = ");
                ListView_Result.Items.Add(y.Num2Str());
                ListView_Result.Items.Add("yfit = ");
                ListView_Result.Items.Add(yfit.Num2Str());
            }
            else if (idx == 4)
            {
                double[,] A = new double[5, 3];
                for (int i = 0; i < 5; i++)
                {
                    A[i, 0] = i * i;
                    A[i, 1] = i;
                    A[i, 2] = 1;
                }
                double[] x = new double[3] { 2, 3, 2 };
                double[] y = WffCal.MatMul(A, x);

                string[] strs = A.Num2Str();
                ListView_Result.Items.Add("A = ");
                for (int i = 0; i < strs.Length; i++)
                {
                    ListView_Result.Items.Add(strs[i]);
                }
                ListView_Result.Items.Add("x = ");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("y = ");
                ListView_Result.Items.Add(y.Num2Str());

                ListView_Result.Items.Add("");
                x = WffCal.Solve(A, y);
                ListView_Result.Items.Add("Solve ");
                ListView_Result.Items.Add(x.Num2Str());
            }
            else if (idx == 5)
            {
                double[,] A = new double[5, 3];
                for (int i = 0; i < 5; i++)
                {
                    A[i, 0] = i * i;
                    A[i, 1] = i;
                    A[i, 2] = 1;
                }
                double[] x = new double[3] { 2, -1, 2 };
                double[] y = WffCal.MatMul(A, x);

                string[] strs = A.Num2Str();
                ListView_Result.Items.Add("A = ");
                for (int i = 0; i < strs.Length; i++)
                {
                    ListView_Result.Items.Add(strs[i]);
                }
                ListView_Result.Items.Add("x = ");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("y = ");
                ListView_Result.Items.Add(y.Num2Str());

                ListView_Result.Items.Add("");
                x = WffCal.Solve(A, y);
                ListView_Result.Items.Add("Solve ");
                ListView_Result.Items.Add(x.Num2Str());

                x = WffCal.Lsqnonneg(A, y);
                y = WffCal.MatMul(A, x);
                ListView_Result.Items.Add("Lsqnonneg");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("yfit");
                ListView_Result.Items.Add(y.Num2Str());
            }
            else if (idx == 6)
            {
                double[] x = WffCal.Linspace(0.1, 5, 10);
                double[] p = new double[3] { 2, 3, 0.1};
                double[] y = WffCal.ExpFitting(x, p);
                ListView_Result.Items.Add("x");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("p");
                ListView_Result.Items.Add(p.Num2Str());
                ListView_Result.Items.Add("y");
                ListView_Result.Items.Add(y.Num2Str());

                p = WffCal.ExpFitting(x, y, 1);
                y = WffCal.ExpFitting(x, p);
                ListView_Result.Items.Add("");
                ListView_Result.Items.Add("ExpFitting");
                ListView_Result.Items.Add(p.Num2Str());
                ListView_Result.Items.Add("yfit");
                ListView_Result.Items.Add(y.Num2Str());
            }
            else if (idx == 7)
            {
                double[] x = WffCal.Linspace(0.1, 5, 10);
                double[] p = new double[3] { 2, 3, 2 };
                double[] y = WffCal.GaussFitting(x, p[0], p[1], p[2]);
                ListView_Result.Items.Add("x");
                ListView_Result.Items.Add(x.Num2Str());
                ListView_Result.Items.Add("p");
                ListView_Result.Items.Add(p.Num2Str());
                ListView_Result.Items.Add("y");
                ListView_Result.Items.Add(y.Num2Str());

                p = WffCal.GaussFitting(x, y);
                y = WffCal.GaussFitting(x, p[0], p[1], p[2]);
                ListView_Result.Items.Add("");
                ListView_Result.Items.Add("GaussFitting");
                ListView_Result.Items.Add(p.Num2Str());
                ListView_Result.Items.Add("yfit");
                ListView_Result.Items.Add(y.Num2Str());
            }
            else if (idx == 8)
            {
                double DTo1 = 820;
                double DTo2 = 1420;
                double DTw1 = 620;
                double DTw2 = 1074;
                double DENo = 0.95;
                double DENw = 1;
                double CNLo = 0.95;
                double CNLw = 1;           
                double phi = 0.1000;
                double DTma1 = 182;
                double DTma2 = 315;
                double DENma = 2.65;
                double n = 2;
                double m = 2;
                double Rw = 0.6;
                double CNLma = -0.05;
                double DTC = 118.1155;
                double DTS = 225.3645;
                double DENb = 2.6640;
                double CNL = 19.6148;
                double a = 0.2;
                double b = 0.3;
                double Rt1 = 14.8906;
                double Rt2 = 14.9461;
                double Rt3 = 14.1472;
                double Swo = 1;
                double lambda = 0.01;
                double Sw1 = 0;
                double Sw2 = 0;
                double Sw3 = 0;
                WffCal.InvSaturation(DTma1, DTma2, DTo1, DTo2, DTw1, DTw2, DENma, DENo, DENw, n, m, Rw, CNLma, CNLo, CNLw, DTC, DTS, phi, DENb, CNL, a, b, Rt1, Rt2, Rt3, Swo, lambda, ref Sw1, ref Sw2, ref Sw3);
            }
        }
    }
}