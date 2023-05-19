namespace Rillium.Script
{
    internal static class FunctionHelpers
    {
        public static List<FunctionInfo> DefaultFunctions()
        {
            return new List<FunctionInfo>()
            {
                BuildNumber(nameof(Math.Abs), (a) => Math.Abs(a), arguments: 1),
                BuildNumber(nameof(Math.Acos), (a) => Math.Acos(a), arguments: 1),
                BuildNumber(nameof(Math.Acosh), (a) => Math.Acosh(a), arguments: 1),
                BuildNumber(nameof(Math.Asin), (a) => Math.Asin(a), arguments: 1),
                BuildNumber(nameof(Math.Asinh), (a) => Math.Asinh(a), arguments: 1),

                BuildNumber(nameof(Math.Atan), (a) => Math.Atan(a), arguments: 1),
                BuildNumber(nameof(Math.Atan2), (a) => Math.Atan2(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.Atanh), (a) => Math.Atanh(a), arguments: 1),
                BuildNumber(nameof(Math.BitDecrement), (a) => Math.BitDecrement(a), arguments: 1),
                BuildNumber(nameof(Math.BitIncrement), (a) => Math.BitIncrement(a), arguments: 1),

                BuildNumber(nameof(Math.Cbrt), (a) => Math.Cbrt(a), arguments: 1),
                BuildNumber(nameof(Math.Ceiling), (a) => Math.Ceiling(a), arguments: 1),
                BuildNumber(nameof(Math.Clamp), (a) => Math.Clamp(a[0], a[1], a[2]), arguments: 3),
                BuildNumber(nameof(Math.CopySign), (a) => Math.CopySign(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.Cos), (a) => Math.Cos(a), arguments: 1),

                BuildNumber(nameof(Math.Cosh), (a) => Math.Cosh(a), arguments: 1),
                BuildNumber(nameof(Math.E), (a) => Math.E, arguments: 0),
                BuildNumber(nameof(Math.Exp), (a) => Math.Exp(a), arguments: 1),
                BuildNumber(nameof(Math.Floor), (a) => Math.Floor(a), arguments: 1),
                BuildNumber(nameof(Math.FusedMultiplyAdd), (a) => Math.FusedMultiplyAdd(a[0], a[1], a[2]), arguments: 3),

                BuildNumber(nameof(Math.IEEERemainder),(a) => Math.IEEERemainder(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.ILogB), (a) => Math.ILogB(a), arguments: 1),
                BuildNumber(nameof(Math.Log),(a) =>  Math.Log(a), arguments: 1),
                BuildNumber(nameof(Math.Log10),(a) =>  Math.Log10(a), arguments: 1),
                BuildNumber(nameof(Math.Log2),(a) =>  Math.Log2(a), arguments: 1),

                BuildNumber(nameof(Math.Max), (a) => Math.Max(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.MaxMagnitude), (a) => Math.MaxMagnitude(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.Min), (a) => Math.Min(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.MinMagnitude), (a) => Math.MinMagnitude(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.PI), (a) => Math.PI, arguments: 0),

                BuildNumber(nameof(Math.Pow), (a) => Math.Pow(a[0], a[1]), arguments: 2),
                BuildNumber(nameof(Math.ReciprocalEstimate), (a) => Math.ReciprocalEstimate(a), arguments: 1),
                BuildNumber(nameof(Math.ReciprocalSqrtEstimate), (a) => Math.ReciprocalSqrtEstimate(a), arguments: 1),
                BuildNumber(nameof(Math.Round), (a) => Math.Round(a), arguments: 1),
                BuildNumber(nameof(Math.Round), (a) => Math.Round(a[0], (int)a[1]), arguments: 2),

                BuildNumber(nameof(Math.ScaleB), (a) => Math.ScaleB(a[0], (int)a[1]), arguments: 2),
                BuildNumber(nameof(Math.Sign), (a) => Math.Sign(a), arguments: 1),
                BuildNumber(nameof(Math.Sin), (a) => Math.Sin(a), arguments: 1),
                BuildNumber(nameof(Math.Sinh), (a) => Math.Sinh(a), arguments: 1),
                BuildNumber(nameof(Math.Sqrt), (a) => Math.Sqrt(a), arguments: 1),

                BuildNumber(nameof(Math.Tan), (a) => Math.Tan(a), arguments: 1),
                BuildNumber(nameof(Math.Tanh), (a) => Math.Tanh(a), arguments: 1),
                BuildNumber(nameof(Math.Tau), (a) => Math.Tau, arguments: 0),
                BuildNumber(nameof(Math.Truncate), (a) => Math.Truncate(a), arguments: 1),
            };
        }

        private static FunctionInfo BuildNumber(
           string name,
           Func<dynamic, dynamic> Function,
           int arguments) =>
           new FunctionInfo()
           {
               Name = name,
               Out = LiteralTypeId.Number,
               Function = Function,
               ArgumentTokens = ToArgumentNumbers(arguments),
           };

        private static List<LiteralTypeId> ToArgumentNumbers(int argumentCount)
        {
            var argumentTypes = new List<LiteralTypeId>();

            for (var i = 0; i < argumentCount; i++)
            {
                argumentTypes.Add(LiteralTypeId.Number);
            }

            return argumentTypes;
        }

        private static FunctionInfo Build(
            string name,
            LiteralTypeId returnType,
            Func<dynamic, dynamic> Function,
            params LiteralTypeId[] argumentTokens) =>
            new FunctionInfo()
            {
                Name = name,
                Out = returnType,
                Function = Function,
                ArgumentTokens = argumentTokens,
            };
    }
}
