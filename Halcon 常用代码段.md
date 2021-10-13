Halcon 常用代码段
================================================================================

文件不更新了，文章持续更新：https://zhuanlan.zhihu.com/p/394122445

[TOC]

# 1. 使用算子处理图像基本流程

```matlab
dev_update_off ()
dev_close_window ()
read_image (Image, 'test.jpg')
get_image_size (Image, Width, Height)
dev_open_window (0, 0, Width, Height, 'black', WindowHandle)
dev_set_draw ('margin')
dev_set_line_width (1)
emphasize (Image, ImageEmphasize, 5, 5, 2)
dev_display(ImageEmphasize)
set_display_font (WindowHandle, 16, 'mono', 'true', 'false')
dev_disp_text ('NG', 'image', 12, 12, 'red', [], [])
```

# 2. 图像预处理

> mean_image、gauss_filter、binomial_filter：消除噪声
> median_image：抑制小斑点、细线
> smooth_image、anisotropic_diffusion：图像平滑
> 腐蚀：对边界向内部收缩，消除边界点，去除小元素
> 膨胀：对边界向外部扩充，填充空洞；
> opening_circle：先腐蚀后膨胀，去除孤立点、毛刺，消除小物体、平滑较大物体边界，同时不改变面积
> closing_circle：先膨胀后腐蚀，填充物体内部细小空洞，连接临近物体、平滑边界，同时不改变面积

# 3. 图像分割

```matlab
* 阈值分割，用到灰度直方图
threshold (Image, Region, MinGray, MaxGray)
connection (Region, ConnectedRegions)
* 特征选择，用到特征直方图
select_shape (Regions, SelectedRegions, Features, Operation, Min, Max)
count_obj (SelectedRegions, Number)
area_center(SelectedRegions, Area, Row, Column)
fill_up (SelectedRegions, RegionFillUp)
```

# 4. 轮廓处理

```matlab
* 提取指定图像的外轮廓
gen_contour_region_xld (Regions, Contours, Mode)
* 将轮廓线分为多个部分
segment_contours_xld (Contours, ContoursSplit, Mode, SmoothCont, MaxLineDist1, MaxLineDist2)
* 通过不同的特征，提取出分割后轮廓中满足要求的轮廓线段
select_contours_xld (Contours, SelectedContours, Feature, Min1, Max1, Max2, Max2)
* 从轮廓线段集合中选择指定线段
select_obj (Objects, ObjectSelected, Index)
* 获取指定轮廓上点的像素坐标
get_contour_xld (Contour, Row, Col)
* 将位于轮廓线 Region 内的图像提取出来
reduce_domain (Image, Region, ImageReduced)
* 计算两个轮廓线之间最小的距离
distance_cc_min (Contour1, Contour2, Mode, DistanceMin)
```

# 5. 基于形状的模板匹配步骤

```matlab
read_image (mage, 'test.jpg')
gen_rectangle1 (Rectangle, Row1, Column1, Row2, Column2)
reduce_domain (Image, Region, ImageReduced)
* 创建模板
create_shape_model (Template, NumLevels, AngleStart, AngleExtent, AngleStep, Optimization, Metric, Contrast, MinContrast, ModelID)
* 得到模板的轮廓
get_shape_model_contours (ModelContours, ModelID, Level)
* 在另一幅图像中寻找模板对应的区域
find_shape_model (Image, ModelID, AngleStart, AngleExtent, MinScore, NumMatches, MaxOverlap, SubPixel, NumLevels, Greediness, Row, Column, Angle, Score)
* 确定匹配位置的轮廓
vector_angle_to_rigid (Row1, Column1, Angle1, Row2, Column2, Angle2， HomMat2D)
* 得到匹配图像的轮廓线
affine_trans_contour_xld (Contours, ContoursAffinTrans, HomMat2D)
```

# 6. 批量导入图片

```matlab
list_image_files ('scratch', 'png', [], ImageFiles)
```

# 7. 局部阈值

```matlab
* 一般找轮廓
read_image (Image, ImageFiles[Index])
mean_image (Image, ImageMean, 5, 5)
dyn_threshold (Image, ImageMean, RegionDynThresh, 5, 'dark')
```

# 8. 频域滤波

```matlab
decompose3 (Image, R, G, B)
rft_generic (B, ImageFFT, 'to_freq', 'none', 'complex', Width)
gen_gauss_filter (ImageGauss, 100, 100, 0, 'n', 'rft', Width, Height)
convol_fft (ImageFFT, ImageGauss, ImageConvol)
rft_generic (ImageConvol, ImageFFT1, 'from_freq', 'none', 'byte', Width)
sub_image (B, ImageFFT1, ImageSub, 2, 100)
```

