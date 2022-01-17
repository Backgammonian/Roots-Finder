using System;
using System.Collections.Generic;
using OxyPlot;

namespace RootsFinder.Methods
{
    public class NewtonsMethod : BaseMethod
    {
        public NewtonsMethod(FunctionExpression functionExpression) : base(functionExpression)
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

                    var X = _f.FunctionValue(b_temp) * _f.FunctionSecondDerivativeValue(b_temp) > 0.0 ? b_temp : a_temp;
                    var iteration = 0.0;

                    do
                    {
                        iteration = _f.FunctionValue(X) / _f.FunctionDerivativeValue(X);
                        X -= iteration;
                    }
                    while (Math.Abs(iteration) > eps);

                    result.Add(new DataPoint(X, 0));
                }
            }

            LastCalculatedRoots.Clear();
            LastCalculatedRoots.AddRange(result);
        }
    }
}
