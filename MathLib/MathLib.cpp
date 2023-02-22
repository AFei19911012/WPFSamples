#include "MathLib.h"
#include "common.h"
#include "nnls.h"
#include "filter.h"
#include "invert.h"
#include "lsfitting.h"


#pragma region �������� ��������
// ����һά double ������
void SaveMat1(double* data, int len, char* fileName)
{
	mat mData = zeros(len, 1);
	for (int i = 0; i < len; i++)
	{
		mData(i) = data[i];
	}
	mData.save(fileName, raw_ascii);
}

// ����һά int ������
void SaveIMat1(int* data, int len, char* fileName)
{
	imat mData = zeros<imat>(len, 1);
	for (int i = 0; i < len; i++)
	{
		mData(i) = data[i];
	}
	mData.save(fileName, raw_ascii);
}

// �����ά int ������
void SaveMat2(double* data, int row, int col, char* fileName)
{
	mat mData = zeros(row, col);
	for (int i = 0; i < row * col; i++)
	{
		mData(i / col, i % col) = data[i];
	}
	mData.save(fileName, raw_ascii);
}

// �����ά double ������
void SaveIMat2(int* data, int row, int col, char* fileName)
{
	imat mData = zeros<imat>(row, col);
	for (int i = 0; i < row * col; i++)
	{
		mData(i / col, i % col) = data[i];
	}
	mData.save(fileName, raw_ascii);
}
#pragma endregion


#pragma region ���������
// ��������������ȷֲ�����˹�ֲ�
double Random(int flag)
{
	return Rand(flag);
}

// ���ȷֲ����������ά����
void RandomU2(double* input, int row, int col)
{
	mat result = randu(row, col);
	for (int i = 0; i < row * col; i++)
	{
		input[i] = result(i / col, i % col);
	}
}

// ���ȷֲ��������һά����
void RandomU1(double* input, int len)
{
	mat result = randu(len, 1);
	for (int i = 0; i < len; i++)
	{
		input[i] = result(i, 0);
	}
}

// ��˹�ֲ����������ά����
void RandomN2(double* input, int row, int col)
{
	mat result = randn(row, col);
	for (int i = 0; i < row * col; i++)
	{
		input[i] = result(i / col, i % col);
	}
}

// ��˹�ֲ��������һά����
void RandomN1(double* input, int len)
{
	mat result = randn(len, 1);
	for (int i = 0; i < len; i++)
	{
		input[i] = result(i);
	}
}

// �����������
int RandomInt(int maxi)
{
	imat result = randi(1, distr_param(0, maxi));
	return result(0);
}

// �����������ά����
void RandomInt2(int* input, int maxi, int row, int col)
{
	imat result = randi(row, col, distr_param(0, maxi));
	for (int i = 0; i < row * col; i++)
	{
		input[i] = result(i / col, i % col);
	}
}

// ���������һά����
void RandomInt1(int* input, int maxi, int len)
{
	imat result = randi(len, 1, distr_param(0, maxi));
	for (int i = 0; i < len; i++)
	{
		input[i] = result(i);
	}
}
#pragma endregion


#pragma region ��׼�� ����
// һά�����׼��
double Stddev1(double* input, int len)
{
	vec data = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		data(i) = input[i];
	}
	return stddev(data);
}

// ��ά���鰴�С����б�׼��
void Stddev2(double* input, int row, int col, int flag, double* result)
{
	mat data = zeros(row, col);
	for (int i = 0; i < row * col; i++)
	{
		data(i / col, i % col) = input[i];
	}
	mat mResult = stddev(data, 0, flag);
	int len = mResult.n_elem;
	for (int i = 0; i < len; i++)
	{
		result[i] = mResult(i);
	}
}

// һά���鷽��
double Var1(double* input, int len)
{
	vec data = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		data(i) = input[i];
	}
	return var(data);
}

// ��ά���鰴�С����з���
void Var2(double* input, int row, int col, int flag, double* result)
{
	mat data = zeros(row, col);
	for (int i = 0; i < row * col; i++)
	{
		data(i / col, i % col) = input[i];
	}
	mat mResult = var(data, 0, flag);
	int len = mResult.n_elem;
	for (int i = 0; i < len; i++)
	{
		result[i] = mResult(i);
	}
}
#pragma endregion


#pragma region �� ��� SVD
// �������
int Rank1(double* input, int row, int col)
{
	mat data = zeros(row, col);
	for (int i = 0; i < row * col; i++)
	{
		data(i / col, i % col) = input[i];
	}
	int r = arma::rank(data);
	return r;
}

// ������ȣ�ָ������
int Rank2(double* input, int row, int col, double tolerance)
{
	mat data = zeros(row, col);
	for (int i = 0; i < row * col; i++)
	{
		data(i / col, i % col) = input[i];
	}
	int r = arma::rank(data, tolerance);
	return r;
}

// ������
void Kron(double* input1, int row1, int col1, double* input2, int row2, int col2, double* result)
{
	mat A = zeros(row1, col1);
	mat B = zeros(row2, col2);
	for (int i = 0; i < row1 * col1; i++)
	{
		A(i / col1, i % col1) = input1[i];
	}
	for (int i = 0; i < row2 * col2; i++)
	{
		B(i / col2, i % col2) = input2[i];
	}
	mat K = kron(A, B);
	int row = K.n_rows;
	int col = K.n_cols;
	for (int i = 0; i < row * col; i++)
	{
		result[i] = K(i / col, i % col);
	}
}

// SVD �ֽ�
void Svd(double* input, int row, int col, double* u, double* s, double* v)
{
	mat data = zeros(row, col);
	for (int i = 0; i < row * col; i++)
	{
		data(i / col, i % col) = input[i];
	}
	mat mU;
	vec vS;
	mat mV;
	svd(mU, vS, mV, data, "std");
	for (int i = 0; i < row * row; i++)
	{
		u[i] = mU(i / row, i % row);
	}
	for (int i = 0; i < (int)vS.n_elem; i++)
	{
		s[i] = vS(i);
	}
	for (int i = 0; i < col * col; i++)
	{
		v[i] = mV(i / col, i % col);
	}
}
#pragma endregion


