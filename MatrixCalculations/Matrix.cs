using System;
using System.Text;

namespace MatrixCalculations;

public class Matrix
{
    public double[,] matrix = null;

    public int CountColumn { get; private set; }
    public int CountRow { get; private set; }
    public double GetMax
    {
        get
        {
            double value = double.MinValue;
            for (int i = 0; i < CountColumn; i++)
                for (int j = 0; j < CountRow; j++)
                    if (matrix[i, j] > value)
                        value = matrix[i, j];
            return value;
        }
    }
    public double GetMin
    {
        get
        {
            double value = double.MaxValue;
            for (int i = 0; i < CountColumn; i++)
                for (int j = 0; j < CountRow; j++)
                    if (matrix[i, j] < value)
                        value = matrix[i, j];
            return value;
        }
    }
    public Matrix(int x = 1, int y = 1)
    {
        matrix = new double[x, y];

        CountColumn = y;
        CountRow = x;
    }

    public double this[int x, int y]
    {
        get { return matrix[x, y]; }
        set { matrix[x, y] = value; }
    }
    public override string ToString()
    {
        StringBuilder ret = new();
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
        Matrix C = new(A.CountRow, A.CountColumn);
        for (int i = 0; i < C.CountRow; i++)
            for (int j = 0; j < C.CountColumn; j++)
                C[i, j] = A[i, j] + B[i, j];
        return C;
    }

    private static Matrix Subtract(Matrix A, Matrix B)
    {
        if (A.CountColumn != B.CountColumn || A.CountRow != B.CountRow)
            throw new Exception("Matrices should have equal size");
        Matrix C = new(A.CountRow, A.CountColumn);
        for (int i = 0; i < C.CountRow; i++)
            for (int j = 0; j < C.CountColumn; j++)
                C[i, j] = A[i, j] - B[i, j];
        return C;
    }

    private static Matrix Multiply(Matrix A, Matrix B)
    {
        if (A.CountColumn != B.CountRow)
            throw new Exception("There should be as many rows in the first matrix as there are columns in the second one");
        Matrix C = new(A.CountRow, B.CountColumn);
        for (int i = 0; i < C.CountRow; i++)
            for (int j = 0; j < C.CountColumn; j++)
                for (int k = 0; k < B.CountRow; k++)
                    C[i, j] += A[i, k] * B[k, j];
        return C;
    }

    private static Matrix MultiplyByNumber(Matrix A, int b)
    {
        Matrix B = new(A.CountRow, A.CountColumn);
        for (int i = 0; i < A.CountRow; i++)
            for (int j = 0; j < A.CountColumn; j++)
                B[i, j] = A[i, j] * b;
        return B;
    }

    public static Matrix GetMinor(Matrix A, int row, int column)
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

    public static double GetDeterminant(Matrix A)
    {
        if (A.CountRow != A.CountColumn)
            throw new Exception("Matrix should be square");
        double Determinant = 0;
        if (A.CountRow == 1)
              return A[0, 0];
        else if (A.CountRow == 2)
            return A[0, 0] * A[1, 1] - A[1, 0] * A[0, 1];
        int j = 0;
        for (int i = 0; i < A.CountRow; i++)
        {
            double sign = Math.Pow(-1, i + j + 2);
            Determinant += A[i, j] * sign * GetDeterminant(GetMinor(A, i, j));
        }
        return Determinant;
    }

    public static Matrix Transponse(Matrix A)
    {
        Matrix B = new(A.CountColumn, A.CountRow);
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
    public static Matrix operator *(int b, Matrix A)
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
