#pragma once
#include "common.h"

// 确定滤波器，来自 matlab 函数 wfilters('db1')
const double db1_Lo_D[2] = { 0.7071, 0.7071 };
const double db1_Hi_D[2] = { -0.7071, 0.7071 };
const double db1_Lo_R[2] = { 0.7071, 0.7071 };
const double db1_Hi_R[2] = { 0.7071, -0.7071 };
const double db2_Lo_D[4] = { -0.129409522550921, 0.224143868041857, 0.836516303737469, 0.482962913144690 };
const double db2_Hi_D[4] = { -0.482962913144690, 0.836516303737469, -0.224143868041857, -0.129409522550921 };
const double db2_Lo_R[4] = { 0.482962913144690, 0.836516303737469, 0.224143868041857, -0.129409522550921 };
const double db2_Hi_R[4] = { -0.129409522550921, -0.224143868041857, 0.836516303737469, -0.482962913144690 };
const double db3_Lo_D[6] = { 0.0352262918821007, -0.0854412738822415, -0.135011020010391, 0.459877502119331, 0.806891509313339, 0.332670552950957 };
const double db3_Hi_D[6] = { -0.332670552950957, 0.806891509313339, -0.459877502119331, -0.135011020010391, 0.0854412738822415, 0.0352262918821007 };
const double db3_Lo_R[6] = { 0.332670552950957, 0.806891509313339, 0.459877502119331, -0.135011020010391, -0.0854412738822415, 0.0352262918821007 };
const double db3_Hi_R[6] = { 0.0352262918821007, 0.0854412738822415, -0.135011020010391, -0.459877502119331, 0.806891509313339, -0.332670552950957 };
const double db4_Lo_D[8] = { -0.0105974017849973, 0.0328830116669830, 0.0308413818359870, -0.187034811718881, -0.0279837694169839, 0.630880767929590, 0.714846570552542, 0.230377813308855 };
const double db4_Hi_D[8] = { -0.230377813308855, 0.714846570552542, -0.630880767929590, -0.0279837694169839, 0.187034811718881, 0.0308413818359870, -0.0328830116669829, -0.0105974017849973 };
const double db4_Lo_R[8] = { 0.230377813308855, 0.714846570552542, 0.630880767929590, -0.0279837694169839, -0.187034811718881, 0.0308413818359870, 0.0328830116669829, -0.0105974017849973 };
const double db4_Hi_R[8] = { -0.0105974017849973, -0.0328830116669829, 0.0308413818359870, 0.187034811718881, -0.0279837694169839, -0.630880767929590, 0.714846570552542, -0.230377813308855 };

const double db5_Lo_D[10] = { 0.00333572528500155, -0.0125807519990155, -0.00624149021301171, 0.0775714938400652, -0.0322448695850295, -0.242294887066190, 0.138428145901103, 0.724308528438574, 0.603829269797473, 0.160102397974125 };
const double db5_Hi_D[10] = { -0.160102397974125, 0.603829269797473, -0.724308528438574, 0.138428145901103, 0.242294887066190, -0.0322448695850295, -0.0775714938400652, -0.00624149021301171, 0.0125807519990155, 0.00333572528500155 };
const double db5_Lo_R[10] = { 0.160102397974125, 0.603829269797473, 0.724308528438574, 0.138428145901103, -0.242294887066190, -0.0322448695850295, 0.0775714938400652, -0.00624149021301171, -0.0125807519990155, 0.00333572528500155 };
const double db5_Hi_R[10] = { 0.00333572528500155, 0.0125807519990155, -0.00624149021301171, -0.0775714938400652, -0.0322448695850295, 0.242294887066190, 0.138428145901103, -0.724308528438574, 0.603829269797473, -0.160102397974125 };

const double db6_Lo_D[12] = { -0.00107730108499558, 0.00477725751101065, 0.000553842200993802, -0.0315820393180312, 0.0275228655300163, 0.0975016055870794, -0.129766867567096, -0.226264693965169, 0.315250351709243, 0.751133908021578, 0.494623890398385, 0.111540743350080 };
const double db6_Hi_D[12] = { -0.111540743350080, 0.494623890398385, -0.751133908021578, 0.315250351709243, 0.226264693965169, -0.129766867567096, -0.0975016055870794, 0.0275228655300163, 0.0315820393180312, 0.000553842200993802, -0.00477725751101065, -0.00107730108499558 };
const double db6_Lo_R[12] = { 0.111540743350080, 0.494623890398385, 0.751133908021578, 0.315250351709243, -0.226264693965169, -0.129766867567096, 0.0975016055870794, 0.0275228655300163, -0.0315820393180312, 0.000553842200993802, 0.00477725751101065, -0.00107730108499558 };
const double db6_Hi_R[12] = { -0.00107730108499558, -0.00477725751101065, 0.000553842200993802, 0.0315820393180312, 0.0275228655300163, -0.0975016055870794, -0.129766867567096, 0.226264693965169, 0.315250351709243, -0.751133908021578, 0.494623890398385, -0.111540743350080 };

