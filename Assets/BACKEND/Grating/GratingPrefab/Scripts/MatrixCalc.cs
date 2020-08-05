using COMPLEX = FFT.COMPLEX;

public class MatrixCalc {


    public static COMPLEX[,] elemMultiply(COMPLEX[,] mat1, COMPLEX[,] mat2) {
        //both matrices have to be the same size
        int r = mat1.GetLength(0);
        int c = mat1.GetLength(1);

        COMPLEX[,] result = new COMPLEX[r, c];
        for (int i = 0; i < r; i++) {
            for (int j = 0; j < c; j++) {
                result[i, j] = multiplyComplex(mat1[i, j], mat2[i, j]);
            }
        }
        return result;
    }

    public static void rotate180(float[,] mat) {
        int r = mat.GetLength(0);
        int c = mat.GetLength(1);

        transpose(mat, r, c);
        reverseColumns(mat, c);
        transpose(mat, r, c);
        reverseColumns(mat, c);
    }

    public static void resizeMat(float[,] mat, int dimx, int dimy) {

    }

    public static COMPLEX multiplyComplex(COMPLEX num1, COMPLEX num2) {
        COMPLEX result;
        double real, img;
        real = (num1.real * num2.real) - (num1.imag * num2.imag);
        img = (num1.real * num2.imag) + (num1.imag * num2.real);
        result = new COMPLEX(real, img);
        return result;
    }

    public static void reverseColumns(float[,] arr, int C) {
        float t;
        for (int i = 0; i < C; i++) {
            for (int j = 0, k = C - 1;
                 j < k; j++, k--) {
                t = arr[j, i];
                arr[j, i] = arr[k, i];
                arr[k, i] = t;
            }
        }
    }

    public static void transpose(float[,] arr, int R, int C) {
        float t;
        for (int i = 0; i < R; i++) {
            for (int j = i; j < C; j++) {
                t = arr[i, j];
                arr[i, j] = arr[j, i];
                arr[j, i] = t;
            }
        }
    }

        
    public static void convertToComplex(float[,] mat) {

    }

    private double[,] convertToDouble(COMPLEX[,] input) {
        double[,] temp = new double[input.GetLength(0), input.GetLength(1)];
        for (int i = 0; i < temp.GetLength(0); i++) {
            for (int j = 0; j < temp.GetLength(1); j++) {
                temp[i, j] = input[i, j].Magnitude();
            }
        }
        return temp;
    }

    private double[,] convertToDouble(int[,] input) {
        double[,] temp = new double[input.GetLength(0), input.GetLength(1)];
        for (int i = 0; i < temp.GetLength(0); i++) {
            for (int j = 0; j < temp.GetLength(1); j++) {
                temp[i, j] = input[i, j];
            }
        }
        return temp;
    }

    public static float[,] convertToFloat(int[,] input) {
        float[,] temp = new float[input.GetLength(0), input.GetLength(1)];
        for (int i = 0; i < temp.GetLength(0); i++) {
            for (int j = 0; j < temp.GetLength(1); j++) {
                temp[i, j] = input[i, j];
            }
        }
        return temp;
    }

    public static float[,] convertToFloat(double[,] input) {
        float[,] temp = new float[input.GetLength(0), input.GetLength(1)];
        for (int i = 0; i < temp.GetLength(0); i++) {
            for (int j = 0; j < temp.GetLength(1); j++) {
                temp[i, j] = (float)input[i, j];
            }
        }
        return temp;
    }

    public static double[,] convertToDouble(float[,] input) {
        double[,] temp = new double[input.GetLength(0), input.GetLength(1)];
        for (int i = 0; i < temp.GetLength(0); i++) {
            for (int j = 0; j < temp.GetLength(1); j++) {
                temp[i, j] = input[i, j];
            }
        }
        return temp;
    }

    public static int[,] convertToInt(float[,] input) {
        int[,] temp = new int[input.GetLength(0), input.GetLength(1)];
        for (int i = 0; i < temp.GetLength(0); i++) {
            for (int j = 0; j < temp.GetLength(1); j++) {
                temp[i, j] = (int)input[i, j];
            }
        }
        return temp;
    }

}
