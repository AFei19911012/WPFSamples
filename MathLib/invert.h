#pragma once
#include "common.h"


// ���� Hessian ����
void HessianCal(mat K, int m, int n, mat f, double a0, mat& h0)
{
	/*
		K���˾��󣬶�ά����
		m��mK���ȣ�
		n��mK��ȣ�
		f���⣬һά��
		a0������ϵ����
		h0������Hessian����
	*/
	int i, j;
	// ��f����ǣ�����0�ĸ�ֵΪ�±꣬С��0�ĸ�ֵ-1
	imat temp = zeros<imat>(n, 1);
	// ��¼f��Ϊ����±�
	imat IndexNumber;
	// ѹ��֮���K
	mat K1;
	// ͳ�ƴ���0�ĸ���
	int num = 0;
	for (i = 0; i < n; i++)
	{
		if (f(i) > 0)
		{
			temp(i) = i;
			num = num + 1;
		}
		else
		{
			temp(i) = -1;
		}
	}
	// f��ȫΪ0
	if (num > 0)
	{
		IndexNumber = zeros<imat>(num, 1);
		j = 0;
		for (i = 0; i < n; i++)
		{
			if (temp(i) > -1)
			{
				IndexNumber(j) = temp(i);
				j = j + 1;
			}
		}
		K1 = zeros(m, num);
		for (i = 0; i < m; i++)
		{
			for (j = 0; j < num; j++)
			{
				K1(i, j) = K(i, IndexNumber(j));
			}
		}
		h0 = K1 * K1.t();
	}
	for (i = 0; i < m; i++)
	{
		h0(i, i) += a0;
	}
}

// T2 ��λ��ת
void T2PhaseRotation(double* Amplitude, double* Noise, int M, double* DataReal, double* DataImaginary)
{
	int i;
	// ������λ��ת�ĵ���
	int N = M;
	if (N > 10)
	{
		N = 10;
	}
	// ������λ��
	double sre = 0;
	double sim = 0;
	for (i = 0; i < N; i++)
	{
		sre += DataReal[i];
		sim += DataImaginary[i];
	}
	// ��λ��
	double alpha = atan(sim / (sre + EPS));
	// �����źź�����
	for (i = 0; i < M; i++)
	{
		Amplitude[i] = DataReal[i] * cos(alpha) + DataImaginary[i] * sin(alpha);
		Noise[i] = DataReal[i] * sin(alpha) - DataImaginary[i] * cos(alpha);
	}
	// ��ת��
	// ��ΪС�� 0 ��ת����ֹ��������׵��ź�ֵ�쳣��   2018-1-17
	if (Amplitude[0] < 0)
	{
		for (i = 0; i < M; i++)
		{
			Amplitude[i] = -Amplitude[i];
		}
	}
}

// T1 ��λ��ת
void T1PhaseRotation(double* Amplitude, double* Noise, int M, double* DataReal, double* DataImaginary)
{
	int i;
	// ������λ��ת�ĵ���
	int N = M / 2 + 1;
	// ������λ��
	double sre = 0;
	double sim = 0;
	for (i = 0; i < N; i++)
	{
		sre += DataReal[i];
		sim += DataImaginary[i];
	}
	// ��λ��
	double alpha = atan(sim / (sre + EPS));
	if (alpha < 0)
	{
		alpha += PI;
	}
	// �����źź�����
	for (i = 0; i < M; i++)
	{
		Amplitude[i] = DataReal[i] * cos(alpha) + DataImaginary[i] * sin(alpha);
		Noise[i] = DataReal[i] * sin(alpha) - DataImaginary[i] * cos(alpha);
	}
	// ��ת��
	if (Amplitude[0] > Amplitude[M - 1])
	{
		for (i = 0; i < M; i++)
		{
			Amplitude[i] = -Amplitude[i];
		}
	}
}

// ���������
double CalculateSNR(double* Amplitude, double* Noise, int M)
{
	vec noise = zeros(M, 1);
	for (int i = 0; i < M; i++)
	{
		noise(i) = Noise[i];
	}
	double s = abs(Amplitude[0]);
	double n = stddev(noise);
	if (n == 0)
	{
		n += 1;
	}
	if (s == 0)
	{
		return -1;
	}
	else
	{
		return 20 * log10(s / n);
	}
}

