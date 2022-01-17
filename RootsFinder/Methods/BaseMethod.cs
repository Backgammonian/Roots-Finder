using System;
using System.Collections.Generic;
using OxyPlot;

namespace RootsFinder.Methods
{
    public class BaseMethod
    {
        protected readonly FunctionExpression _f;
        protected List<DataPoint> _lastCalculatedRoots;

        public BaseMethod(FunctionExpression functionExpression)
        {
            _f = functionExpression;
            _lastCalculatedRoots = new List<DataPoint>();
        }

        public List<DataPoint> LastCalculatedRoots
        { 
            get
            {
                return _lastCalculatedRoots;
            }
            protected set
            {
                _lastCalculatedRoots = value;
            }
        }

        public virtual void CalculateRoots(double a, double b, double eps, Func<bool> breakConditionFuncion)
        {
            var result = new List<DataPoint>();
            if (!_f.IsSyntaxCorrect)
            {
                LastCalculatedRoots.Clear();
                return;
            }

            if (a > b)
            {
                var t = a;
                a = b;
                b = t;
            }

            for (var i = a; i < b; i += 10.0 * eps)
            {
                if (breakConditionFuncion())
                {
                    break;
                }

                if (_f.FunctionValue(i) * _f.FunctionValue(i + 10.0 * eps) < 0.0)
                {
                    var a_temp = i;
                    var b_temp = i + 10.0 * eps;
                    var X = (a_temp + b_temp) / 2.0;

                    result.Add(new DataPoint(X, 0));
                }
            }

            LastCalculatedRoots.Clear();
            LastCalculatedRoots.AddRange(result);
        }
    }
}
