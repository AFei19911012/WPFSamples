#pragma once
#include "common.h"


// 计算 Hessian 矩阵
void HessianCal(mat K, int m, int n, mat f, double a0, mat& h0)
{
	/*
		K：核矩阵，二维矩阵；
		m：mK长度；
		n：mK宽度；
		f：解，一维；
		a0：正则化系数；
		h0：返回Hessian矩阵
	*/
	int i, j;
	// 给f做标记，大于0的赋值为下标，小于0的赋值-1
	imat temp = zeros<imat>(n, 1);
	// 记录f不为零的下标
	imat IndexNumber;
	// 压缩之后的K
	mat K1;
	// 统计大于0的个数
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
	// f不全为0
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

// T2 相位旋转
void T2PhaseRotation(double* Amplitude, double* Noise, int M, double* DataReal, double* DataImaginary)
{
	int i;
	// 用于相位旋转的点数
	int N = M;
	if (N > 10)
	{
		N = 10;
	}
	// 计算相位角
	double sre = 0;
	double sim = 0;
	for (i = 0; i < N; i++)
	{
		sre += DataReal[i];
		sim += DataImaginary[i];
	}
	// 相位角
	double alpha = atan(sim / (sre + EPS));
	// 计算信号和噪声
	for (i = 0; i < M; i++)
	{
		Amplitude[i] = DataReal[i] * cos(alpha) + DataImaginary[i] * sin(alpha);
		Noise[i] = DataReal[i] * sin(alpha) - DataImaginary[i] * cos(alpha);
	}
	// 翻转？
	// 改为小于 0 翻转，防止低信噪比首点信号值异常低   2018-1-17
	if (Amplitude[0] < 0)
	{
		for (i = 0; i < M; i++)
		{
			Amplitude[i] = -Amplitude[i];
		}
	}
}

// T1 相位旋转
void T1PhaseRotation(double* Amplitude, double* Noise, int M, double* DataReal, double* DataImaginary)
{
	int i;
	// 用于相位旋转的点数
	int N = M / 2 + 1;
	// 计算相位角
	double sre = 0;
	double sim = 0;
	for (i = 0; i < N; i++)
	{
		sre += DataReal[i];
		sim += DataImaginary[i];
	}
	// 相位角
	double alpha = atan(sim / (sre + EPS));
	if (alpha < 0)
	{
		alpha += PI;
	}
	// 计算信号和噪声
	for (i = 0; i < M; i++)
	{
		Amplitude[i] = DataReal[i] * cos(alpha) + DataImaginary[i] * sin(alpha);
		Noise[i] = DataReal[i] * sin(alpha) - DataImaginary[i] * cos(alpha);
	}
	// 翻转？
	if (Amplitude[0] > Amplitude[M - 1])
	{
		for (i = 0; i < M; i++)
		{
			Amplitude[i] = -Amplitude[i];
		}
	}
}

// 计算信噪比
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

// 窗函数滤波
void AmplitudeFilt(double* Amplitude, int filterIndex, int M, double* FAmplitude)
{
	int i, Width;
	// 滤波指数为0即退出，返回原始信号
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
	// 初始化
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
	// 计算滤波后信号
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
	// 释放
	delete[] PeakIndex;
	delete[] WindowWidth;
	delete[] WindowRange;
	delete[] prob;
	delete[] Amplitude1;
}

// 数据抽样
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
	// 释放
	delete[] SampIndex;
	delete[] tp;
	delete[] flag;
}

// BRD
void BRD(mat K, mat d, int M, int N, double a0, double* T2Dist)
{
	// 初始化
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
		// 计算Hessian矩阵  heavyside output:0|1
		// h0 = K*sparse( diag(f>0) )*K' + a0.*eye(size(G))
		HessianCal(K, M, N, f, a0, h0);
		// grad
		g0 = h0 * c0 - d;
		// phi_0 = 1/2*c0'*h0*c0-c0'*d
		phi_0 = 0.5 * c0.t() * h0 * c0 - c0.t() * d;
		// 牛顿方向  p = -h0\g0
		p = -1 * solve(h0, g0);

		// 不精确线性搜索，Wolf-Powell准则
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
				// 非线性搜索找到步长
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
			// 搜索区间过窄
			if (abs(a - b) < 1e-3)
			{
				break;
			}
		}
		// 变化幅度过小
		if (norm(c0 - c_1) / (norm(c0) + EPS) < 1e-3)
		{
			break;
		}
		c0 = c_1;
	}
	// 返回解
	for (int i = 0; i < N; i++)
	{
		T2Dist[i] = f_1(i);
	}
}