#pragma region һά��ֵ ���� �������
// һά��ֵ������ڡ����ԡ�����ڣ����٣������ԣ����٣�������
void Interp1(double* x, double* y, int len1, double* xi, int len2, int flag, double* yi)
{
	vec vX = zeros<vec>(len1);
	vec vY = zeros<vec>(len1);
	vec vXi = zeros<vec>(len2);
	for (int i = 0; i < len1; i++)
	{
		vX(i) = x[i];
		vY(i) = y[i];
	}
	for (int i = 0; i < len2; i++)
	{
		vXi(i) = xi[i];
	}
	vec vYi;
	if (flag == 1)
	{
		interp1(vX, vY, vXi, vYi, "nearest");
	}
	else if (flag == 2)
	{
		interp1(vX, vY, vXi, vYi);
	}
	else if (flag == 3)
	{
		interp1(vX, vY, vXi, vYi, "*nearest");
	}
	else if (flag == 4)
	{
		interp1(vX, vY, vXi, vYi, "*linear");
	}
	else if (flag == 5)
	{
		vYi = Spline(vX, vY, vXi);
	}
	for (int i = 0; i < len2; i++)
	{
		yi[i] = vYi(i);
	}
}

// һά�����������򡢽���
void Sort(double* input, int len, int flag, double* result)
{
	vec data = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		data(i) = input[i];
	}
	vec mR;
	if (flag == 1)
	{
		mR = sort(data, "ascend");
	}
	else
	{
		mR = sort(data, "descend");
	}
	for (int i = 0; i < len; i++)
	{
		result[i] = mR(i);
	}
}

// һά����������ţ����򡢽���
void SortIndex(double* input, int len, int flag, int* result)
{
	vec data = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		data(i) = input[i];
	}
	uvec mR;
	if (flag == 1)
	{
		mR = sort_index(data, "ascend");
	}
	else
	{
		mR = sort_index(data, "descend");
	}
	for (int i = 0; i < len; i++)
	{
		result[i] = mR(i);
	}
}
#pragma endregion


#pragma region ����Ҷ�任 �渵��Ҷ�任
// ����Ҷ�任
void Fft(double* x, double* y, int len, double* xx, double* yy, double* amp)
{
	vec vX = zeros<vec>(len);
	vec vY = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		vX(i) = x[i];
		vY(i) = y[i];
	}
	cx_vec sig = zeros<cx_vec>(len);
	sig.set_real(vX);
	sig.set_imag(vY);
	cx_vec result = fft(sig);
	mat mReal = real(result);
	mat mImag = imag(result);
	for (int i = 0; i < len; i++)
	{
		xx[i] = mReal(i);
		yy[i] = mImag(i);
		amp[i] = sqrt(mReal(i) * mReal(i) + mImag(i) * mImag(i));
	}
}

// ����Ҷ�任���渵��Ҷ�任
void Ifft(double* x, double* y, int len, double* xx, double* yy, double* amp)
{
	vec vX = zeros<vec>(len);
	vec vY = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		vX(i) = x[i];
		vY(i) = y[i];
	}
	cx_vec sig = zeros<cx_vec>(len);
	sig.set_real(vX);
	sig.set_imag(vY);
	cx_vec result = ifft(sig);
	mat mReal = real(result);
	mat mImag = imag(result);
	for (int i = 0; i < len; i++)
	{
		xx[i] = mReal(i);
		yy[i] = mImag(i);
		amp[i] = sqrt(mReal(i) * mReal(i) + mImag(i) * mImag(i));
	}
}
#pragma endregion


#pragma region ����ʽ��� ����ʽ���ֵ SVD�ⷽ�� �Ǹ���С���˽ⷽ�� ָ��˥����� ��˹���
// ����ʽ���
void Polyfit(double* x, double* y, int len, int N, double* p)
{
	vec vX = zeros<vec>(len);
	vec vY = zeros<vec>(len);
	for (int i = 0; i < len; i++)
	{
		vX(i) = x[i];
		vY(i) = y[i];
	}
	vec vP = polyfit(vX, vY, N);
	for (int i = 0; i <= N; i++)
	{
		p[i] = vP(i);
	}
}

// ����ʽ���ֵ
void Polyval(double* x, int len1, double* p, int len2, double* y)
{
	vec vX = zeros<vec>(len1);
	vec vP = zeros<vec>(len2);
	for (int i = 0; i < len1; i++)
	{
		vX(i) = x[i];
	}
	for (int i = 0; i < len2; i++)
	{
		vP(i) = p[i];
	}
	vec vY = polyval(vP, vX);
	for (int i = 0; i < len1; i++)
	{
		y[i] = vY(i);
	}
}

// SVD �ⷽ��
void Solve(double* A, int m, int n, double* y, double* x)
{
	mat mA = zeros(m, n);
	mat mY = zeros(m, 1);
	for (int i = 0; i < m; i++)
	{
		for (int j = 0; j < n; j++)
		{
			mA(i, j) = A[i * n + j];
		}
		mY(i) = y[i];
	}
	// ʹ��������ⷽ��
	mat mX = Solve(mA, mY);
	for (int i = 0; i < n; i++)
	{
		x[i] = mX(i);
	}
}

// �Ǹ���С����
void Lsqnonneg(double* A, int m, int n, double* y, double* x)
{
	mat C = zeros(m, n);
	mat d = zeros(m, 1);
	for (int i = 0; i < m * n; i++)
	{
		C(i / n, i % n) = A[i];
	}
	for (int i = 0; i < m; i++)
	{
		d(i) = y[i];
	}
	mat result = NNLS(C, d);
	for (int i = 0; i < n; i++)
	{
		x[i] = result(i);
	}
}

// ָ��˥����ϣ����֧����ָ��
void ExpFitting(double* x, double* y, int len, int N, double* p)
{
	mat mX = zeros(len, 1);
	mat mY = zeros(len, 1);
	for (int i = 0; i < len; i++)
	{
		mX(i) = x[i];
		mY(i) = y[i];
	}
	mat mP = LSFittingT2(mX, mY, N);
	for (int i = 0; i < 2 * N + 1; i++)
	{
		p[i] = mP(i);
	}
}