const double db7_Lo_D[14] = { 0.000353713800001040, -0.00180164070399983, 0.000429577973004703, 0.0125509985560138, -0.0165745416310156, -0.0380299369350346, 0.0806126091510659, 0.0713092192670500, -0.224036184994166, -0.143906003929106, 0.469782287405359, 0.729132090846555, 0.396539319482306, 0.0778520540850624 };
const double db7_Hi_D[14] = { -0.0778520540850624, 0.396539319482306, -0.729132090846555, 0.469782287405359, 0.143906003929106, -0.224036184994166, -0.0713092192670500, 0.0806126091510659, 0.0380299369350346, -0.0165745416310156, -0.0125509985560138, 0.000429577973004703, 0.00180164070399983, 0.000353713800001040 };
const double db7_Lo_R[14] = { 0.0778520540850624, 0.396539319482306, 0.729132090846555, 0.469782287405359, -0.143906003929106, -0.224036184994166, 0.0713092192670500, 0.0806126091510659, -0.0380299369350346, -0.0165745416310156, 0.0125509985560138, 0.000429577973004703, -0.00180164070399983, 0.000353713800001040 };
const double db7_Hi_R[14] = { 0.000353713800001040, 0.00180164070399983, 0.000429577973004703, -0.0125509985560138, -0.0165745416310156, 0.0380299369350346, 0.0806126091510659, -0.0713092192670500, -0.224036184994166, 0.143906003929106, 0.469782287405359, -0.729132090846555, 0.396539319482306, -0.0778520540850624 };

const double db8_Lo_D[16] = { -0.000117476784002282, 0.000675449405998557, -0.000391740372995977, -0.00487035299301066, 0.00874609404701566, 0.0139810279170155, -0.0440882539310647, -0.0173693010020221, 0.128747426620186, 0.000472484573997973, -0.284015542962428, -0.0158291052560239, 0.585354683654869, 0.675630736298013, 0.312871590914466, 0.0544158422430816 };
const double db8_Hi_D[16] = { -0.0544158422430816, 0.312871590914466, -0.675630736298013, 0.585354683654869, 0.0158291052560239, -0.284015542962428, -0.000472484573997973, 0.128747426620186, 0.0173693010020221, -0.0440882539310647, -0.0139810279170155, 0.00874609404701566, 0.00487035299301066, -0.000391740372995977, -0.000675449405998557, -0.000117476784002282 };
const double db8_Lo_R[16] = { 0.0544158422430816, 0.312871590914466, 0.675630736298013, 0.585354683654869, -0.0158291052560239, -0.284015542962428, 0.000472484573997973, 0.128747426620186, -0.0173693010020221, -0.0440882539310647, 0.0139810279170155, 0.00874609404701566, -0.00487035299301066, -0.000391740372995977, 0.000675449405998557, -0.000117476784002282 };
const double db8_Hi_R[16] = { -0.000117476784002282, -0.000675449405998557, -0.000391740372995977, 0.00487035299301066, 0.00874609404701566, -0.0139810279170155, -0.0440882539310647, 0.0173693010020221, 0.128747426620186, -0.000472484573997973, -0.284015542962428, 0.0158291052560239, 0.585354683654869, -0.675630736298013, 0.312871590914466, -0.0544158422430816 };

