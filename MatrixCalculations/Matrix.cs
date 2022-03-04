using System;
using System.Text;

namespace MatrixCalculations;

public class Matrix
{
    public decimal[,] matrix = null;

    public int CountColumn { get; private set; }
    public int CountRow { get; private set; }
    public decimal GetMax
    {
        get
        {
            decimal value = decimal.MinValue;
            for (int i = 0; i < CountColumn; i++)
                for (int j = 0; j < CountRow; j++)
                    if (matrix[i, j] > value)
                        value = matrix[i, j];
            return value;
        }
    }
    public decimal GetMin
    {
        get
        {
            decimal value = decimal.MaxValue;
            for (int i = 0; i < CountColumn; i++)
                for (int j = 0; j < CountRow; j++)
                    if (matrix[i, j] < value)
                        value = matrix[i, j];
            return value;
        }
    }
    public Matrix(int x = 1, int y = 1)
    {
        matrix = new decimal[x, y];

        CountColumn = y;
        CountRow = x;
    }
    public decimal this[int x, int y]
    {
        get { return matrix[x, y]; }
        set { matrix[x, y] = value; }
    }
    public override string ToString()
    {
        StringBuilder ret = new StringBuilder();
        int SpaceAling;
        if (Convert.ToString(GetMax).Length > Convert.ToString(GetMin).Length)
            SpaceAling = Convert.ToString(GetMax).Length;
        else
            SpaceAling = Convert.ToString(GetMin).Length;
        if (matrix == null) return ret.ToString();
        for (int i = 0; i < CountRow; i++)
        {
            for (int j = 0; j < CountColumn; j++)
            {
                string ToAppend = Convert.ToString(matrix[i, j]);
                while (ToAppend.Length < SpaceAling)
                    ToAppend += ' ';
                ToAppend += ' ';
                ret.Append(ToAppend);
            }
            ret.Append('\n');
        }
        return ret.ToString();
    }

    private static Matrix Sum(Matrix A, Matrix B)
    {
        if (A.CountColumn != B.CountColumn || A.CountRow != B.CountRow)
            throw new Exception("Matrices should have equal size");
        Matrix C = new Matrix(A.CountRow, A.CountColumn);
        for (int i = 0; i < C.CountRow; i++)
            for (int j = 0; j < C.CountColumn; j++)
                C[i, j] = A[i, j] + B[i, j];
        return C;
    }

    private static Matrix Subtract(Matrix A, Matrix B)
    {
        if (A.CountColumn != B.CountColumn || A.CountRow != B.CountRow)
            throw new Exception("Matrices should have equal size");
        Matrix C = new Matrix(A.CountRow, A.CountColumn);
        for (int i = 0; i < C.CountRow; i++)
            for (int j = 0; j < C.CountColumn; j++)
                C[i, j] = A[i, j] - B[i, j];
        return C;
    }

    private static Matrix Multiply(Matrix A, Matrix B)
    {
        if (A.CountColumn != B.CountRow)
            throw new Exception("There should be as many rows in the first matrix as there are columns in the second one");
        Matrix C = new Matrix(A.CountRow, B.CountColumn);
        for (int i = 0; i < C.CountRow; i++)
            for (int j = 0; j < C.CountColumn; j++)
                for (int k = 0; k < B.CountRow; k++)
                    C[i, j] += A[i, k] * B[k, j];
        return C;
    }

    private static Matrix MultiplyByNumber(Matrix A, int b)
    {
        Matrix B = new Matrix(A.CountRow, A.CountColumn);
        for (int i = 0; i < A.CountRow; i++)
            for (int j = 0; j < A.CountColumn; j++)
                B[i, j] = A[i, j] * b;
        return B;
    }

    private static Matrix MultiplyByNumber(Matrix A, decimal b)
    {
        Matrix B = new Matrix(A.CountRow, A.CountColumn);
        for (int i = 0; i < A.CountRow; i++)
            for (int j = 0; j < A.CountColumn; j++)
                B[i, j] = A[i, j] * b;
        return B;
    }
    
    private static Matrix MultiplyByNumber(Matrix A, double b)
    {
        Matrix B = new Matrix(A.CountRow, A.CountColumn);
        for (int i = 0; i < A.CountRow; i++)
            for (int j = 0; j < A.CountColumn; j++)
                B[i, j] = A[i, j] * (decimal)b;
        return B;
    }

    public static Matrix Pow(Matrix A, int n)
    {
        if (n < 1)
            throw new Exception("Power should be bigger than one");
        Matrix B = A;
        for (int i = 0; i < n - 1; i++)
            A *= B;
        return A;
    }

    public static Matrix Minor(Matrix A, int row, int column)
    {
        if (A.CountRow != A.CountColumn)
            throw new Exception("Matrix should be square");
        if (A.CountRow == 1)
            throw new Exception("Matrix should be bigger than 1x1");
        Matrix result = new Matrix(A.CountRow-1, A.CountColumn-1);
        int m = 0, k;
        for (int i = 0; i < A.CountRow; i++)
        {
            if (i == row) continue;
            k = 0;
            for (int j = 0; j < A.CountColumn; j++)
            {
                if (j == column) continue;
                result[m, k++] = A[i, j];
            }
            m++;
        }
        return result;
    }
    public static Matrix InverseMatrix(Matrix A)
    {
        decimal det = Determinant(A);
        if (det == 0)
            throw new Exception("This matrix can't be inversed");
        A = Transponse(A);
        Matrix B = new Matrix(A.CountColumn, A.CountRow);
        for (int i = 0; i < A.CountRow; i++)
            for (int j = 0; j < A.CountColumn; j++)
                B[i,j] = (decimal)Math.Pow(-1, i + j + 2) * Determinant(Minor(A, i, j));
        A = B * (1 / det);
        return A;
    }
    public static decimal Determinant(Matrix A)
    {
        if (A.CountRow != A.CountColumn)
            throw new Exception("Matrix should be square");
        decimal Determinant = 0;
        if (A.CountRow == 1)
              return A[0, 0];
        else if (A.CountRow == 2)
            return A[0, 0] * A[1, 1] - A[1, 0] * A[0, 1];
        int j = 0;
        for (int i = 0; i < A.CountRow; i++)
        {
            int sign = Convert.ToInt32(Math.Pow(-1, i + j + 2));
            Determinant += A[i, j] * sign * Matrix.Determinant(Minor(A, i, j));
        }
        return Determinant;
    }

    public static Matrix Transponse(Matrix A)
    {
        Matrix B = new Matrix(A.CountColumn, A.CountRow);
        for (int i = 0; i < B.CountRow; i++)
            for (int j = 0; j < B.CountColumn; j++)
                B[i, j] = A[j, i];
        return B;
    }

    public static Matrix operator *(Matrix A, Matrix B)
    {
        return Multiply(A, B);
    }

    public static Matrix operator *(Matrix A, int b)
    {
        return MultiplyByNumber(A, b);
    }
    public static Matrix operator *(Matrix A, decimal b)
    {
        return MultiplyByNumber(A, b);
    }
    public static Matrix operator *(Matrix A, double b)
    {
        return MultiplyByNumber(A, b);
    }

    public static Matrix operator +(Matrix A, Matrix B)
    {
        return Sum(A, B);
    }

    public static Matrix operator -(Matrix A, Matrix B)
    {
        return Subtract(A, B);
    }
}