// ��˹���
void GaussFitting(double* x0, double* y0, int M, double* p)
{
	mat x = zeros(M, 1);
	mat y = zeros(M, 1);
	for (int i = 0; i < M; i++)
	{
		x(i) = x0[i];
		y(i) = y0[i];
	}
	// ��������
	int nParam = 3;
	double a, b, c;
	double a1, b1, c1;
	// Least Squares Minimization
	// ���Բ�ͬ�ĳ�ʼֵ����
	for (int num = 0; num < 100; num++)
	{
		// ��ʼֵ
		a = sqrt(Rand(0));
		b = sqrt(Rand(0));
		c = sqrt(Rand(0));
		// ����в�
		mat r = y - pow(a, 2) * exp(-pow(x - b, 2) / pow(c, 2));
		mat r1 = r;
		// Ŀ�꺯��
		double f = pow(norm(r), 2);
		// �������ӳ�ֵ
		double lambda = 1;
		// ���±��
		bool updateFlag = true;
		// �ȶ�����
		int cntConv = 0;
		// ����������
		int maxIter = 100;
		// ��������
		int it = 0;
		// ��������
		mat g = zeros(nParam, 1);
		mat H = eye(nParam, nParam);
		while (it < maxIter)
		{
			it = it + 1;
			if (updateFlag)
			{
				mat Ja = 2 * a * exp(-pow(x - b, 2) / pow(c, 2));
				mat Jb = (2 * x - 2 * b) / pow(c, 2) * pow(a, 2) % exp(-pow(x - b, 2) / pow(c, 2));
				mat Jc = 2 * pow(x - b, 2) / pow(c, 3) * pow(a, 2) % exp(-pow(x - b, 2) / pow(c, 2));
				mat J = join_rows(Ja, Jb);
				J = join_rows(J, Jc);
				g = -2 * J.t() * r;
				H = 2 * J.t() * J;
			}
			mat Hess = H + lambda * eye(nParam, nParam);
			int hRank = arma::rank(Hess);
			if (hRank < nParam)
			{
				it = maxIter;
				break;
			}
			mat s = Solve(-Hess, g);
			a1 = a + s(0);
			b1 = b + s(1);
			c1 = c + s(2);
			r1 = y - pow(a1, 2) * exp(-pow(x - b1, 2) / pow(c1, 2));
			double f1 = pow(norm(r1), 2);
			double fdr = (f - f1) / (f + EPS);
			if (fdr > 0)
			{
				a = a1;
				b = b1;
				c = c1;
				f = f1;
				r = r1;
				lambda = 0.1 * lambda;
				updateFlag = true;
			}
			else
			{
				lambda = 10 * lambda;
				updateFlag = false;
			}
			if ((fdr >= 0) && (fdr <= 0.0005))
			{
				cntConv = cntConv + 1;
			}
			else
			{
				cntConv = 0;
			}
			if ((cntConv > 9) || (abs(s).max() < 1e-6))
			{
				break;
			}
		}
		if (it < maxIter)
		{
			break;
		}
	}
	p[0] = a;
	p[1] = b;
	p[2] = c;
}
#pragma endregion


#pragma region ��ά��� С���˲� �Ǿֲ���ֵ�˲�
// ��ά���
void Conv2(double* input1, int row1, int col1, double* input2, int row2, int col2, double* output, int row3, int col3)
{
	mat A = zeros(row1, col1);
	mat B = zeros(row2, col2);
	for (int i = 0; i < row1 * col1; i++)
	{
		A(i / col1, i % col1) = input1[i];
	}
	for (int i = 0; i < row2 * col2; i++)
	{
		B(i / col2, i % col2) = input2[i];
	}
	mat C;
	if (row3 == row1 && col3 == col1)
	{
		C = conv2(A, B, "same");
	}
	else
	{
		C = conv2(A, B);
	}
	for (int i = 0; i < row3 * col3; i++)
	{
		output[i] = C(i / col3, i % col3);
	}
}

// һά sym8 С���˲��������Ĳ�ȫ������ֵ
void Wden(double* Input, double* Output, int signal_size)
{
	// ������ sym8 ��ϵ�������� Matlab wfilters('sym8') �������
	double Lo_D[16] = { -0.00338241595100613, -0.000542132331791148, 0.0316950878114930, 0.00760748732491761, -0.143294238350810, -0.0612733590676585, 0.481359651258372,	0.777185751700524,
		0.364441894835331, -0.0519458381077090, -0.0272190299170560, 0.0491371796736075, 0.00380875201389062, -0.0149522583370482, -0.000302920514721367, 0.00188995033275946 };
	double Hi_D[16] = { -0.00188995033275900, -0.000302920514721367, 0.0149522583370480, 0.00380875201389062, -0.0491371796736075, -0.0272190299170560, 0.0519458381077090, 0.364441894835331 ,
		-0.777185751700524, 0.481359651258372, 0.0612733590676585, -0.143294238350810, -0.00760748732491761, 0.0316950878114930, 0.000542132331791148, -0.00338241595100613 };
	double Lo_R[16] = { 0.00188995033275946, -0.000302920514721367, -0.0149522583370482, 0.00380875201389062, 0.0491371796736075, -0.0272190299170560, -0.0519458381077090, 0.364441894835331,
		0.777185751700524, 0.481359651258372, -0.0612733590676585, -0.143294238350810, 0.00760748732491761, 0.0316950878114930, -0.000542132331791148, -0.00338241595100613 };
	double Hi_R[16] = { -0.00338241595100613, 0.000542132331791148, 0.0316950878114930, -0.00760748732491761, -0.143294238350810, 0.0612733590676585, 0.481359651258372, -0.777185751700524,
		0.364441894835331, 0.0519458381077090, -0.0272190299170560, -0.0491371796736075, 0.00380875201389062, 0.0149522583370482, -0.000302920514721367, -0.00188995033275946 };
	int filterLen = 16;   // �˲�������
	int Scale = 4;        // �˲�����
	// ��ʼ��Ҫ����
	vec mLo_D = zeros(filterLen, 1);
	vec mHi_D = zeros(filterLen, 1);
	vec mLo_R = zeros(filterLen, 1);
	vec mHi_R = zeros(filterLen, 1);
	for (int i = 0; i < filterLen; i++)
	{
		mLo_D(i) = Lo_D[i];
		mHi_D(i) = Hi_D[i];
		mLo_R(i) = Lo_R[i];
		mHi_R(i) = Hi_R[i];
	}
	vec mInput = zeros(signal_size, 1);
	for (int i = 0; i < signal_size; i++)
	{
		mInput(i) = Input[i];
	}
	int srcLen = signal_size;
	imat msgLen = zeros<imat>(Scale + 2, 1);   // ��ͬ���źų���
	msgLen(0) = srcLen;
	for (int i = 1; i < Scale + 1; i++)
	{
		int exLen = (srcLen + filterLen - 1) / 2;
		srcLen = exLen;
		msgLen(i) = exLen;
	}
	msgLen(Scale + 1) = srcLen;
	int allSize = accu(msgLen) - signal_size;
	vec dstCoef = WaveDec(mInput, msgLen, allSize, Scale, mLo_D, mHi_D, filterLen);
	vec pDet = dstCoef.rows(allSize - msgLen(1), allSize - 1);
	double thr = GetThr(pDet, msgLen);
	dstCoef = Wthresh(dstCoef, thr, allSize, msgLen(Scale));
	vec mOutput = WaveRec(dstCoef, msgLen, Scale, mLo_R, mHi_R, filterLen);
	for (int i = 0; i < signal_size; i++)
	{
		Output[i] = mOutput(i);
	}
}

