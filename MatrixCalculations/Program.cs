using System;
using System.Text;

namespace MatrixCalculations;
class Program
{
    static void Main()
    {
        Matrix m1 = new(3, 3);
        m1[0,0] = 1; m1[0,1] = -1; m1[0,2] = 1;
        m1[1,0] = 2; m1[1,1] = 1; m1[1,2] = 1;
        m1[2,0] = 1; m1[2,1] = 1; m1[2,2] = 2;
        Console.WriteLine();

        Matrix m2 = new(3, 3);
        FillRandom(ref m2, 0, 10);

        Console.WriteLine(m1);
        Console.WriteLine($"m1 inverse\n{Matrix.InverseMatrix(m1)}");
        Console.WriteLine($"m1 Minor from 3 3\n{Matrix.SqrMinor(m1, 2, 2)}");
        Console.WriteLine($"m1 determinant - \n{Matrix.Determinant(m1)}");
        Console.WriteLine();
        Console.WriteLine($"m1 multiply 2\n{m1 * 2}");
        Console.WriteLine();
        Console.WriteLine($"m1*m1 \n{m1 * m1}");
        Console.WriteLine();
        Console.WriteLine($"m1^10 \n{Matrix.Pow(m1, 10)}");
        Console.WriteLine();
        Console.WriteLine($"m1 transpose \n{Matrix.Transpose(m1)}");
        Console.WriteLine();
        Console.WriteLine($"m1 + m2\n{m2 + m1}");
        Console.WriteLine();
        Console.WriteLine($"m2 - m1\n{m2 - m1}");
        Console.WriteLine();
        Console.WriteLine($"m1 * inverse M1\n{m1 * Matrix.InverseMatrix(m1)}");
        Console.WriteLine();
        Print(m1, m2, m1 * m2);
        Console.Read();
    }

    static Matrix FillRandom(ref Matrix matrix, int a, int b)
    {
        Random rnd = new();
        for (int i = 0; i < matrix.CountRow; i++)
            for (int j = 0; j < matrix.CountColumn; j++)
                matrix[i, j] = rnd.Next(a, b);
        return matrix;
    }

    static void Print(Matrix A, Matrix B, Matrix C)
    {
        for (int i = 0; i < C.CountRow; i++)
        {
            for (int j = 0; j < C.CountColumn; j++)
            {
                Console.Write($"C[{i},{j}] = ");
                for (int k = 0; k < B.CountRow; k++)
                {
                    if (k == 0)
                        Console.Write($"{A[i, k]} * {B[k, j]} ");
                    else
                        Console.Write($"+ {A[i, k]} * {B[k, j]} ");
                }
                Console.WriteLine($"= {C[i, j]}");
            }
        }
    }
}