// T1-T2 反演
void T1T2(double** EH, double* tau1, int Nw, double* tau2, int NECH, double Tmin, double Tmax, double alpha, int Nt, int invModel, double** T2T1Dist,
	double** EHfit, double* T, double* T1Dist, double* T2Dist, double& ReError)
{
	int i, j;
	int Ne = 200;  // 抽样点数200
	if ((invModel != 1) && (invModel != 2))
	{
		return;
	}

	// 新类型替换
	mat mEH = zeros(Nw, NECH);
	for (i = 0; i < Nw; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			mEH(i, j) = EH[i][j];
		}
	}

	// 初始化
	mat K1 = zeros(Nw, Nt);
	mat K20 = zeros(NECH, Nt);
	vec T1 = logspace(log10(Tmin), log10(Tmax), Nt);  // T1和T2相等
	vec T2 = T1;
	// 返回弛豫时间
	for (i = 0; i < Nt; i++)
	{
		T[i] = T1(i);
	}

	// 核矩阵赋值
	for (i = 0; i < NECH; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			K20(i, j) = exp(-tau2[i] / T2(j));
		}
	}
	switch (invModel)   // 饱和恢复还是反转恢复
	{
	case 1:  // 饱和恢复
		for (i = 0; i < Nw; i++)
		{
			for (j = 0; j < Nt; j++)
			{
				K1(i, j) = 1 - exp(-tau1[i] / T1(j));
			}
		}
		break;
	case 2:  // 反转恢复
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

	// 数据压缩
	if (NECH <= Ne)  // 万一奇葩设定采集数据点数恁么少，比200还小呢
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
	// 一点处理
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
	// SVD 分解、数据压缩
	double cutnumber = 10000;  // 条件数10000，截断标准
							   // K1核矩阵
	mat u;
	vec s;
	mat v;
	svd(u, s, v, K1, "std");
	int ncut = Nw;
	if (Nw > Nt)    // 万一奇葩设定弛豫时间点数比采集数据条数还少呢
	{
		ncut = Nt;
	}
	// K1 保留奇异值个数
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
	// 压缩后矩阵
	mat u1 = u.cols(0, num1 - 1);
	mat v1 = v.cols(0, num1 - 1);
	mat s1 = zeros(num1, num1);
	for (i = 0; i < num1; i++)
	{
		s1(i, i) = s(i);
	}
	// K2 核矩阵
	ncut = Nt;
	if (Nt > Ne)  // 万一奇葩设定弛豫时间点数比采集数据点数还多呢
	{
		ncut = Ne;
	}
	u = zeros(Ne, Ne);
	v = zeros(Nt, Nt);
	s = zeros<vec>(ncut, 1);
	svd(u, s, v, K2, "std");
	// K2 保留奇异值个数
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
	// 压缩后矩阵
	mat u2 = u.cols(0, num2 - 1);
	mat v2 = v.cols(0, num2 - 1);
	mat s2 = zeros(num2, num2);
	for (i = 0; i < num2; i++)
	{
		s2(i, i) = s(i);
	}

	// 矩阵变换
	mat Mr = u1.t() * EH0 * u2;
	mat K1r = s1 * v1.t();
	mat K2r = s2 * v2.t();
	mat K = kron(K1r, K2r);
	mat Mrs = reshape(Mr.t(), num1 * num2, 1);

	// BRD 反演
	double* aT2T1Dist = new double[Nt * Nt];
	BRD(K, Mrs, num1 * num2, Nt * Nt, alpha, aT2T1Dist);
	// 矩阵变换
	mat T2T1 = zeros(Nt, Nt);
	for (i = 0; i < Nt; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			T2T1(i, j) = aT2T1Dist[i * Nt + j];
			T2T1Dist[i][j] = T2T1(i, j);
		}
	}
	// T2 分布和 T1 分布 
	// 积分谱
	mat mT2Dist = sum(T2T1, 0);
	mat mT1Dist = sum(T2T1, 1);
	// 拟合信号和拟合相对误差
	mat mEHfit = K1 * T2T1 * K20.t();
	ReError = norm(mEHfit - mEH, 2) / norm(mEH, 2);
	// 返回 T1 和 T2
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

	// 释放
	delete[] aT2T1Dist;
}