// �������˲�
void AmplitudeFilt(double* Amplitude, int filterIndex, int M, double* FAmplitude)
{
	int i, Width;
	// �˲�ָ��Ϊ0���˳�������ԭʼ�ź�
	if (filterIndex == 0)
	{
		for (i = 0; i < M; i++)
		{
			FAmplitude[i] = Amplitude[i];
		}
		return;
	}
	int* PeakIndex = new int[M];
	int* WindowWidth = new int[M];
	// ��ʼ��
	for (i = 0; i < M; i++)
	{
		PeakIndex[i] = i + 1;
		WindowWidth[i] = (int)round((PeakIndex[i] - filterIndex) / (2 * filterIndex + 1));
	}
	Width = WindowWidth[M - 1];
	if (Width == 0)
	{
		return;
	}
	int* WindowRange = new int[2 * Width + 1];
	double* prob = new double[2 * Width + 1];
	for (i = 0; i < 2 * Width + 1; i++)
	{
		WindowRange[i] = i - Width;
		prob[i] = exp(WindowRange[i] * filterIndex / M);
	}
	double* Amplitude1 = new double[M + Width];
	for (i = 0; i < M; i++)
	{
		Amplitude1[i] = Amplitude[i];
	}
	for (i = 0; i < Width; i++)
	{
		Amplitude1[M + i] = Amplitude[M - i - 2];
	}
	// �����˲����ź�
	double temp;
	for (i = 0; i < M; i++)
	{
		temp = 0;
		for (int j = -WindowWidth[i]; j <= WindowWidth[i]; j++)
		{
			temp = temp + Amplitude1[i + j] * prob[WindowWidth[M - 1] + j];
		}
		FAmplitude[i] = temp / (2 * WindowWidth[i] + 1);
	}
	// �ͷ�
	delete[] PeakIndex;
	delete[] WindowWidth;
	delete[] WindowRange;
	delete[] prob;
	delete[] Amplitude1;
}

// ���ݳ���
void AmplitudeSample(int M, int SampNumber, int* IndexNumber)
{
	int i, th;
	double* SampIndex = new double[SampNumber];
	int* tp = new int[SampNumber];
	int* flag = new int[SampNumber];
	if (SampNumber < M)
	{
		th = 0;
		for (i = 0; i < SampNumber; i++)
		{
			SampIndex[i] = pow(10.0, log(M) * log10(exp(1)) * i / (SampNumber - 1));
			tp[i] = 1 + i;
			flag[i] = 0;
			if (SampIndex[i] <= tp[i])
			{
				flag[i] = 1;
			}
			th = th + flag[i];
		}
		for (i = 0; i < th; i++)
		{
			IndexNumber[i] = tp[i] - 1;
		}
		for (i = th; i < SampNumber; i++)
		{
			IndexNumber[i] = (int)round(SampIndex[i] - 1);
		}
	}
	else
	{
		for (i = 0; i < M; i++)
		{
			IndexNumber[i] = i;
		}
	}
	// �ͷ�
	delete[] SampIndex;
	delete[] tp;
	delete[] flag;
}