/*
	һά db С���˲���������㼶
	scale���˲�������1~9��
	dbn���˲�����1~9��
	isHard��true��ʾӲ��ֵ��false��ʾ����ֵ
*/
void Wdbn(double* Input, double* Output, int signal_size, int scale, int dbn, bool isHard)
{
	int filterLen = 2 * dbn;
	int Scale = scale;
	vec mLo_D = zeros(filterLen, 1);
	vec mHi_D = zeros(filterLen, 1);
	vec mLo_R = zeros(filterLen, 1);
	vec mHi_R = zeros(filterLen, 1);
	switch (dbn)
	{
	case 1:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db1_Lo_D[i];
			mHi_D(i) = db1_Hi_D[i];
			mLo_R(i) = db1_Lo_R[i];
			mHi_R(i) = db1_Hi_R[i];
		}
		break;
	case 2:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db2_Lo_D[i];
			mHi_D(i) = db2_Hi_D[i];
			mLo_R(i) = db2_Lo_R[i];
			mHi_R(i) = db2_Hi_R[i];
		}
		break;
	case 3:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db3_Lo_D[i];
			mHi_D(i) = db3_Hi_D[i];
			mLo_R(i) = db3_Lo_R[i];
			mHi_R(i) = db3_Hi_R[i];
		}
		break;
	case 4:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db4_Lo_D[i];
			mHi_D(i) = db4_Hi_D[i];
			mLo_R(i) = db4_Lo_R[i];
			mHi_R(i) = db4_Hi_R[i];
		}
		break;
	case 5:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db5_Lo_D[i];
			mHi_D(i) = db5_Hi_D[i];
			mLo_R(i) = db5_Lo_R[i];
			mHi_R(i) = db5_Hi_R[i];
		}
		break;
	case 6:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db6_Lo_D[i];
			mHi_D(i) = db6_Hi_D[i];
			mLo_R(i) = db6_Lo_R[i];
			mHi_R(i) = db6_Hi_R[i];
		}
		break;
	case 7:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db7_Lo_D[i];
			mHi_D(i) = db7_Hi_D[i];
			mLo_R(i) = db7_Lo_R[i];
			mHi_R(i) = db7_Hi_R[i];
		}
		break;
	case 8:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db8_Lo_D[i];
			mHi_D(i) = db8_Hi_D[i];
			mLo_R(i) = db8_Lo_R[i];
			mHi_R(i) = db8_Hi_R[i];
		}
		break;
	case 9:
		for (int i = 0; i < filterLen; i++)
		{
			mLo_D(i) = db9_Lo_D[i];
			mHi_D(i) = db9_Hi_D[i];
			mLo_R(i) = db9_Lo_R[i];
			mHi_R(i) = db9_Hi_R[i];
		}
		break;
	default:
		exit(0);
		break;
	}

	int srcLen = signal_size;
	imat msgLen = zeros<imat>(Scale + 2, 1);
	msgLen(0) = srcLen;
	for (int i = 1; i < Scale + 1; i++)
	{
		int exLen = (srcLen + filterLen - 1) / 2;
		srcLen = exLen;
		msgLen(i) = exLen;
	}
	msgLen(Scale + 1) = srcLen;
	int allSize = accu(msgLen) - signal_size;

	vec mInput = zeros(signal_size, 1);
	for (int i = 0; i < signal_size; i++)
	{
		mInput(i) = Input[i];
	}
	vec dstCoef = WaveDec(mInput, msgLen, allSize, Scale, mLo_D, mHi_D, filterLen);
	vec pDet = dstCoef.rows(allSize - msgLen(1), allSize - 1);
	double thr = GetThr(pDet, msgLen);
	dstCoef = Wthresh(dstCoef, thr, allSize, msgLen(Scale), isHard);
	vec mOutput = WaveRec(dstCoef, msgLen, Scale, mLo_R, mHi_R, filterLen);
	for (int i = 0; i < signal_size; i++)
	{
		Output[i] = mOutput(i);
	}
}

// ͼ�� db С���˲���������㼶��ȫ������ֵ
void Wdbn2(double* Input, double* Output, int height, int width, int scale, int dbn)
{
	int i, j;
	double** Input0 = new double* [height];
	double** Output0 = new double* [height];
	for (i = 0; i < height; i++)
	{
		Input0[i] = new double[width];
		Output0[i] = new double[width];
	}
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			Input0[i][j] = Input[i * width + j];
		}
	}
	Wdbn2(Input0, Output0, height, width, scale, dbn);
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			Output[i * width + j] = Output0[i][j];
		}
	}
	for (i = 0; i < height; i++)
	{
		delete[] Input0[i];
		delete[] Output0[i];
	}
	Input0 = NULL;
	Output0 = NULL;
}

