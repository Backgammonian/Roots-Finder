using System;
using System.Collections.Generic;
using OxyPlot;
using OxyPlot.Series;

namespace RootsFinder
{
    public class FunctionGraphBuilder
    {
        private readonly FunctionExpression _f;
        private readonly double _dx;

        public FunctionGraphBuilder(FunctionExpression functionExpression)
        {
            _f = functionExpression;
            _dx = 0.01;
        }

        private List<LineSeries> GetRootsPoints(List<DataPoint> roots, OxyColor color, MarkerType markerType, double markerHeight)
        {
            var output = new List<LineSeries>();

            foreach (var value in roots)
            {
                var line = new LineSeries();
                line.Color = color;
                line.Points.Add(new DataPoint(value.X, value.Y - markerHeight / 2.0));
                line.Points.Add(new DataPoint(value.X, value.Y + markerHeight / 2.0));

                output.Add(line);

                var point = new LineSeries();
                point.Color = color;
                point.MarkerType = markerType;
                point.Points.Add(new DataPoint(value.X, value.Y));

                output.Add(point);
            }

            return output;
        }

        private List<LineSeries> GetRestrictionPoints(double start, double end, OxyColor color, MarkerType markerType, double markerHeight)
        {
            var output = new List<LineSeries>();

            var lineStart = new LineSeries();
            lineStart.Color = color;
            lineStart.Points.Add(new DataPoint(start, -markerHeight / 2.0));
            lineStart.Points.Add(new DataPoint(start, markerHeight / 2.0));

            output.Add(lineStart);

            var lineEnd = new LineSeries();
            lineEnd.Color = color;
            lineEnd.Points.Add(new DataPoint(end, -markerHeight / 2.0));
            lineEnd.Points.Add(new DataPoint(end, markerHeight / 2.0));

            output.Add(lineEnd);

            var pointStart = new LineSeries();
            pointStart.Color = color;
            pointStart.MarkerType = markerType;
            pointStart.Points.Add(new DataPoint(start, 0));

            output.Add(pointStart);

            var pointEnd = new LineSeries();
            pointEnd.Color = color;
            pointEnd.MarkerType = markerType;
            pointEnd.Points.Add(new DataPoint(end, 0));

            output.Add(pointEnd);

            return output;
        }

        private List<LineSeries> CreateFunctionGraph(double start, double end, double precision, OxyColor color)
        {
            var result = new List<LineSeries>();
            var points = new List<DataPoint>();

            for (var x = start; x <= end; x += precision)
            {
                var validPoint = false;
                var y = _f.FunctionValue(x);

                if (points.Count == 0)
                {
                    validPoint = true;
                }
                else
                {
                    var dy = y - points[^1].Y;

                    if (Math.Abs(dy / precision) < 1000)
                    {
                        validPoint = true;
                    }
                }

                if (validPoint)
                {
                    points.Add(new DataPoint(x, y));
                }
                else
                {
                    var graphSegment = new LineSeries();
                    graphSegment.Color = color;
                    graphSegment.Points.AddRange(points);
                    result.Add(graphSegment);

                    points.Clear();
                }
            }

            var lastGraphSegment = new LineSeries();
            lastGraphSegment.Color = color;
            lastGraphSegment.Points.AddRange(points);
            result.Add(lastGraphSegment);

            return result;
        }

        public List<LineSeries> GetFunctionGraph(FunctionGraphConfiguration config)
        {
            var result = new List<LineSeries>();

            if (!(config.CanBeDrawn && _f.IsSyntaxCorrect && _f.CanCalculate))
            {
                return result;
            }

            var xAxis = new LineSeries();
            xAxis.Color = OxyColors.Gray;
            xAxis.Points.Add(new DataPoint(config.End * 1.5, 0));
            xAxis.Points.Add(new DataPoint(config.Start * 1.5, 0));

            var yAxis = new LineSeries();
            yAxis.Color = OxyColors.Gray;
            yAxis.Points.Add(new DataPoint(0, config.Top * 1.5));
            yAxis.Points.Add(new DataPoint(0, config.Bottom * 1.5));

            var precision = Math.Abs(config.Start - config.End) > 1000 ? _dx * 10.0 : _dx;
            var functionGraph = CreateFunctionGraph(config.Start, config.End, precision, config.GraphColor);

            var rootsPoints = GetRootsPoints(config.Roots, config.RootsColor, config.RootsMarkerType, config.RootsMarkerHeight);
            var restrictionPoints = GetRestrictionPoints(config.RootsStart, config.RootsEnd, config.RestrictionPointsColor, config.RestrictionPointsMarkerType, config.RestrictionPointsMarkerHeight);

            result.Add(xAxis);
            result.Add(yAxis);
            result.AddRange(functionGraph);
            result.AddRange(rootsPoints);
            result.AddRange(restrictionPoints);

            return result;
        }
    }
}
