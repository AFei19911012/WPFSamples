#pragma once
#include "common.h"

// 对结果进行排序
mat SortResult(mat p)
{
	mat pp = p;
	int len = p.n_elem;
	if (len != 5 && len != 7)
	{
		return pp;
	}
	int halfLen = len / 2;
	mat p1 = zeros(1, halfLen);
	mat p2 = zeros(1, halfLen);
	for (int i = 0; i < halfLen; i++)
	{
		p1[i] = p[2 * i];
		p2[i] = p[2 * i + 1];
	}
	for (int i = 0; i < halfLen; i++)
	{
		int indexMin = p2.index_min();
		pp[2 * i] = p1[indexMin];
		pp[2 * i + 1] = p2[indexMin];
		p2[indexMin] = 1e+12;
	}
	return pp;
}

// 指数衰减拟合，最多支持三指数
mat LSFittingT2(mat x, mat y, int invFlag)
{
	// 数据长度
	int M = x.n_elem;
	// 变量个数
	int nParam = 2 * invFlag + 1;
	double a, b, aa, bb, aaa, bbb, c;
	double a1, b1, aa1, bb1, aaa1, bbb1, c1;
	// Least Squares Minimization
	// 尝试不同的初始值反演
	for (int num = 0; num < 100; num++)
	{
		// 初始值
		a = sqrt(Rand(0));
		b = sqrt(Rand(0));
		aa = sqrt(Rand(0));
		bb = sqrt(Rand(0));
		aaa = sqrt(Rand(0));
		bbb = sqrt(Rand(0));
		c = sqrt(Rand(0));
		// 计算残差
		mat r = y - (pow(a, 2) * exp(-x / pow(b, 2)) + pow(c, 2));
		mat r1 = r;
		if (invFlag == 2)
		{
			r = y - (pow(a, 2) * exp(-x / pow(b, 2)) + pow(aa, 2) * exp(-x / pow(bb, 2)) + pow(c, 2));
		}
		else if (invFlag == 3)
		{
			r = y - (pow(a, 2) * exp(-x / pow(b, 2)) + pow(aa, 2) * exp(-x / pow(bb, 2)) + pow(aaa, 2) * exp(-x / pow(bbb, 2)) + pow(c, 2));
		}
		// 目标函数
		double f = pow(norm(r), 2);
		// 正则化因子初值
		double lambda = 1;
		// 更新标记
		bool updateFlag = true;
		// 稳定次数
		int cntConv = 0;
		// 最大迭代次数
		int maxIter = 100;
		// 迭代次数
		int it = 0;
		// 迭代计算
		mat g = zeros(nParam, 1);
		mat H = eye(nParam, nParam);
		while (it < maxIter)
		{
			it = it + 1;
			if (updateFlag)
			{
				mat Ja = 2 * a * exp(-x / pow(b, 2));
				mat Jb = 2 * pow(a, 2) * x % exp(-x / pow(b, 2)) / pow(b, 3);
				mat Jc = 2 * c * ones(M, 1);
				mat J = join_rows(Ja, Jb);
				if (invFlag == 1)
				{
					J = join_rows(J, Jc);
				}
				else
				{
					mat Jaa = 2 * aa * exp(-x / pow(bb, 2));
					mat Jbb = 2 * pow(aa, 2) * x % exp(-x / pow(bb, 2)) / pow(bb, 3);
					J = join_rows(J, Jaa);
					J = join_rows(J, Jbb);
					if (invFlag == 2)
					{
						J = join_rows(J, Jc);
					}
					else
					{
						mat Jaaa = 2 * aaa * exp(-x / pow(bbb, 2));
						mat Jbbb = 2 * pow(aaa, 2) * x % exp(-x / pow(bbb, 2)) / pow(bbb, 3);
						J = join_rows(J, Jaaa);
						J = join_rows(J, Jbbb);
						J = join_rows(J, Jc);
					}
				}
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
			if (invFlag == 1)
			{
				c1 = c + s(2);
				r1 = y - (pow(a1, 2) * exp(-x / pow(b1, 2)) + pow(c1, 2));
			}
			else
			{
				aa1 = aa + s(2);
				bb1 = bb + s(3);
				if (invFlag == 2)
				{
					c1 = c + s(4);
					r1 = y - (pow(a1, 2) * exp(-x / pow(b1, 2)) + pow(aa1, 2) * exp(-x / pow(bb1, 2)) + pow(c1, 2));
				}
				else
				{
					aaa1 = aaa + s(4);
					bbb1 = bbb + s(5);
					c1 = c + s(6);
					r1 = y - (pow(a1, 2) * exp(-x / pow(b1, 2)) + pow(aa1, 2) * exp(-x / pow(bb1, 2)) + pow(aaa1, 2) * exp(-x / pow(bbb1, 2)) + pow(c1, 2));
				}
			}
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
				if (invFlag > 1)
				{
					aa = aa1;
					bb = bb1;
					if (invFlag == 3)
					{
						aaa = aaa1;
						bbb = bbb1;
					}
				}
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
	mat result = zeros(nParam, 1);
	result(0) = a * a;
	result(1) = b * b;
	if (invFlag == 1)
	{
		result(2) = c * c;
	}
	else
	{
		result(2) = aa * aa;
		result(3) = bb * bb;
		if (invFlag == 2)
		{
			result(4) = c * c;
		}
		else
		{
			result(4) = aaa * aaa;
			result(5) = bbb * bbb;
			result(6) = c * c;
		}
	}
	result = SortResult(result);
	return result;
}