const double db9_Lo_D[18] = { 3.93473199950261e-05, -0.000251963188998179, 0.000230385763995413, 0.00184764688296113, -0.00428150368190472, -0.00472320475789483, 0.0223616621235152, 0.000250947114991938, -0.0676328290595240, 0.0307256814783229, 0.148540749334760, -0.0968407832208790, -0.293273783272587, 0.133197385822089, 0.657288078036639, 0.604823123676779, 0.243834674637667, 0.0380779473631673 };
const double db9_Hi_D[18] = { -0.0380779473631673, 0.243834674637667, -0.604823123676779, 0.657288078036639, -0.133197385822089, -0.293273783272587, 0.0968407832208790, 0.148540749334760, -0.0307256814783229, -0.0676328290595240, -0.000250947114991938, 0.0223616621235152, 0.00472320475789483, -0.00428150368190472, -0.00184764688296113, 0.000230385763995413, 0.000251963188998179, 3.93473199950261e-05 };
const double db9_Lo_R[18] = { 0.0380779473631673, 0.243834674637667, 0.604823123676779, 0.657288078036639, 0.133197385822089, -0.293273783272587, -0.0968407832208790, 0.148540749334760, 0.0307256814783229, -0.0676328290595240, 0.000250947114991938, 0.0223616621235152, -0.00472320475789483, -0.00428150368190472, 0.00184764688296113, 0.000230385763995413, -0.000251963188998179, 3.93473199950261e-05 };
const double db9_Hi_R[18] = { 3.93473199950261e-05, 0.000251963188998179, 0.000230385763995413, -0.00184764688296113, -0.00428150368190472, 0.00472320475789483, 0.0223616621235152, -0.000250947114991938, -0.0676328290595240, -0.0307256814783229, 0.148540749334760, 0.0968407832208790, -0.293273783272587, -0.133197385822089, 0.657288078036639, -0.604823123676779, 0.243834674637667, -0.0380779473631673 };

// 一些子函数
double Array_Max(mat Input);
mat Array_Max(mat Input, mat w);
mat Array_Pad(mat Input, int window_width);
mat Img_Interp(mat Input, int dx, int dy);
mat Img_Shift(mat Input, int dx, int dy);
vec WaveDec(vec srcData, imat msgLen, int allSize, int Scale, vec Lo_D, vec Hi_D, int filterLen);
vec Dwt(vec srcData, int srcLen, vec Lo_D, vec Hi_D, int filterLen);
double GetThr(vec detCoef, imat msgLen);
vec Wthresh(vec dstCoef, double thr, int allSize, int gap);
vec Wthresh(vec dstCoef, double thr, int allSize, int gap, bool isHard);
vec WaveRec(vec srcCoef, imat msgLen, int Scale, vec Lo_R, vec Hi_R, int filterLen);
vec Idwt(vec srcCoef, int dstLen, vec Lo_R, vec Hi_R, int filterLen);
vec WaveDec2(vec srcData, imat msgHeight, imat msgWidth, int allSize, int Scale, vec Lo_D, vec Hi_D, int filterLen);
vec Dwt2(vec srcImage, int height, int width, imat msgHeight, imat msgWidth, vec Lo_D, vec Hi_D, int filterLen);
vec AdjustData(vec dstCoef, int height, int width);
vec WaveRec2(vec srcCoef, imat msgHeight, imat msgWidth, int Scale, vec Lo_R, vec Hi_R, int filterLen);
vec Idwt2(vec srcCoef, int dstHeight, int dstWidth, imat msgHeight, imat msgWidth, vec Lo_R, vec Hi_R, int filterLen);
vec IAdjustData(vec dstCoef, int height, int width);
double GetThr(vec detCoef, imat msgHeight, imat msgWidth);
void Dwt(vec srcData, int srcLen, vec Lo_D, vec Hi_D, int filterLen, vec& cA, vec& cD);
vec Idwt(vec cA, vec cD, int dstLen, vec Lo_R, vec Hi_R, int filterLen);
vec ShiftLeft(vec x);
vec ShiftRight(vec x);

double Array_Max(mat Input)
{
	int heigth = Input.n_rows;
	int width = Input.n_cols;
	double result = Input(0, 0);
	for (int i = 0; i < heigth; i++)
	{
		for (int j = 0; j < width; j++)
		{
			if (result < Input(i, j))
			{
				result = Input(i, j);
			}
		}
	}
	return result;
}

mat Array_Max(mat Input, mat w)
{
	int m_row = Input.n_rows;
	int n_col = Input.n_cols;
	mat result = Input;
	for (int i = 0; i < m_row; i++)
	{
		for (int j = 0; j < n_col; j++)
		{
			if (result(i, j) < w(i, j))
			{
				result(i, j) = w(i, j);
			}
		}
	}
	return result;
}

