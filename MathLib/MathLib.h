#pragma once
// 导出的函数
extern "C"
{
	_declspec(dllexport) void SaveMat1(double* data, int len, char* fileName);
	_declspec(dllexport) void SaveIMat1(int* data, int len, char* fileName);
	_declspec(dllexport) void SaveMat2(double* data, int row, int col, char* fileName);
	_declspec(dllexport) void SaveIMat2(int* data, int row, int col, char* fileName);

	_declspec(dllexport) double Random(int flag);
	_declspec(dllexport) void RandomU2(double* input, int row, int col);
	_declspec(dllexport) void RandomU1(double* input, int len);
	_declspec(dllexport) void RandomN2(double* input, int row, int col);
	_declspec(dllexport) void RandomN1(double* input, int len);
	_declspec(dllexport) int RandomInt(int maxi);
	_declspec(dllexport) void RandomInt2(int* input, int maxi, int row, int col);
	_declspec(dllexport) void RandomInt1(int* input, int maxi, int len);


	_declspec(dllexport) double Stddev1(double* input, int len);
	_declspec(dllexport) void Stddev2(double* input, int row, int col, int flag, double* result);
	_declspec(dllexport) double Var1(double* input, int len);
	_declspec(dllexport) void Var2(double* input, int row, int col, int flag, double* result);


	_declspec(dllexport) int Rank1(double* input, int row, int col);
	_declspec(dllexport) int Rank2(double* input, int row, int col, double tolerance);
	_declspec(dllexport) void Kron(double* input1, int row1, int col1, double* input2, int row2, int col2, double* result);
	_declspec(dllexport) void Svd(double* input, int row, int col, double* u, double* s, double* v);


	_declspec(dllexport) void Interp1(double* x, double* y, int len1, double* xi, int len2, int flag, double* yi);
	_declspec(dllexport) void Sort(double* input, int len, int flag, double* result);
	_declspec(dllexport) void SortIndex(double* input, int len, int flag, int* result);


	_declspec(dllexport) void Fft(double* x, double* y, int len, double* xx, double* yy, double* amp);
	_declspec(dllexport) void Ifft(double* x, double* y, int len, double* xx, double* yy, double* amp);


	_declspec(dllexport) void Polyfit(double* x, double* y, int len, int N, double* p);
	_declspec(dllexport) void Polyval(double* x, int len1, double* p, int len2, double* y);
	_declspec(dllexport) void Solve(double* A, int m, int n, double* y, double* x);
	_declspec(dllexport) void Lsqnonneg(double* A, int m, int n, double* y, double* x);
	_declspec(dllexport) void ExpFitting(double* x, double* y, int len, int N, double* p);
	_declspec(dllexport) void GaussFitting(double* x0, double* y0, int M, double* p);


	_declspec(dllexport) void Conv2(double* input1, int row1, int col1, double* input2, int row2, int col2, double* output, int row3, int col3);
	_declspec(dllexport) void Wden(double* Input, double* Output, int signal_size);
	_declspec(dllexport) void Wdbn(double* Input, double* Output, int signal_size, int scale, int dbn, bool isHard);
	_declspec(dllexport) void Wdbn2(double* Input, double* Output, int height, int width, int scale, int dbn);
	_declspec(dllexport) void WHaar(double* Input, double* Output, int signal_size, int scale);
	_declspec(dllexport) void Nlm(double* Input, double* Output, int signal_size, int window_width, int patch_width, double sigma);
	_declspec(dllexport) void Nlm2(double* Input, double* Output, int height, int width, int window_width, int patch_width, double sigma);


	_declspec(dllexport) void InvSIRT(double* TArray, double* MeaAmp, int M, int N, int iters, double* T2Dist);
	_declspec(dllexport) void InvBRD(double* TArray, double* MeaAmp, int M, int N, double a0, double* T2Dist);
	_declspec(dllexport) void InvCG(double* TArray, double* MeaAmp, int M, int N, int iters, double* T2Dist);
	_declspec(dllexport) void InvSVD(double* TArray, double* MeaAmp, int M, int N, double a0, double* T2Dist);


	_declspec(dllexport) void InvT1T2(double* EH, double* tau1, int Nw, double* tau2, int NECH, double Tmin, double Tmax, double alpha, int Nt, int invModel, double* T2T1Dist, double* EHfit, double* T, double* T1Dist, double* T2Dist, double& ReError);
	_declspec(dllexport) void InvDT2(double* EH, double* Gk2, int Ng, double* tau, int NECH, double Dmin, double Dmax, int Nd, double Tmin, double Tmax, double alpha, int Nt, double Delta1, double delta2, double* T2DDist, double* EHfit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError);
	_declspec(dllexport) void InvDT2_Gcst(double* SEQtau2, double* SEQsig, int* NECH, int N_echo, double* TE, double Gcst, double Dmin, double Dmax, double Tmin, double Tmax, int nPreset, double alpha, double* T2DDist, double* d_fit, double* D, double* DDist, double* T2, double* T2Dist, double& ReError);
}