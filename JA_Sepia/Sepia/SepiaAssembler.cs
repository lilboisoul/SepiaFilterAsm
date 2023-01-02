using System;
using System.Runtime.InteropServices;

namespace JA_Sepia.Sepia
{
    internal class SepiaAssembly : SepiaInterface
    {
        [DllImport(@"C:\Users\kdusz\source\repos\JA_Sepia\x64\Debug\SepiaAsm.dll")]
        static extern void SepiaAsmAlgorithm(IntPtr dataPtr, byte sepiaCoefficient, int startIndex, int endIndex);

        public override void RunAlgorithm(IntPtr dataPtr, byte sepiaCoefficient, int startIndex, int endIndex)
        {
            SepiaAsmAlgorithm(dataPtr, sepiaCoefficient, startIndex, endIndex);
        }
    }
}