/*
	ƽ�Ʋ���С���˲�����㼶
	length(signal) = N * 2^L;
*/
void WHaar(double* Input, double* Output, int signal_size, int scale)
{
	if (signal_size > 300000)
	{
		for (int i = 0; i < signal_size; i++)
		{
			Output[i] = Input[i];
		}
		return;
	}
	int minLen = (int)pow(2, scale);
	int sigLen = signal_size;
	if (signal_size % minLen != 0)
	{
		sigLen = (signal_size / minLen + 1) * minLen;
	}
	// db1
	vec mLo_D = { 0.707106781186548, 0.707106781186548 };
	vec mHi_D = { -0.707106781186548, 0.707106781186548 };
	vec mLo_R = { 0.707106781186548, 0.707106781186548 };
	vec mHi_R = { 0.707106781186548, -0.707106781186548 };
	// input
	vec mInput = zeros(signal_size, 1);
	for (int i = 0; i < signal_size; i++)
	{
		mInput(i) = Input[i];
	}
	// fill
	vec sig = zeros(sigLen, 1);
	sig.rows(0, signal_size - 1) = mInput;
	if (sigLen > signal_size)
	{
		for (int i = 0; i < sigLen - signal_size; i++)
		{
			sig(signal_size + i) = mInput(signal_size - 1);
		}
	}
	mat table = zeros(sigLen, scale + 1);
	table.col(0) = sig;
	// dwt
	vec lowCoef = zeros(sigLen, 1);
	for (int i = 1; i <= scale; i++)
	{
		int size = sigLen / (int)pow(2, i);
		lowCoef = table.col(0);
		// loop
		for (int j = 0; j <= (int)pow(2, i) - 2; j = j + 2)
		{
			vec sig0 = lowCoef.rows(size * j, size * (j + 2) - 1);
			vec cA = zeros(size, 1);
			vec cD = zeros(size, 1);
			Dwt(sig0, 2 * size, mLo_D, mHi_D, 2, cA, cD);
			table.col(0).rows(j * size, (j + 1) * size - 1) = cA;
			table.col(i).rows(j * size, (j + 1) * size - 1) = cD;
			sig0 = ShiftLeft(sig0);
			Dwt(sig0, 2 * size, mLo_D, mHi_D, 2, cA, cD);
			table.col(0).rows((j + 1) * size, (j + 2) * size - 1) = cA;
			table.col(i).rows((j + 1) * size, (j + 2) * size - 1) = cD;
		}
	}
	// thresh
	double sigma = median(arma::abs(table.col(1))) / 0.6745;
	double thr = sigma * sqrt(2 * log(sigLen));
	// loop
	for (int i = 1; i <= scale; i++)
	{
		// loop
		for (int j = 0; j < sigLen; j++)
		{
			if (abs(table(j, i)) < thr)
			{
				table(j, i) = 0;
			}
			else if (table(j, i) >= thr)
			{
				table(j, i) = table(j, i) - thr;
			}
			else
			{
				table(j, i) = table(j, i) + thr;
			}
		}
	}
	// idwt
	for (int i = scale; i > 0; i--)
	{
		int size = sigLen / (int)pow(2, i);
		// loop
		for (int j = 0; j <= (int)pow(2, i) - 2; j = j + 2)
		{
			vec cA = table.col(0).rows(j * size, (j + 1) * size - 1);
			vec cD = table.col(i).rows(j * size, (j + 1) * size - 1);
			vec sig0 = Idwt(cA, cD, 2 * size, mLo_R, mHi_R, 2);
			cA = table.col(0).rows((j + 1) * size, (j + 2) * size - 1);
			cD = table.col(i).rows((j + 1) * size, (j + 2) * size - 1);
			vec sig1 = Idwt(cA, cD, 2 * size, mLo_R, mHi_R, 2);
			sig1 = ShiftRight(sig1);
			lowCoef.rows(size * j, size * (j + 2) - 1) = (sig0 + sig1) / 2;
		}
		table.col(0) = lowCoef;
	}
	// return
	for (int i = 0; i < signal_size; i++)
	{
		Output[i] = table(i, 0);
	}
}

// һά�Ǿֲ���ֵ
void Nlm(double* Input, double* Output, int signal_size, int window_width, int patch_width, double sigma)
{
	int i, j, i1;
	mat mInput = zeros(signal_size + 2 * patch_width, 1);
	// mInput = padarray(input, [0 patch_width], 'replicate');
	for (i = 0; i < signal_size; i++)
	{
		mInput(patch_width + i) = Input[i];
	}
	for (i = 0; i < patch_width; i++)
	{
		mInput(i) = Input[0];
		mInput(signal_size + patch_width + i) = Input[signal_size - 1];
	}
	// normalization
	double max_sig = Array_Max(mInput);
	mInput = mInput / max_sig;
	// kernel
	mat kernel = zeros(2 * patch_width + 1, 1);
	for (j = 1; j <= patch_width; j++)
	{
		for (i = -j; i <= j; i++)
		{
			kernel(patch_width - i) = kernel(patch_width - i) + 1.0 / pow(2 * j + 1, 2);
		}
	}
	kernel = kernel / patch_width;
	kernel = kernel / accu(kernel);
	// main process
	mat W1 = zeros(2 * patch_width + 1, 1);
	mat W2 = zeros(2 * patch_width + 1, 1);
	double average, s_w, d, w;
	int r_min, r_max;
	for (i = 1; i <= signal_size; i++)
	{
		i1 = i + patch_width;
		W1 = mInput.rows(i1 - patch_width - 1, i1 + patch_width - 1);
		average = 0;
		s_w = 0;
		r_min = ((i1 - window_width) > (patch_width + 1)) ? (i1 - window_width) : (patch_width + 1);
		r_max = ((i1 + window_width) < (patch_width + signal_size)) ? (i1 + window_width) : (patch_width + signal_size);
		for (j = r_min; j <= r_max; j++)
		{
			if (j == i1)
			{
				continue;
			}
			W2 = mInput.rows(j - patch_width - 1, j + patch_width - 1);
			d = accu(kernel % (W1 - W2) % (W1 - W2));
			w = exp(-d / (sigma * sigma));
			s_w = s_w + w;
			average = average + w * mInput(j - 1);
		}
		average = average + w * mInput(i1 - 1);
		s_w = s_w + w;
		Output[i - 1] = max_sig * average / s_w;
	}
}

// �Ǿֲ���ֵͼ���˲�
void Nlm2(double* Input, double* Output, int height, int width, int window_width, int patch_width, double sigma)
{
	int i, j;
	double** Input0 = new double* [height];
	double** Output0 = new double* [height];
	for (i = 0; i < height; i++)
	{
		Input0[i] = new double[width];
		Output0[i] = new double[width];
	}
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			Input0[i][j] = Input[i * width + j];
		}
	}
	Nlm2(Input0, Output0, height, width, window_width, patch_width, sigma);
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			Output[i * width + j] = Output0[i][j];
		}
	}
	for (i = 0; i < height; i++)
	{
		delete[] Input0[i];
		delete[] Output0[i];
	}
	Input0 = NULL;
	Output0 = NULL;
}
#pragma endregion