mat Array_Pad(mat Input, int window_width)
{
	int i, j;
	int m_row = Input.n_rows;
	int n_col = Input.n_cols;
	mat result = zeros(m_row + 2 * window_width, n_col + 2 * window_width);
	for (i = 0; i < m_row; i++)
	{
		for (j = 0; j < n_col; j++)
		{
			result(window_width + i, window_width + j) = Input(i, j);
		}
	}
	for (i = 0; i < m_row; i++)
	{
		for (j = 0; j < window_width; j++)
		{
			result(window_width + i, window_width - j - 1) = Input(i, j);
			result(window_width + i, n_col + window_width + j) = Input(i, n_col - j - 1);
		}
	}
	for (j = 0; j < n_col + 2 * window_width; j++)
	{
		for (i = 0; i < window_width; i++)
		{
			result(i, j) = result(2 * window_width - i - 1, j);
			result(m_row + window_width + i, j) = result(m_row + window_width - i - 1, j);
		}
	}
	return result;
}

mat Img_Interp(mat Input, int dx, int dy)
{
	mat t = Img_Shift(Input, dx, dy);
	mat mdiff = (Input - t) % (Input - t);
	mat result = cumsum(mdiff, 1);
	result = cumsum(result);
	return result;
}

mat Img_Shift(mat Input, int dx, int dy)
{
	int m_row = Input.n_rows;
	int n_col = Input.n_cols;
	int i, j;
	mat result = zeros(m_row, n_col);
	int flag1 = 0;
	int flag2 = 0;
	if (dx > 0)
	{
		flag1 = 1;
	}
	if (dy > 0)
	{
		flag2 = 1;
	}
	switch (2 * flag1 + flag2)
	{
	case 0:
	{
		for (i = -dx; i < m_row; i++)
		{
			for (j = -dy; j < n_col; j++)
			{
				result(i, j) = Input(dx + i, dy + j);
			}
		}
		break;
	}
	case 1:
	{
		for (i = -dx; i < m_row; i++)
		{
			for (j = 0; j < n_col - dy; j++)
			{
				result(i, j) = Input(dx + i, dy + j);
			}
		}
		break;
	}
	case 2:
	{
		for (i = 0; i < m_row - dx; i++)
		{
			for (j = -dy; j < n_col; j++)
			{
				result(i, j) = Input(dx + i, dy + j);
			}
		}
		break;
	}
	case 3:
	{
		for (i = 0; i < m_row - dx; i++)
		{
			for (j = 0; j < n_col - dy; j++)
			{
				result(i, j) = Input(dx + i, dy + j);
			}
		}
		break;
	}
	default:
		break;
	}
	return result;
}

vec ShiftLeft(vec x)
{
	int len = x.n_elem;
	vec y = zeros(len, 1);
	y.rows(0, len - 2) = x.rows(1, len - 1);
	y(len - 1) = x(0);
	return y;
}

vec ShiftRight(vec x)
{
	int len = x.n_elem;
	vec y = zeros(len, 1);
	y(0) = x(len - 1);
	y.rows(1, len - 1) = x.rows(0, len - 2);
	return y;
}

vec WaveDec2(vec srcData, imat msgHeight, imat msgWidth, int allSize, int Scale, vec Lo_D, vec Hi_D, int filterLen)
{
	int height = msgHeight(0);
	int width = msgWidth(0);
	vec dstCoef = zeros(allSize, 1);
	vec tempImage = srcData.rows(0, height * width - 1);
	vec tempDst = zeros(4 * msgHeight(1) * msgWidth(1), 1);
	int gap = allSize - 4 * msgHeight(1) * msgWidth(1);
	for (int i = 1; i <= Scale; i++)
	{
		tempDst = Dwt2(tempImage, height, width, msgHeight, msgWidth, Lo_D, Hi_D, filterLen);
		height = msgHeight(i);
		width = msgWidth(i);
		tempImage = tempDst.rows(0, height * width - 1);
		dstCoef.rows(gap, gap + 4 * height * width - 1) = tempDst.rows(0, 4 * height * width - 1);
		gap -= 4 * msgWidth(i + 1) * msgHeight(i + 1) - height * width;
	}
	return dstCoef;
}

vec Dwt2(vec srcImage, int height, int width, imat msgHeight, imat msgWidth, vec Lo_D, vec Hi_D, int filterLen)
{
	int exHeight = (height + filterLen - 1) / 2 * 2;
	int exWidth = (width + filterLen - 1) / 2 * 2;
	vec dstCoef = zeros(4 * msgHeight(1) * msgWidth(1), 1);

	vec tempImage = zeros(exHeight * exWidth, 1);
	vec tempARow = zeros(width, 1);
	vec tempExRow = zeros(exWidth, 1);
	for (int i = 0; i < height; i++)
	{
		tempARow.rows(0, width - 1) = srcImage.rows(i * width, i * width + width - 1);
		tempExRow = Dwt(tempARow, width, Lo_D, Hi_D, filterLen);
		tempImage.rows(i * exWidth, i * exWidth + exWidth - 1) = tempExRow.rows(0, exWidth - 1);
	}

	vec tempACol = zeros(height, 1);
	vec tempExCol = zeros(exHeight, 1);
	for (int i = 0; i < exWidth; i++)
	{
		for (int j = 0; j < height; j++)
		{
			tempACol(j) = tempImage(j * exWidth + i);
		}
		tempExCol = Dwt(tempACol, height, Lo_D, Hi_D, filterLen);
		for (int j = 0; j < exHeight; j++)
		{
			dstCoef(j * exWidth + i) = tempExCol(j);
		}
	}
	dstCoef = AdjustData(dstCoef, exHeight, exWidth);
	return dstCoef;
}

