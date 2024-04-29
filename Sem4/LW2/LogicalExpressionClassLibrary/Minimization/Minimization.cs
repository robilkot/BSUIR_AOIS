using LabLogger;
using LogicalExpressionClassLibrary.Minimization.Strategy;

namespace LogicalExpressionClassLibrary.Minimization
{
    public static class Minimization
    {
        public static LogicalExpression Minimize(this LogicalExpression expr, NormalForms normalForm, IMinimizationStrategy strategy = null!)
        {
            strategy ??= new CombinedStrategy();
            Logger.Log($"Started minimizing using {strategy.GetType().Name}: {expr}", Logger.Levels.Info);

            var toReturn = strategy.Minimize(expr, normalForm);

            Logger.Log($"Finished minimizing using {strategy.GetType().Name}: {toReturn}", Logger.Levels.Info);

            return toReturn;
        }
    }
}
