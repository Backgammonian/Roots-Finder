using org.mariuszgromada.math.mxparser;

namespace RootsFinder
{
    public class FunctionExpression
    {
        private Expression _functionExpression;
        private Expression _firstDerivativeOfFunction;
        private Expression _secondDerivativeOfFunction;

        public FunctionExpression()
        {
            IsSyntaxCorrect = false;
        }

        public bool IsSyntaxCorrect { get; private set; }
        public bool CanCalculate { get; set; }
        public string Function { get; private set; }

        private bool IsFunctionNotValid(string function)
        {
            return function.Contains("&") ||
                function.Contains("&&") ||
                function.Contains(@"/\") ||
                function.Contains("~&") ||
                function.Contains("~&&") ||
                function.Contains(@"~/\") ||
                function.Contains("|") ||
                function.Contains("||") ||
                function.Contains(@"\/") ||
                function.Contains("~|") ||
                function.Contains("~||") ||
                function.Contains(@"~\/") ||
                function.Contains("(+)") ||
                function.Contains("-->") ||
                function.Contains("<--") ||
                function.Contains("-/>") ||
                function.Contains("</-") ||
                function.Contains("<->") ||
                function.Contains("~") ||
                function.Contains("┐") ||
                function.Contains("=") ||
                function.Contains("==") ||
                function.Contains("<>") ||
                function.Contains("~=") ||
                function.Contains("!=") ||
                function.Contains("<") ||
                function.Contains(">") ||
                function.Contains("<=") ||
                function.Contains(">=") ||
                function.Contains("deg") ||
                function.Contains("not") ||
                function.Contains("Bell") ||
                function.Contains("Luc") ||
                function.Contains("Fib") ||
                function.Contains("harm") ||
                function.Contains("ispr") ||
                function.Contains("Pi") ||
                function.Contains("Ei") ||
                function.Contains("li") ||
                function.Contains("Li") ||
                function.Contains("erf") ||
                function.Contains("erfc") ||
                function.Contains("erfInv") ||
                function.Contains("erfcInv") ||
                function.Contains("ulp") ||
                function.Contains("C") ||
                function.Contains("Bern") ||
                function.Contains("Stirl1") ||
                function.Contains("Stirl2") ||
                function.Contains("Worp") ||
                function.Contains("Euler") ||
                function.Contains("KDelta") ||
                function.Contains("EulerPol") ||
                function.Contains("Harm") ||
                function.Contains("rUni") ||
                function.Contains("rUnid") ||
                function.Contains("round") ||
                function.Contains("rNor") ||
                function.Contains("if") ||
                function.Contains("chi") ||
                function.Contains("CHi") ||
                function.Contains("Chi") ||
                function.Contains("cHi") ||
                function.Contains("pUni") ||
                function.Contains("cUni") ||
                function.Contains("qUni") ||
                function.Contains("pNor") ||
                function.Contains("cNor") ||
                function.Contains("qNor") ||
                function.Contains("iff") ||
                function.Contains("ConFrac") ||
                function.Contains("ConPol") ||
                function.Contains("gcd") ||
                function.Contains("lcm") ||
                function.Contains("add") ||
                function.Contains("multi") ||
                function.Contains("mean") ||
                function.Contains("var") ||
                function.Contains("std") ||
                function.Contains("rList") ||
                function.Contains("sum") ||
                function.Contains("prod") ||
                function.Contains("int") ||
                function.Contains("der-") ||
                function.Contains("der+") ||
                function.Contains("dern") ||
                function.Contains("diff") ||
                function.Contains("difb") ||
                function.Contains("avg") ||
                function.Contains("vari") ||
                function.Contains("stdi") ||
                function.Contains("mini") ||
                function.Contains("maxi") ||
                function.Contains("solve") ||
                function.Contains("[gam]") ||
                function.Contains("[phi]") ||
                function.Contains("[PN]") ||
                function.Contains("[B*]") ||
                function.Contains("[F'd]") ||
                function.Contains("[F'a]") ||
                function.Contains("[C2]") ||
                function.Contains("[M1]") ||
                function.Contains("[B2]") ||
                function.Contains("[B4]") ||
                function.Contains("[BN'L]") ||
                function.Contains("[Kat]") ||
                function.Contains("[K*]") ||
                function.Contains("[K.]") ||
                function.Contains("[B'L]") ||
                function.Contains("[RS'm]") ||
                function.Contains("[EB'e]") ||
                function.Contains("[Bern]") ||
                function.Contains("[GKW'l]") ||
                function.Contains("[HSM's]") ||
                function.Contains("[lm]") ||
                function.Contains("[Cah]") ||
                function.Contains("[Ll]") ||
                function.Contains("[AG]") ||
                function.Contains("[L*]") ||
                function.Contains("[L.]") ||
                function.Contains("[Dz3]") ||
                function.Contains("[A3n]") ||
                function.Contains("[Bh]") ||
                function.Contains("[Pt]") ||
                function.Contains("[L2]") ||
                function.Contains("[Nv]") ||
                function.Contains("[Ks]") ||
                function.Contains("[Kh]") ||
                function.Contains("[FR]") ||
                function.Contains("[La]") ||
                function.Contains("[P2]") ||
                function.Contains("[Om]") ||
                function.Contains("[MRB]") ||
                function.Contains("[li2]") ||
                function.Contains("[EG]") ||
                function.Contains("[c]") ||
                function.Contains("[G.]") ||
                function.Contains("[g]") ||
                function.Contains("[hP]") ||
                function.Contains("[h-]") ||
                function.Contains("[lP]") ||
                function.Contains("[mP]") ||
                function.Contains("[tP]") ||
                function.Contains("[ly]") ||
                function.Contains("[au]") ||
                function.Contains("[pc]") ||
                function.Contains("[kpc]") ||
                function.Contains("[Earth-R-eq]") ||
                function.Contains("[Earth-R-po]") ||
                function.Contains("[Earth-R]") ||
                function.Contains("[Earth-M]") ||
                function.Contains("[Earth-D]") ||
                function.Contains("[Moon-R]") ||
                function.Contains("[Moon-M]") ||
                function.Contains("[Moon-D]") ||
                function.Contains("[Solar-R]") ||
                function.Contains("[Solar-M]") ||
                function.Contains("[Mercury-R]") ||
                function.Contains("[Mercury-M]") ||
                function.Contains("[Mercury-D]") ||
                function.Contains("[Venus-R]") ||
                function.Contains("[Venus-M]") ||
                function.Contains("[Venus-D]") ||
                function.Contains("[Mars-R]") ||
                function.Contains("[Mars-M]") ||
                function.Contains("[Mars-D]") ||
                function.Contains("[Jupiter-R]") ||
                function.Contains("[Jupiter-M]") ||
                function.Contains("[Jupiter-D]") ||
                function.Contains("[Saturn-R]") ||
                function.Contains("[Saturn-M]") ||
                function.Contains("[Saturn-D]") ||
                function.Contains("[Uranus-R]") ||
                function.Contains("[Uranus-M]") ||
                function.Contains("[Uranus-D]") ||
                function.Contains("[Neptune-R]") ||
                function.Contains("[Neptune-M]") ||
                function.Contains("[Neptune-D]") ||
                function.Contains("@~") ||
                function.Contains("@&") ||
                function.Contains("@^") ||
                function.Contains("@|") ||
                function.Contains("@<<") ||
                function.Contains("@>>") ||
                function.Contains("[%]") ||
                function.Contains("[%%]") ||
                function.Contains("[Y]") ||
                function.Contains("[sept]") ||
                function.Contains("[Z]") ||
                function.Contains("[sext]") ||
                function.Contains("[E]") ||
                function.Contains("[quint]") ||
                function.Contains("[P]") ||
                function.Contains("[quad]") ||
                function.Contains("[T]") ||
                function.Contains("[tril]") ||
                function.Contains("[G]") ||
                function.Contains("[bil]") ||
                function.Contains("[M]") ||
                function.Contains("[mil]") ||
                function.Contains("[k]") ||
                function.Contains("[th]") ||
                function.Contains("[hecto]") ||
                function.Contains("[hund]") ||
                function.Contains("[deca]") ||
                function.Contains("[ten]") ||
                function.Contains("[deci]") ||
                function.Contains("[centi]") ||
                function.Contains("[milli]") ||
                function.Contains("[mic]") ||
                function.Contains("[n]") ||
                function.Contains("[p]") ||
                function.Contains("[f]") ||
                function.Contains("[a]") ||
                function.Contains("[z]") ||
                function.Contains("[y]") ||
                function.Contains("[m]") ||
                function.Contains("[km]") ||
                function.Contains("[cm]") ||
                function.Contains("[mm]") ||
                function.Contains("[inch]") ||
                function.Contains("[yd]") ||
                function.Contains("[ft]") ||
                function.Contains("[mile]") ||
                function.Contains("[nmi]") ||
                function.Contains("[m2]") ||
                function.Contains("[cm2]") ||
                function.Contains("[mm2]") ||
                function.Contains("[are]") ||
                function.Contains("[ha]") ||
                function.Contains("[acre]") ||
                function.Contains("[km2]") ||
                function.Contains("[mm3]") ||
                function.Contains("[cm3]") ||
                function.Contains("[m3]") ||
                function.Contains("[km3]") ||
                function.Contains("[ml]") ||
                function.Contains("[l]") ||
                function.Contains("[gall]") ||
                function.Contains("[pint]") ||
                function.Contains("[s]") ||
                function.Contains("[ms]") ||
                function.Contains("[min]") ||
                function.Contains("[h]") ||
                function.Contains("[day]") ||
                function.Contains("[week]") ||
                function.Contains("[yearj]") ||
                function.Contains("[kg]") ||
                function.Contains("[gr]") ||
                function.Contains("[mg]") ||
                function.Contains("[dag]") ||
                function.Contains("[t]") ||
                function.Contains("[oz]") ||
                function.Contains("[lb]") ||
                function.Contains("[b]") ||
                function.Contains("[kb]") ||
                function.Contains("[Mb]") ||
                function.Contains("[Gb]") ||
                function.Contains("[Tb]") ||
                function.Contains("[Pb]") ||
                function.Contains("[Eb]") ||
                function.Contains("[Zb]") ||
                function.Contains("[Yb]") ||
                function.Contains("[B]") ||
                function.Contains("[kB]") ||
                function.Contains("[MB]") ||
                function.Contains("[GB]") ||
                function.Contains("[TB]") ||
                function.Contains("[PB]") ||
                function.Contains("[EB]") ||
                function.Contains("[ZB]") ||
                function.Contains("[YB]") ||
                function.Contains("[J]") ||
                function.Contains("[eV]") ||
                function.Contains("[keV]") ||
                function.Contains("[MeV]") ||
                function.Contains("[GeV]") ||
                function.Contains("[TeV]") ||
                function.Contains("[m/s]") ||
                function.Contains("[km/h]") ||
                function.Contains("[mi/h]") ||
                function.Contains("[knot]") ||
                function.Contains("[m/s2]") ||
                function.Contains("[km/h2]") ||
                function.Contains("[mi/h2]") ||
                function.Contains("[rad]") ||
                function.Contains("[deg]") ||
                function.Contains("[']") ||
                function.Contains("['']");
        }

        public bool TryLoadFunction(string function)
        {
            if (IsFunctionNotValid(function))
            {
                Function = string.Empty;
                IsSyntaxCorrect = false;
                return false;
            }

            var argument = new Argument("x = 0");
            var expression = new Expression(function, argument);

            if (!expression.checkSyntax())
            {
                Function = string.Empty;
                IsSyntaxCorrect = false;
                return false;
            }

            _functionExpression = expression;

            var firstDerivativeOfFunctionString = string.Format("der({0}, x)", function);
            var argumentOfFirstDerivative = new Argument("x = 0");
            _firstDerivativeOfFunction = new Expression(firstDerivativeOfFunctionString, argumentOfFirstDerivative);

            var secondDerivativeOfFunctionString = string.Format("der(der({0}, x), x)", function);
            var argumentOfSecondDerivative = new Argument("x = 0");
            _secondDerivativeOfFunction = new Expression(secondDerivativeOfFunctionString, argumentOfSecondDerivative);

            Function = function;
            IsSyntaxCorrect = true;
            return true;
        }

        public void ResetFunction()
        {
            IsSyntaxCorrect = false;
        }

        public double FunctionValue(double x)
        {
            if (IsSyntaxCorrect && CanCalculate)
            {
                _functionExpression.setArgumentValue("x", x);
                return _functionExpression.calculate();
            }

            return 0;
        }

        public double FunctionDerivativeValue(double x)
        {
            if (IsSyntaxCorrect && CanCalculate)
            {
                _firstDerivativeOfFunction.setArgumentValue("x", x);
                return _firstDerivativeOfFunction.calculate();
            }

            return 0;
        }

        public double FunctionSecondDerivativeValue(double x)
        {
            if (IsSyntaxCorrect && CanCalculate)
            {
                _secondDerivativeOfFunction.setArgumentValue("x", x);
                return _secondDerivativeOfFunction.calculate();
            }
  
            return 0;
        }
    }
}