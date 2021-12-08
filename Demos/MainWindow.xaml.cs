using Demos.Method;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Demos
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private string Thread_String { get; set; }
        private string Thread_Para_String { get; set; }
        private string Task_String { get; set; }
        private string Task_Para_String { get; set; }
        private string Timer_String { get; set; }

        private bool CanMove { get; set; }
        private Point PointMoveOri { get; set; }
        private bool IsPanning { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Thread_String = TB_Thread.Text;
            Thread_Para_String = TB_ThreadPara.Text;
            Task_String = TB_Task.Text;
            Task_Para_String = TB_Task_Para.Text;
            Timer_String = TB_Timer.Text;

            CanMove = false;
            PointMoveOri = new Point();
            IsPanning = false;
        }

        /// <summary>
        /// NPOI 读写 Excel
        /// 建议用 IWorkbook ISheet IRow ICell 统一管理避免不必要的错误
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnNPOI_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"test.xlsx";

            // 获取 Sheet 名称
            IWorkbook workbook;
            FileStream file;
            using (file = File.OpenRead(filename))
            {
                // xls：HSSFWorkbook；
                // xlsx:：XSSFWorkbook
                workbook = new XSSFWorkbook(file);
            }
            int sheet_count = workbook.NumberOfSheets;
            List<string> sheet_names = new List<string>();
            for (int i = 0; i < sheet_count; i++)
            {
                sheet_names.Add(workbook.GetSheetName(i));
            }

            // 读取 Excel 指定 sheet 内容
            // workbook --> sheet --> row --> cell
            List<List<string>> content = new List<List<string>>();
            // 读取名为 test 的 sheet
            ISheet sheet = workbook.GetSheet("test");
            IRow row;
            ICell cell;
            List<string> row_content;
            for (int i = 0; i <= sheet.LastRowNum; i++)
            {
                row_content = new List<string>();
                // 行
                row = sheet.GetRow(i);
                // 不为 null 则继续
                if (row != null)
                {
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        // cell
                        cell = sheet.GetRow(i).GetCell(j);
                        if (cell != null)
                        {
                            // 如果是公式，则读取公式计算的值
                            if (cell.CellType == CellType.Formula)
                            {
                                cell.SetCellType(CellType.String);
                                row_content.Add(cell.StringCellValue);
                            }
                            else
                            {
                                row_content.Add(cell.ToString());
                            }
                        }
                        else
                        {
                            row_content.Add("");
                        }
                    }
                }
                content.Add(row_content);
            }


            // 写内容到新的 sheet 如已有该名称的 sheet 则报错
            // workbook --> sheet --> row --> cell
            if (!sheet_names.Contains("sheetDemo"))
            {
                sheet = workbook.CreateSheet("sheetDemo");
                // 先创建一个 row 重复创建会清空之前的内容
                row = sheet.CreateRow(0);
                row.CreateCell(0).SetCellValue("A1");
                row.CreateCell(1).SetCellValue("A2");
                row.CreateCell(3).SetCellValue("A4");
                row = sheet.CreateRow(2);
                row.CreateCell(0).SetCellValue("C1");
                row.CreateCell(2).SetCellValue("C3");
                // 保存
                using (file = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    workbook.Write(file);
                }
            }

            // 写内容到现有 Excel 指定 sheet
            // 建议先备份，写失败后会清空 Excel 内容
            file = File.OpenRead(filename);
            workbook = WorkbookFactory.Create(file);
            sheet = workbook.GetSheet("test");
            row = sheet.GetRow(2);
            if (row == null)
            {
                row = sheet.CreateRow(2);
            }
            cell = row.GetCell(3);
            if (cell == null)
            {
                cell = row.CreateCell(3);
            }
            // 赋值
            cell.SetCellValue(23);

            sheet.GetRow(4).CreateCell(4);
            sheet.GetRow(4).GetCell(4).SetCellValue(44);
            // 赋值
            cell.SetCellValue(23);
            // 类型
            cell.SetCellType(CellType.Numeric);
            // 样式
            ICellStyle style = workbook.CreateCellStyle();
            // 居中
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            // 字体
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            style.SetFont(font);
            // 应用样式
            cell.CellStyle = style;
            // 强制计算公式的值
            sheet.ForceFormulaRecalculation = true;
            // 保存
            file = File.Create(filename);
            workbook.Write(file);
            file.Close();


            // 读取 Excel 指定行内容
            using (file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(file);
            }
            sheet = workbook.GetSheet("test");
            row_content = new List<string>();
            // 获取第 6 行内容
            row = sheet.GetRow(5);
            if (row != null)
            {
                for (int j = 0; j < row.LastCellNum; j++)
                {
                    cell = row.GetCell(j);
                    if (cell != null)
                    {
                        // 如果是公式，则获取公式计算结果
                        if (cell.CellType == CellType.Formula)
                        {
                            //XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(workbook);
                            //if (eva.Evaluate(cell).StringValue != null)
                            //{
                            //    // 字符串结果
                            //    row_content.Add(eva.Evaluate(cell).StringValue);
                            //}
                            //else
                            //{
                            //    // 数值结果
                            //    row_content.Add(eva.Evaluate(cell).NumberValue.ToString());
                            //}
                            cell.SetCellType(CellType.String);
                            row_content.Add(cell.StringCellValue);
                        }
                        else
                        {
                            // 这样显示的内容是公式
                            row_content.Add(cell.ToString());
                        }
                    }
                    else
                    {
                        row_content.Add("");
                    }
                }
            }
        }


        /// <summary>
        /// 线程 任务 计时器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnThread_Click(object sender, RoutedEventArgs e)
        {
            // 线程：无参
            Thread thread = new Thread(ThreadEvent)
            {
                IsBackground = true
            };
            thread.Start();

            // 线程：带参
            //Thread thread_para = new Thread(new ThreadStart(delegate { ThreadParaEvent("带参"); }));
            Thread thread_para = new Thread(() => { ThreadParaEvent("带参"); });
            thread_para.Start();

            _ = MessageBox.Show("线程状态：" + thread.IsAlive.ToString() + "\n" + "线程状态：" + thread_para.IsAlive.ToString());
        }

        private void ThreadEvent()
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_Thread.Text = Thread_String + TB_Timer.Text;
            }
            );
        }

        private void ThreadParaEvent(string para)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_ThreadPara.Text = Thread_Para_String + para + " " + TB_Timer.Text;
            }
            );
            Thread.Sleep(3000);
        }

        private void BtnTask_Click(object sender, RoutedEventArgs e)
        {
            // 任务：无参
            Task task = new Task(TaskEvent);
            task.Start();

            // 任务：带参
            Task task_para = new Task(() => { TaskParaEvent("带参"); });
            task_para.Start();

            _ = MessageBox.Show("线程状态：" + task.Status.ToString() + "\n" + "线程状态：" + task_para.Status.ToString());
        }

        private void TaskEvent()
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_Task.Text = Task_String + TB_Timer.Text;
            }
            );
        }

        private void TaskParaEvent(string para)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_Task_Para.Text = Task_Para_String + para + " " + TB_Timer.Text;
            }
            );
            Thread.Sleep(3000);
        }

        private void BtnTimer_Click(object sender, RoutedEventArgs e)
        {
            DispatcherTimer update_timer = new DispatcherTimer
            {
                // 1s 执行一次
                Interval = new TimeSpan(0, 0, 1)
            };
            update_timer.Tick += new EventHandler(TimerEvent);
            update_timer.Start();
        }

        private void TimerEvent(object sender, EventArgs e)
        {
            // 在新的线程里用如下方式更新到界面
            _ = Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
            (ThreadStart)delegate ()
            {
                TB_Timer.Text = Timer_String + string.Format("{0:G}", DateTime.Now);
            }
            );
        }

        private void BtnWaitHandle_Click(object sender, RoutedEventArgs e)
        {
            Task task = new Task(TaskWaitHandle);
            task.Start();
        }

        private void TaskWaitHandle()
        {
            // 2 个任务同步执行
            AutoResetEvent[] watchers = new AutoResetEvent[2];
            for (int i = 0; i < watchers.Length; i++)
            {
                watchers[i] = new AutoResetEvent(false);
            }
            Task[] tasks = new Task[2];
            tasks[0] = new Task(() =>
            {
                TaskEvent();
                // 线程执行完的时候通知
                _ = watchers[0].Set();
            });

            tasks[1] = new Task(() =>
            {
                TaskParaEvent("带参");
                // 线程执行完的时候通知
                _ = watchers[1].Set();
            });

            tasks[0].Start();
            tasks[1].Start();

            bool result = WaitHandle.WaitAll(watchers);
            if (result)
            {
                _ = MessageBox.Show("线程状态：" + tasks[0].Status.ToString() + "\n" + "线程状态：" + tasks[1].Status.ToString());
            }
        }


        /// <summary>
        /// InkCanvas 绘制箭头
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DrawingCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CanMove = true;
            PointMoveOri = e.GetPosition(e.Device.Target);
        }
        private void DrawingCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point curPoint = e.GetPosition(e.Device.Target);
            // 创建箭头 每次刷新
            if (CanMove && !IsPanning)
            {
                DrawingCanvas.Strokes.Clear();
                DrawingCanvas.Strokes.Add(InkCanvasMethod.CreateArrow(PointMoveOri, curPoint));
            }

            // 平移
            if (CanMove && IsPanning)
            {
                Matrix matrixMove = new Matrix();
                matrixMove.Translate(curPoint.X - PointMoveOri.X, curPoint.Y - PointMoveOri.Y);
                DrawingCanvas.Strokes.Transform(matrixMove, false);
                // 更新初始移动位置
                PointMoveOri = curPoint;
            }
        }
        private void DrawingCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            CanMove = false;
        }
        private void DrawingCanvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            // 当前点为中心缩放
            Point curPoint = e.GetPosition(e.Device.Target);
            Matrix matrix = new Matrix();
            if (e.Delta > 0)
            {
                matrix.ScaleAt(1.25, 1.25, curPoint.X, curPoint.Y);
            }
            else
            {
                matrix.ScaleAt(0.8, 0.8, curPoint.X, curPoint.Y);
            }
            DrawingCanvas.Strokes.Transform(matrix, false);
        }
        private void DrawingCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            IsPanning = true;
            if (e.ClickCount > 1)
            {
                DrawingCanvas.Strokes.Clear();
                IsPanning = false;
            }
        }
    }
}