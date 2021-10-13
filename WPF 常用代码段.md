WPF 常用代码段
================================================================================

文件不更新了，文章持续更新：https://zhuanlan.zhihu.com/p/401958234

[TOC]

--------------------------------------------------------------------------------

# 1. 获取指定目录下文件、文件夹

```c#
DirectoryInfo folder = new DirectoryInfo(@"D:demos\images");
FileInfo[] files = folder.GetFiles();
DirectoryInfo[] folders = folder.GetDirectories();

// 判断文件夹是否存在，不存在则创建
DirectoryInfo folder = new DirectoryInfo(vm.StrSaveImagePath);
if (!folder.Exists)
{
    folder.Create();
}
// 删除文件
File.Delete(files[i].FullName);

// 一些属性
FileInfo.Exists：文件是否存在；
FileInfo.Name：文件名；
FileInfo.Extensioin：文件扩展名；
FileInfo.FullName：文件完整路径；
FileInfo.Directory：文件所在目录；
FileInfo.DirectoryName：文件完整路径；
FileInfo.Length：文件大小（字节数）；
FileInfo.IsReadOnly：文件是否只读；
FileInfo.CreationTime：文件创建时间；
FileInfo.LastAccessTime：文件访问时间；
FileInfo.LastWriteTime：文件修改时间；
```

# 2. string.Format 格式化输出

```c#
string.Format("{0:N1}", 56789);           // 56,789.0
string.Format("{0:N2}", 56789);           // 56,789.00
string.Format("{0:F1}", 56789);           // 56789.0
string.Format("{0:F2}", 56789);           // 56789.00
(56789 / 100.0).ToString("#.##");         // 567.89
(56789 / 100).ToString("#.##");           // 567
string.Format("{0:C}", 0.2)               // ￥0.20 
string.Format("{0:C1}", 23.15)            // ￥23.2
string.Format("{0:D3}", 23)               // 023
string.Format("{0:D2}", 1223)            // 1223
string.Format("{0:N}", 14200)            // 14,200.00
string.Format("{0:N3}", 14200.2458)      // 14,200.246
string.Format("{0:P}", 0.24583)          // 24.58%
string.Format("{0:P1}", 0.24583)         // 24.6%
string.Format("{0:0000.00}", 12394.039)  // 12394.04
string.Format("{0:0000.00}", 194.039)    // 0194.04
string.Format("{0:###.##}", 12394.039)   // 12394.04
string.Format("{0:####.#}", 194.039)     // 194
```

# 3. 日期时间

```c#
// 当前时间
DateTime currentTime = DateTime.Now;
int year = currentTime.Year;
int month = currentTime.Month;
int day = currentTime.Day;
int hour = currentTime.Hour;
int minute = currentTime.Minute;
int second = currentTime.Second;
int millisecond = currentTime.Millisecond;

// 间隔
DateTime t1 = new DateTime(2021, 8, 20);
DateTime t2 = DateTime.Now;
double day = t2.Subtract(t1).TotalDays;

// 格式化输出
string.Format("{0:d}", System.DateTime.Now)  // 2009-3-20
string.Format("{0:D}", System.DateTime.Now)  // 2009年3月20日
string.Format("{0:f}", System.DateTime.Now)  // 2009年3月20日 15:37
string.Format("{0:F}", System.DateTime.Now)  // 2009年3月20日 15:37:52
string.Format("{0:g}", System.DateTime.Now)  // 2009-3-20 15:38
string.Format("{0:G}", System.DateTime.Now)  // 2009-3-20 15:39:27
string.Format("{0:m}", System.DateTime.Now)  // 3月20日
string.Format("{0:t}", System.DateTime.Now)   // 15:41
string.Format("{0:T}", System.DateTime.Now)  // 15:41:50
```

# 4. 线程

```c#
Thread thread = new Thread(TestUnitThread);
thread.Start();
thread.IsBackground = true;

private void TestUnitThread()
{
    Thread.Sleep(2000);
}
```

