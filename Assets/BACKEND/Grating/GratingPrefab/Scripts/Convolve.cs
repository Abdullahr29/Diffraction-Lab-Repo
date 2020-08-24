using System;
using Random = UnityEngine.Random;
using COMPLEX = FFT.COMPLEX;

public static class Convolve 
{   
    //A simple iterative convolution algorithm to be used when the kernel is much smaller than the bitmap, for the gaussian blurring 
    public static float[,] simpleConv(float[,] input, float[,] kernel, int resolution){
        // find center position of kernel (half of kernel size)
        int size = kernel.GetLength(0);
        float[,] result = new float[resolution, resolution];
        float sum;
        int posX, posY;
        for(int i = 0; i < resolution; i++) {
            for(int j = 0; j < resolution; j++) {
                sum = 0;
                posX = i - size / 2;
                posY = j - size / 2;     
                for (int k = 0; k < size; k++) {
                    for (int l = 0; l < size; l++) {
                        if (((posX + k) > -1) && ((posX + k) < resolution) && ((posY + l) > -1) && ((posY + l) < resolution)) {
                            sum = sum + (input[posX + k, posY + l] * kernel[k, l]);
                        }
                    }
                }              
                result[i, j] = sum;
            }
        }
        return result;
    }

    //FFT convolution algorithm for images and kernels of same size
    public static float[,] convFFT(COMPLEX[,] input, float[,] kernel) {
        /*
            Need to prepare the output of the intensity matrix as a COMPLEX matrix(after shifting before normalising)
            Need to rotate kernel by 180 
            Need to do a 2dFFT on roatated kernel and then shift, kept as COMPLEX
            Need to do an element wise multiplication of both matrices, which are complex numbers, complex multiplication
            Need to do inverse FFT of resulting matrix to get final convolution
         */
        MatrixCalc.rotate180(kernel);
        FFT kernelMap = new FFT(kernel);
        kernelMap.ForwardFFT();
        COMPLEX[,] kernelFFT = kernelMap.FFTShifted;
        COMPLEX[,] result = MatrixCalc.elemMultiply(input, kernelFFT);
        kernelMap.InverseFFT(result);
        return kernelMap.FFTLog;
    }
         
    //matrix generation algorithms

    //takes the bitmap and an angle and supperposes the rows of the bitmap with an exponential with its peak at where the angle points on the bitmap to achieve skew
    public static float[,] biasSkew(float[,] output, float angle, int resolution, int slits) {//can be further optimised to take place with FFT
        float[] mult = new float[resolution];
        //angle above 30, output is 0

        int fac = (int)((resolution / 2) / 30);//divided the bitmap in 1/30ths
        int loc = (int)(fac * angle);//multiplies it by the angle to see highest illumination point 
        float c = (float)(0.02 * -1 * loc);//values for the equation of the exponential
        float lim = loc + (resolution / 2);//finding the highest point of intensity 
        int count = -1 * (resolution / 2);

        float ampSkew = Data.param[slits, 0] * angle;//calling control parameters from the data matrix 
        float ampOrig = Data.param[slits, 1] * angle;

        for (int i = 0; i < (int)(lim + 1); i++) {
            mult[i] = (float)(ampSkew * Math.Exp(0.02 * (count + i) + c));
        }
        for (int j = (int)(lim + 1); j < resolution; j++) {
            mult[j] = (float)(ampSkew * Math.Exp(-1 * 0.02 * (count + j) - 1 * c));
        }
        for (int i = 0; i < resolution; i++) {
            for (int j = 0; j < resolution; j++) {
                output[i, j] = output[i, j] + (float)(output[i, j] * ampOrig) + (output[i, j] * mult[j]);
            }
        }

        return output;
    }

