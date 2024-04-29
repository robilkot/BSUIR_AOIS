using LabLogger;
using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.Minimization;
using LogicalExpressionClassLibrary.Minimization.Strategy;
using System.Collections;

Logger.UseConsoleLogger();
Logger.Level = Logger.Levels.Info;

static void NextCombination(BitArray bits)
{
    for (int i = bits.Length - 1; i >= 0; i--)
    {
        bits[i] = !bits[i];

        if (!bits[i])
        {
            break;
        }
    }
}

#region Init truth table

List<Dictionary<string, bool>> truthTable = [];

const int states = 16;

for (int i = 0; i < states * 2; i++)
{
    truthTable.Add([]);
}

string[] pvars = ["q40", "q30", "q20", "q10"];
string[] vars = ["q4", "q3", "q2", "q1"];
string[] hvars = ["h4", "h3", "h2", "h1"];

foreach (var row in truthTable)
{
    foreach (var v in pvars)
    {
        row.Add(v, false);
    }
    foreach (var v in vars)
    {
        row.Add(v, false);
    }
    foreach (var v in hvars)
    {
        row.Add(v, false);
    }

    row.Add("v", false);
}

#endregion

#region Fill truth table

BitArray qpBitMask = new(4, true);

for (int i = 0; i < states; i++)
{
    for (int k = 0; k < 4; k++)
    {
        // current state
        truthTable[2 * i][vars[k]]
            = truthTable[2 * i + 1][vars[k]]
            = qpBitMask[k];
        
        // previous state
        truthTable[(2 * i + 1) % (states * 2)][pvars[k]]
            = truthTable[(2 * i + 2) % (states * 2)][pvars[k]]
            = qpBitMask[k];

        // memory initiation
        truthTable[2 * i][hvars[k]] = truthTable[2 * i][vars[k]] ^ truthTable[2 * i][pvars[k]];
        truthTable[2 * i + 1][hvars[k]] = truthTable[2 * i + 1][vars[k]] ^ truthTable[2 * i + 1][pvars[k]];
    }

    truthTable[2 * i]["v"] = true;
    truthTable[2 * i + 1]["v"] = false;

    NextCombination(qpBitMask);
}

var variables = new List<string>(pvars)
{
    "v"
};

#endregion

foreach (var v in hvars)
{
    var initialFCNF = LogicalExpression.NFFromTruthTable(truthTable, variables, v, NormalForms.FCNF);
    var initialFDNF = LogicalExpression.NFFromTruthTable(truthTable, variables, v, NormalForms.FDNF);

    //Logger.Log($"{v} FCNF: {initialFCNF}");
    var minimizedFCNF = initialFCNF.Minimize(NormalForms.FCNF, new TableStrategy());
    Logger.Log($"{v} FCNF min: {minimizedFCNF}");

    //Logger.Log($"{v} FDNF: {initialFDNF}");
    var minimizedFDNF = initialFDNF.Minimize(NormalForms.FDNF, new TableStrategy());
    Logger.Log($"{v} FDNF min: {minimizedFDNF}");
}