# 5. 计时器

```c#
private DispatcherTimer del_timer;

del_timer = new DispatcherTimer
{
    // 一天执行一次
    Interval = new TimeSpan(1, 0, 0, 0)
};
del_timer.Tick += new EventHandler(DeleteFiles);
del_timer.Start();

private void DeleteFiles(object sender, EventArgs e)
{
    DeleFiles();
}

private void DeleFiles()
{

}
```

# 6. 保存配置文件 Serializable 序列化和反序列化

MVVM 模式不行，可创建一个相同内容的类

~~~c#
// CfgBearingVMs.cs
[Serializable]
public class CfgBearingVMs
{
 private int intSelectedIndexDel;
 public int IntSelectedIndexDel
 {
     get => intSelectedIndexDel;
     set { intSelectedIndexDel = value; }
 }

 private string strSaveImagePath;
 public string StrSaveImagePath
 {
     get => strSaveImagePath;
     set { strSaveImagePath = value; }
 }

 public CfgBearingVMs()
 {
     IntSelectedIndexDel = 2;
     StrSaveImagePath = @"C:\Users\Administrator";
 }
}

// CfgBearingVM.cs
public class CfgBearingVM : ViewModelBase
{
    private int intSelectedIndexDel;
    public int IntSelectedIndexDel
    {
        get => intSelectedIndexDel;
        set => Set(ref intSelectedIndexDel, value);
    }

```c#
private string strSaveImagePath;
public string StrSaveImagePath
{
    get => strSaveImagePath;
    set => Set(ref strSaveImagePath, value);
}

public CfgBearingVM()
{
    IntSelectedIndexDel = 2;
    StrSaveImagePath = @"C:\Users\Administrator";
}
```

}

// 保存
CfgBearingVMs vm = new CfgBearingVMs(EnumAcqMode, IntSelectedIndexAcq, IntSelectedIndexDel, StrSaveImagePath);
IFormatter formatter = new BinaryFormatter();
Stream stream = new FileStream("setting/setting.bin", FileMode.Create, FileAccess.Write, FileShare.None);
formatter.Serialize(stream, vm);
stream.Close();

