using System.Collections.Generic;
using OxyPlot;

namespace RootsFinder
{
    public class FunctionGraphConfiguration
    {
        public double Start { get; set; }
        public double End { get; set; }
        public double Top { get; set; }
        public double Bottom { get; set; }
        public double RootsStart { get; set; }
        public double RootsEnd { get; set; }
        private double _precision;
        public double Precision
        { 
            get { return _precision; }
            set
            {
                if (value > 0)
                {
                    _precision = value;
                }
            }
        }
        public bool CanBeDrawn { get; set; }
        public List<DataPoint> Roots { get; set; }
        public OxyColor GraphColor { get; set; }
        public OxyColor RootsColor { get; set; }
        public MarkerType RootsMarkerType { get; set; }
        public double RootsMarkerHeight { get; set; }
        public OxyColor RestrictionPointsColor { get; set; }
        public MarkerType RestrictionPointsMarkerType { get; set; }
        public double RestrictionPointsMarkerHeight { get; set; }

        public FunctionGraphConfiguration()
        {
            Precision = 0.01;
            SetDefaultColorsAndMarkers();
        }

        private void SetDefaultColorsAndMarkers()
        {
            GraphColor = OxyColors.Black;
            RootsColor = OxyColors.Black;
            RootsMarkerType = MarkerType.Circle;
            RootsMarkerHeight = 2;
            RestrictionPointsColor = OxyColors.DarkGray;
            RestrictionPointsMarkerType = MarkerType.Square;
            RestrictionPointsMarkerHeight = 6;
        }

        public void MoveHorizontally(double newStart, double newEnd)
        {
            Start = newStart;
            End = newEnd;
        }

        public void MoveVertically(double newTop, double newBottom)
        {
            Top = newTop;
            Bottom = newBottom;
        }
    }
}
