namespace LogicalExpressionClassLibrary.Minimization.Strategy
{
    public class EvaluationStrategy : IMinimizationStrategy
    {
        public LogicalExpression Minimize(LogicalExpression input, NormalForms form)
        {
            LogicalExpression source = form switch
            {
                NormalForms.FCNF => input.FCNF,
                NormalForms.FDNF => input.FDNF,
                _ => throw new NotImplementedException()
            };

            source = source.MergeConstituents(form);

            var implicants = MinimizationHelper.GetConstituents(source, form);

            if(implicants.Count < 2)
            {
                ConsoleLogger.Log($"Expression already minimized", ConsoleLogger.DebugLevels.Info);
                return source;
            }

            HashSet<string> allImplicants = implicants.Select(i => i.ToString()!).ToHashSet();

            if (form == NormalForms.FDNF)
            {
                foreach (var implicant in implicants)
                {
                    ConsoleLogger.Log($"Trying to remove implicant {implicant}", ConsoleLogger.DebugLevels.Debug);

                    var variables = MinimizationHelper.GetVariables(implicant, form);
                    HashSet<string> currentImplicantsCombination = [];

                    foreach (var implicantToModify in allImplicants)
                    {
                        string modifiedString = implicantToModify;

                        foreach (var variable in variables)
                        {
                            modifiedString =
                                modifiedString.Replace(variable.variable.ToString()!,
                                variable.inverted ?
                                ((char)LogicalSymbols.True).ToString()! :
                                ((char)LogicalSymbols.False).ToString()!);
                        }

                        currentImplicantsCombination.Add(modifiedString);

                        ConsoleLogger.Log($"Modified implicant {implicantToModify} to {modifiedString}", ConsoleLogger.DebugLevels.Debug);
                    }

                    LogicalExpression remainder = MinimizationHelper.BuildNFFromStringSet(currentImplicantsCombination, form);

                    ConsoleLogger.Log($"Expression without implicant {implicant}\n{remainder.ToTruthTableString()}", ConsoleLogger.DebugLevels.Debug);

                    if (remainder.IsContradictive())
                    {
                        allImplicants.Remove(implicant.ToString()!);
                        ConsoleLogger.Log($"Found odd implicant {implicant}", ConsoleLogger.DebugLevels.Debug);

                        if (allImplicants.Count < 2)
                            break;
                    }
                }
            }
            else if (form == NormalForms.FCNF)
            {
                foreach (var implicant in implicants)
                {
                    ConsoleLogger.Log($"Trying to remove implicant {implicant}", ConsoleLogger.DebugLevels.Debug);

                    var variables = MinimizationHelper.GetVariables(implicant, form);

                    bool implicantIsOdd = false;

                    foreach (var variable in variables)
                    {
                        HashSet<string> currentImplicantsCombination = [];

                        foreach (var implicantToModify in allImplicants)
                        {
                            string modifiedString = implicantToModify;

                            modifiedString =
                                modifiedString.Replace(variable.variable.ToString()!,
                                variable.inverted ?
                                ((char)LogicalSymbols.False).ToString()! :
                                ((char)LogicalSymbols.True).ToString()!);

                            currentImplicantsCombination.Add(modifiedString);

                            ConsoleLogger.Log($"Modified implicant {implicantToModify} to {modifiedString}", ConsoleLogger.DebugLevels.Debug);
                        }

                        LogicalExpression exprWithoutOneVariable = MinimizationHelper.BuildNFFromStringSet(currentImplicantsCombination, form);

                        ConsoleLogger.Log($"{exprWithoutOneVariable} without {variable}\n{exprWithoutOneVariable.ToTruthTableString()}", ConsoleLogger.DebugLevels.Debug);

                        if (exprWithoutOneVariable.IsContradictive())
                        {
                            implicantIsOdd = true;
                            break;
                        }
                    }

                    if (implicantIsOdd)
                    {
                        allImplicants.Remove(implicant.ToString()!);
                        ConsoleLogger.Log($"Found odd implicant {implicant}", ConsoleLogger.DebugLevels.Debug);

                        if (allImplicants.Count < 2)
                            break;
                    }
                }
            }
            else
                throw new NotImplementedException();

            try
            {
                return MinimizationHelper.BuildNFFromStringSet(allImplicants, form);
            }
            catch (Exception ex)
            {
                ConsoleLogger.Log(ex.Message, ConsoleLogger.DebugLevels.Error);
                return LogicalExpression.Empty;
            }
        }
    }
}
