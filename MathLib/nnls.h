#pragma once
#include "common.h"

// ����һЩ�Ӻ���
bool NNLS_Out_Loop_Condition(mat Z, mat w, double tol);
mat NNLS_Set_Value_wz(mat P, mat Z, mat w);
mat NNLS_Set_Value_z(mat C, mat P, mat d);
bool NNLS_Inner_Loop_Condition(mat z, mat P);
double NNLS_Set_Value_alpha(mat x, mat z, mat Q);


// NNLS ������ �޸��� Matlab
mat NNLS(mat C, mat d)
{
	// x = NNLS(C, d)���� x ���� min(norm(d - C*x)) �� x >= 0��
	// ��ʼ��
	int m = C.n_rows;
	int n = C.n_cols;
	mat nZeros = zeros(n, 1);
	mat wz = nZeros;
	mat P = zeros(n, 1);      // Matlab: P = false(n, 1);
	mat Z = ones(n, 1);       // Matlab: Z = true(n, 1);
	mat x = nZeros;
	mat resid = d - C * x;
	mat w = C.t() * resid;
	mat z = x;
	// ����׼��
	int iter = 0;
	int itmax = 3 * n;
	double tol = 10 * EPS * norm(C, 1) * m;  // �������  tol = 10*eps*norm(C,1)*length(C);
	int t;
	mat Q;
	double alpha;
	// ���ѭ��
	while (NNLS_Out_Loop_Condition(Z, w, tol))
	{
		z = nZeros;
		wz = NNLS_Set_Value_wz(P, Z, w);   // Matlab: wz(P) = -Inf; wz(Z) = w(Z);
		t = wz.index_max();
		// ���� t ���㼯�ϵ�������  Matlab: P(t) = true; Z(t) = false;
		P(t) = 1;
		Z(t) = 0;
		// �����м��  Matlab: z(P) = C(:, P)\d;
		z = NNLS_Set_Value_z(C, P, d);
		// �ڲ�ѭ��
		while (NNLS_Inner_Loop_Condition(z, P))
		{
			iter = iter + 1;
			if (iter > itmax)
			{
				x = z;
				return x;
			}
			Q = any(z.t() <= 0) % P.t();  // Matlab: Q = (z <= 0) & P;
			alpha = NNLS_Set_Value_alpha(x, z, Q);
			x = x + alpha * (z - x);
			for (int i = 0; i < n; i++)  // Matlab: Z = ((abs(x) < tol) & P) | Z;
			{
				if ((abs(x(i)) < tol) && (P(i) == 1) || (Z(i) == 1))
				{
					Z(i) = 1;
				}
				P(i) = 1 - Z(i);  // Matlab: P = ~Z;
			}
			z = nZeros;
			z = NNLS_Set_Value_z(C, P, d);
		}
		x = z;
		resid = d - C * x;
		w = C.t() * resid;
	}
	return x;
}


// ���õ��Ӻ���
bool NNLS_Out_Loop_Condition(mat Z, mat w, double tol)
{
	int n = Z.n_elem;
	bool result = false;
	bool bool1 = any(vectorise(Z));
	bool bool2 = false;
	for (int i = 0; i < n; i++)
	{
		if (Z[i] == 1)
		{
			if (w[i] > tol)
			{
				bool2 = true;
			}
		}
	}
	if (bool1 && bool2)
	{
		result = true;
	}
	return result;
}

mat NNLS_Set_Value_wz(mat P, mat Z, mat w)
{
	int n = P.n_elem;
	mat wz = zeros(n, 1);
	for (int i = 0; i < n; i++)
	{
		if (P[i] == 1)
		{
			wz[i] = -1.0e+300;   // Matlab -Inf
		}
		if (Z[i] == 1)
		{
			wz[i] = w[i];
		}
	}
	return wz;
}

mat NNLS_Set_Value_z(mat C, mat P, mat d)
{
	int m = C.n_rows;
	int n = C.n_cols;
	int n_cols = 0;   // ����Ԫ�ظ���
	for (int i = 0; i < n; i++)
	{
		if (P(i) == 1)
		{
			n_cols = n_cols + 1;
		}
	}
	mat C_part = zeros(m, n_cols);  // ��ȡ����Ԫ�ض�Ӧ�ľ���
	n_cols = 0;
	for (int i = 0; i < n; i++)
	{
		if (P(i) == 1)
		{
			C_part.col(n_cols) = C.col(i);
			n_cols = n_cols + 1;
		}
	}
	// mat z_part = solve(C_part, d);   // ��⣬���ܻ����쳣
	mat z_part = Solve(C_part, d);
	mat z = zeros(n, 1);
	n_cols = 0;
	for (int i = 0; i < n; i++)
	{
		if (P(i) == 1)
		{
			z(i) = z_part(n_cols);
			n_cols = n_cols + 1;
		}
	}
	return z;
}

bool NNLS_Inner_Loop_Condition(mat z, mat P)
{
	int n = P.n_elem;
	bool result = false;
	for (int i = 0; i < n; i++)
	{
		if (P(i) == 1)
		{
			if (z(i) <= 0)
			{
				result = true;
				break;
			}
		}
	}
	return result;
}

double NNLS_Set_Value_alpha(mat x, mat z, mat Q)
{
	int n = z.n_elem;
	double result = 1;
	double alpha;
	for (int i = 0; i < n; i++)
	{
		if (Q(i) == 1)
		{
			alpha = x(i) / (x(i) - z(i));
			if (alpha < result)
			{
				result = alpha;
			}
		}
	}
	return result;
}