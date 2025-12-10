using Xunit;
using DeepSigma.Charting.Plotl;
using DeepSigma.Charting.Models;
using DeepSigma.Charting.Enum;

namespace DeepSigma.Charting.Plotly.Test;

public class Tests
{
    [Fact] public void Test2()
    {
        Charting.Chart2D chart = new();
        chart.Title = $"Test Chart-{DateTime.Now:yyyy-MM-dd}";

        Axis2D x_axis = new() { Key = AxisDimension.X, Title = "Time", AxisPosition = Enum.Chart2DAxisPosition.Bottom };
        Axis2D y_axis = new() { Key = AxisDimension.Y, Title = "Value", AxisPosition = Enum.Chart2DAxisPosition.Left }; 
        chart.Axes.AddAxis(x_axis);
        chart.Axes.AddAxis(y_axis);

        DataSeries<IDataModel> data = new();
        data.Add(new Models.XYData(0, 1399));
        data.Add(new Models.XYData(1, 1405));
        data.Add(new Models.XYData(2, 1399));
        data.Add(new Models.XYData(3, 1500));
        data.Add(new Models.XYData(4, 1434));

        ChartSeriesAbstract chart_series = new ChartDataSeries() { ChartType = Enum.DataSeriesChartType.Line, SeriesName = "SPX", Data = data };
        chart.Series.Add(chart_series);

        DataSeries<IDataModel> data2 = new();
        data2.Add(new Models.XYData(0, 1540));
        data2.Add(new Models.XYData(1, 1600));
        data2.Add(new Models.XYData(2, 1549));
        data2.Add(new Models.XYData(3, 1567));
        data2.Add(new Models.XYData(4, 1590));

        ChartSeriesAbstract chart_series2 = new ChartDataSeries() { ChartType = Enum.DataSeriesChartType.Spline, SeriesName = "RTY", Data = data2 };
        chart.Series.Add(chart_series2);

        ChartGenerator.Create(chart);
        Assert.True(true);
    }
}
