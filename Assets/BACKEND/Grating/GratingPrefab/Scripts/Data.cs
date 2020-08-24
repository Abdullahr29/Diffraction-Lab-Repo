using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Data
{
    /*Matrix of parameters, where the row is the number of slits, 0 being a rectangular aperature and where the columns are:
    0 - ampSkew from Convolve.BiasSkew
    1 - ampOrig from Convolve.BiasSkew
    2 - noiseFac from Convolve.genNoise
    3 - noiseCont from Convolve.genNoise
    4 - nLoopFac from IntensityCalcFloat.noiseLoops
    5 - gaussWeight from Convolve.genFocalGaussian
    */
    public static readonly float[,] param = new float[,] {  {1.2f, 0.5f, 0.08f, 50f, 5f, 0},//tested
                                                            {1.2f, 0.5f, 0.08f, 50f, 5f, 0},//tested
                                                            {1.8f, 0.5f, 0.02f, 20f, 3f, 0},//tested
                                                            {1.2f, 0.5f, 0.08f, 50f, 5f, 0},//not tested
                                                            {1.2f, 0.5f, 0.08f, 50f, 5f, 0},//not tested 
                                                            {1.2f, 0.5f, 0.08f, 50f, 5f, 0}  };//not tested 




}
