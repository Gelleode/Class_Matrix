using System;
using System.Text;

namespace MatrixCalculations
{
    class Program
    {
        public class Matrix
        {
            public float[,] matrix = null;

            public int CountColumn { get; private set; }
            public int CountRow { get; private set; }

            public Matrix(int x = 1, int y = 1)
            {
                matrix = new float[x, y];

                CountColumn = y;
                CountRow = x;
            }

            public float this[int x, int y]
            {
                get { return matrix[x, y]; }
                set { matrix[x, y] = value; }
            }
            public override string ToString()
            {
                StringBuilder ret = new StringBuilder();
                if (matrix == null) return ret.ToString();

                for (int i = 0; i < CountRow; i++)
                {
                    for (int t = 0; t < CountColumn; t++)
                    {
                        ret.Append(matrix[i, t]);
                        ret.Append("\t");
                    }
                    ret.Append("\n");
                }
                return ret.ToString();
            }

            public static dynamic Sum(Matrix A, Matrix B)
            {
                if (A.CountColumn == B.CountColumn & A.CountRow == B.CountRow)
                {
                    Matrix C = new Matrix(A.CountRow, A.CountColumn);
                    for (int i = 0; i < C.CountRow; i++)
                        for (int j = 0; j < C.CountColumn; j++)
                            C[i, j] = A[i, j] + B[i, j];
                    return C;
                }
                else return "Can't add these matrices";
            }
            
            public static dynamic Subtract(Matrix A, Matrix B)
            {
                if (A.CountColumn == B.CountColumn & A.CountRow == B.CountRow)
                {
                    Matrix C = new Matrix(A.CountRow, A.CountColumn);
                    for (int i = 0; i < C.CountRow; i++)
                        for (int j = 0; j < C.CountColumn; j++)
                            C[i, j] = A[i, j] - B[i, j];
                    return C;
                }
                else return "Can't subtract these matrices";
            }

            public static dynamic Multiply(Matrix A, Matrix B)
            {
                if (A.CountColumn == B.CountRow)
                {
                    Matrix C = new Matrix(A.CountRow, B.CountColumn);
                    for (int i = 0; i < C.CountRow; i++)
                        for(int j = 0; j < C.CountColumn; j++)
                            for (int k = 0; k < B.CountRow; k++)
                                C[i, j] += A[i, k] * B[k, j];
                    return C;
                }
                else return "Can't multiply these matrices";
            }

            public static Matrix MultiplyByNumber(Matrix A, int b)
            {
                Matrix B = new Matrix(A.CountRow, A.CountColumn);
                for (int i = 0; i < A.CountRow; i++)
                    for (int j = 0; j < A.CountColumn; j++)
                        B[i, j] = A[i, j] * b;
                return B;
            }

            public static Matrix Transponse(Matrix A)
            {
                Matrix B = new Matrix(A.CountColumn, A.CountRow);
                for (int i = 0; i < B.CountRow; i++)
                    for (int j = 0; j < B.CountColumn; j++)
                        B[i, j] = A[j, i];
                return B;
            }

            public static dynamic operator *(Matrix A, Matrix B)
            {
                return Multiply(A, B);
            }

            public static dynamic operator *(Matrix A, int b)
            {
                return MultiplyByNumber(A, b);
            }

            public static dynamic operator +(Matrix A, Matrix B)
            {
                return Sum(A, B);
            }

            public static dynamic operator -(Matrix A, Matrix B)
            {
                return Subtract(A, B);
            }
        }

        static void Main()
        {
            Matrix m1 = new Matrix(2, 3);

            for (int i = 0; i < m1.CountRow; i++)
            {
                string[] s = Console.ReadLine().Split(' ');
                for (int j = 0; j < m1.CountColumn; j++)
                    m1.matrix[i, j] = Convert.ToInt32(s[j]);
            }
            Console.WriteLine();

            Matrix m2 = new Matrix(3, 2);
            for (int i = 0; i < m2.CountRow; i++)
            {
                string[] s = Console.ReadLine().Split(' ');
                for (int j = 0; j < m2.CountColumn; j++)
                    m2.matrix[i, j] = Convert.ToInt32(s[j]);
            }

            Console.WriteLine($"m1 multiply 2\n{m1*2}");
            Console.WriteLine();
            Console.WriteLine($"m1*m2 \n{m1*m2}");
            Console.WriteLine();
            Console.WriteLine($"m1 transpose \n{Matrix.Transponse(m1)}");
            Console.WriteLine();
            Console.WriteLine($"m1 + m2\n{m2+m1}");
            Console.WriteLine();
            Console.WriteLine($"m2 - m1\n{m2-m1}");
            Console.WriteLine();
            Print(m1, m2, m1*m2);
            Console.ReadKey(true);
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
}