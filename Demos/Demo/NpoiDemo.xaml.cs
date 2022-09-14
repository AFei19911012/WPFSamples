using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using NPOI.SS.Util;

namespace Demos.Demo
{
    /// <summary>
    /// NpoiDemo.xaml 的交互逻辑
    /// </summary>
    public partial class NpoiDemo : UserControl
    {
        public NpoiDemo()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 创建 Excel 并写入内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCreateExcel_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"Data\test1.xlsx";
            // 建议用 IWorkbook ISheet IRow ICell 统一管理避免不必要的错误
            // workbook --> sheet --> row --> cell
            // 创建空白工作簿
            IWorkbook workbook = new XSSFWorkbook();
            // 创建空白表格 指定 Sheet 名称
            ISheet sheet = workbook.CreateSheet("Sheet1");
            // 创建行 从 0 开始
            IRow row = sheet.CreateRow(0);
            // 在 row 中创建 cell 写内容
            ICell cell = row.CreateCell(0);
            cell.SetCellValue("行1列1");
            cell = row.CreateCell(2);
            cell.SetCellValue("行1列3");

            row = sheet.CreateRow(1);
            cell = row.CreateCell(1);
            cell.SetCellValue("行2列2");

            // 保存到本地
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
        }

        /// <summary>
        /// 新增内容到现有 Excel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAppendExcel_Click(object sender, RoutedEventArgs e)
        {
            // 打开 Excel 文件
            string filename = @"Data\test1.xlsx";
            using (FileStream file = new FileStream(filename, FileMode.Open))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
                // 获取指定 Sheet
                ISheet sheet = workbook.GetSheet("Sheet1");
                // 新增内容的行号
                int row_idx = sheet.LastRowNum + 1;
                // 添加行
                IRow row = sheet.CreateRow(row_idx);
                // 添加列
                ICell cell = row.CreateCell(0);
                // 赋值
                cell.SetCellValue("新增行");

                cell = row.CreateCell(2);
                // 赋值
                cell.SetCellValue(12.3);

                // 保存
                FileStream fs = new FileStream(filename, FileMode.Create);
                workbook.Write(fs);
                fs.Close();
                file.Close();
            }
        }

        /// <summary>
        /// 设置单元格格式：合并、居中、字体颜色、背景颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonCellStyleExcel_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"Data\test2.xlsx";
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet("Sheet1");

            // 创建行 从 0 开始
            IRow row = sheet.CreateRow(0);
            // 在 row 中创建 cell 写内容
            ICell cell = row.CreateCell(0);
            cell.SetCellValue("工程");
            // 居中
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            // 红色字体
            IFont font = workbook.CreateFont();
            font.Color = IndexedColors.Red.Index;
            style.SetFont(font);
            // 黄色填充
            style.FillForegroundColor = IndexedColors.Yellow.Index;
            style.FillPattern = FillPattern.SolidForeground;
            // 设置样式
            cell.CellStyle = style;
            // 合并单元格
            _ = sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 5));

            // 第二行 
            row = sheet.CreateRow(1);
            cell = row.CreateCell(0);
            cell.SetCellValue("项目1");
            // 居中 + 黄色填充
            style = workbook.CreateCellStyle();
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style.FillForegroundColor = IndexedColors.Yellow.Index;
            style.FillPattern = FillPattern.SolidForeground;
            // 设置样式
            cell.CellStyle = style;
            _ = sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 2));

            cell = row.CreateCell(3);
            cell.SetCellValue("项目2");
            cell.CellStyle = style;
            _ = sheet.AddMergedRegion(new CellRangeAddress(1, 1, 3, 5));

            // 第三行
            row = sheet.CreateRow(2);
            for (int i = 0; i < 6; i += 3)
            {
                cell = row.CreateCell(i);
                cell.SetCellValue("时间");

                cell = row.CreateCell(i + 1);
                cell.SetCellValue("子项1");

                cell = row.CreateCell(i + 2);
                cell.SetCellValue("子项2");
            }

            // 保存到本地
            using (FileStream file = new FileStream(filename, FileMode.Create))
            {
                workbook.Write(file);
                file.Close();
            }
        }

        /// <summary>
        /// 获取 Sheet 名称
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSheetNamesExcel_Click(object sender, RoutedEventArgs e)
        {
            string filename = @"Data\test.xlsx";
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
            string names = "";
            for (int i = 0; i < sheet_count; i++)
            {
                names += string.Format("第 {0} 个 Sheet：{1}\n", i + 1, workbook.GetSheetName(i));
            }
            _ = MessageBox.Show(names);
        }

        /// <summary>
        /// 读取指定 Sheet 的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonReadContentExcel_Click(object sender, RoutedEventArgs e)
        {
            // 读取 Excel 指定 sheet 内容
            // workbook --> sheet --> row --> cell
            string filename = @"Data\test.xlsx";
            using (FileStream file = new FileStream(filename, FileMode.Open))
            {
                IWorkbook workbook = new XSSFWorkbook(file);
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
                    content.Add(row_content);
                }
                _ = MessageBox.Show("读取完成");
            }
        }
    }
}