// BRD
void BRD(mat K, mat d, int M, int N, double a0, double* T2Dist)
{
	// ��ʼ��
	mat c0 = zeros(M, 1);
	double rho = 0.3;
	double lambda = 0.7;
	mat f = zeros(N, 1);
	mat h0 = zeros(M, M);
	mat g0 = zeros(M, 1);
	mat phi_0 = zeros(1, 1);
	mat p = zeros(M, 1);
	mat c_1 = zeros(M, 1);
	mat f_1 = zeros(N, 1);
	mat h_1 = zeros(M, M);
	mat phi_1 = zeros(1, 1);
	mat g_1 = zeros(M, 1);
	mat g0p = zeros(1, 1);
	mat g1p = zeros(1, 1);
	// C-step
	while (1)
	{
		// f = K'*c0; f(f<0) = 0;
		f = K.t() * c0;
		for (int i = 0; i < N; i++)
		{
			if (f(i) < 0)
			{
				f(i) = 0;
			}
		}
		// ����Hessian����  heavyside output:0|1
		// h0 = K*sparse( diag(f>0) )*K' + a0.*eye(size(G))
		HessianCal(K, M, N, f, a0, h0);
		// grad
		g0 = h0 * c0 - d;
		// phi_0 = 1/2*c0'*h0*c0-c0'*d
		phi_0 = 0.5 * c0.t() * h0 * c0 - c0.t() * d;
		// ţ�ٷ���  p = -h0\g0
		p = -1 * solve(h0, g0);

		// ����ȷ����������Wolf-Powell׼��
		double a = 1e-3;
		double b = 1;
		double lam = b;
		while (a < b)
		{
			c_1 = c0 + lam * p;
			f_1 = K.t() * c_1;
			for (int i = 0; i < N; i++)
			{
				if (f_1(i) < 0)
				{
					f_1(i) = 0;
				}
			}
			HessianCal(K, M, N, f_1, a0, h_1);
			phi_1 = 0.5 * c_1.t() * h_1 * c_1 - c_1.t() * d;
			g0 = h0 * c0 - d;
			g_1 = h_1 * c_1 - d;
			g0p = g0.t() * p;
			g1p = g_1.t() * p;
			double temp1 = phi_1(0);
			double temp2 = phi_0(0) + rho * lam * g0p(0);
			double temp3 = g1p(0);
			double temp4 = lambda * g0p(0);
			// upper
			if (temp1 <= temp2)
			{
				// �����������ҵ�����
				// lower
				if (temp3 >= temp4)
				{
					break;
				}
				else
				{
					a = lam;
					lam = 0.5 * (a + b);
				}
			}
			else
			{
				b = lam;
				lam = 0.5 * (a + b);
			}
			// ���������խ
			if (abs(a - b) < 1e-3)
			{
				break;
			}
		}
		// �仯���ȹ�С
		if (norm(c0 - c_1) / (norm(c0) + EPS) < 1e-3)
		{
			break;
		}
		c0 = c_1;
	}
	// ���ؽ�
	for (int i = 0; i < N; i++)
	{
		T2Dist[i] = f_1(i);
	}
}

