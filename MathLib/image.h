#pragma once
#include "common.h"

// 区域生长
imat RegionGrowing(imat Img, int x0, int y0, int reg_maxdist)
{
	/*
	Img：二维灰度值数组，0~255之间；
	x0：种子点横坐标序号；
	y0：种子点纵坐标序号；
	reg_maxdist：阈值；
	返回和 Img 等大的数组，感兴趣区域用 1 表示；
	*/
	// 维度
	int row = Img.n_rows;
	int col = Img.n_cols;
	imat Jm = zeros<imat>(row, col);  // 输出
	double reg_mean = Img(x0, y0);   // 种子点的灰度值
	Jm(x0, y0) = 1;   // 种子点设置为1
	double reg_sum = reg_mean;  // 符合生长条件的灰度值总和
	int reg_num = 1;  // 符合生长条件的点的个数
	int count = 1;    // 每次判断周围八个点中符合条件的数目
					  // 记录已选择点的序号
	imat reg_choose = zeros<imat>(row * col, 2);
	reg_choose(reg_num - 1, 0) = x0;
	reg_choose(reg_num - 1, 1) = y0;
	int num = 1;  // 第一个点
	double s_temp;   // 周围八个点中符合条件的点的灰度值总和
	int i, j, k, u, v;
	while (count > 0)
	{
		s_temp = 0;
		count = 0;
		// 对新增的每个点遍历，避免重复
		for (k = 0; k < num; k++)
		{
			i = reg_choose(reg_num - num + k, 0);
			j = reg_choose(reg_num - num + k, 1);
			// 已确定且不是边界上的点
			if ((Jm(i, j) == 1) && (i > 0) && (i < row - 1) && (j > 0) && (j < col - 1))
			{
				// 八邻域
				for (u = -1; u <= 1; u++)
				{
					for (v = -1; v <= 1; v++)
					{
						// 未处理且满足生长条件的点
						if ((Jm(i + u, j + v) == 0) && (abs(Img(i + u, j + v) - reg_mean) <= reg_maxdist))
						{
							Jm(i + u, j + v) = 1;   // 对应点设置为1
							count = count + 1;
							reg_choose(reg_num + count - 1, 0) = i + u;
							reg_choose(reg_num + count - 1, 1) = j + v;
							// 灰度值存入 s_temp 中
							s_temp = s_temp + Img(i + u, j + v);
						}
					}
				}
			}
		}
		num = count;   // 新增的点
		reg_num = reg_num + count;  // 区域内总点数
		reg_sum = reg_sum + s_temp;  // 区域内总灰度值
		//reg_mean = reg_sum / reg_num;  // 区域灰度平均值
	}
	// 在外面再加一圈 1
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

// 区域边界
imat RegionBoundary(imat Jm)
{
	/*
	Jm：输入，感兴趣区域值为 1，其余为 0；
	返回边界点坐标，第一列为横向，第二列为纵向；
	*/
	int xlen = Jm.n_rows;
	int ylen = Jm.n_cols;
	imat Je = zeros<imat>(xlen + 2, ylen + 2);  // 防止出现边界问题
	int i, j;
	for (i = 1; i <= xlen; i++)
	{
		for (j = 1; j <= ylen; j++)
		{
			Je(i, j) = Jm(i - 1, j - 1);
		}
	}
	imat F = zeros<imat>(xlen + 2, ylen + 2);           // 标记 Je 中已经选择的边界点
	imat P = zeros<imat>((xlen + 2) * (ylen + 2), 2);   // 边界序号，临时的
														// 确定第一个边界点 A，按行选择，从左至右
	int flag = 0;
	int m, n;
	for (i = 1; i < xlen + 2; i++)
	{
		for (j = 1; j < ylen + 2; j++)
		{
			if (Je(i, j) == 1)
			{
				// 初始点
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
	// 初始化
	i = m;
	j = n;
	int dir = 7;
	flag = 1;
	int count = 0;
	// 从初始点开始遍历，直到满足退出条件
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
	// 输出的时候还原
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