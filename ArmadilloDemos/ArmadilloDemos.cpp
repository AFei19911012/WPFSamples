#include <iostream>
#include <iomanip>
#include "armadillo"
#include "../MathLib/MathLib.cpp"

using namespace std;
using namespace arma;


#pragma region ���Ժ�������
// Armadillo ����
void DemoArmadillo()
{
    cout << "Armadillo ...\n";

    mat A;               // ����һ������ ��Сδ֪
    A.print("A = ");     // ��ӡ
    mat B(2, 3);        // ָ����С
    B(1, 1) = 1.1;      // ֱ�Ӹ�ֵ
    B.print("B = ");
    A = 5.2;            // ��Ϊ 1*1 �ľ���
    A.print("A = ");
    cout << endl;
    A.set_size(2, 2);  // ���ô�С  
    A.fill(5);         // ȫ����ֵΪ 5
    A.print("A.fill(5) = ");

    A.save("D:\\MyPrograms\\VisualStudio2019\\WPFSamples\\bin\\A.txt", raw_ascii);   // ����Ϊ�ı�
    B.load("D:\\MyPrograms\\VisualStudio2019\\WPFSamples\\bin\\A.txt");              // �����ı�
    B.print("B = ");

    // ���� Matlab ����  ��ʼ�� eye��ones��zeros��randu��randn��randi
    mat C = randu(3, 3);
    C.print("C = ");
    // ��Ƭ 
    mat D = C.rows(0, 1);          // 0 �� 1 ��
    D.print("C.rows(0, 1) = ");
    mat E = C(span(0, 1), span(0, 1));       // 0 ��1 ��  0 �� 1 ��
    E.print("C(span(0, 1), span(0, 1)) = ");
    mat F = C.col(0);       // ĳ��
    F.print("C.col(0) = ");
    // ���ֵ������ ����
    A = max(C);
    B = max(C, 1);
    A.print("max(C) = ");
    B.print("max(C, 1) = ");
    // ����Ԫ�����
    double total = accu(C);
    cout << "accu(C) = " << total << endl;

    // ��������
    mat M = C * F;
    M.print("C * C.col(0) = ");
    // ����ֵ�ֽ�
    mat u;
    vec s;
    mat v;
    svd(u, s, v, C);
    u.print("u = ");
    s.print("s = ");
    v.print("v = ");

    // ����
    C.reset();
    C.print("C.reset() = ");
}


// ����ģ�����
void DemoTemplateFunction()
{
    int ia = 5;
    int ib = 4;
    cout << "Max(5, 4) = " << Max(ia, ib) << endl;
    cout << "Min(5, 4) = " << Min(ia, ib) << endl;
    Swap(ia, ib);
    cout << "Swap(5, 4)  ia = " << ia << endl;
    cout << endl;

    double f1 = 1.2;
    double f2 = 2.5;
    cout << "Max(1.2, 2.5) = " << Max(f1, f2) << endl;
    cout << "Min(1.2, 2.5) = " << Min(f1, f2) << endl;
    Swap(f1, f2);
    cout << "Swap(1.2, 2.5)  f1 = " << f1 << endl;
    cout << endl;

    string s1 = "Hello";
    string s2 = "World";
    cout << "Max(Hello, World) = " << Max(s1, s2) << endl;
    cout << "Min(Hello, World) = " << Min(s1, s2) << endl;
    Swap(s1, s2);
    cout << "Swap(Hello, World)  s1 = " << s1 << endl;
    cout << endl;
}


// ����ʽ��ϲ���
void DemoPolyfit1()
{
    double* x = new double[10];
    double* y = new double[10];
    for (int i = 0; i < 10; i++)
    {
        x[i] = i;
        y[i] = 2 * x[i] * x[i] + 3 * x[i] + 2;
    }
    double* p = new double[3];
    // ���ö���ʽ��Ϻ���
    Polyfit(x, y, 10, 2, p);
    cout << p[0] << endl << p[1] << endl << p[2] << endl;
}
void DemoPolyfit2()
{
    vec x = linspace(0, 9, 10);
    //  x % x �ǵ�ˣ��������� ��Ϥ Matlab �ľ��� .*  
    // ������ x * x ���ⲻ���Ͼ������㷨��
    vec y = 2 * x % x + 3 * x + 2;
    vec p = polyfit(x, y, 2);
    p.print();
}
void DemoPolyfit3()
{
    vec x = linspace(0, 9, 10);
    vec y = 2 * x % x + 3 * x + 2;
    mat A = zeros(10, 3);
    A.col(0) = x % x;
    A.col(1) = x;
    A.col(2) = ones(10, 1);
    // ����ʽ�����򵥷���
    mat p = solve(A, y);
    p.print();
}
void DemoPolyfit4()
{
    vec x = linspace(0, 9, 10);
    vec y = 2 * x % x + 3 * x + 2;
    mat A = zeros(10, 3);
    A.col(0) = x % x;
    A.col(1) = x;
    A.col(2) = ones(10, 1);
    // ����Ȼ���ǵ�д nnls ����ʱ��solve ������Ȼ�ڵ�����ʮ���ε�ʱ�����ʧ�ܣ����Ǻ������������ svd �����ķ���
    mat p = Solve(A, y);
    p.print();
}


