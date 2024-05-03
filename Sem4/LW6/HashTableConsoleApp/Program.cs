using HashTableClassLibrary;
using LabLogger;

Logger.UseConsoleLogger();

Logger.Level = 0
    | Logger.Levels.Debug
    | Logger.Levels.Info
    | Logger.Levels.Error
    | Logger.Levels.Warning
    ;

HashTable<string, string> hashTable = new();

List<(string, string)> kvPairs = [];

foreach (var key in new List<string>() { "ab", "aB", "Ab", "AB" })
{
    foreach (var c in Enumerable.Range(0, 4))
    {
        kvPairs.Add((key + c.ToString(), c.ToString()));
    }
}

Logger.Log("--- Adding");

foreach (var kvp in kvPairs)
{
    hashTable.AddOrUpdate(kvp.Item1, kvp.Item2);
    //Logger.Log($"i {kvp.Item1} {kvp.Item2}");
}

foreach (var node in hashTable)
{
    Logger.Log($"w {node.Key} {node.Value}");
}

Logger.Log(hashTable.Count.ToString());

Logger.Log("--- Removing");

foreach (int i in Enumerable.Range(8, 8))
{
    hashTable.Remove(kvPairs[i].Item1);
}

foreach (var node in hashTable)
{
    Logger.Log($"w {node.Key} {node.Value}");
}

Logger.Log(hashTable.Count);