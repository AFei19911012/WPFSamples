#pragma once
#include "common.h"

// ��������
imat RegionGrowing(imat Img, int x0, int y0, int reg_maxdist)
{
	/*
	Img����ά�Ҷ�ֵ���飬0~255֮�䣻
	x0�����ӵ��������ţ�
	y0�����ӵ���������ţ�
	reg_maxdist����ֵ��
	���غ� Img �ȴ�����飬����Ȥ������ 1 ��ʾ��
	*/
	// ά��
	int row = Img.n_rows;
	int col = Img.n_cols;
	imat Jm = zeros<imat>(row, col);  // ���
	double reg_mean = Img(x0, y0);   // ���ӵ�ĻҶ�ֵ
	Jm(x0, y0) = 1;   // ���ӵ�����Ϊ1
	double reg_sum = reg_mean;  // �������������ĻҶ�ֵ�ܺ�
	int reg_num = 1;  // �������������ĵ�ĸ���
	int count = 1;    // ÿ���ж���Χ�˸����з�����������Ŀ
					  // ��¼��ѡ�������
	imat reg_choose = zeros<imat>(row * col, 2);
	reg_choose(reg_num - 1, 0) = x0;
	reg_choose(reg_num - 1, 1) = y0;
	int num = 1;  // ��һ����
	double s_temp;   // ��Χ�˸����з��������ĵ�ĻҶ�ֵ�ܺ�
	int i, j, k, u, v;
	while (count > 0)
	{
		s_temp = 0;
		count = 0;
		// ��������ÿ��������������ظ�
		for (k = 0; k < num; k++)
		{
			i = reg_choose(reg_num - num + k, 0);
			j = reg_choose(reg_num - num + k, 1);
			// ��ȷ���Ҳ��Ǳ߽��ϵĵ�
			if ((Jm(i, j) == 1) && (i > 0) && (i < row - 1) && (j > 0) && (j < col - 1))
			{
				// ������
				for (u = -1; u <= 1; u++)
				{
					for (v = -1; v <= 1; v++)
					{
						// δ�������������������ĵ�
						if ((Jm(i + u, j + v) == 0) && (abs(Img(i + u, j + v) - reg_mean) <= reg_maxdist))
						{
							Jm(i + u, j + v) = 1;   // ��Ӧ������Ϊ1
							count = count + 1;
							reg_choose(reg_num + count - 1, 0) = i + u;
							reg_choose(reg_num + count - 1, 1) = j + v;
							// �Ҷ�ֵ���� s_temp ��
							s_temp = s_temp + Img(i + u, j + v);
						}
					}
				}
			}
		}
		num = count;   // �����ĵ�
		reg_num = reg_num + count;  // �������ܵ���
		reg_sum = reg_sum + s_temp;  // �������ܻҶ�ֵ
		//reg_mean = reg_sum / reg_num;  // ����Ҷ�ƽ��ֵ
	}
	// �������ټ�һȦ 1
	imat Jm_copy = Jm;
	for (int i = 1; i < row - 1; i++)
	{
		for (int j = 1; j < col - 1; j++)
		{
			if (Jm_copy(i, j) == 1)
			{
				Jm(i - 1, j - 1) = 1;
				Jm(i - 1, j) = 1;
				Jm(i - 1, j + 1) = 1;
				Jm(i - 1, j) = 1;
				Jm(i - 1, j + 1) = 1;
				Jm(i + 1, j - 1) = 1;
				Jm(i + 1, j) = 1;
				Jm(i + 1, j + 1) = 1;
			}
		}
	}
	return Jm;
}

// ����߽�
imat RegionBoundary(imat Jm)
{
	/*
	Jm�����룬����Ȥ����ֵΪ 1������Ϊ 0��
	���ر߽�����꣬��һ��Ϊ���򣬵ڶ���Ϊ����
	*/
	int xlen = Jm.n_rows;
	int ylen = Jm.n_cols;
	imat Je = zeros<imat>(xlen + 2, ylen + 2);  // ��ֹ���ֱ߽�����
	int i, j;
	for (i = 1; i <= xlen; i++)
	{
		for (j = 1; j <= ylen; j++)
		{
			Je(i, j) = Jm(i - 1, j - 1);
		}
	}
	imat F = zeros<imat>(xlen + 2, ylen + 2);           // ��� Je ���Ѿ�ѡ��ı߽��
	imat P = zeros<imat>((xlen + 2) * (ylen + 2), 2);   // �߽���ţ���ʱ��
														// ȷ����һ���߽�� A������ѡ�񣬴�������
	int flag = 0;
	int m, n;
	for (i = 1; i < xlen + 2; i++)
	{
		for (j = 1; j < ylen + 2; j++)
		{
			if (Je(i, j) == 1)
			{
				// ��ʼ��
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
	// ��ʼ��
	i = m;
	j = n;
	int dir = 7;
	flag = 1;
	int count = 0;
	// �ӳ�ʼ�㿪ʼ������ֱ�������˳�����
	int xx, yy, p, q, xxx, yyy;
	while (1)
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
		while (abs(Je(i, j) - 1) > 0.1)
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
		F(i, j) = 1;
		xxx = i;
		yyy = j;
		count = count + 1;
		P(count - 1, 1) = xxx;
		P(count - 1, 0) = yyy;
		if ((m == xx) && (n == yy) && (p == xxx) && (q == yyy))
		{
			break;
		}
	}
	// �����ʱ��ԭ
	int num_non_zero = 0;
	for (i = 0; i < (xlen + 2) * (ylen + 2); i++)
	{
		if (P(i, 1) > 0)
		{
			num_non_zero = num_non_zero + 1;
		}
		else
		{
			break;
		}
	}
	imat result = zeros<imat>(num_non_zero, 2);
	for (i = 0; i < num_non_zero; i++)
	{
		result(i, 0) = P(i, 0);
		result(i, 1) = P(i, 1);
	}
	return result;
}