// T1-T2 ����
void T1T2(double** EH, double* tau1, int Nw, double* tau2, int NECH, double Tmin, double Tmax, double alpha, int Nt, int invModel, double** T2T1Dist,
	double** EHfit, double* T, double* T1Dist, double* T2Dist, double& ReError)
{
	int i, j;
	int Ne = 200;  // ��������200
	if ((invModel != 1) && (invModel != 2))
	{
		return;
	}

	// �������滻
	mat mEH = zeros(Nw, NECH);
	for (i = 0; i < Nw; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			mEH(i, j) = EH[i][j];
		}
	}

	// ��ʼ��
	mat K1 = zeros(Nw, Nt);
	mat K20 = zeros(NECH, Nt);
	vec T1 = logspace(log10(Tmin), log10(Tmax), Nt);  // T1��T2���
	vec T2 = T1;
	// ���س�ԥʱ��
	for (i = 0; i < Nt; i++)
	{
		T[i] = T1(i);
	}

	// �˾���ֵ
	for (i = 0; i < NECH; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			K20(i, j) = exp(-tau2[i] / T2(j));
		}
	}
	switch (invModel)   // ���ͻָ����Ƿ�ת�ָ�
	{
	case 1:  // ���ͻָ�
		for (i = 0; i < Nw; i++)
		{
			for (j = 0; j < Nt; j++)
			{
				K1(i, j) = 1 - exp(-tau1[i] / T1(j));
			}
		}
		break;
	case 2:  // ��ת�ָ�
		for (i = 0; i < Nw; i++)
		{
			for (j = 0; j < Nt; j++)
			{
				K1(i, j) = 1 - 2 * exp(-tau1[i] / T1(j));
			}
		}
		break;
	default:
		break;
	}

	// ����ѹ��
	if (NECH <= Ne)  // ��һ�����趨�ɼ����ݵ����ô�٣���200��С��
	{
		Ne = NECH;
	}
	mat EH0 = zeros(Nw, Ne);
	mat K2 = zeros(Ne, Nt);
	int* IndexNumber = new int[Ne];
	AmplitudeSample(NECH, Ne, IndexNumber);
	for (i = 0; i < Nw; i++)
	{
		for (j = 0; j < Ne; j++)
		{
			EH0(i, j) = mEH(i, IndexNumber[j]);
		}
	}
	for (i = 0; i < Ne; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			K2(i, j) = K20(IndexNumber[i], j);
		}
	}
	// һ�㴦��
	for (j = 0; j < Nt; j++)
	{
		if (K2(0, j) < EPS)
		{
			for (i = 0; i < Ne; i++)
			{
				K2(i, j) = 0;
			}
		}
		else
		{
			break;
		}
	}
	// SVD �ֽ⡢����ѹ��
	double cutnumber = 10000;  // ������10000���ضϱ�׼
							   // K1�˾���
	mat u;
	vec s;
	mat v;
	svd(u, s, v, K1, "std");
	int ncut = Nw;
	if (Nw > Nt)    // ��һ�����趨��ԥʱ������Ȳɼ���������������
	{
		ncut = Nt;
	}
	// K1 ��������ֵ����
	int num1 = 0;
	for (i = 0; i < ncut; i++)
	{
		if (s(0) / s(i) <= cutnumber)
		{
			num1 = num1 + 1;
		}
		else
		{
			break;
		}
	}
	// ѹ�������
	mat u1 = u.cols(0, num1 - 1);
	mat v1 = v.cols(0, num1 - 1);
	mat s1 = zeros(num1, num1);
	for (i = 0; i < num1; i++)
	{
		s1(i, i) = s(i);
	}
	// K2 �˾���
	ncut = Nt;
	if (Nt > Ne)  // ��һ�����趨��ԥʱ������Ȳɼ����ݵ���������
	{
		ncut = Ne;
	}
	u = zeros(Ne, Ne);
	v = zeros(Nt, Nt);
	s = zeros<vec>(ncut, 1);
	svd(u, s, v, K2, "std");
	// K2 ��������ֵ����
	int num2 = 0;
	for (i = 0; i < ncut; i++)
	{
		if (s(0) / s(i) <= cutnumber)
		{
			num2 = num2 + 1;
		}
		else
		{
			break;
		}
	}
	// ѹ�������
	mat u2 = u.cols(0, num2 - 1);
	mat v2 = v.cols(0, num2 - 1);
	mat s2 = zeros(num2, num2);
	for (i = 0; i < num2; i++)
	{
		s2(i, i) = s(i);
	}

	// ����任
	mat Mr = u1.t() * EH0 * u2;
	mat K1r = s1 * v1.t();
	mat K2r = s2 * v2.t();
	mat K = kron(K1r, K2r);
	mat Mrs = reshape(Mr.t(), num1 * num2, 1);

	// BRD ����
	double* aT2T1Dist = new double[Nt * Nt];
	BRD(K, Mrs, num1 * num2, Nt * Nt, alpha, aT2T1Dist);
	// ����任
	mat T2T1 = zeros(Nt, Nt);
	for (i = 0; i < Nt; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			T2T1(i, j) = aT2T1Dist[i * Nt + j];
			T2T1Dist[i][j] = T2T1(i, j);
		}
	}
	// T2 �ֲ��� T1 �ֲ� 
	// ������
	mat mT2Dist = sum(T2T1, 0);
	mat mT1Dist = sum(T2T1, 1);
	// ����źź����������
	mat mEHfit = K1 * T2T1 * K20.t();
	ReError = norm(mEHfit - mEH, 2) / norm(mEH, 2);
	// ���� T1 �� T2
	for (i = 0; i < Nt; i++)
	{
		T2Dist[i] = mT2Dist(i);
		T1Dist[i] = mT1Dist(i);
	}
	for (i = 0; i < Nw; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			EHfit[i][j] = mEHfit(i, j);
		}
	}

	// �ͷ�
	delete[] aT2T1Dist;
}