// D-T2 反演
void DT2(double** EH, double* Gk2, int Ng, double* tau, int NECH, double Dmin, double Dmax, int Nd, double Tmin, double Tmax, double alpha, int Nt, double Delta1,
	double delta2, double** T2DDist, double** EHfit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError)
{
	int i, j;
	int Ne = 200;  // 抽样点数200
	double gamma = 2 * PI * 42.576e+6;

	// 新类型替换
	mat mEH = zeros(Ng, NECH);
	for (i = 0; i < Ng; i++)
	{
		for (j = 0; j < NECH; j++)
		{
			mEH(i, j) = EH[i][j];
		}
	}

	// 初始化
	mat K1 = zeros(Ng, Nd);
	mat K20 = zeros(NECH, Nt);
	vec mT2 = logspace(log10(Tmin), log10(Tmax), Nt);  // T2 弛豫时间
	vec mD = logspace(log10(Dmin), log10(Dmax), Nd);   // D 分布范围
	// 返回弛豫时间
	for (i = 0; i < Nt; i++)
	{
		T2[i] = mT2(i);
	}
	// 返回扩散系数分布范围
	for (i = 0; i < Nd; i++)
	{
		D[i] = mD(i);
	}

	// 核矩阵赋值
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
	// 数据压缩
	if (NECH <= Ne)  // 万一奇葩设定采集数据点数恁么少，比200还小呢
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
	// 一点处理
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

	// SVD 分解、数据压缩
	double cutnumber = 10000;  // 条件数10000，截断标准
							   // K1核矩阵
	mat u;
	vec s;
	mat v;
	svd(u, s, v, K1, "std");
	int ncut = Ng;
	if (Ng > Nt)  // 万一奇葩设定弛豫时间点数比采集数据条数还少呢
	{
		ncut = Nt;
	}
	// K1 保留奇异值个数
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
	// 压缩后矩阵
	mat u1 = u.cols(0, num1 - 1);
	mat v1 = v.cols(0, num1 - 1);
	mat s1 = zeros(num1, num1);
	for (i = 0; i < num1; i++)
	{
		s1(i, i) = s(i);
	}
	// K2 核矩阵
	ncut = Nt;
	if (Nt > Ne)  // 万一奇葩设定弛豫时间点数比采集数据点数还多呢
	{
		ncut = Ne;
	}
	u = zeros(Ne, Ne);
	v = zeros(Nt, Nt);
	s = zeros<vec>(ncut, 1);
	svd(u, s, v, K2, "std");
	// K2 保留奇异值个数
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
	// 压缩后矩阵
	mat u2 = u.cols(0, num2 - 1);
	mat v2 = v.cols(0, num2 - 1);
	mat s2 = zeros(num2, num2);
	for (i = 0; i < num2; i++)
	{
		s2(i, i) = s(i);
	}

	// 矩阵变换
	mat Mr = u1.t() * EH0 * u2;
	mat K1r = s1 * v1.t();
	mat K2r = s2 * v2.t();
	mat K = kron(K1r, K2r);
	mat Mrs = reshape(Mr.t(), num1 * num2, 1);

	// BRD 反演
	double* aT2DDist = new double[Nd * Nt];
	BRD(K, Mrs, num1 * num2, Nd * Nt, alpha, aT2DDist);
	// 矩阵变换
	mat T2D = zeros(Nd, Nt);
	for (i = 0; i < Nd; i++)
	{
		for (j = 0; j < Nt; j++)
		{
			T2D(i, j) = aT2DDist[i * Nt + j];
			T2DDist[i][j] = T2D(i, j);
		}
	}
	// T2 分布和 D 分布
	// 积分谱
	mat mT2Dist = sum(T2D, 0); 
	mat mDDist = sum(T2D, 1);
	// 拟合信号和拟合相对误差
	mat mEHfit = K1 * T2D * K20.t();
	ReError = norm(mEHfit - mEH, 2) / norm(mEH, 2);
	// 返回 D 和 T2
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

	// 释放
	delete[] IndexNumber;
	delete[] aT2DDist;
}