vec AdjustData(vec dstCoef, int height, int width)
{
	vec tmpDst = dstCoef.rows(0, height / 2 * width - 1);
	int pos1 = 0;
	int pos2 = height / 2 * width / 2;
	for (int i = 0; i < height / 2; i++)
	{
		for (int j = 0; j < width; j++)
		{
			if (j < width / 2)
			{
				dstCoef(pos1++) = tmpDst(i * width + j);
			}
			else
			{
				dstCoef(pos2++) = tmpDst(i * width + j);
			}
		}
	}
	return dstCoef;
}

vec WaveRec2(vec srcCoef, imat msgHeight, imat msgWidth, int Scale, vec Lo_R, vec Hi_R, int filterLen)
{
	int height = msgHeight[0];
	int width = msgWidth[0];
	vec tempImage = zeros(4 * msgHeight(1) * msgWidth(1), 1);
	int minCoefSize = 4 * msgHeight(Scale) * msgWidth(Scale);
	tempImage.rows(0, minCoefSize - 1) = srcCoef.rows(0, minCoefSize - 1);
	int gap = minCoefSize;
	vec dstData = zeros(msgHeight(0) * msgWidth(0), 1);
	for (int i = Scale; i >= 1; i--)
	{
		int nextHeight = msgHeight(i - 1);
		int nextWidth = msgWidth(i - 1);
		dstData = Idwt2(tempImage, nextHeight, nextWidth, msgHeight, msgWidth, Lo_R, Hi_R, filterLen);
		if (i > 1)
		{
			tempImage.rows(0, nextHeight * nextWidth - 1) = dstData.rows(0, nextHeight * nextWidth - 1);
			tempImage.rows(nextHeight * nextWidth, 4 * nextHeight * nextWidth - 1) = srcCoef.rows(gap, gap + 3 * nextHeight * nextWidth - 1);
			gap += 3 * nextHeight * nextWidth;
		}
	}
	return dstData;
}

vec Idwt2(vec srcCoef, int dstHeight, int dstWidth, imat msgHeight, imat msgWidth, vec Lo_R, vec Hi_R, int filterLen)
{
	int srcHeight = (dstHeight + filterLen - 1) / 2 * 2;
	int srcWidth = (dstWidth + filterLen - 1) / 2 * 2;
	srcCoef = IAdjustData(srcCoef, srcHeight, srcWidth);
	vec tempARow = zeros(srcHeight, 1);
	vec tempDstRow = zeros(dstHeight, 1);
	vec tempImage = zeros(srcWidth * dstHeight, 1);
	for (int i = 0; i < srcWidth; i++)
	{
		for (int j = 0; j < srcHeight; j++)
		{
			tempARow(j) = srcCoef(j * srcWidth + i);
		}
		tempDstRow = Idwt(tempARow, dstHeight, Lo_R, Hi_R, filterLen);
		for (int j = 0; j < dstHeight; j++)
		{
			tempImage(j * srcWidth + i) = tempDstRow(j);
		}
	}

	vec tempACol = zeros(srcWidth, 1);
	vec tempDstCol = zeros(dstWidth, 1);
	vec dstImage = zeros(msgHeight(0) * msgWidth(0), 1);
	for (int i = 0; i < dstHeight; i++)
	{
		tempACol = tempImage.rows(i * srcWidth, i * srcWidth + srcWidth - 1);
		tempDstCol = Idwt(tempACol, dstWidth, Lo_R, Hi_R, filterLen);
		dstImage.rows(i * dstWidth, i * dstWidth + dstWidth - 1) = tempDstCol;
	}
	return dstImage;
}

