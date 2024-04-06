namespace LogicalExpressionClassLibrary.Minimization.Strategy
{
    public class TableStrategy : IMinimizationStrategy
    {
        public LogicalExpression Minimize(LogicalExpression input, NormalForms form)
        {
            LogicalExpression source = form switch
            {
                NormalForms.FCNF => input.FCNF,
                NormalForms.FDNF => input.FDNF,
                _ => throw new NotImplementedException()
            };

            var karnaugh = Karnaugh.Karnaugh.CreateKarnaugh(source, form);

            if (ConsoleLogger.DebugLevel.HasFlag(ConsoleLogger.DebugLevels.Info))
            {
                karnaugh.PrintTable();
            }

            var nodes = karnaugh.GetConstituents();

            try
            {
                return MinimizationHelper.BuildNFFromStringSet(nodes, form);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Log(ex.Message, ConsoleLogger.DebugLevels.Error);
                return LogicalExpression.Empty;
            }
        }
    }
}
