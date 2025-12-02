//using DeepSigma.General.Extensions;
using DeepSigma.Charting;
using DeepSigma.Charting.Enum;
using DeepSigma.Charting.Interfaces;
using DeepSigma.Charting.Models;
using Microsoft.FSharp.Core;
using Plotly.NET;
using Plotly.NET.CSharp;
using Plotly.NET.LayoutObjects;


using CSharp = Plotly.NET.CSharp;

namespace DeepSigma.Charting.Plotl;

/// <summary>
/// Generates various types of charts using Plotly.NET
/// </summary>
public class ChartGenerator
{

    /// <summary>
    /// Creates and displays a chart based on the provided Chart2D configuration.
    /// </summary>
    /// <param name="chart_config"></param>
    public static void Create(Charting.Chart2D chart_config)
    {
        List<IChartSeriesAbstract> all_series = chart_config.Series;
        bool show_legend = chart_config.ShowLegend;
  
        List<GenericChart> traces = [];
        foreach (IChartSeriesAbstract series in all_series)
        {
            List<XYData> data = series.Data.GetAllDataPoints().Select(x => (XYData)x).ToList();
            GenericChart chart_result = GetChart(series);
            traces.Add(chart_result);
        }

        GenericChart chart = CSharp.Chart.Combine(traces);
        chart.WithTitle(chart_config.Title, new Font() { });
        chart.WithSize(1200, 600);
        chart.WithLayout(GetDefaultLayout());

        Axis2D? x_axis = chart_config.Axes.TryToGetAxis("X");
        Axis2D? y_axis = chart_config.Axes.TryToGetAxis("Y");
        CSharp.GenericChartExtensions.WithXAxisStyle<double, double, string>(chart, Title: Plotly.NET.Title.init(x_axis?.Title ?? "x-axis"))
        .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(y_axis?.Title ?? "y-axis", Side: StyleParam.Side.Left));

