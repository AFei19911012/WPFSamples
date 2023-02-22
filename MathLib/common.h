#pragma once

#include "armadillo"

using namespace arma;

// ����
const double EPS = 2.2204e-16;           // ��С��
const double PI = 3.141592653589793;     // ��


#pragma region ����ģ��
// ��Сֵ
template <typename T>
inline T Min(T a, T b)
{
	return a > b ? b : a;
}

// ���ֵ
template <typename T>
inline T Max(T a, T b)
{
	return a > b ? a : b;
}

// ����
template <typename T>
inline void Swap(T& a, T& b)
{
	T temp = a;
	a = b;
	b = temp;
}
#pragma endregion


// SVD �ⷽ��
mat Solve(mat A, mat y)
{
	int m = A.n_rows;
	int n = A.n_cols;
	mat u;
	vec vs;
	mat v;
	svd(u, vs, v, A, "std");
	mat s = zeros(n, m);
	// Сֵ
	int len = Min(m, n);
	for (int i = 0; i < len; i++)
	{
		s(i, i) = 1 / (vs(i) + EPS);
		if (s(i, i) > 1e+12)
		{
			s(i, i) = 0;   // �ض�
		}
	}
	mat x = v * s * u.t() * y;
	return x;
}

// ����������ȷֲ�����˹�ֲ�
double Rand(int flag)
{
	//arma_rng::set_seed_random();
	mat result = randu(1);
	if (flag == 1)
	{
		result = randn(1);
	}
	return result(0);
}

// ������ֵ
mat Spline(mat x, mat y, mat xx)
{
	int i, j, ii;
	int n = x.n_elem;
	mat a = y.rows(0, n - 2);
	mat b = zeros(n - 1, 1);
	mat d = zeros(n - 1, 1);
	mat dx = diff(x);
	mat dy = diff(y);
	mat A = zeros(n, n);
	mat B = zeros(n, 1);
	A(0, 0) = 1;
	A(n - 1, n - 1) = 1;
	for (i = 1; i < n - 1; i++)
	{
		A(i, i - 1) = dx(i - 1);
		A(i, i) = 2 * (dx(i - 1) + dx(i));
		A(i, i + 1) = dx(i);
		B(i) = 3 * (dy(i) / dx(i) - dy(i - 1) / dx(i - 1));
	}
	mat c = Solve(A, B);   // A\B
	for (i = 0; i < n - 1; i++)
	{
		d(i) = (c(i + 1) - c(i)) / (3 * dx(i));
		b(i) = dy(i) / dx(i) - dx(i) * (2 * c(i) + c(i + 1)) / 3;
	}
	int m = xx.n_elem;
	mat yy = zeros(m, 1);
	for (i = 0; i < m; i++)
	{
		for (ii = 0; ii < n - 1; ii++)
		{
			if ((xx(i) >= x(ii)) && (xx(i) < x(ii + 1)))
			{
				j = ii;
				break;
			}
			else if (abs(xx(i) - x(n - 1)) < EPS)  // xx(i) == x(n)
			{
				j = n - 2;
			}
		}
		yy(i) = a(j) + b(j) * (xx(i) - x(j)) + c(j) * pow(xx(i) - x(j), 2) + d(j) * pow(xx(i) - x(j), 3);
	}
	return yy;
}