// D-T2 ����
void DT2(double** EH, double* Gk2, int Ng, double* tau, int NECH, double Dmin, double Dmax, int Nd, double Tmin, double Tmax, double alpha, int Nt, double Delta1,
	double delta2, double** T2DDist, double** EHfit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError)
{
	int i, j;
	int Ne = 200;  // ��������200
	double gamma = 2 * PI * 42.576e+6;

	// �������滻
	mat mEH = zeros(Ng, NECH);
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			mEH(i, j) = EH[i][j];
		}
	}

	// ��ʼ��
	mat K1 = zeros(Ng, Nd);
	mat K20 = zeros(NECH, Nt);
	vec mT2 = logspace(log10(Tmin), log10(Tmax), Nt);  // T2 ��ԥʱ��
	vec mD = logspace(log10(Dmin), log10(Dmax), Nd);   // D �ֲ���Χ
	// ���س�ԥʱ��
	for (i = 0; i < Nt; i++)
	{
		T2[i] = mT2(i);
	}
	// ������ɢϵ���ֲ���Χ
	for (i = 0; i < Nd; i++)
	{
		D[i] = mD(i);
	}

	// �˾���ֵ
	for (i = 0; i < NECH; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			K20(i, j) = exp(-tau[i] / T2[j]);
		}
	}
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < Nd; j++)
		{
			K1(i, j) = exp(-gamma * gamma * Gk2[i] * delta2 * delta2 * (Delta1 - delta2 / 3) * mD(j));
		}
	}
	// ����ѹ��
	if (NECH <= Ne)  // ��һ�����趨�ɼ����ݵ����ô�٣���200��С��
	{
		Ne = NECH;
	}
	mat EH0 = zeros(Ng, Ne);
	mat K2 = zeros(Ne, Nt);
	int* IndexNumber = new int[Ne];
	AmplitudeSample(NECH, Ne, IndexNumber);
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < Ne; j++)
		{
			EH0(i, j) = mEH(i, IndexNumber[j]);
		}
	}
	for (i = 0; i < Ne; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			K2(i, j) = K20(IndexNumber[i], j);
		}
	}
	// һ�㴦��
	for (j = 0; j < Nt; j++)
	{
		if (K2(0, j) < EPS)
		{
			for (i = 0; i < Ne; i++)
			{
				K2(i, j) = 0;
			}
		}
		else
		{
			break;
		}
	}

	// SVD �ֽ⡢����ѹ��
	double cutnumber = 10000;  // ������10000���ضϱ�׼
							   // K1�˾���
	mat u;
	vec s;
	mat v;
	svd(u, s, v, K1, "std");
	int ncut = Ng;
	if (Ng > Nt)  // ��һ�����趨��ԥʱ������Ȳɼ���������������
	{
		ncut = Nt;
	}
	// K1 ��������ֵ����
	int num1 = 0;
	for (i = 0; i < ncut; i++)
	{
		if (s(0) / s(i) <= cutnumber)
		{
			num1 = num1 + 1;
		}
		else
		{
			break;
		}
	}
	// ѹ�������
	mat u1 = u.cols(0, num1 - 1);
	mat v1 = v.cols(0, num1 - 1);
	mat s1 = zeros(num1, num1);
	for (i = 0; i < num1; i++)
	{
		s1(i, i) = s(i);
	}
	// K2 �˾���
	ncut = Nt;
	if (Nt > Ne)  // ��һ�����趨��ԥʱ������Ȳɼ����ݵ���������
	{
		ncut = Ne;
	}
	u = zeros(Ne, Ne);
	v = zeros(Nt, Nt);
	s = zeros<vec>(ncut, 1);
	svd(u, s, v, K2, "std");
	// K2 ��������ֵ����
	int num2 = 0;
	for (i = 0; i < ncut; i++)
	{
		if (s(0) / s(i) <= cutnumber)
		{
			num2 = num2 + 1;
		}
		else
		{
			break;
		}
	}
	// ѹ�������
	mat u2 = u.cols(0, num2 - 1);
	mat v2 = v.cols(0, num2 - 1);
	mat s2 = zeros(num2, num2);
	for (i = 0; i < num2; i++)
	{
		s2(i, i) = s(i);
	}

	// ����任
	mat Mr = u1.t() * EH0 * u2;
	mat K1r = s1 * v1.t();
	mat K2r = s2 * v2.t();
	mat K = kron(K1r, K2r);
	mat Mrs = reshape(Mr.t(), num1 * num2, 1);

	// BRD ����
	double* aT2DDist = new double[Nd * Nt];
	BRD(K, Mrs, num1 * num2, Nd * Nt, alpha, aT2DDist);
	// ����任
	mat T2D = zeros(Nd, Nt);
	for (i = 0; i < Nd; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			T2D(i, j) = aT2DDist[i * Nt + j];
			T2DDist[i][j] = T2D(i, j);
		}
	}
	// T2 �ֲ��� D �ֲ�
	// ������
	mat mT2Dist = sum(T2D, 0); 
	mat mDDist = sum(T2D, 1);
	// ����źź����������
	mat mEHfit = K1 * T2D * K20.t();
	ReError = norm(mEHfit - mEH, 2) / norm(mEH, 2);
	// ���� D �� T2
	for (i = 0; i < Nd; i++)
	{
		DDist[i] = mDDist(i);
	}
	for (i = 0; i < Nt; i++)
	{
		T2Dist[i] = mT2Dist(i);
	}
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			EHfit[i][j] = mEHfit(i, j);
		}
	}

	// �ͷ�
	delete[] IndexNumber;
	delete[] aT2DDist;
}