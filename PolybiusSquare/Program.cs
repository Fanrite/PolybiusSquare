using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolybiusSquare
{
    public enum Method
    {
        Method1,
        Method2
    }

    class PolybiusSquare
    {
        char[,] square;
        string alphabet;
        Method encryptMethod;

        public PolybiusSquare(string alphabet = null, Method cipherMethod = Method.Method1)
        {
            this.alphabet = alphabet ?? "АБВГҐДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ ";
            encryptMethod = cipherMethod;
        }

        char[,] GetSquare(string key)
        {
            var newAlphabet = alphabet;
            for (int i = 0; i < key.Length; i++)
            {
                newAlphabet = newAlphabet.Replace(key[i].ToString(), "");
            }

            newAlphabet = key + newAlphabet + "0123456789!@#$%^&*)_+-=<>?,.";

            var n = (int)Math.Ceiling(Math.Sqrt(alphabet.Length));

            square = new char[n, n];
            var index = 0;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (index < newAlphabet.Length)
                    {
                        square[i, j] = newAlphabet[index];
                        index++;
                    }
                }
            }

            return square;
        }

        bool FindSymbol(char[,] symbolsTable, char symbol, out int column, out int row)
        {
            var l = symbolsTable.GetUpperBound(0) + 1;
            for (int i = 0; i < l; i++)
            {
                for (int j = 0; j < l; j++)
                {
                    if (symbolsTable[i, j] == symbol)
                    {
                        row = i;
                        column = j;
                        return true;
                    }
                }
            }
            row = -1;
            column = -1;
            return false;
        }

        public string PolibiusEncrypt(string text, string password)
        {
            var outputText = "";
            var square = GetSquare(password);
            var m = text.Length;
            switch (encryptMethod)
            {
                case Method.Method1:
                    for (int i = 0; i < m; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            var newRowIndex = rowIndex == square.GetUpperBound(1)
                                ? 0
                                : rowIndex + 1;
                            outputText += square[newRowIndex, columnIndex].ToString();
                        }
                    }
                    break;

                case Method.Method2:
                    var coordinates = new int[m * 2];
                    for (int i = 0; i < m; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            coordinates[i] = columnIndex;
                            coordinates[i + m] = rowIndex;
                        }
                    }

                    for (int i = 0; i < m * 2; i += 2)
                    {
                        outputText += square[coordinates[i + 1], coordinates[i]];
                    }
                    break;
            }

            return outputText;
        }

        public string PolybiusDecrypt(string text, string password)
        {
            var outputText = "";
            var square = GetSquare(password);
            switch (encryptMethod)
            {
                case Method.Method1:
                    for (int i = 0; i < text.Length; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            var newRowIndex = rowIndex == 0
                                ? square.GetUpperBound(1)
                                : rowIndex - 1;
                            outputText += square[newRowIndex, columnIndex].ToString();
                        }
                    }
                    break;

                case Method.Method2:
                    var m = text.Length;
                    var coordinates = new int[m * 2];
                    var j = 0;
                    for (int i = 0; i < m; i++)
                    {
                        if (FindSymbol(square, text[i], out int columnIndex, out int rowIndex))
                        {
                            coordinates[j] = columnIndex;
                            coordinates[j + 1] = rowIndex;
                            j += 2;
                        }
                    }

                    for (int i = 0; i < m; i++)
                    {
                        outputText += square[coordinates[i + m], coordinates[i]];
                    }
                    break;
            }

            return outputText;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var polybius = new PolybiusSquare();
            Console.Write("Введіть текст: ");
            var message = Console.ReadLine().ToUpper();
            Console.Clear();
            Console.Write("Введіть пароль(без повторення символів): ");
            var pass = Console.ReadLine().ToUpper();
            var cipherText = polybius.PolibiusEncrypt(message, pass);
            Console.Clear();
            Console.WriteLine($"Зашифрований  текст: {cipherText}");
            Console.WriteLine($"Розшифрований текст: {message}");
            Console.ReadLine();
        }
    }
}