// 读取
using (FileStream stream = new FileStream("setting/setting.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
{
    BinaryFormatter b = new BinaryFormatter();
    CfgBearingVMs vm = b.Deserialize(stream) as CfgBearingVMs;
    stream.Close();
}
~~~

# 7. 屏幕截图

```c#
private void CaptureScreen()
{
 System.Drawing.Size size = Screen.PrimaryScreen.Bounds.Size;
 System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(size.Width, size.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
 System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap);
 g.CopyFromScreen(0, 0, 0, 0, size);
 bitmap.Save("screen.png", System.Drawing.Imaging.ImageFormat.Png);
}
```

# 8. TextBlock 多行文本

> ```c#
> // 后台代码
> TextBlock1.Text = "AAAAAAA\nBBBBBBBB";
> ```
>
> XAML代码
> <TextBlock Text="AAAAAAA&#x000A;BBBBBB"/>

# 9. 控件相对位置

```c#
// 相对于父级偏移量
Vector vector = VisualTreeHelper.GetOffset(myTextBlock);
Point currentPoint = new Point(vector.X, vector.Y);
// 相对于 WIndow 偏移量
GeneralTransform transform = myTextBlock.TransformToAncestor(this);
Point currentPoint = transform .Transform(new Point(0, 0));
```

# 10. 屏幕尺寸

```c#
// 全屏尺寸
double width = SystemParameters.PrimaryScreenWidth;
double height = SystemParameters.PrimaryScreenHeight;

// 不含任务栏尺寸
double width = SystemParameters.WorkArea.Width;
double height = SystemParameters.WorkArea.Height;
```

# 11. 显示图像

> XAML 代码
> <Image Source="pack://application:,,,/images/test.bmp"/>
>
> ```c#
> BitmapImage image = new BitmapImage();
> image.BeginInit();
> image.UriSource = new Uri(@"test.png", UriKind.RelativeOrAbsolute);
> image.EndInit();
> ImageShow.Source = image; 
> ```

# 12. 程序暂停

```c#
// 线程里直接
Thread.Sleep(1000);

public static class DispatcherHelper
{
    [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
    public static void DoEvents()
    {
        DispatcherFrame frame = new DispatcherFrame();
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(ExitFrames), frame);
        try 
        { 
            Dispatcher.PushFrame(frame); 
        }
        catch (InvalidOperationException) 
        { 
        }
    }
    private static object ExitFrames(object frame)
    {
        ((DispatcherFrame)frame).Continue = false;
        return null;
    }
}

// 暂停 1s
DateTime t = DateTime.Now.AddMilliseconds(1000);
while (DateTime.Now < t)
{
    DispatcherHelper.DoEvents();
}
```

# 13. 试错

```c#
try
{
	// do something
}
catch (Exception ex)
{
	string msg = ex.Message;
}
```

# 14. 一维数组保存成文本

```c#
double[] a1 = new double[4];
a1[3] = 8;
File.WriteAllLines("a1.txt", a1.Select(d => d.ToString()));
```

# 15. 文件路径

```c#
string dirPath = @"D:\TestDir";
string filePath = @"D:\TestDir\TestFile.txt";
// 当前路径
Environment.CurrentDirectory;
// 文件或文件夹所在目录
Path.GetDirectoryName(filePath);     // D:\TestDir
Path.GetDirectoryName(dirPath);      // D:\
// 文件扩展名
Path.GetExtension(filePath);         // .txt
// 文件名
Path.GetFileName(filePath);          // TestFile.txt
Path.GetFileName(dirPath);           // TestDir
Path.GetFileNameWithoutExtension(filePath); // TestFile
// 绝对路径
Path.GetFullPath(filePath);          // D:\TestDir\TestFile.txt
Path.GetFullPath(dirPath);           // D:\TestDir  
// 更改扩展名
Path.ChangeExtension(filePath, ".jpg"); // D:\TestDir\TestFile.jpg
// 根目录
Path.GetPathRoot(dirPath);           //D:\      
// 生成路径
Path.Combine(new string[] { @"D:\", "BaseDir", "SubDir", "TestFile.txt" }); // D:\BaseDir\SubDir\TestFile.txt
// 生成随机文件
Path.GetRandomFileName();
// 创建临时文件
Path.GetTempFileName();
// 返回当前系统的临时文件夹的路径
Path.GetTempPath();
// 文件名中无效字符
Path.GetInvalidFileNameChars();
// 路径中无效字符
Path.GetInvalidPathChars();
```

# 16. 渐变色

```c#
// XAML 
<Grid x:Name="GridBack">
<Grid.Background>
    <LinearGradientBrush>
        <LinearGradientBrush.GradientStops>
            <GradientStop Offset="0" Color="Red"/>
            <GradientStop Offset="0.5" Color="Indigo"/>
            <GradientStop Offset="1.0" Color="Violet"/>
        </LinearGradientBrush.GradientStops>
    </LinearGradientBrush>
</Grid.Background>
</Grid>

// 等价后台代码
GradientStop color1 = new GradientStop
{
    Offset = 0,
    Color = Colors.Red,
};
GradientStop color2 = new GradientStop
{
    Offset = 0.5,
    Color = Colors.Indigo,
};
GradientStop color3 = new GradientStop
{
    Offset = 1,
    Color = Colors.Violet,
};
LinearGradientBrush brush = new LinearGradientBrush();
brush.GradientStops.Add(color1);
brush.GradientStops.Add(color2);
brush.GradientStops.Add(color3);
GridBack.Background = brush;
```

# 17. Button 复杂背景

```c#
<Button HorizontalAlignment="Center" VerticalAlignment="Center" Background="White" BorderBrush="White">
    <Grid>
        <Polygon Points="100,25 125,0 200,25 125,50" Fill="LightBlue"/>
        <Polygon Points="100,25 75,0 0,25 75,50" Fill="LightPink"/>
    </Grid>
</Button>
```

# 18. 后台设置元素绑定

```c#
// XAML 绑定
<Slider x:Name="SliderFont" Margin="10" Minimum="10" Maximum="40" SmallChange="1" LargeChange="4"/>
<Button x:Name="ButtonFont" Content="Binding fontsize" Margin="5"
        FontSize="{Binding ElementName=SliderFont, Path=Value, Mode=TwoWay}"/>

// 后台绑定
Binding binding = new Binding
{
    Source = SliderFont,
    Path = new PropertyPath("Value"),
    Mode = BindingMode.TwoWay,
};
ButtonFont.SetBinding(FontSizeProperty, binding);

// 注意设置 Mode 为 TwoWay，否则若其它地方有修改，则绑定失效，比如
ButtonFont.FontSize = 30;
```

# 19. 资源样式动画

```c#
// 资源定义
<Window.Resources>
    <ImageBrush x:Key="TileBrush" TileMode="Tile" ViewportUnits="Absolute" Viewport="0 0 16 16" Opacity="0.5"
                ImageSource="Resource\Image\icon.ico"/>

    <Style x:Key="BigFontButtonStyle" TargetType="Button">
        <Setter Property="FontFamily" Value="Times New Roman"/>
        <Setter Property="FontSize" Value="24"/>
        <Setter Property="FontWeight" Value="Bold"/>
    </Style>

    <!-- 资源继承 -->
    <Style x:Key="EmphasizeBigFont" BasedOn="{StaticResource BigFontButtonStyle}" TargetType="Button">
        <Setter Property="Control.Foreground" Value="Magenta"/>
        <Setter Property="Control.Background" Value="DarkBlue"/>
    </Style>

    <!-- 关联事件 -->
    <Style x:Key="MouseOverHighlightStyle" TargetType="TextBlock">
        <Setter Property="Padding" Value="5"/>
        <EventSetter Event="MouseEnter" Handler="TextBlock_MouseEnter"/>
        <EventSetter Event="MouseLeave" Handler="TextBlock_MouseLeave"/>
        <!-- 事件触发器，字体缩放效果 -->
        <Style.Triggers>
            <EventTrigger RoutedEvent="Mouse.MouseEnter">
                <EventTrigger.Actions>
                    <BeginStoryboard>
                        <Storyboard>
                            <DoubleAnimation Duration="0:0:0.2" Storyboard.TargetProperty="FontSize" To="24"/>
                        </Storyboard>
                    </BeginStoryboard>
                </EventTrigger.Actions>
            </EventTrigger>
            <EventTrigger RoutedEvent="Mouse.MouseLeave">
                <EventTrigger.Actions>
                    <BeginStoryboard>
                        <Storyboard>
                            <DoubleAnimation Duration="0:0:1" Storyboard.TargetProperty="FontSize"/>
                        </Storyboard>
                    </BeginStoryboard>
                </EventTrigger.Actions>
            </EventTrigger>
        </Style.Triggers>
    </Style>
</Window.Resources>

// 资源使用
<Button Content="DynamicResource" Margin="10" Background="{DynamicResource TileBrush}"/>
<Button Content="StaticResource" Margin="10" Background="{StaticResource TileBrush}"/>
<Button Content="ButtonStyle" Margin="10" Style="{StaticResource EmphasizeBigFont}"/>
<TextBlock Text="Hover over me" Margin="10" Style="{StaticResource MouseOverHighlightStyle}"/>

// 后台代码
// 后台修改资源，动态资源改变
Resources["TileBrush"] = new SolidColorBrush(Colors.LightGoldenrodYellow);

private void TextBlock_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
{
    (sender as TextBlock).Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
}

private void TextBlock_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
{
    (sender as TextBlock).Background = null;
}
```

# 20. VS2019 C# 类注释模板

> C:\Program Files (x86)\Microsoft Visual Studio\2019\Professional\Common7\IDE\ItemTemplates\CSharp\Code\2052\Class\Class.cs

```c#
namespace $rootnamespace$
{
    ///
    /// ----------------------------------------------------------------
    /// Copyright @Taosy.W $year$ All rights reserved
    /// Author      : Taosy.W
    /// Created Time: $time$
    /// Description :
    /// ------------------------------------------------------
    /// Version      Modified Time            Modified By    Modified Content
    /// V1.0.0.0     $time$    Taosy.W                 
    ///
    public class $safeitemrootname$
    {
    }
}
```

# 21. 子线程更新UI

```c#
// 在新的线程里用如下方式更新到界面
Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
(ThreadStart)delegate ()
{
	// 列表更新
	DataListNG.Add(dataModel);
}
);
```

# 22.  ComboBox绑定枚举、显示描述

```c#
// 枚举
public enum Gender
{
    Male = 0,
    Female
}

// 资源搞一波
<Window.Resources>
    <ObjectDataProvider x:Key="DemoGender" MethodName="GetValues" ObjectType="{x:Type core:Enum}">
        <ObjectDataProvider.MethodParameters>               
            <x:Type Type="model:Gender"/>  
        </ObjectDataProvider.MethodParameters>
    </ObjectDataProvider>
</Window.Resources>

<ComboBox ItemsSource="{Binding Source={StaticResource DemoGender}}" SelectedIndex="0"/>

// 显示描述属性
<ComboBox ItemsSource="{Binding EnumsDescription}" DisplayMemberPath="Value" SelectedValuePath="Key" SelectedValue="{Binding ExampleProperty}"/>

// 添加描述属性
public enum Gender
{
    [Description("男性")]
    Male = 0,
    [Description("女性")]
    Female
}

// ViewModel
private Dictionary<Gender, string> enumsDescription;
public Dictionary<Gender, string> EnumsDescription
{
    get => new Dictionary<Gender, string>()
    {
        {Gender.Male, "男性"},
        {Gender.Female, "女性"},
    };
    set => Set(ref enumsDescription, value);
}
// 或者下面方式
private Dictionary<Gender, string> enumsDescription;
public Dictionary<Gender, string> EnumsDescription
{
    get
    {
        Dictionary<Gender, string> pairs = new Dictionary<Gender, string>();
        foreach (Gender item in Enum.GetValues(typeof(Gender)))
        {
            DescriptionAttribute attributes = (DescriptionAttribute)item.GetType().GetField(item.ToString()).GetCustomAttribute(typeof(DescriptionAttribute), false);
            pairs.Add(item, attributes.Description);
        }
        return pairs;
    }

    set => Set(ref enumsDescription, value);
}

private Gender exampleProperty;
public Gender ExampleProperty
{
    get => exampleProperty;
    set => Set(ref exampleProperty, value);
}
```

# 23. MVVM常用控件数据、命令绑定

> 绑定字符串
> 绑定数值
> 绑定控件属性
> RadioButton枚举绑定
> 转换器
> ComboBox显示枚举描述
> DataGrid数据绑定
> 命令绑定

参考项目：[MvvmCmdBinding](https://github.com/AFei19911012/WPFSamples/tree/main/MvvmCmdBinding)

# 24. DataGrid 指定行颜色

```xaml
// 根据条件设置颜色
<DataGrid Height="400" SelectionMode="Single" HeadersVisibility="All" AutoGenerateColumns="False" 
          CanUserSortColumns="False" ItemsSource="{Binding DataListCalInfo}">
    <DataGrid.Columns>
        <DataGridTextColumn Width="auto" Binding="{Binding Header}" IsReadOnly="True" Header="项目"/>
        <DataGridTextColumn Width="*" Binding="{Binding Content}" IsReadOnly="True" Header="描述">
            <DataGridTextColumn.CellStyle>
                <Style TargetType="DataGridCell">
                    <Style.Triggers>
                        <DataTrigger Binding="{Binding IsEffective}" Value="false">
                            <Setter Property="Background" Value="{Binding ColorFalse}"/>
                        </DataTrigger>
                        <DataTrigger Binding="{Binding IsEffective}" Value="true">
                            <Setter Property="Background" Value="{Binding ColorTrue}"/>
                        </DataTrigger>
                    </Style.Triggers>
                </Style>
            </DataGridTextColumn.CellStyle>
        </DataGridTextColumn>
    </DataGrid.Columns>
/DataGrid>
```

# 25. 横竖分隔线

```xaml
// 横向分隔线
<Separator Background="{DynamicResource PrimaryBrush}"/>

// 纵向分隔线
<GridSplitter Width="1" Background="{DynamicResource PrimaryBrush}"/>
```

# 26. 打开文件、选择文件夹对话框

```c#
// 打开文件对话框
using Microsoft.Win32;

OpenFileDialog dialog = new OpenFileDialog
{
    Title = "选择标定图片",
    Filter = "图像文件(*.jpg;*.png;*.bmp)|*.jpg;*.png;*.bmp",
    RestoreDirectory = true,
};

if (dialog.ShowDialog() != true)
{
    return;
}
string filename = dialog.FileName;


// 打开文件夹对话框
System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
{
    return;
}
string foldername = dialog.SelectedPath.Trim();
```

# 27. Canvas 平移和缩放

```c#
// xaml 测试用例
<Border x:Name="DrawingBorder" ClipToBounds="True"
        MouseDown="DrawingBorder_MouseDown" MouseMove="DrawingBorder_MouseMove" 
        MouseUp="DrawingBorder_MouseUp" MouseWheel="DrawingBorder_MouseWheel">
    <Grid>
        <halcon:HSmartWindowControlWPF Name="HalconWPF" BorderThickness="1" BorderBrush="{DynamicResource PrimaryBrush}"/>
        <Canvas x:Name="DrawingCanvas">
            <Canvas.RenderTransform>
                <TransformGroup/>
            </Canvas.RenderTransform>
            <Line X1="100" Y1="100" X2="200" Y2="200" Stroke="Red" StrokeThickness="5"/>
            <Rectangle Canvas.Left="250" Canvas.Top="250" Width="100" Height="100" Fill="Red"/>
            <Ellipse Canvas.Left="400" Canvas.Top="200" Width="100" Height="100" Fill="Red"/>
        </Canvas>
    </Grid>
</Border>

// 后台
private Point StartPoint;
private bool CanMove = false;

/// <summary>
/// 点击鼠标
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void DrawingBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
{
    StartPoint = e.GetPosition(e.Device.Target);
    CanMove = true;
}

/// <summary>
/// 平移
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void DrawingBorder_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
{
    Point curPoint = e.GetPosition(e.Device.Target);
    Vector vector = curPoint - StartPoint;
    if (CanMove)
    {
        TransformGroup group = DrawingCanvas.RenderTransform as TransformGroup;
        group.Children.Add(new TranslateTransform(vector.X, vector.Y));
        DrawingCanvas.RenderTransform = group;
        // 记得更新起点
        StartPoint = curPoint;
    }
}

/// <summary>
/// 鼠标弹起
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void DrawingBorder_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
{
    CanMove = false;
}

/// <summary>
/// 缩放
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
private void DrawingBorder_MouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
{
    Point curPoint = e.GetPosition(e.Device.Target);
    TransformGroup group = DrawingCanvas.RenderTransform as TransformGroup;
    if (e.Delta > 0)
    {
        group.Children.Add(new ScaleTransform(HalconWPF.HZoomFactor, HalconWPF.HZoomFactor, curPoint.X, curPoint.Y));
    }
    else
    {
        group.Children.Add(new ScaleTransform(1 / HalconWPF.HZoomFactor, 1 / HalconWPF.HZoomFactor, curPoint.X, curPoint.Y));
    }
    // 更新
    DrawingCanvas.RenderTransform = group;
}
```



