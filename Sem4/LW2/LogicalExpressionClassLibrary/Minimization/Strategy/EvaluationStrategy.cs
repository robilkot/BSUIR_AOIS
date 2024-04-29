using LabLogger;

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
                Logger.Log($"Expression already minimized", Logger.Levels.Info);
                return source;
            }

            HashSet<string> allImplicants = implicants.Select(i => i.ToString()!).ToHashSet();

            if (form == NormalForms.FDNF)
            {
                foreach (var implicant in implicants)
                {
                    Logger.Log($"Trying to remove implicant {implicant}", Logger.Levels.Debug);

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

                        Logger.Log($"Modified implicant {implicantToModify} to {modifiedString}", Logger.Levels.Debug);
                    }

                    LogicalExpression remainder = MinimizationHelper.BuildNFFromStringSet(currentImplicantsCombination, form);

                    Logger.Log($"Expression without implicant {implicant}\n{remainder.ToTruthTableString()}", Logger.Levels.Debug);

                    if (remainder.IsContradictive())
                    {
                        allImplicants.Remove(implicant.ToString()!);
                        Logger.Log($"Found odd implicant {implicant}", Logger.Levels.Debug);

                        if (allImplicants.Count < 2)
                            break;
                    }
                }
            }
            else if (form == NormalForms.FCNF)
            {
                foreach (var implicant in implicants)
                {
                    Logger.Log($"Trying to remove implicant {implicant}", Logger.Levels.Debug);

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

                            Logger.Log($"Modified implicant {implicantToModify} to {modifiedString}", Logger.Levels.Debug);
                        }

                        LogicalExpression exprWithoutOneVariable = MinimizationHelper.BuildNFFromStringSet(currentImplicantsCombination, form);

                        Logger.Log($"{exprWithoutOneVariable} without {variable}\n{exprWithoutOneVariable.ToTruthTableString()}", Logger.Levels.Debug);

                        if (exprWithoutOneVariable.IsContradictive())
                        {
                            implicantIsOdd = true;
                            break;
                        }
                    }

                    if (implicantIsOdd)
                    {
                        allImplicants.Remove(implicant.ToString()!);
                        Logger.Log($"Found odd implicant {implicant}", Logger.Levels.Debug);

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
                Logger.Log(ex.Message, Logger.Levels.Error);
                return LogicalExpression.Empty;
            }
        }
    }
}
