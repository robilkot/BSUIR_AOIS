using HashTableClassLibrary;
using HashTableConsoleApp;
using LabLogger;

Logger.UseConsoleLogger();

Logger.Level = 0
    | Logger.Levels.Debug
    | Logger.Levels.Info
    | Logger.Levels.Error
    | Logger.Levels.Warning
    ;

HashTable<string, string> hashTable = new();

Logger.Log("--- Adding");

foreach (var kvp in LiteratureData.Data)
{
    hashTable.AddOrUpdate(kvp.Item1, kvp.Item2);
}

foreach (var node in hashTable)
{
    Logger.Log($"{node.Key} - {node.Value}");
}

Logger.Log(hashTable.Count.ToString());