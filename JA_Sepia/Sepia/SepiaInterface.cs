using System;

namespace JA_Sepia.Sepia
{
    public abstract class SepiaInterface
    {
        public abstract void RunAlgorithm(IntPtr dataPtr, byte sepiaCoefficient, int startIndex, int endIndex);
    }
}
