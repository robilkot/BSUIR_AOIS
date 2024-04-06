using LogicalExpressionClassLibrary.Minimization.Strategy;

namespace LogicalExpressionClassLibrary.Minimization
{
    public static class Minimization
    {
        public static LogicalExpression Minimize(this LogicalExpression expr, NormalForms normalForm, IMinimizationStrategy strategy = null!)
        {
            strategy ??= new CombinedStrategy();
            ConsoleLogger.Log($"Started minimizing using {strategy.GetType().Name}: {expr}", ConsoleLogger.DebugLevels.Info);

            var toReturn = strategy.Minimize(expr, normalForm);

            ConsoleLogger.Log($"Finished minimizing using {strategy.GetType().Name}: {toReturn}", ConsoleLogger.DebugLevels.Info);

            return toReturn;
        }
    }
}