#pragma region NMR �����㷨: sirt brd cg svd
//sirt �����㷨
void InvSIRT(double* TArray, double* MeaAmp, int M, int N, int iters, double* T2Dist)
{
	// ����������
	mat mTArray = zeros(M, N);
	mat mAmplitude = zeros(M, 1);
	for (int i = 0; i < M; i++)
	{
		mAmplitude(i) = MeaAmp[i];
	}
	for (int i = 0; i < M * N; i++)
	{
		mTArray(i / N, i % N) = TArray[i];
	}
	// ����
	// % ��ͬ��Matlab�е� .*
	mat Lambda = sum(mTArray.t() % mTArray.t());
	Lambda = 1 / Lambda.t();
	mat mTarray1 = zeros(M, N);
	for (int i = 0; i < M; i++)
	{
		for (int j = 0; j < N; j++)
		{
			if (mTArray(i, j) != 0)
			{
				mTarray1(i, j) = 1;
			}
		}
	}
	// �з���Ԫ�ظ�����
	mat LA = sum(mTarray1) + EPS;
	// �����⣻
	mat IterAmplitude = zeros(1, N);
	// ����⣻
	mat Amplitude1 = zeros(M, 1);
	// �����
	mat IterErr = zeros(M, 1);
	// ���ͣ�
	mat IterSum = zeros(1, N);
	// ��������
	int t = 0;
	// ����
	while (t < iters)
	{
		Amplitude1 = mTArray * IterAmplitude.t();
		IterErr = mAmplitude - Amplitude1;
		IterSum = Lambda.t() % IterErr.t() * mTArray;
		// / ��ͬ��Matlab�е� ./
		IterAmplitude = IterSum / LA + IterAmplitude;
		for (int i = 0; i < N; i++)
		{
			if (IterAmplitude(i) < 0)
			{
				IterAmplitude(i) = 0;
			}
		}
		t = t + 1;
	}
	// ���ؽ�
	for (int i = 0; i < N; i++)
	{
		T2Dist[i] = IterAmplitude(i);
		if (T2Dist[i] < EPS)
		{
			T2Dist[i] = 0;
		}
	}
}

// ������ brd
void InvBRD(double* TArray, double* MeaAmp, int M, int N, double a0, double* T2Dist)
{
	// ���������滻
	mat K = zeros(M, N);
	mat d = zeros(M, 1);
	mat x = zeros(N, 1);
	for (int i = 0; i < M; i++)
	{
		d(i) = MeaAmp[i];
	}
	for (int i = 0; i < M * N; i++)
	{
		K(i / N, i % N) = TArray[i];
	}
	// ��ʼ�������
	BRD(K, d, M, N, a0, T2Dist);
}

// �����ݶ� cg
void InvCG(double* TArray, double* MeaAmp, int M, int N, int iters, double* T2Dist)
{
	// ���������滻
	mat K = zeros(M, N);
	mat d = zeros(M, 1);
	for (int i = 0; i < M; i++)
	{
		d(i) = MeaAmp[i];
	}
	for (int i = 0; i < M * N; i++)
	{
		K(i / N, i % N) = TArray[i];
	}
	// ����
	mat mX = zeros(N, 1);   // ��ʼֵ
	mat r = d - K * mX;     // �в�
	mat At = K.t();         // ת��
	// ����
	int it = 0;
	while (it < iters)
	{
		mat d0 = At * r;
		mat Ad0 = K * d0;
		double alpha = (norm(d0) * norm(d0)) / (norm(Ad0) * norm(Ad0));
		mat dx = alpha * d0;
		mX = mX + dx;
		r = r - alpha * Ad0;
		mat d = At * r;
		double beta = (norm(d) * norm(d)) / (norm(d0) * norm(d0));
		d = d + beta * d0;
		d0 = d;
		it = it + 1;
		if (norm(dx) < 0.01)
		{
			break;
		}
	}
	// ��������
	for (int i = 0; i < N; i++)
	{
		T2Dist[i] = mX(i);
		if (T2Dist[i] < EPS)
		{
			T2Dist[i] = 0;
		}
	}
}

// ����ֵ�ֽ� svd
void InvSVD(double* TArray, double* MeaAmp, int M, int N, double a0, double* T2Dist)
{
	// ���������滻
	mat K = zeros(M, N);
	mat d = zeros(M, 1);
	for (int i = 0; i < M; i++)
	{
		d(i) = MeaAmp[i];
	}
	for (int i = 0; i < M * N; i++)
	{
		K(i / N, i % N) = TArray[i];
	}
	// ����
	// �����
	mat mAtA = K.t() * K + a0 * eye(N, N);
	mat mF = inv(mAtA) * K.t() * d;
	// SVD
	mat mU;
	vec mS0;
	mat mV;
	svd(mU, mS0, mV, K, "std");   // ����������ѹ��
	mat mS = zeros(N, N);
	for (int i = 0; i < N; i++)
	{
		mS(i, i) = mS0(i);
	}
	// ���������
	double Q0 = norm(d - K * mF) / sqrt(N);
	double snr = K(0) / Q0;
	// �ض�ϵ��
	double cut = (1.45 * snr + 25) / mS(0, 0);
	// ����ֵ�ض�
	for (int i = 0; i < N; i++)
	{
		mS(i, i) = mS(i, i) / (mS(i, i) * mS(i, i) + a0 * a0);
		if (mS(i, i) > cut)
		{
			mS(i, i) = 0.0;
		}
	}
	// �Ǹ�����
	mat mVStUt = mV * mS.t() * mU.t();
	int it = 0;
	while (it < N / 2)
	{
		double minValue = mF.min();
		int minIndex = mF.index_min();
		if (minValue < 0)
		{
			mF(minIndex) = 0;
		}
		else
		{
			break;
		}
		mF = mVStUt * (K * mF);
		it = it + 1;
	}
	// ��������
	for (int i = 0; i < N; i++)
	{
		T2Dist[i] = mF(i);
		if (T2Dist[i] < EPS)
		{
			T2Dist[i] = 0;
		}
	}
}
#pragma endregion


