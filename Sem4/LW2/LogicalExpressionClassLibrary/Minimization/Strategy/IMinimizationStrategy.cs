namespace LogicalExpressionClassLibrary.Minimization.Strategy
{
    public interface IMinimizationStrategy
    {
        public LogicalExpression Minimize(LogicalExpression input, NormalForms form);
    }
}