        // Save to file
        string path = Path.Combine(@"C:\Users\brend\Downloads\", $"Demo Chart - {DateTime.Now:yyyy-MM-dd}-{Guid.NewGuid()}.html");
        File.WriteAllText(path, GetHTMLChartWithBackground(chart));

        // Open in default browser
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        });
    }

    private static GenericChart GetChart(IChartSeriesAbstract chart)
    {
        return chart switch
        {
            ChartDataSeries => GetDataSeriesChart((ChartDataSeries)chart, true),
            ChartFinancialSeries => GetFinancialSeriesChart((ChartFinancialSeries)chart),
            ChartCategoricalSeries => GetCategoricalSeriesChart((ChartCategoricalSeries)chart),
            _ => throw new NotImplementedException()
        };

    }

    private static GenericChart GetDataSeriesChart(ChartDataSeries chart, bool IncludeMarkers = true)
    {
        List<XYData> data = chart.Data.GetAllDataPoints().Cast<XYData>().ToList();
        List<double> X = data.Select(x => x.X).ToList();
        List<double> Y = data.Select(x => x.Y).ToList();

        return chart.ChartType switch
        {
            DataSeriesChartType.Line => CSharp.Chart.Line<double, double, string>(X, Y, Name: chart.SeriesName, ShowMarkers: IncludeMarkers),
            DataSeriesChartType.Spline => CSharp.Chart.Spline<double, double, string>(X, Y, Name: chart.SeriesName, ShowMarkers: IncludeMarkers),
            DataSeriesChartType.Area => CSharp.Chart.Area<double, double, string>(X, Y, Name: chart.SeriesName, ShowMarkers: IncludeMarkers),
            DataSeriesChartType.Scatter => CSharp.Chart.Scatter<double, double, string>(X, Y, StyleParam.Mode.Markers, Name: chart.SeriesName),
            DataSeriesChartType.Histogram => CSharp.Chart.Column<double, double, string>(Y, Keys: new (X, true), Name: chart.SeriesName),
            _ => throw new NotImplementedException()
        };
    }

    private static GenericChart GetFinancialSeriesChart(ChartFinancialSeries chart)
    {
        List<CandleData> data = chart.Data.GetAllDataPoints().Cast<CandleData>().ToList();
        List<DateTime> DateTimes = data.Select(x => x.TimeStamp).ToList();
        List<decimal> Open = data.Select(x => x.Open).ToList();
        List<decimal> High = data.Select(x => x.High).ToList();
        List<decimal> Low = data.Select(x => x.Low).ToList();
        List<decimal> Close = data.Select(x => x.Close).ToList();
        List<decimal> TotalVolume = data.Select(x => x.TotalVolume).ToList();

        return chart.ChartType switch
        {
            FinancialSeriesChartType.CandleStick => CSharp.Chart.Candlestick<decimal, DateTime, string>(Open, High, Low, Close, DateTimes, Name: chart.SeriesName, ShowLegend: true),
            FinancialSeriesChartType.Volume => CSharp.Chart.Histogram<DateTime, decimal, string>(new(DateTimes, true), new(TotalVolume, true), Name: chart.SeriesName + "(Volume)", ShowLegend: true),
            _ => throw new NotImplementedException()
        };
    }

    private static GenericChart GetCategoricalSeriesChart(ChartCategoricalSeries chart)
    {
        List<CategoricalData> data = chart.Data.GetAllDataPoints().Cast<CategoricalData>().ToList();
        List<string> Categories = data.Select(x => x.Category).ToList();
        List<double> Values = data.Select(x => x.Value).ToList();

        return chart.ChartType switch
        {
            CategoricalSeriesChartType.Pie => CSharp.Chart.Pie<double, string, string>(Values, Labels: new(Categories, true), Name: chart.SeriesName),
            CategoricalSeriesChartType.Donut => CSharp.Chart.Doughnut<double, string, string>(Values, Labels: new(Categories, true), Name: chart.SeriesName),
            CategoricalSeriesChartType.Column => CSharp.Chart.Column<double, string, string>(Values, Keys: new(Categories, true), Name: chart.SeriesName),
            CategoricalSeriesChartType.Bar => CSharp.Chart.Bar<double, string, string>(Values, Keys: new(Categories, true), Name: chart.SeriesName),
            _ => throw new NotImplementedException()
        };
    }


    private static Layout GetDefaultLayout()
    {
        Layout layout = new();
        layout.SetValue("plot_bgcolor", "rgba(0,0,0,0.95)");
        layout.SetValue("bgcolor", "rgba(0,0,0,0.95)");
        layout.SetValue("paper_bgcolor", "rgba(0,0,0,0.95)");
        layout.SetValue("font", new { color = "white" });
        layout.SetValue("legend", new { font = new { color = "white" } });
        return layout;
    }

    private static string GetHTMLChartWithBackground(GenericChart chart)
    {
        string chart_html = GenericChart.toChartHTML(chart);
        string html = $@"
        <html>
          <head>
            <script src=""https://cdn.plot.ly/plotly-latest.min.js""></script>
            <style>
              body {{
                background-color: #111111;
                color: white;
                margin: 0;
                padding: 0;
              }}
            </style>
          </head>
          <body>
            {chart_html}
          </body>
        </html>";
        return html;
    }

    private static void CreateLine(double[] x, double[] y)
    {
        LinearAxis xAxis = new();
        xAxis.SetValue("title", "xAxis");
        xAxis.SetValue("font", new { color = "white" });
        xAxis.SetValue("showline", true);
        xAxis.SetValue("zeroline", false);
        xAxis.SetValue("gridcolor", "rgba(255,255,255,0.1)");

        LinearAxis yAxis = new();
        yAxis.SetValue("title", "yAxis");
        xAxis.SetValue("font", new { color = "white" });
        yAxis.SetValue("showline", true);
        yAxis.SetValue("zeroline", false);
        yAxis.SetValue("gridcolor", "rgba(255,255,255,0.1)");

        Layout layout = new();
        layout.SetValue("xaxis", xAxis);
        layout.SetValue("yaxis", yAxis);
        layout.SetValue("showlegend", true);
        layout.SetValue("title", "Demo Chart");
        layout.SetValue("width", 1000);
        layout.SetValue("height", 600);
        layout.SetValue("plot_bgcolor", "rgba(0,0,0,0.95)");
        layout.SetValue("bgcolor", "rgba(0,0,0,0.95)");
        layout.SetValue("font", new { color = "white" });
        layout.SetValue("legend", new { font = new { color = "white" } });

        // set grid color
        layout.SetValue("gridcolor", "rgba(255,255,255,0.1)");

        Trace trace = new("scatter");
        trace.SetValue("x", x);
        trace.SetValue("y", y);
        trace.SetValue("mode", "markers");
        trace.SetValue("name", "Hello from C#");

        Plotly.NET.CSharp.GenericChartExtensions.Show(
        GenericChart
            .ofTraceObject(true, trace)
            .WithLayout(layout));
    }
}
