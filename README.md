# DeepSigma.Charting.Plotly

A C# adapter library that renders `DeepSigma.Charting` models with Plotly.NET.

This package turns `Chart2D` configurations into interactive Plotly-based HTML charts. It currently supports common XY series, financial series, and categorical series, then combines them into a single chart with a dark themed layout.

## Features

- Renders `DeepSigma.Charting.Chart2D` objects with Plotly.NET
- Supports multiple series in one chart
- Applies a built-in dark theme layout
- Sets axis titles from the `Chart2D` axis configuration
- Writes charts to an HTML file and opens them in the default browser

## Supported chart types

### Data series
- Line
- Spline
- Area
- Scatter
- Histogram

### Financial series
- Candlestick
- Volume

### Categorical series
- Pie
- Donut
- Column
- Bar

## Installation

Add the package reference to your project:

```xml
<PackageReference Include="DeepSigma.Charting.Plotly" Version="1.1.0.0" />
```

This library currently targets:

- `.NET 10.0`

Key dependencies used by the project:

- `DeepSigma.Charting` `1.2.0`
- `Plotly.NET` `5.1.0`
- `Plotly.NET.CSharp` `0.13.0`

## Usage

The main entry point is:

```csharp
ChartGenerator.Create(chart);
```

Example:

```csharp
using DeepSigma.Charting.Plotl;
using DeepSigma.Charting.Models;
using DeepSigma.Charting.Enum;

Charting.Chart2D chart = new();
chart.Title = "Sample Chart";

Axis2D xAxis = new()
{
    Key = AxisDimension.X,
    Title = "Time",
    AxisPosition = Chart2DAxisPosition.Bottom
};

Axis2D yAxis = new()
{
    Key = AxisDimension.Y,
    Title = "Value",
    AxisPosition = Chart2DAxisPosition.Left
};

chart.Axes.AddAxis(xAxis);
chart.Axes.AddAxis(yAxis);

DataSeries series1 = new();
series1.Add(new XYData(0, 10));
series1.Add(new XYData(1, 15));
series1.Add(new XYData(2, 12));

ChartSeriesAbstract lineSeries = new ChartDataSeries()
{
    ChartType = DataSeriesChartType.Line,
    SeriesName = "Series A",
    Data = series1
};

DataSeries series2 = new();
series2.Add(new XYData(0, 8));
series2.Add(new XYData(1, 11));
series2.Add(new XYData(2, 17));

ChartSeriesAbstract splineSeries = new ChartDataSeries()
{
    ChartType = DataSeriesChartType.Spline,
    SeriesName = "Series B",
    Data = series2
};

chart.Series.Add(lineSeries);
chart.Series.Add(splineSeries);

ChartGenerator.Create(chart);
```

## Current behavior

`ChartGenerator.Create(...)` currently:

1. Converts each series into a Plotly chart
2. Combines all traces into one Plotly figure
3. Applies a fixed size of `1200 x 600`
4. Applies a dark background and white text styling
5. Writes the output to an HTML file
6. Opens that HTML file with the system default browser

## Testing

The repository includes an xUnit test project that constructs a `Chart2D` instance with two series and calls `ChartGenerator.Create(...)` as a smoke test.

Run tests with:

```bash
dotnet test
```

## Project structure

```text
DeepSigma.Charting.Plotly/
├── DeepSigma.Charting.Plotly/
│   ├── Assets/
│   ├── ChartGenerator.cs
│   └── DeepSigma.Charting.Plotly.csproj
├── DeepSigma.Charting.Plotly.Test/
│   ├── DeepSigma.Charting.Plotly.Test.csproj
│   └── Tests.cs
├── LICENSE
└── README.md
```

## License

MIT
