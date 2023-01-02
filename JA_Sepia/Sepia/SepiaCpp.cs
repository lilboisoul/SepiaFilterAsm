using System;
using System.Runtime.InteropServices;

namespace JA_Sepia.Sepia
{
    internal class SepiaCpp : SepiaInterface
    {
        [DllImport(@"C:\Users\kdusz\source\repos\JA_Sepia\x64\Release\SepiaCpp.dll")]
        static extern void SepiaCppAlgorithm(IntPtr dataPtr, byte sepiaCoefficient, int startIndex, int endIndex);

        public override void RunAlgorithm(IntPtr dataPtr, byte sepiaCoefficient, int startIndex, int endIndex)
        {
            SepiaCppAlgorithm(dataPtr, sepiaCoefficient, startIndex, endIndex);
        }
    }
}
