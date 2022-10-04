using OfficeOpenXml;
using OfficeOpenXml.Drawing.Chart;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Demos.Demo
{
    /// <summary>
    /// EpplusDemo.xaml 的交互逻辑
    /// </summary>
    public partial class EpplusDemo : UserControl
    {
        public EpplusDemo()
        {
            InitializeComponent();
        }

        private void ButtonCreateExcel_Click(object sender, RoutedEventArgs e)
        {
            string file_path = @"Data\test3.xlsx";

            FileInfo newFile = new FileInfo(file_path);
            if (newFile.Exists)
            {
                newFile.Delete();
                newFile = new FileInfo(file_path);
            }

            // Epplus: Please set the ExcelPackage.LicenseContext property
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Results");
                worksheet.Cells[1, 1].Value = "测试";
                worksheet.Cells[1, 1, 1, 2].Merge = true;      // 合并
                worksheet.Cells[1, 1, 1, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;   // 居中
                for (int i = 0; i < 10; i++)
                {
                    worksheet.Cells[3 + i, 1].Value = i + 1;
                    worksheet.Cells[3 + i, 2].Value = Math.Sin(i / 10.0 * Math.PI);
                }
                // 网格线
                worksheet.View.ShowGridLines = true;
                // 绘制图表
                // 曲线                                       
                ExcelChart linechart = worksheet.Drawings.AddChart("lineChart", eChartType.XYScatterSmoothNoMarkers);
                linechart.SetPosition(3, 10, 2, 40);
                linechart.SetSize(700, 500);
                linechart.Legend.Remove();    // 删除图例
                ExcelChartSerie ser = linechart.Series.Add(worksheet.Cells[3, 2, 3 + 10, 2], worksheet.Cells[3, 1, 3 + 10, 1]);
                ser.Header = "Distribution";    // series名称
                linechart.XAxis.Title.Text = "XAxis";
                linechart.XAxis.LogBase = 10;
                linechart.XAxis.MajorTickMark = eAxisTickMark.Out;
                linechart.XAxis.MinorTickMark = eAxisTickMark.In;
                linechart.XAxis.MinorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.LongDash;
                linechart.XAxis.TickLabelPosition = eTickLabelPosition.Low;
                linechart.YAxis.Title.Text = "YAxis";
                linechart.YAxis.MajorTickMark = eAxisTickMark.Out;
                linechart.YAxis.MinorTickMark = eAxisTickMark.In;
                linechart.YAxis.MajorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.LongDash;
                linechart.YAxis.CrossesAt = 0.1;
                linechart.YAxis.MinValue = 0;
                linechart.Title.Text = "LineChart Example";
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // 柱状图
                ExcelChart barchart = worksheet.Drawings.AddChart("barChart", eChartType.ColumnClustered);
                barchart.SetPosition(3, 5, 15, 40);
                barchart.SetSize(700, 500);
                barchart.Legend.Remove();    // 删除图例
                barchart.Series.Add(worksheet.Cells[3, 2, 3 + 10, 2], worksheet.Cells[3, 1, 3 + 10, 1]);
                barchart.XAxis.Title.Text = "XAxis";
                barchart.XAxis.MajorTickMark = eAxisTickMark.Out;
                barchart.XAxis.MinorTickMark = eAxisTickMark.None;
                barchart.YAxis.Title.Text = "YAxis";
                barchart.YAxis.MajorTickMark = eAxisTickMark.Out;
                barchart.YAxis.MinorTickMark = eAxisTickMark.None;
                barchart.YAxis.MinValue = 0;
                barchart.YAxis.MajorGridlines.LineStyle = OfficeOpenXml.Drawing.eLineStyle.LongDash;
                barchart.Title.Text = "BarChart Example";
                // 保存
                package.Save();
            }
        }
    }
}