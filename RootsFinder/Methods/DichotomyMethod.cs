using System;
using System.Collections.Generic;
using OxyPlot;

namespace RootsFinder.Methods
{
    public class DichotomyMethod : BaseMethod
    {
        public DichotomyMethod(FunctionExpression functionExpression) : base(functionExpression)
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
                    var a_temp = i;
                    var b_temp = i + 10.0 * eps;
                    var X = 0.0;

                    do
                    {
                        X = (a_temp + b_temp) / 2.0;

                        var f1 = _f.FunctionValue(X);
                        var f2 = _f.FunctionValue(a_temp);

                        if (f1 * f2 < 0.0)
                        {
                            b_temp = X;
                        }
                        else
                        {
                            a_temp = X;
                        }
                    }
                    while (Math.Abs(a_temp - b_temp) > 2.0 * eps);

                    result.Add(new DataPoint(X, 0));
                }
            }

            LastCalculatedRoots.Clear();
            LastCalculatedRoots.AddRange(result);
        }
    }
}