#pragma region ��ά NMR �����㷨: T1-T2 D-T2 ���ݶ�D-T2
/*
	T1-T2
	EH�������źţ�ÿ��Ϊһ�β�����
	tau1���ȴ�ʱ�䣻
	Nw���ȴ�ʱ�������
	tau2������ʱ�䣻
	NECH������������
	Tmin����С��ԥʱ�䣬T1��T2һ�£�
	Tmax������ԥʱ�䣬T1��T2һ�£�
	alpha���������ӣ�
	Nt�����ݵ�����T1��T2һ�£�
	invModel������ģ�ͣ�1��ʾ���ͻָ���2��ʾ��ת�ָ���
	T2T1Dist�����ݽ����
	EHfit����Ͻ����
	T1Dist��T1�ֲ�����T1�������ۻ���
	T2Dist��T2�ֲ�����T2�������ۻ���
*/
void InvT1T2(double* EH, double* tau1, int Nw, double* tau2, int NECH, double Tmin, double Tmax, double alpha, int Nt, int invModel, double* T2T1Dist, double* EHfit, double* T, double* T1Dist, double* T2Dist, double& ReError)
{
	int i, j;
	double** EH0 = new double* [Nw];
	for (i = 0; i < Nw; i++)
	{
		EH0[i] = new double[NECH];
	}
	// ��ֵ
	for (i = 0; i < Nw; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			EH0[i][j] = EH[i * NECH + j];
		}
	}
	// ���ݽ��
	double** T2T1Dist0 = new double* [Nt];
	for (i = 0; i < Nt; i++)
	{
		T2T1Dist0[i] = new double[Nt];
	}
	double** EHfit0 = new double* [Nw];
	for (i = 0; i < Nw; i++)
	{
		EHfit0[i] = new double[NECH];
	}
	// ���ú���
	T1T2(EH0, tau1, Nw, tau2, NECH, Tmin, Tmax, alpha, Nt, invModel, T2T1Dist0, EHfit0, T, T1Dist, T2Dist, ReError);
	// ��ԭ���
	for (i = 0; i < Nt; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			T2T1Dist[i * Nt + j] = T2T1Dist0[i][j];
		}
	}
	for (i = 0; i < Nw; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			EHfit[i * NECH + j] = EHfit0[i][j];
		}
	}
	// �ͷ�
	for (i = 0; i < Nw; i++)
	{
		delete[] EH0[i];
		delete[] EHfit0[i];
		EH0[i] = NULL;
		EHfit0[i] = NULL;
	}
	delete[] T2T1Dist0;
}

/*
	D-T2
	Gk2���ݶ�ƽ����
	tau������ʱ�䣻
	Dmin����ɢϵ����Сֵ��
	Dmax����ɢϵ�����ֵ��
	Nd����ɢϵ��������
	Tmin��T2��ԥʱ����Сֵ��
	Tmax��T2��ԥʱ�����ֵ��
	Nt��T2��ԥʱ�������
	Delta1������
	delta2���ģ�
	T2DDist�����ݽ����
	EHfit����Ͻ����
	D����ɢϵ��ֵ��
	DDist��D�ֲ�����D�������ۻ���
	T2�������ԥʱ��ֵ��
	T2Dist��T2�ֲ�����T2�������ۻ���
*/
void InvDT2(double* EH, double* Gk2, int Ng, double* tau, int NECH, double Dmin, double Dmax, int Nd, double Tmin, double Tmax, double alpha, int Nt, double Delta1, double delta2, double* T2DDist, double* EHfit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError)
{
	int i, j;
	double** EH0 = new double* [Ng];
	for (i = 0; i < Ng; i++)
	{
		EH0[i] = new double[NECH];
	}
	// ��ֵ
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			EH0[i][j] = EH[i * NECH + j];
		}
	}
	// ���ݽ��
	double** T2DDist0 = new double* [Nd];
	for (i = 0; i < Nd; i++)
	{
		T2DDist0[i] = new double[Nt];
	}
	double** EHfit0 = new double* [Ng];
	for (i = 0; i < Ng; i++)
	{
		EHfit0[i] = new double[NECH];
	}
	// ���ú���
	DT2(EH0, Gk2, Ng, tau, NECH, Dmin, Dmax, Nd, Tmin, Tmax, alpha, Nt, Delta1, delta2, T2DDist0, EHfit0, D, DDist, T2, T2Dist, ReError);
	// ��ԭ���
	for (i = 0; i < Nd; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			T2DDist[i * Nt + j] = T2DDist0[i][j];
		}
	}
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			EHfit[i * NECH + j] = EHfit0[i][j];
		}
	}
	// �ͷ�
	for (i = 0; i < Ng; i++)
	{
		delete[] EH0[i];
		delete[] EHfit0[i];
		EH0[i] = NULL;
		EHfit0[i] = NULL;
	}
	delete[] T2DDist0;
}

/*
	���ݶ� D-T2
	SEQtau2���ɼ�ʱ�䣬һ�У�
	SEQsig���ɼ��źţ�һ�У�
	NECH��ÿ�βɼ��Ļز�����
	N_echo���ز���������
	TE��ÿ�βɼ��Ļز������
	Gcst���ݶȣ�
	nPreset��Ԥ�貼�������D��T2һ�£�
	d_fit������źţ�
*/
void InvDT2_Gcst(double* SEQtau2, double* SEQsig, int* NECH, int N_echo, double* TE, double Gcst, double Dmin, double Dmax, double Tmin, double Tmax, int nPreset, double alpha, double* T2DDist, double* d_fit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError)
{
	int i, j;
	int Ne = 200;  // ��������
	double gamma = 2 * PI * 42.576e+6;
	double beta = -1.0 / 12 * gamma * gamma * Gcst * Gcst;    // ע��ǰ��Ҫ�� 1.0

	// ����ֵ
	vec mT2 = logspace(log10(Tmin), log10(Tmax), nPreset);  // T2 ��ԥʱ��
	vec mD = logspace(log10(Dmin), log10(Dmax), nPreset);   // D �ֲ���Χ
	mat matT2 = zeros(1, nPreset);
	mat matD = zeros(1, nPreset);
	// ���س�ԥʱ��
	for (i = 0; i < nPreset; i++)
	{
		T2[i] = mT2(i);
		matT2(i) = mT2(i);
	}
	// ������ɢϵ���ֲ���Χ
	for (i = 0; i < nPreset; i++)
	{
		D[i] = mD(i);
		matD(i) = mD(i);
	}

	// ��ʼ��
	mat Ks = zeros(N_echo * Ne, nPreset * nPreset);
	mat ds = zeros(N_echo * Ne, 1);
	int* NECH_accu = new int[N_echo];   // �ز���������ۼ�
	NECH_accu[0] = NECH[0];
	for (i = 1; i < N_echo; i++)
	{
		NECH_accu[i] = NECH_accu[i - 1] + NECH[i];
	}
	int num = 0;
	int* IndexNumber = new int[Ne];   // һ������ͺ�
	double t;
	mat k2 = zeros(1, nPreset);    // �����˾����һ��
	mat k1 = zeros(1, nPreset);
	for (i = 0; i < N_echo; i++)   // ���ÿһ���ز�������ѹ��
	{
		// ����ѹ��
		Ne = 200;
		if (NECH[i] <= Ne)  // ����������⴦��
		{
			Ne = NECH[i];
		}
		AmplitudeSample(NECH[i], Ne, IndexNumber);
		for (j = 0; j < Ne; j++)
		{
			if (i == 0)
			{
				t = SEQtau2[IndexNumber[j]];
				ds[num] = SEQsig[IndexNumber[j]];
			}
			else
			{
				t = SEQtau2[NECH_accu[i - 1] + IndexNumber[j]];
				ds[num] = SEQsig[NECH_accu[i - 1] + IndexNumber[j]];
			}
			k2 = exp(-t / matT2);
			k1 = exp(beta * t * TE[i] * TE[i] * matD);
			Ks.row(num) = kron(k2, k1);
			num = num + 1;
		}
	}
	mat mKs = Ks.rows(0, num - 1);
	mat mds = ds.rows(0, num - 1);
	// BRD ����
	double* aT2DDist = new double[nPreset * nPreset];
	BRD(mKs, mds, num, nPreset * nPreset, alpha, aT2DDist);
	// ����任
	mat x = zeros(nPreset * nPreset, 1);
	for (i = 0; i < nPreset * nPreset; i++)
	{
		x(i) = aT2DDist[i];
	}
	mat T2D = reshape(x, nPreset, nPreset);
	for (i = 0; i < nPreset; i++)
	{
		for (j = 0; j < nPreset; j++)
		{
			T2DDist[i * nPreset + j] = T2D(i, j);
		}
	}
	// T2 �ֲ��� D �ֲ�
	mat mT2Dist = sum(T2D, 0);
	mat mDDist = sum(T2D, 1);
	// ����źź����������
	mat matKs = zeros(NECH_accu[N_echo - 1], nPreset * nPreset);
	int count = 0;
	for (i = 0; i < N_echo; i++)
	{
		for (j = 0; j < NECH[i]; j++)
		{
			t = SEQtau2[count];
			k2 = exp(-t / matT2);
			k1 = exp(beta * t * TE[i] * TE[i] * matD);
			matKs.row(count) = kron(k2, k1);
			count = count + 1;
		}
	}
	mat md_fit = matKs * x;
	// ���� D �� T2
	for (i = 0; i < nPreset; i++)
	{
		DDist[i] = mDDist(i);
	}
	for (i = 0; i < nPreset; i++)
	{
		T2Dist[i] = mT2Dist(i);
	}
	// ������Ͻ��
	for (i = 0; i < count; i++)
	{
		d_fit[i] = md_fit(i);
	}
	// ����������
	mat mSEQsig = zeros(count, 1);
	for (i = 0; i < count; i++)
	{
		mSEQsig(i) = SEQsig[i];
	}
	ReError = norm(md_fit - mSEQsig) / norm(mSEQsig);
	// �ͷ�
	delete[] NECH_accu;
	delete[] IndexNumber;
	delete[] aT2DDist;
}
#pragma endregion


