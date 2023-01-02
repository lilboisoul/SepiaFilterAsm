#include "pch.h"
#include <iostream>
void Sepia(BYTE* pixels, BYTE sepiaCoefficient, int startIndex, int endIndex)
{
    for (int i = startIndex; i < endIndex; i += 3)
    {
        //Desaturating pixels 
        BYTE b = pixels[i];
        BYTE g = pixels[i + 1];
        BYTE r = pixels[i + 2];

        BYTE average = (b + g + r) / 3;

        //Adding sepia coefficients to red and green channels
        pixels[i] = average;
        average + sepiaCoefficient > 255 ? pixels[i + 1] = 255 : pixels[i + 1] = average + sepiaCoefficient;
        average + sepiaCoefficient*2 > 255 ? pixels[i + 2] = 255 : pixels[i + 2] = average + (sepiaCoefficient*2);
    }
}

extern "C" __declspec(dllexport) void SepiaCppAlgorithm(BYTE* pixels, BYTE sepiaCoefficient, int startIndex, int endIndex)
{
    Sepia(pixels, sepiaCoefficient, startIndex, endIndex);
}