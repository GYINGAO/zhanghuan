using DevExpress.Utils;
using DevExpress.XtraCharts;
using qqqq.models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace qqqq.utils {
    public class Draw {
        /// <summary>
        /// 向chartcontrol中添加一条曲线，绘制横向偏差图
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="chart"></param>
        /// <param name="seriesName"></param>
        public static void AddOneSeries(List<Deviation> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.Line);
            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {
                sp = new SeriesPoint(dtSource[i].designMile, dtSource[i].fym);
                series.Points.Add(sp);
            }
            chart.Series.Add(series);
        }

        /// <summary>
        /// 向chartcontrol中添加一条曲线，绘制垂向偏差图
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="chart"></param>
        /// <param name="seriesName"></param>
        public static void AddOneSeries_chuixiang(List<Deviation> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.Line);
            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {
                sp = new SeriesPoint(dtSource[i].designMile, Math.Round(dtSource[i].fhm, 3));
                series.Points.Add(sp);
            }
            chart.Series.Add(series);
            //系列名在图表上的横向位置
            chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            //系列名在图表上的纵向位置,指定在底部
            chart.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            chart.Legend.Direction = LegendDirection.LeftToRight;
            //打开复选框，可以任意选择系列
            chart.Legend.UseCheckBoxes = true;
            XYDiagram xyDiagram = (XYDiagram)chart.Diagram;
            xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisX.Title.Text = "里程(m)";
            xyDiagram.AxisX.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisX.Title.Alignment = StringAlignment.Center;
            xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisY.Title.Text = "垂向偏差值(mm)";
            xyDiagram.AxisY.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisY.Title.Alignment = StringAlignment.Center;
            xyDiagram.EnableAxisXScrolling = true;
            xyDiagram.EnableAxisYScrolling = true;
            xyDiagram.EnableAxisXZooming = true;//仅X轴进行缩放
            //网格显示和设置
            xyDiagram.AxisY.GridLines.Visible = true;
            xyDiagram.AxisY.GridLines.MinorVisible = true;
            //刻度线显示和设置
            xyDiagram.AxisY.Tickmarks.Visible = true;
            xyDiagram.AxisY.Tickmarks.MinorVisible = true;
            //用自动刻度分隔线
            xyDiagram.AxisY.AutoScaleBreaks.Enabled = true;
            xyDiagram.AxisY.AutoScaleBreaks.MaxCount = 20;
        }

        /// <summary>
        /// 设置横向偏差图chart的图例属性和坐标轴属性
        /// </summary>
        /// <param name="targetChart"></param>
        public static void SetLegendAndXY(ChartControl targetChart) {
            //系列名在图表上的横向位置
            targetChart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            //系列名在图表上的纵向位置,指定在底部
            targetChart.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            targetChart.Legend.Direction = LegendDirection.LeftToRight;
            //打开复选框，可以任意选择系列
            targetChart.Legend.UseCheckBoxes = true;
            XYDiagram xyDiagram = (XYDiagram)targetChart.Diagram;
            xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisX.Title.Text = "里程(m)";
            xyDiagram.AxisX.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisX.Title.Alignment = StringAlignment.Center;
            xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisY.Title.Text = "横向偏差值(mm)";
            xyDiagram.AxisY.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisY.Title.Alignment = StringAlignment.Center;
            xyDiagram.EnableAxisXScrolling = true;
            xyDiagram.EnableAxisYScrolling = true;
            xyDiagram.EnableAxisXZooming = true;//仅X轴进行缩放
            //网格显示和设置
            xyDiagram.AxisY.GridLines.Visible = true;
            xyDiagram.AxisY.GridLines.MinorVisible = true;
            //刻度线显示和设置
            xyDiagram.AxisY.Tickmarks.Visible = true;
            xyDiagram.AxisY.Tickmarks.MinorVisible = true;
            //用自动刻度分隔线
            xyDiagram.AxisY.AutoScaleBreaks.Enabled = true;
            xyDiagram.AxisY.AutoScaleBreaks.MaxCount = 20;

            //xyDiagram.EnableAxisYZooming = true;//X、Y轴同时缩放
        }

        /// <summary>
        /// 向chartcontrol中添加一条曲线，绘制垂向偏差图
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="chart"></param>
        /// <param name="seriesName"></param>
        public static void AddAnotherSeries(List<Deviation> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.Line);

            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {
                sp = new SeriesPoint(dtSource[i].designMile, dtSource[i].fhm);
                series.Points.Add(sp);
            }
            chart.Series.Add(series);
        }

        /// <summary>
        /// 设置chart的图例属性和坐标轴属性（垂向偏差）
        /// </summary>
        /// <param name="targetChart"></param>
        public static void SetHLegendAndXY(ChartControl targetChart) {
            //系列名在图表上的横向位置
            targetChart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            //系列名在图表上的纵向位置,指定在底部
            targetChart.Legend.AlignmentVertical = LegendAlignmentVertical.TopOutside;
            targetChart.Legend.Direction = LegendDirection.LeftToRight;
            //打开复选框，可以任意选择系列
            targetChart.Legend.UseCheckBoxes = true;
            XYDiagram xyDiagram = (XYDiagram)targetChart.Diagram;
            xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisX.Title.Text = "里程(m)";
            xyDiagram.AxisX.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisX.Title.Alignment = StringAlignment.Center;
            xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisY.Title.Text = "垂向偏差值(mm)";
            xyDiagram.AxisY.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisY.Title.Alignment = StringAlignment.Center;
            xyDiagram.EnableAxisXScrolling = true;
            xyDiagram.EnableAxisYScrolling = true;
            xyDiagram.EnableAxisXZooming = true;//仅X轴进行缩放
            //网格显示和设置
            xyDiagram.AxisY.GridLines.Visible = true;
            xyDiagram.AxisY.GridLines.MinorVisible = true;
            //刻度线显示和设置
            xyDiagram.AxisY.Tickmarks.Visible = true;
            xyDiagram.AxisY.Tickmarks.MinorVisible = true;
            //用自动刻度分隔线
            xyDiagram.AxisY.AutoScaleBreaks.Enabled = true;
            xyDiagram.AxisY.AutoScaleBreaks.MaxCount = 20;

            //xyDiagram.EnableAxisYZooming = true;//X、Y轴同时缩放
        }



        /// <summary>
        /// 向chartcontrol中添加一条曲线，绘制设计平曲线
        /// </summary>
        /// <param name="dtSource"></param>
        /// <param name="chart"></param>
        /// <param name="seriesName"></param>
        public static void AddHorizontalSeries(List<Horizontal> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.ScatterLine);
            // 下一句为定义线上点标识，有他才能设置线上点
            ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //设置线上点标识为圆形
            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ////设置点标识为红色
            //((LineSeriesView)series.View).LineMarkerOptions.Color = Color.Red;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 4;
            //设置线型类型为虚线
            //((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Dash;

            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {

                sp = new SeriesPoint(dtSource[i].Y, dtSource[i].X) { Tag = new { pType = dtSource[i].pType, } };
                //设置主点颜色
                series.Points.Add(sp);

            }
            chart.Series.Add(series);
            series.ToolTipPointPattern = "主点类型：{pType}\r\nX：{A}\r\nY：{V} ";

        }

        public static void AddJDSeries(List<JD> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.ScatterLine);
            // 下一句为定义线上点标识，有他才能设置线上点
            ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //设置线上点标识为圆形
            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ////设置点标识为红色
            //((LineSeriesView)series.View).LineMarkerOptions.Color = Color.Red;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 6;
            //设置线型类型为虚线
            //((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Dash;

            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {

                sp = new SeriesPoint(dtSource[i].Y, dtSource[i].X) { Tag = new { pName = dtSource[i].pName, } };
                //设置主点颜色
                series.Points.Add(sp);

            }
            chart.Series.Add(series);
            series.ToolTipPointPattern = "交点名：{pName}\r\nX：{A}\r\nY：{V} ";

        }

        public static void AddBPDSeries(List<BPD> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.ScatterLine);
            // 下一句为定义线上点标识，有他才能设置线上点
            ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //设置线上点标识为圆形
            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ////设置点标识为红色
            //((LineSeriesView)series.View).LineMarkerOptions.Color = Color.Red;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 6;
            //设置线型类型为虚线
            //((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Dash;

            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {

                sp = new SeriesPoint(dtSource[i].mileage, dtSource[i].H);
                //设置主点颜色
                series.Points.Add(sp);

            }
            chart.Series.Add(series);
            series.ToolTipPointPattern = "变坡点里程：{A}\r\n高程：{V} ";

        }

        public static void AddSQXSeries(List<Vertical> dtSource, ChartControl chart, string seriesName) {
            Series series = new Series(seriesName, ViewType.ScatterLine);
            // 下一句为定义线上点标识，有他才能设置线上点
            ((LineSeriesView)series.View).MarkerVisibility = DevExpress.Utils.DefaultBoolean.True;
            //设置线上点标识为圆形
            ((LineSeriesView)series.View).LineMarkerOptions.Kind = MarkerKind.Circle;
            ////设置点标识为红色
            //((LineSeriesView)series.View).LineMarkerOptions.Color = Color.Red;
            ((LineSeriesView)series.View).LineMarkerOptions.Size = 6;
            //设置线型类型为虚线
            //((LineSeriesView)series.View).LineStyle.DashStyle = DashStyle.Dash;

            SeriesPoint sp;
            for (int i = 0; i < dtSource.Count; i++) {

                sp = new SeriesPoint(dtSource[i].mileage, dtSource[i].H) { Tag = new { pType = dtSource[i].pType, } };
                //设置主点颜色
                series.Points.Add(sp);

            }
            chart.Series.Add(series);
            series.ToolTipPointPattern = "类型：{pType}\r\n里程：{A}\r\n高程：{V} ";

            // Disable a crosshair cursor.
            chart.CrosshairEnabled = DefaultBoolean.False;

            // Enable chart tooltips. 
            chart.ToolTipEnabled = DefaultBoolean.True;

            // Show a tooltip's beak.
            ToolTipController controller = new ToolTipController();
            chart.ToolTipController = controller;
            controller.ShowBeak = true;
            controller.KeepWhileHovered = true;
            controller.AutoPopDelay = 20 * 1000;

            chart.ToolTipOptions.ShowForPoints = true;

            chart.ToolTipOptions.ShowForSeries = false;

            // Change the default tooltip mouse position to relative position.
            ToolTipRelativePosition relativePosition = new ToolTipRelativePosition();
            chart.ToolTipOptions.ToolTipPosition = relativePosition;

            // Specify the tooltip relative position offsets.  
            relativePosition.OffsetX = 2;
            relativePosition.OffsetY = 2;


            //系列名在图表上的横向位置
            chart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            //系列名在图表上的纵向位置,指定在底部
            chart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
            chart.Legend.Direction = LegendDirection.LeftToRight;

            //打开复选框，可以任意选择系列
            chart.Legend.UseCheckBoxes = true;

            XYDiagram xyDiagram = (XYDiagram)chart.Diagram;
            xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisX.Title.Text = "里程(m)";
            xyDiagram.AxisX.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisX.Title.Alignment = StringAlignment.Center;

            xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisY.Title.Text = "高程(m)";
            xyDiagram.AxisY.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisY.Title.Alignment = StringAlignment.Center;
            xyDiagram.EnableAxisXScrolling = true;
            xyDiagram.EnableAxisYScrolling = true;
            xyDiagram.EnableAxisXZooming = true;//仅X轴进行缩放
            xyDiagram.EnableAxisYZooming = true;//仅Y轴进行缩放

            // 网格显示和设置
            xyDiagram.AxisY.GridLines.Visible = true;
            xyDiagram.AxisY.GridLines.MinorVisible = true;
            // 刻度线显示和设置
            xyDiagram.AxisY.Tickmarks.Visible = true;
            xyDiagram.AxisY.Tickmarks.MinorVisible = true;

        }





        /// <summary>
        /// 设置chart的图例属性和坐标轴属性（平曲线）
        /// </summary>
        /// <param name="targetChart"></param>
        public static void SetHorizontalLegendAndXY(ChartControl targetChart) {
            // Disable a crosshair cursor.
            targetChart.CrosshairEnabled = DefaultBoolean.False;

            // Enable chart tooltips. 
            targetChart.ToolTipEnabled = DefaultBoolean.True;

            // Show a tooltip's beak.
            ToolTipController controller = new ToolTipController();
            targetChart.ToolTipController = controller;
            controller.ShowBeak = true;
            controller.KeepWhileHovered = true;
            controller.AutoPopDelay = 20 * 1000;

            targetChart.ToolTipOptions.ShowForPoints = true;

            targetChart.ToolTipOptions.ShowForSeries = false;

            // Change the default tooltip mouse position to relative position.
            ToolTipRelativePosition relativePosition = new ToolTipRelativePosition();
            targetChart.ToolTipOptions.ToolTipPosition = relativePosition;

            // Specify the tooltip relative position offsets.  
            relativePosition.OffsetX = 2;
            relativePosition.OffsetY = 2;


            //系列名在图表上的横向位置
            targetChart.Legend.AlignmentHorizontal = LegendAlignmentHorizontal.Left;
            //系列名在图表上的纵向位置,指定在底部
            targetChart.Legend.AlignmentVertical = LegendAlignmentVertical.Top;
            targetChart.Legend.Direction = LegendDirection.LeftToRight;

            //打开复选框，可以任意选择系列
            targetChart.Legend.UseCheckBoxes = true;

            XYDiagram xyDiagram = (XYDiagram)targetChart.Diagram;
            xyDiagram.AxisX.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisX.Title.Text = "Y坐标(m)";
            xyDiagram.AxisX.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisX.Title.Alignment = StringAlignment.Center;

            xyDiagram.AxisY.Title.Visibility = DefaultBoolean.True;
            xyDiagram.AxisY.Title.Text = "X坐标(m)";
            xyDiagram.AxisY.Title.Font = new Font("Thomas", 9, FontStyle.Bold);
            xyDiagram.AxisY.Title.Alignment = StringAlignment.Center;
            xyDiagram.EnableAxisXScrolling = true;
            xyDiagram.EnableAxisYScrolling = true;
            xyDiagram.EnableAxisXZooming = true;//仅X轴进行缩放
            xyDiagram.EnableAxisYZooming = true;//仅Y轴进行缩放

            // 网格显示和设置
            xyDiagram.AxisY.GridLines.Visible = true;
            xyDiagram.AxisY.GridLines.MinorVisible = true;
            // 刻度线显示和设置
            xyDiagram.AxisY.Tickmarks.Visible = true;
            xyDiagram.AxisY.Tickmarks.MinorVisible = true;

            // 设置坐标轴范围
            xyDiagram.AxisX.WholeRange.Auto = false;
            xyDiagram.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram.AxisX.WholeRange.SetMinMaxValues(455000, 490000);
            xyDiagram.AxisX.WholeRange.SideMarginsValue = 0;
            xyDiagram.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram.AxisX.VisualRange.Auto = false;
            xyDiagram.AxisX.VisualRange.SetMinMaxValues(0, 490000 / 4);
            //Y
            xyDiagram.AxisY.WholeRange.Auto = false;
            xyDiagram.AxisY.WholeRange.AutoSideMargins = false;
            xyDiagram.AxisY.WholeRange.SetMinMaxValues(2706000, 2721000);
            xyDiagram.AxisY.WholeRange.SideMarginsValue = 0;
            xyDiagram.AxisY.WholeRange.AutoSideMargins = false;
            xyDiagram.AxisY.VisualRange.Auto = false;
            xyDiagram.AxisY.VisualRange.SetMinMaxValues(0, 2721000 / 4);

        }
    }
}
