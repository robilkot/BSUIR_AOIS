﻿using LabLogger;
using LogicalExpressionClassLibrary;
using LogicalExpressionClassLibrary.Minimization;
using LogicalExpressionClassLibrary.Minimization.Strategy;
using System.Collections;

Logger.UseConsoleLogger();
//Logger.Level = Logger.Levels.Info;

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

const int states = 8;

for (int i = 0; i < states * 2; i++)
{
    truthTable.Add([]);
}

string[] pvars = ["q30", "q20", "q10"];
string[] vars = ["q3", "q2", "q1"];
string[] hvars = ["h3", "h2", "h1"];

//string[] pvars = ["q40", "q30", "q20", "q10"];
//string[] vars = ["q4", "q3", "q2", "q1"];
//string[] hvars = ["h4", "h3", "h2", "h1"];

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

BitArray qpBitMask = new(pvars.Length, true);

for (int i = 0; i < states; i++)
{
    for (int k = 0; k < pvars.Length; k++)
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
    var form = NormalForms.FDNF;

    var initialFDNF = LogicalExpression.NFFromTruthTable(truthTable, variables, v, form);

    Console.WriteLine($"{v} {form}: {initialFDNF}");
    var minimized = initialFDNF.Minimize(form);
    Console.WriteLine($"{v} {form} minimized: {minimized}");

    Console.WriteLine();
}

