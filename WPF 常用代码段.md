WPF 常用代码段
================================================================================

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
double width = SystemParameters.PrimaryScreenWidth;
double height = SystemParameters.PrimaryScreenHeight;
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