vec IAdjustData(vec dstCoef, int height, int width)
{
	vec tmpDst = dstCoef.rows(0, height / 2 * width - 1);
	int pos1 = 0;
	int pos2 = height / 2 * width / 2;
	for (int i = 0; i < height / 2; i++)
	{
		for (int j = 0; j < width; j++)
		{
			if (j < width / 2)
			{
				dstCoef(i * width + j) = tmpDst(pos1++);
			}
			else
			{
				dstCoef(i * width + j) = tmpDst(pos2++);
			}
		}
	}
	return dstCoef;
}

vec WaveDec(vec srcData, imat msgLen, int allSize, int Scale, vec Lo_D, vec Hi_D, int filterLen)
{
	vec tmpSrc = srcData;
	int gap = 2 * msgLen(1);
	vec dstCoef = zeros(allSize, 1);
	for (int i = 1; i < Scale + 1; i++)
	{
		int curSigLen = msgLen(i - 1);
		vec tmpDst = Dwt(tmpSrc, curSigLen, Lo_D, Hi_D, filterLen);
		dstCoef.rows(allSize - gap, allSize - gap - 1 + 2 * msgLen(i)) = tmpDst.rows(0, 2 * msgLen(i) - 1);
		tmpSrc.rows(0, msgLen(i) - 1) = tmpDst.rows(0, msgLen(i) - 1);
		gap = gap - msgLen(i);
		gap = gap + 2 * msgLen(i + 1);
	}
	return dstCoef;
}

vec Dwt(vec srcData, int srcLen, vec Lo_D, vec Hi_D, int filterLen)
{
	int decLen = (srcLen + filterLen - 1) / 2;
	vec dstCoef = zeros(2 * decLen, 1);
	double tmp;
	for (int i = 0; i < decLen; i++)
	{
		for (int j = 0; j < filterLen; j++)
		{
			int k = 2 * i - j + 1;
			if (k < 0 && k >= -filterLen + 1)
			{
				tmp = srcData(-k - 1);
			}
			else if (k >= 0 && k <= srcLen - 1)
			{
				tmp = srcData(k);
			}
			else if (k > srcLen - 1 && k <= srcLen + filterLen - 2)
			{
				tmp = srcData(2 * srcLen - k - 1);
			}
			else
			{
				tmp = 0.0;
			}
			dstCoef(i) = dstCoef(i) + Lo_D(j) * tmp;
			dstCoef(i + decLen) = dstCoef(i + decLen) + Hi_D(j) * tmp;
		}
	}
	return dstCoef;
}

void Dwt(vec srcData, int srcLen, vec Lo_D, vec Hi_D, int filterLen, vec& cA, vec& cD)
{
	int decLen = (srcLen + filterLen - 1) / 2;
	vec dstCoef = Dwt(srcData, srcLen, Lo_D, Hi_D, filterLen);
	cA = dstCoef.rows(0, decLen - 1);
	cD = dstCoef.rows(decLen, 2 * decLen - 1);
}

double GetThr(vec detCoef, imat msgLen)
{
	detCoef = arma::abs(detCoef);
	double sigma = median(detCoef) / 0.6745;
	double thr = sigma * sqrt(2 * log(msgLen(0)));
	return thr;
}

double GetThr(vec detCoef, imat msgHeight, imat msgWidth)
{
	detCoef = arma::abs(detCoef);
	double sigma = median(detCoef) / 0.6745;
	double thr = sigma * sqrt(2 * log(msgHeight(0) * msgWidth(0)));
	return thr;
}

vec Wthresh(vec dstCoef, double thr, int allSize, int gap)
{
	for (int i = gap; i < allSize; i++)
	{
		if (abs(dstCoef(i)) < thr)
		{
			dstCoef(i) = 0;
		}
		else
		{
			if (dstCoef(i) < 0)
			{
				dstCoef(i) = thr - abs(dstCoef(i));
			}
			else
			{
				dstCoef(i) = abs(dstCoef(i)) - thr;
			}
		}
	}
	return dstCoef;
}

vec Wthresh(vec dstCoef, double thr, int allSize, int gap, bool isHard)
{
	if (isHard)
	{
		for (int i = gap; i < allSize; i++)
		{
			if (abs(dstCoef(i)) < thr)
			{
				dstCoef(i) = 0;
			}
		}
	}
	else
	{
		dstCoef = Wthresh(dstCoef, thr, allSize, gap);
	}
	return dstCoef;
}

