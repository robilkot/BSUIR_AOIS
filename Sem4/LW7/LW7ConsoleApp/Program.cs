using DiagonalMatrixClassLibrary;

DiagonalMatrix diagonalMatrix = new();

bool[] word = [true, true, true, true, true, false, false, false, false, true, true, true, false, false, false, false];
diagonalMatrix.SetWord(13, word);

Console.WriteLine(diagonalMatrix);
Console.WriteLine("Word 3: " + diagonalMatrix.GetWord(13).ToRowString());