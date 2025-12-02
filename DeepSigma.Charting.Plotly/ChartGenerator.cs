//using DeepSigma.General.Extensions;
using DeepSigma.Charting;
using DeepSigma.Charting.Interfaces;
using DeepSigma.Charting.Models;
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
    public static void LineChart(Charting.Chart2D chart_config)
    {
        List<IChartSeriesAbstract> all_series = chart_config.Series;
        Axis2D? x_axis = chart_config.Axes.TryToGetAxis("X");
        Axis2D? y_axis = chart_config.Axes.TryToGetAxis("Y");
        bool show_legend = chart_config.ShowLegend;

        chart_config.
        List<GenericChart> traces = [];
        foreach (IChartSeriesAbstract series in all_series)
        {
            
            List<XYData> data = series.Data.GetAllDataPoints().Select(x => (XYData)x).ToList();
            traces.Add(CSharp.Chart.Line<double, double, string>(
               x: data.Select(x => x.X),
               y: data.Select(x => x.Y),
               Name: series.SeriesName,
               ShowLegend: show_legend,
               ShowMarkers: true
           ));
        }

        GenericChart chart = CSharp.Chart.Combine(traces);
        chart.WithTitle(chart_config.Title, new Font() { });
        chart.WithSize(1200, 600);
        chart.WithLayout(GetDefaultLayout());

        CSharp.GenericChartExtensions.WithXAxisStyle<double, double, string>(chart, Title: Plotly.NET.Title.init(x_axis?.Title ?? "x-axis"))
        .WithYAxisStyle<double, double, string>(Title: Plotly.NET.Title.init(y_axis?.Title ?? "y-axis", Side: StyleParam.Side.Left));

        // Save to file
        string path = Path.Combine(@"C:\Users\brend\Downloads\", $"chart-{DateTime.Now:yyyy-MM-dd}-{Guid.NewGuid()}.html");
        File.WriteAllText(path, GetHTMLChartWithBackground(chart));

        // Open in default browser
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = path,
            UseShellExecute = true
        });
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
