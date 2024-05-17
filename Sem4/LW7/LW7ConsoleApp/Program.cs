using LW7ConsoleApp;

DiagonalMatrix diagonalMatrix = new DiagonalMatrix();

bool[] m = [true, true, true, true, true, false, false, false, false, true, true, true, false, false, false, false];
diagonalMatrix.SetWord(3, m);


Console.WriteLine(diagonalMatrix);
Console.WriteLine();
Console.WriteLine("Word 3: " + diagonalMatrix.GetWord(3).ToRowString());