vec WaveRec(vec srcCoef, imat msgLen, int Scale, vec Lo_R, vec Hi_R, int filterLen)
{
	vec tmpSrcCoef = zeros(2 * msgLen(1), 1);
	tmpSrcCoef.rows(0, 2 * msgLen(Scale) - 1) = srcCoef.rows(0, 2 * msgLen(Scale) - 1);
	int gap = 2 * msgLen(Scale);
	vec dstData = zeros(msgLen(0), 1);
	for (int i = Scale; i > 0; i--)
	{
		int curDstLen = msgLen(i - 1);
		dstData = Idwt(tmpSrcCoef, curDstLen, Lo_R, Hi_R, filterLen);
		if (i != 1)
		{
			tmpSrcCoef.rows(0, curDstLen - 1) = dstData.rows(0, curDstLen - 1);
			tmpSrcCoef.rows(curDstLen, 2 * curDstLen - 1) = srcCoef.rows(gap, gap + curDstLen - 1);
			gap = gap + msgLen(i - 1);
		}
	}
	return dstData;
}

vec Idwt(vec srcCoef, int dstLen, vec Lo_R, vec Hi_R, int filterLen)
{
	int recLen = (dstLen + filterLen - 1) / 2;
	vec recData = zeros(dstLen, 1);
	for (int i = 0; i < dstLen; i++)
	{
		for (int j = 0; j < recLen; j++)
		{
			int k = i - 2 * j + filterLen - 2;
			if (k >= 0 && k < filterLen)
			{
				recData(i) = recData(i) + Lo_R(k) * srcCoef(j) + Hi_R(k) * srcCoef(j + recLen);
			}
		}
	}
	return recData;
}

vec Idwt(vec cA, vec cD, int dstLen, vec Lo_R, vec Hi_R, int filterLen)
{
	int recLen = (dstLen + filterLen - 1) / 2;
	vec srcCoef = zeros(2 * recLen, 1);
	srcCoef.rows(0, recLen - 1) = cA;
	srcCoef.rows(recLen, 2 * recLen - 1) = cD;
	vec recData = Idwt(srcCoef, dstLen, Lo_R, Hi_R, filterLen);
	return recData;
}

void Wdbn2(double** Input, double** Output, int height, int width, int scale, int dbn)
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
	int srcHeight = height;
	int srcWidth = width;

	imat msgHeight = zeros<imat>(Scale + 2, 1);
	imat msgWidth = zeros<imat>(Scale + 2, 1);
	msgHeight(0) = height;
	msgWidth(0) = width;
	for (int i = 1; i <= Scale; i++)
	{
		int exHeight = (srcHeight + filterLen - 1) / 2;
		srcHeight = exHeight;
		msgHeight(i) = srcHeight;
		int exWidth = (srcWidth + filterLen - 1) / 2;
		srcWidth = exWidth;
		msgWidth(i) = srcWidth;
	}
	msgHeight(Scale + 1) = srcHeight;
	msgWidth(Scale + 1) = srcWidth;

	int allSize = 0;
	int curPartSize = 0;
	int prePartSize = 0;
	for (int i = 1; i <= Scale; i++)
	{
		curPartSize = msgHeight(i) * msgWidth(i);
		allSize += curPartSize * 4 - prePartSize;
		prePartSize = curPartSize;
	}

	mat noiseImage = zeros(height * width, 1);
	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			noiseImage(i * width + j) = Input[i][j];
		}
	}

	vec imagCoef = WaveDec2(noiseImage, msgHeight, msgWidth, allSize, Scale, mLo_D, mHi_D, filterLen);
	int hlSize = msgHeight(1) * msgWidth(1);
	vec hlCoef = imagCoef.rows(allSize - hlSize * 3, allSize - hlSize * 3 + hlSize - 1);
	double thr = GetThr(hlCoef, msgHeight, msgWidth);
	imagCoef = Wthresh(imagCoef, thr, allSize, msgHeight(Scale + 1) * msgWidth(Scale + 1));
	vec mDstImage = WaveRec2(imagCoef, msgHeight, msgWidth, Scale, mLo_R, mHi_R, filterLen);

	for (int i = 0; i < height; i++)
	{
		for (int j = 0; j < width; j++)
		{
			Output[i][j] = round(mDstImage(i * width + j));
			if (Output[i][j] < 0)
			{
				Output[i][j] = 0;
			}
		}
	}
}