# 9. 差异模型

```matlab
create_shape_model (ImageReduced, 5, rad(-10), rad(20), 'auto', 'none', 'use_polarity', 20, 10, ShapeModelID)
create_variation_model (Width, Height, 'byte', 'standard', VariationModelID)
find_shape_model (Image, ShapeModelID, rad(-10), rad(20), 0.5, 1, 0.5, 'least_squares', 0, 0.9, Row, Column, Angle, Score)
vector_angle_to_rigid (Row, Column, Angle, RowRef, ColumnRef, 0, HomMat2D)
affine_trans_image (Image, ImageTrans, HomMat2D, 'constant', 'false')
train_variation_model (ImageTrans, VariationModelID)
get_variation_model (MeanImage, VarImage, VariationModelID)
prepare_variation_model (VariationModelID, 20, 3)
erosion_rectangle1 (RegionFillUp, RegionROI, 1, 15)
compare_variation_model (ImageReduced, RegionDiff, VariationModelID)
```

# 10. 选择指定区域

```matlab
gen_empty_region (Region)
for I := 1 to Number by 1
    select_obj (Regions, ObjectSelected, I)
    union2 (Region, ObjectSelected, Region)
endfor
```

# 11. 根据骨架合并直线

```matlab
threshold (Image, Region, 0, 150)
connection (Region, ConnectedRegions)
skeleton (ConnectedRegions, Skeleton)
gen_contours_skeleton_xld (Skeleton, Contours, 1, 'filter')
union_collinear_contours_xld (Contours, UnionContours, 30, 2, 10, 0.7, 'attr_keep')
```

# 12. 获取骨架、XLD 端点

```python
gen_contour_region_xld (Region, Contours, 'center')
gen_region_contour_xld (Contours, Region, 'filled')
skeleton (Region, Skeleton)
junctions_skeleton (Skeleton, EndPoints, JuncPoints)
get_region_points (EndPoints, Rows, Columns)
```

# 13. Halcon 图像自适应显示

```c#
HOperatorSet.GenEmptyObj(out HObject ho_Image);
ho_Image.Dispose();
HOperatorSet.ReadImage(out ho_Image, filename);
HOperatorSet.GetImageSize(ho_Image, out HTuple width, out HTuple height);
//HOperatorSet.SetPart(ho_Window, 0, 0, height - 1, width - 1);
HOperatorSet.DispObj(ho_Image, ho_Window);

double wRatio = Halcon.ActualWidth / ImageWidth;
double hRatio = Halcon.ActualHeight / ImageHeight;
double ratio = Math.Min(wRatio, hRatio);
// Halcon 是 WPF 控件对象
Halcon.HImagePart = wRatio > hRatio
    ? new Rect
    {
        X = -0.5 * (Halcon.ActualWidth / ratio - ImageWidth),
        Y = 0,
        Width = Halcon.ActualWidth / ratio,
        Height = Halcon.ActualHeight / ratio
    }
    : new Rect
    {
        X = 0,
        Y = -0.5 * (Halcon.ActualHeight / ratio - ImageHeight),
        Width = Halcon.ActualWidth / ratio,
        Height = Halcon.ActualHeight / ratio
    };
    
// 内置方法
// 图像自适应显示
Halcon.SetFullImagePart();
```

# 14. Halcon 控件坐标对应的图像坐标

```c#
// 控件坐标和图像坐标转换，有些许误差
private Point GetImageHalconPoint(double x, double y, bool flag = true)
{
	// Halcon 控件宽高
	double cHeight = Halcon.ActualHeight;
	double cWidth = Halcon.ActualWidth;
	// Halcon 图像区域
	double x0 = Halcon.HImagePart.X;
	double y0 = Halcon.HImagePart.Y;
	double imHeight = Halcon.HImagePart.Height;
	double imWidth = Halcon.HImagePart.Width;
	double ratio_y = imHeight / cHeight;
	double ratio_x = imWidth / cWidth;
	// 当前点坐标：相对控件或者相对图像
	double x1;
	double y1;
	// Halcon → Image
	if (flag)
	{

		x1 = (ratio_x * x) + x0;
		y1 = (ratio_y * y) + y0;
	}
	else
	// Image → Halcon
	{
		x1 = (x - x0) / ratio_x;
		y1 = (y - y0) / ratio_y;
	}
	return new Point(x1, y1);
}

// Halcon 事件
private void Halcon_HMouseMove(object sender, HSmartWindowControlWPF.HMouseEventArgsWPF e)
{
	// 图像坐标
	int row = (int)e.Row;
	int column = (int)e.Column;
}
```

