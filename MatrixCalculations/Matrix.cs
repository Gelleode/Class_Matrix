using System;
using System.Globalization;
using System.Text;

namespace MatrixCalculations;

public class Matrix
{
    public decimal[,] matrix;

    public int CountColumn { get; private set; }
    public int CountRow { get; private set; }

    public decimal GetMaxElement
    {
        get
        {
            var value = decimal.MinValue;
            for (var i = 0; i < CountColumn; i++)
            for (var j = 0; j < CountRow; j++)
                if (matrix[i, j] > value)
                    value = matrix[i, j];
            return value;
        }
    }

    public decimal GetMinElement
    {
        get
        {
            var value = decimal.MaxValue;
            for (var i = 0; i < CountColumn; i++)
            for (var j = 0; j < CountRow; j++)
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
        get => matrix[x, y];
        set => matrix[x, y] = value;
    }

    public override string ToString()
    {
        var ret = new StringBuilder();
        var spaceAlignment =
            Convert.ToString(GetMaxElement, CultureInfo.InvariantCulture).Length >
            Convert.ToString(GetMinElement, CultureInfo.InvariantCulture).Length
                ? Convert.ToString(GetMaxElement, CultureInfo.InvariantCulture).Length
                : Convert.ToString(GetMinElement, CultureInfo.InvariantCulture).Length;
        if (matrix == null) return ret.ToString();
        for (var i = 0; i < CountRow; i++)
        {
            if (matrix[i, 0] >= 0)
                ret.Append(' ');
            for (var j = 0; j < CountColumn; j++)
            {
                var toAppend = Convert.ToString(matrix[i, j], CultureInfo.InvariantCulture);
                while (toAppend.Length != spaceAlignment)
                    toAppend += ' ';
                toAppend += ' ';

                if (matrix[i, j] < 0)
                    toAppend += ' ';

                if (j < CountColumn - 1)
                    if (matrix[i, j + 1] < 0)
                        toAppend = toAppend.Remove(toAppend.Length - 1, 1);

                ret.Append(toAppend);
            }

            ret.Append('\n');
        }

        return ret.ToString();
    }

    private static Matrix Sum(Matrix a, Matrix b)
    {
        if (a.CountColumn != b.CountColumn || a.CountRow != b.CountRow)
            throw new Exception("Matrices should have equal size");
        var result = new Matrix(a.CountRow, a.CountColumn);
        for (var i = 0; i < result.CountRow; i++)
        for (var j = 0; j < result.CountColumn; j++)
            result[i, j] = a[i, j] + b[i, j];
        return result;
    }

    private static Matrix Subtract(Matrix a, Matrix b)
    {
        if (a.CountColumn != b.CountColumn || a.CountRow != b.CountRow)
            throw new Exception("Matrices should have equal size");
        var result = new Matrix(a.CountRow, a.CountColumn);
        for (var i = 0; i < result.CountRow; i++)
        for (var j = 0; j < result.CountColumn; j++)
            result[i, j] = a[i, j] - b[i, j];
        return result;
    }

    private static Matrix Multiply(Matrix a, Matrix b)
    {
        if (a.CountColumn != b.CountRow)
            throw new Exception(
                "There should be as many rows in the first matrix as there are columns in the second one");
        var result = new Matrix(a.CountRow, b.CountColumn);
        for (var i = 0; i < result.CountRow; i++)
        for (var j = 0; j < result.CountColumn; j++)
        for (var k = 0; k < b.CountRow; k++)
            result[i, j] += a[i, k] * b[k, j];
        return result;
    }

    private static Matrix MultiplyByNumber(Matrix a, int b)
    {
        var result = new Matrix(a.CountRow, a.CountColumn);
        for (var i = 0; i < a.CountRow; i++)
        for (var j = 0; j < a.CountColumn; j++)
            result[i, j] = a[i, j] * b;
        return result;
    }

    private static Matrix MultiplyByNumber(Matrix a, decimal b)
    {
        var result = new Matrix(a.CountRow, a.CountColumn);
        for (var i = 0; i < a.CountRow; i++)
        for (var j = 0; j < a.CountColumn; j++)
            result[i, j] = a[i, j] * b;
        return result;
    }

    private static Matrix MultiplyByNumber(Matrix a, double b)
    {
        var result = new Matrix(a.CountRow, a.CountColumn);
        for (var i = 0; i < a.CountRow; i++)
        for (var j = 0; j < a.CountColumn; j++)
            result[i, j] = a[i, j] * (decimal)b;
        return result;
    }

    public static Matrix Pow(Matrix a, int n)
    {
        if (n < 1) throw new Exception("Power should be bigger than one");
        var result = a;
        for (var i = 0; i < n - 1; i++)
            a *= result;
        return a;
    }

    public static Matrix SqrMinor(Matrix a, int row, int column)
    {
        if (a.CountRow != a.CountColumn) throw new Exception("Matrix should be square");
        if (a.CountRow == 1) throw new Exception("Matrix should be bigger than 1x1");
        var result = new Matrix(a.CountRow - 1, a.CountColumn - 1);
        var m = 0;
        for (var i = 0; i < a.CountRow; i++)
        {
            if (i == row) continue;
            var k = 0;
            for (var j = 0; j < a.CountColumn; j++)
            {
                if (j == column) continue;
                result[m, k++] = a[i, j];
            }

            m++;
        }

        return result;
    }

    public static Matrix InverseMatrix(Matrix a)
    {
        var det = Determinant(a);
        if (det == 0)
            throw new Exception("This matrix can't be inversed");
        a = Transpose(a);
        var result = new Matrix(a.CountColumn, a.CountRow);
        for (var i = 0; i < a.CountRow; i++)
        for (var j = 0; j < a.CountColumn; j++)
            result[i, j] = Cofactor(a, i, j);
        a = result * (1 / det);
        return a;
    }

    public static decimal Determinant(Matrix a)
    {
        if (a.CountRow != a.CountColumn) throw new Exception("Matrix should be square");
        decimal determinant = 0;
        switch (a.CountRow)
        {
            case 1:
                return a[0, 0];
            case 2:
                return a[0, 0] * a[1, 1] - a[1, 0] * a[0, 1];
        }
        var j = 0;
        for (var i = 0; i < a.CountRow; i++)
            determinant += a[i, j] * Cofactor(a, i, j);
        return determinant;
    }

    public static Matrix Transpose(Matrix a)
    {
        var result = new Matrix(a.CountColumn, a.CountRow);
        for (var i = 0; i < result.CountRow; i++)
        for (var j = 0; j < result.CountColumn; j++)
            result[i, j] = a[j, i];
        return result;
    }

    public static decimal Cofactor(Matrix a, int row, int column) =>
        (decimal)Math.Pow(-1, row + column + 2) * Determinant(SqrMinor(a, row, column));

    public static Matrix operator *(Matrix a, Matrix b) => Multiply(a, b);
    public static Matrix operator *(Matrix a, int b) => MultiplyByNumber(a, b);
    public static Matrix operator *(Matrix a, decimal b) => MultiplyByNumber(a, b);
    public static Matrix operator *(Matrix a, double b) => MultiplyByNumber(a, b);
    public static Matrix operator +(Matrix a, Matrix b) => Sum(a, b);
    public static Matrix operator -(Matrix a, Matrix b) => Subtract(a, b);
}