void Nlm2(double** Input, double** Output, int height, int width, int window_width, int patch_width, double sigma)
{
	int i, j, dx, dy;
	mat de_input = zeros(height, width);  // 初始化去噪图像矩阵
	mat de_weigth = zeros(height, width);    // 初始化权值矩阵
	mat cum_weigth = zeros(height, width);   // 初始化叠加权值
	// 归一化
	mat mInput = zeros(height, width);
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			mInput(i, j) = Input[i][j];
		}
	}
	double max_gray = Array_Max(mInput);  // max_gray = 255;
	mInput = mInput / max_gray;
	// 边界扩充
	mat pw = Array_Pad(mInput, window_width);
	mat pp = Array_Pad(mInput, patch_width);
	// 主循环
	int m_row = pw.n_rows;
	int n_col = pw.n_cols;
	mat Sd = zeros(m_row, n_col);
	mat SDist = zeros(height, width);
	mat m_w = zeros(height, width);
	mat m_v = zeros(height, width);
	for (dx = -patch_width; dx <= patch_width; dx++)
	{
		for (dy = -patch_width; dy <= patch_width; dy++)
		{
			if ((dx != 0) || (dy != 0))
			{
				Sd = Img_Interp(pw, dx, dy);
				SDist = Sd(span(window_width, m_row - window_width - 1), span(window_width, n_col - window_width - 1)) +
					Sd(span(0, m_row - 2 * window_width - 1), span(0, n_col - 2 * window_width - 1)) -
					Sd(span(0, m_row - 2 * window_width - 1), span(window_width, n_col - window_width - 1)) -
					Sd(span(window_width, m_row - window_width - 1), span(0, n_col - 2 * window_width - 1));
				mat Sd1 = Sd(span(window_width, m_row - window_width - 1), span(window_width, n_col - window_width - 1));
				mat Sd2 = Sd(span(0, m_row - 2 * window_width - 1), span(0, n_col - 2 * window_width - 1));
				mat Sd3 = Sd(span(0, m_row - 2 * window_width - 1), span(window_width, n_col - window_width - 1));
				mat Sd4 = Sd(span(window_width, m_row - window_width - 1), span(0, n_col - 2 * window_width - 1));
				m_w = exp(-SDist / (2 * sigma * sigma));
				m_v = pp(span(patch_width + dx, patch_width + dx + height - 1), span(patch_width + dy, patch_width + dy + width - 1));
				de_input = de_input + m_w % m_v;
				de_weigth = Array_Max(de_weigth, m_w);
				cum_weigth = cum_weigth + m_w;
			}
		}
	}
	de_input = de_input + de_weigth % mInput;
	de_input = de_input / (cum_weigth + de_weigth);
	de_input = de_input * max_gray;
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			Output[i][j] = round(de_input(i, j));
		}
	}
}

void Beltrami(double** Input, double** Output, int height, int width, int num_iter, double delta_t)
{
	int i, j;
	// 归一化
	mat mInput = zeros(height, width);
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			mInput(i, j) = Input[i][j];
		}
	}
	double max_gray = Array_Max(mInput);  // max_gray = 255;
	mInput = mInput / max_gray;
	// 梯度矩阵
	mat hx = zeros(3, 3);
	mat hy = zeros(3, 3);
	mat hxx = zeros(3, 3);
	mat hyy = zeros(3, 3);
	mat hxy = zeros(3, 3);
	hx(1, 0) = -0.5;
	hx(1, 2) = 0.5;
	hy(0, 1) = -0.5;
	hy(2, 1) = 0.5;
	hxx(1, 0) = 1;
	hxx(1, 1) = -2;
	hxx(1, 2) = 1;
	hyy(0, 1) = 1;
	hyy(1, 1) = -2;
	hyy(2, 1) = 1;
	hxy(0, 0) = 1;
	hxy(0, 2) = -1;
	hxy(2, 0) = -1;
	hxy(2, 2) = 1;
	// copy
	mat lk = mInput;
	mat lxx = zeros(height, width);
	mat lyy = zeros(height, width);
	mat lx = zeros(height, width);
	mat ly = zeros(height, width);
	mat lxy = zeros(height, width);
	mat lkx = zeros(height, width);
	for (i = 0; i < num_iter; i++)
	{
		lxx = conv2(lk, hxx, "same");
		lyy = conv2(lk, hyy, "same");
		lx = conv2(lk, hx, "same");
		ly = conv2(lk, hy, "same");
		lxy = conv2(lk, hxy, "same");
		// 对梯度下降方程积分
		lkx = lk + delta_t * ((lxx % (1 + ly % ly) + lyy % (1 + lx % lx) - 2 * lx % ly % lxy) / ((1 + lx % lx + ly % ly) % (1 + lx % lx + ly % ly)));
		lk = lkx;
	}
	lkx = lkx * max_gray;
	for (i = 0; i < height; i++)
	{
		for (j = 0; j < width; j++)
		{
			Output[i][j] = round(lkx(i, j));
		}
	}
}