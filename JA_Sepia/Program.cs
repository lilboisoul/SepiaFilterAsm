using System;
using JA_Sepia.Extensions;
using JA_Sepia.Sepia;
using JA_Sepia.Enums;
namespace Sepia
{
    class Program
    {
        static void Main()
        {
            try {
                //Get data from user
                    Console.WriteLine("Enter path to bitmap file: ");
                    String inputBitmapImagePath = Console.ReadLine(); // @"C:\Users\kdusz\Desktop\sample2.bmp";

                    Console.WriteLine("\nEnter path for output file: ");
                    String outputBitmapImagePath = Console.ReadLine(); // @"C:\Users\kdusz\Desktop\result2.bmp";

                    Console.WriteLine("\nChoose the algorithm:\n" +
                        "1 - ASSEMBLY \n" +
                        "2 - C++");

                    Method chosenMethod = ChooseMethod();

                    byte sepiaCoefficient = ChooseSepiaCoefficient();

                    byte numberOfThreads = ChooseNumberOfThreads();

                //Load bitmap and prepare to run the algorithm

                    IntPtr dataPointer = BitmapExtension.LoadBitmap(inputBitmapImagePath);
                    int bitmapWidth = BitmapExtension.GetBitmap().Width;
                    int bitmapHeight = BitmapExtension.GetBitmap().Height;

                    Prepare(sepiaCoefficient, inputBitmapImagePath, outputBitmapImagePath, chosenMethod);

                //Run algorithm

                    Run(numberOfThreads, sepiaCoefficient, inputBitmapImagePath, outputBitmapImagePath, chosenMethod);

                //Save result to file
                BitmapExtension.SaveBitmap(outputBitmapImagePath);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.ReadLine();

        }

        static void Prepare(byte sepiaCoefficient, String inputBitmapImagePath, String outputBitmapImagePath, Method chosenMethod)
        {
            Console.WriteLine("\nPreparing algorithm...");
            for (int i = 1; i <= 64; i++)
            {
                IntPtr dataPtr = BitmapExtension.LoadBitmap(inputBitmapImagePath);
                int bitmapWidth = BitmapExtension.GetBitmap().Width;
                int bitmapHeight = BitmapExtension.GetBitmap().Height;
                SepiaManager sepiaManager = new SepiaManager(i, sepiaCoefficient, dataPtr, bitmapWidth, bitmapHeight, chosenMethod);
                sepiaManager.Execute(false);
                BitmapExtension.SaveBitmap(outputBitmapImagePath);
                
            }
        }

        static void Run(int numberOfThreads, byte sepiaCoefficient, String inputBitmapImagePath, String outputBitmapImagePath, Method chosenMethod)
        {
            Console.WriteLine("\nNumber of threads: " + numberOfThreads);

            IntPtr dataPtr = BitmapExtension.LoadBitmap(inputBitmapImagePath);
            int bitmapWidth = BitmapExtension.GetBitmap().Width;
            int bitmapHeight = BitmapExtension.GetBitmap().Height;

            SepiaManager sepiaManager = new SepiaManager(numberOfThreads, sepiaCoefficient, dataPtr, bitmapWidth, bitmapHeight, chosenMethod);
            sepiaManager.Execute(true);

            BitmapExtension.SaveBitmap(outputBitmapImagePath);
            Console.WriteLine("\n");

        }

        static Method ChooseMethod()
        {
            Method method;
            String userInput = Console.ReadLine();
            if (userInput == "1")
            {
                method = Method.ASM;
            }
            else if(userInput == "2")
            {
                method = Method.CPP;
            }
            else
            {
                throw new NotImplementedException("Wrong method!");
            }

            return method;
        }

        static byte ChooseSepiaCoefficient()
        {
            Console.WriteLine("\nEnter the value of sepia coefficient [20 - 40]: ");
            byte sepiaCoefficient = byte.Parse(Console.ReadLine());

            if(sepiaCoefficient < 20 || sepiaCoefficient > 40)
            {
                throw new NotImplementedException("Wrong value!");
            }

            return sepiaCoefficient;
        }

        static byte ChooseNumberOfThreads()
        {
            Console.WriteLine("\nEnter the number of threads: [1 - 64]");
            byte numberOfThreads = byte.Parse(Console.ReadLine());

            if (numberOfThreads < 1 || numberOfThreads > 64)
            {
                throw new NotImplementedException("Wrong value!");
            }

            return numberOfThreads;
        }

    }
}