#pragma region ���Ͷȷ���
void InvSaturation(double DTma1, double DTma2, double DTo1, double DTo2, double DTw1, double DTw2, double DENma, double DENo, double DENw, double n, double m, double Rw, double CNLma, double CNLo, double CNLw, double DTC, double DTS, double phi, double DENb, double CNL, double a, double b, double Rt1, double Rt2, double Rt3, double Swo, double lambda, double& Sw1, double& Sw2, double& Sw3)
{
	// ϵ������ A
	mat A = zeros(8, 6);
	A(0, 0) = DTo1;
	A(0, 1) = DTo1;
	A(0, 2) = DTo1;
	A(0, 3) = DTw1;
	A(0, 4) = DTw1;
	A(0, 5) = DTw1;
	A(1, 0) = DTo2;
	A(1, 1) = DTo2;
	A(1, 2) = DTo2;
	A(1, 3) = DTw2;
	A(1, 4) = DTw2;
	A(1, 5) = DTw2;
	A(2, 0) = DENo;
	A(2, 1) = DENo;
	A(2, 2) = DENo;
	A(2, 3) = DENw;
	A(2, 4) = DENw;
	A(2, 5) = DENw;
	A(3, 0) = CNLo;
	A(3, 1) = CNLo;
	A(3, 2) = CNLo;
	A(3, 3) = CNLw;
	A(3, 4) = CNLw;
	A(3, 5) = CNLw;
	A(4, 3) = 1 / phi;
	A(5, 4) = 1 / phi;
	A(6, 5) = 1 / phi;
	A(7, 0) = 1;
	A(7, 1) = 1;
	A(7, 2) = 1;
	A(7, 3) = 1;
	A(7, 4) = 1;
	A(7, 5) = 1;
	//A.save("D:\\MyPrograms\\VisualStudio2019\\WPFSamples\\bin\\A.txt", raw_ascii);

	// ���� Y
	mat Y = zeros(8, 1);
	Y(0) = DTC - (1 - phi) * DTma1;
	Y(1) = DTS - (1 - phi) * DTma1;
	Y(2) = DENb - (1 - phi) * DENma;
	Y(3) = CNL - (1 - phi) * CNLma;
	Y(4) = pow(a * b * Rw / (pow(phi, m) * Rt1), 1.0 / n);
	Y(5) = pow(a * b * Rw / (pow(phi, m) * Rt2), 1.0 / n);
	Y(6) = pow(a * b * Rw / (pow(phi, m) * Rt3), 1.0 / n);
	Y(7) = 3 * phi;

	// �ⷽ��
	mat X = zeros(1, 6);
	mat AtA = A.t() * A + lambda * eye(6, 6);
	X = inv(AtA) * A.t() * Y;
	// ���㱥�Ͷ�
	double sw1 = X(3) / phi;
	double sw2 = X(4) / phi;
	double sw3 = X(5) / phi;

	double aa = AtA(0, 0);

	// Լ������
	if (sw1 >= 1 || sw2 >= 1 || sw3 >= 1)
	{
		Sw1 = Min(sw1, 1.0);
		Sw2 = Min(sw2, 1.0);
		Sw3 = Min(sw3, 1.0);
	}
	else
	{
		if (sw3 / Swo >= 0.9)
		{
			Sw1 = sw1;
			Sw2 = sw2;
			Sw3 = sw3;
		}
		else
		{
			if (sw2 / Swo >= 0.9)
			{
				Sw1 = sw1;
				Sw2 = sw2;
				Sw3 = sw2 / sw1 * sw2;
			}
			else
			{
				if (sw1 / Swo >= 0.9)
				{
					Sw1 = sw1;
					Sw2 = sw1;
					Sw3 = sw1;
				}
				else
				{
					Sw1 = 0.9 * Swo;
					Sw2 = 0.95 * Swo;
					Sw3 = 0.9 * Swo;
				}
			}
		}
	}
}
#pragma endregion