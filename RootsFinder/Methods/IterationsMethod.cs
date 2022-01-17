using System;
using System.Collections.Generic;
using OxyPlot;

namespace RootsFinder.Methods
{
    public class IterationsMethod : BaseMethod
    {
        public IterationsMethod(FunctionExpression functionExpression) : base(functionExpression)
        {
        }

        public override void CalculateRoots(double a, double b, double eps, Func<bool> breakConditionFuncion)
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
                    var X = i;
                    var X0 = 0.0;
                    var L = 1.0 / _f.FunctionDerivativeValue(X);
                    var n = 0;

                    do
                    {
                        X0 = X;
                        X -= _f.FunctionValue(X) * L;
                        n += 1;
                    }
                    while ((Math.Abs(X0 - X) >= eps) && (n <= 200));

                    result.Add(new DataPoint(X, 0));
                }
            }

            LastCalculatedRoots.Clear();
            LastCalculatedRoots.AddRange(result);
        }
    }
}
