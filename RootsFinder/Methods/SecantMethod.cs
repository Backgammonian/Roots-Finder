using System;
using System.Collections.Generic;
using OxyPlot;

namespace RootsFinder.Methods
{
    public class SecantMethod : BaseMethod
    {
        public SecantMethod(FunctionExpression functionExpression) : base(functionExpression)
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

                    var a_check = _f.FunctionValue(a_temp) * _f.FunctionSecondDerivativeValue(a_temp);
                    var b_check = _f.FunctionValue(b_temp) * _f.FunctionSecondDerivativeValue(b_temp);
                 
                    var C = 0.0;
                    if (a_check > 0.0)
                    {
                        C = a_temp;
                    }
                    else
                    if (b_check > 0.0)
                    {
                        C = b_temp;
                    }

                    var X = 0.0;
                    if (a_check < 0.0)
                    {
                        X = a_temp;
                    }
                    else
                    if (b_check < 0.0)
                    {
                        X = b_temp;
                    }

                    var iteration = 0.0;
                    do
                    {
                        iteration = _f.FunctionValue(X) * ((X - C) / (_f.FunctionValue(X) - _f.FunctionValue(C)));
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