// Solve ����
void DemoSolve()
{
    double* A = new double[10 * 3];
    double* y = new double[10];
    for (int i = 0; i < 10; i++)
    {
        y[i] = 2.0* i * i + 3 * i + 2;
        A[i * 3] = 1.0 * i * i;
        A[i * 3 + 1] = i;
        A[i * 3 + 2] = 1;
    }
    double* x = new double[3];
    // ����ⷽ��
    Solve(A, 10, 3, y, x);
    cout << x[0] << endl << x[1] << endl << x[2] << endl;
}


// NNLS ����
void DemoNNLS()
{
    vec x = linspace(0, 9, 10);
    vec y = 2 * x % x - 0.5 * x + 2;
    mat A = zeros(10, 3);
    A.col(0) = x % x;
    A.col(1) = x;
    A.col(2) = ones(10, 1);
    mat p1 = solve(A, y);
    p1.print("p1 = ");
    cout << endl;
    mat p2 = NNLS(A, y);
    p2.print("p2 = ");
}


// һά�ź��˲�����
void DemoFilter1D()
{
    mat dataIn;
    // ���Ե�ַ
    dataIn.load("D:\\MyPrograms\\VisualStudio2019\\WPFSamples\\bin\\noiseSignal.txt");
    int signal_size = dataIn.n_rows;
    double* signal = new double[signal_size];
    for (int i = 0; i < signal_size; i++)
    {
        signal[i] = dataIn(i);
    }
    double* dsignal = new double[signal_size];
    double* dsignal1 = new double[signal_size];
    double* dsignal2 = new double[signal_size];
    int scale = 4;
    int dbn = 8;
    bool ish = false;
    Wden(signal, dsignal, signal_size);
    WHaar(signal, dsignal1, signal_size, 4);
    Nlm(signal, dsignal2, signal_size, 5, 4, 0.1);
    for (int i = 0; i < 20; i++)
    {
        cout << setprecision(9) << dsignal[i] << "  " << setprecision(9) << dsignal1[i] << "  " << setprecision(9) << dsignal2[i] << endl;
    }
    delete[] dsignal;
    dsignal = NULL;
    delete[] signal;
    signal = NULL;
    delete[] dsignal1;
    dsignal1 = NULL;
}

// ��ά�ź��˲�����
void DemoFilter2D()
{
    imat signal2D;
    signal2D.load("D:\\MyPrograms\\VisualStudio2019\\WPFSamples\\bin\\signal2D.txt");
    int height = signal2D.n_rows;
    int width = signal2D.n_cols;
    double** signal = new double* [height];
    double** dsignal = new double* [height];
    for (int i = 0; i < height; i++)
    {
        signal[i] = new double[width];
        dsignal[i] = new double[width];
    }

    for (int i = 0; i < height; i++)
    {
        for (int j = 0; j < width; j++)
        {
            signal[i][j] = signal2D(i, j);
        }
    }
    int scale = 3;
    int dbn = 3;
    bool ish = false;
    int window_width = 4;
    int patch_width = 3;
    double sigma = 0.15;
    Wdbn2(signal, dsignal, height, width, scale, dbn);
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            cout << dsignal[i][j] << " ";
        }
        cout << endl;
    }
    cout << endl;

    int num_iter = 30;
    double delta_t = 0.01;
    Beltrami(signal, dsignal, height, width, num_iter, delta_t);
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            cout << dsignal[i][j] << " ";
        }
        cout << endl;
    }
    cout << endl;

    Nlm2(signal, dsignal, height, width, 5, 4, 0.1);
    for (int i = 0; i < 10; i++)
    {
        for (int j = 0; j < 10; j++)
        {
            cout << dsignal[i][j] << " ";
        }
        cout << endl;
    }
    cout << endl;
}
#pragma endregion

int main()
{
    system("color 0A");

    // ���� Armadillo  
    DemoArmadillo();

    // ����ģ�����
    //DemoTemplateFunction();

    // ����ʽ��ϲ���
    //DemoPolyfit4();

    // Solve ����
    //DemoSolve();

    // NNLS ����
    //DemoNNLS();

    // һά�ź��˲�����
    //DemoFilter1D();

    // ��ά�ź��˲�����
    //DemoFilter2D();

    // ������˳�
    system("pause");
}