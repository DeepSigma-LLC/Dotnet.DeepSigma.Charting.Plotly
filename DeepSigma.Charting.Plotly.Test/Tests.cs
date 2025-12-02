using Xunit;
using DeepSigma.Charting.Plotl;
using DeepSigma.Charting;
using DeepSigma.Charting.Models;

namespace DeepSigma.Charting.Plotly.Test;

public class Tests
{
    [Fact] public void Test2()
    {
        Charting.Chart2D chart = new();
        chart.Title = $"Test Chart-{DateTime.Now:yyyy-MM-dd}";

        Axis2D x_axis = new() { Key = "X", Title = "Time", AxisPosition = Enum.Chart2DAxisPosition.Bottom };
        Axis2D y_axis = new() { Key = "Y", Title = "Value", AxisPosition = Enum.Chart2DAxisPosition.Left }; 
        chart.Axes.AddAxis(x_axis);
        chart.Axes.AddAxis(y_axis);

        DataSeries<IDataModel> data = new();
        data.Add(new Models.XYData(0, 1399));
        data.Add(new Models.XYData(1, 1405));
        data.Add(new Models.XYData(2, 1399));
        data.Add(new Models.XYData(3, 1500));
        data.Add(new Models.XYData(4, 1434));

        ChartSeriesAbstract chart_series = new ChartDataSeries() { ChartType = Enum.DataChartType.Line, SeriesName = "SPX", Data = data };
        chart.Series.Add(chart_series);

        DataSeries<IDataModel> data2 = new();
        data2.Add(new Models.XYData(0, 540));
        data2.Add(new Models.XYData(1, 600));
        data2.Add(new Models.XYData(2, 549));
        data2.Add(new Models.XYData(3, 567));
        data2.Add(new Models.XYData(4, 590));

        ChartSeriesAbstract chart_series2 = new ChartDataSeries() { ChartType = Enum.DataChartType.Line, SeriesName = "RTY", Data = data2 };
        chart.Series.Add(chart_series2);

        ChartGenerator.LineChart(chart);
        Assert.True(true);
    }
}