    //can be used to generate a kernel of a specified size to be used in a convolution to smooth output
    public static float[,] genFocalGaussian(int resolution, float weight, int slits) {//weight can be controlled using data matrix
        double[,] kernel = new double[resolution, resolution];
        weight = Data.param[slits, 5];
        double kernelSum = 0;
        int foff = (resolution - 1) / 2;
        double distance;
        double constant = 1d / (2 * Math.PI * weight * weight);
        for (int y = -foff; y <= foff; y++) {
            for (int x = -foff; x <= foff; x++) {
                distance = ((y * y) + (x * x)) / (2 * weight * weight);
                kernel[y + foff, x + foff] = constant * Math.Exp(-distance);
                kernelSum += kernel[y + foff, x + foff];
            }
        }
        for (int y = 0; y < resolution; y++) {
            for (int x = 0; x < resolution; x++) {
                kernel[y, x] = (kernel[y, x] * 1d) / kernelSum * 100;
            }
        }
        return MatrixCalc.convertToFloat(kernel);
    }

    //generates noise in a tiered fashion at specified values 
    public static float[,] genNoise(float[,] mat, float distance, float focalLength, int slits) {//distance is from the lens, 0.5 is the focal length, max is 1,
        float min, max, factor; 
        distance = Math.Abs(distance - focalLength);
        //float control = Data.param[slits, 3];; also noiseCont from Data matrix
        //float distFact = distance * control;
        int size = mat.GetLength(0);
        //if(distance == 0) {
        //    distFact = 1;
        //}
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                
                if (mat[i, j] < 0.05f) {//may need to be parameterised and put into the data class 
                    mat[i, j] = Random.Range(0, 0.07f);
                }
                else {
                    factor = Data.param[slits, 2];                   
                    max = mat[i, j] * factor;
                    min = -1f * max;
                    mat[i, j] = mat[i,j] + Random.Range(min, max);
                }             
                
            }
        }
        return mat;
    }

    //creates dirac delta functions at specific seperation to be convolved with bitmap to create multiple slit bitmaps
    public static int[,] genNSlits(int slits, int size, float slitSep, float maxSlitDim) {
        //multiple slits using dirac delta matrices
        int[,] result = new int[size,size];
        double ts = size / 256;//thickness of dirac delta, needs to be somewthing resolvable, may be set to 1
        int tSW = (int)Math.Ceiling(ts); //the width of a single dirac-delta spike
        int zsf = (int)(slitSep/maxSlitDim); //the "number of 0s" between spikes, slit seperation, need to see if width after conv makes noticeable difference
        int count;
        int numZeros = (size - ((tSW * slits) + (zsf * (slits - 1))))/2;//the number of "0s" on the outsides of the dirac-delta spikes
        for(int i = 0; i < size; i++) { 
            count = 0;
            for(int j = 0; j < numZeros-1; j++) {
                result[i, count] = 0;
                count++;
            }         
            for(int k = 0; k < tSW; k++) {
                result[i, count] = 1;
                count++;
            }
            for(int k = 1; k < slits; k++) {
                for(int l = 0; l < zsf; l++) {
                    result[i, count] = 0;
                    count++;
                }
                for(int l = 0; l < tSW; l++) {
                    result[i, count] = 1;
                    count++;
                }                           
            }                                                                                  
            for (int j = size - numZeros; j < size; j++) {
                result[i, count] = 0;
                count++;
            }
        }
        return result;
    } 

    public static float[,] genBeamXSection(float polAngle, float aziAngle, int size) {//unimplemented
        //cross section and angles of incidence of beam on slits 
        //theta and phi measure the deviation from the normal of the grating and do not measure the angle in a normal way
        float control3 = 1.0f;
        float control4 = 1.0f;
        float control5 = 0.0f;
        float theta_beam = aziAngle;
        float phi_beam = polAngle;
        float[,] illuminationmatrix = new float[size, size];
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                illuminationmatrix[i, j] = (control3 * theta_beam * j + control4 * phi_beam * i) + control5;
                // Debug.Log(illuminationmatrix[i, j]);
            }
        }
        return illuminationmatrix;
    }

    public static float[,] inverseSquare(float[,] mat, float dist, int size) {//dist = distance from grating to screen.
        //method to create the r^{-2} law for intensity of matrix values based on screen distance
        for (int i = 0; i < size; i++) {
            for (int j = 0; j < size; j++) {
                mat[i, j] = mat[i, j] * (1 / (dist * dist));
            }
        }

        return mat;
    }


}
