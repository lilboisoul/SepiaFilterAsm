using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using JA_Sepia.Enums;

namespace JA_Sepia.Sepia
{
    class SepiaManager
    {
        private List<Task> tasks = new List<Task>();
        private int numberOfThreads;
        private int bitmapWidth, bitmapHeight;
        private IntPtr dataPtr;
        private byte sepiaCoefficient;
        private Method chosenMethod;
        
        private SepiaInterface method;

        private void SelectMethod(Method chosenMethod)
        {
            switch (chosenMethod)
            {
                case Method.ASM:
                    method = new SepiaAssembly();
                    break;
                case Method.CPP:
                    method = new SepiaCpp();
                    break;
                default:
                    method = null;
                    break;
            }
        }

        public SepiaManager(int numOfTasks, byte sepiaCoefficient, IntPtr dataPtr, int bitmapWidth, int bitmapHeight, Method chosenMethod)
        {
            this.numberOfThreads = numOfTasks;
            this.dataPtr = dataPtr;
            this.sepiaCoefficient = sepiaCoefficient;
            this.bitmapWidth = bitmapWidth;
            this.bitmapHeight = bitmapHeight;
            this.chosenMethod = chosenMethod;
            SelectMethod(chosenMethod);
        }

        private void InitializeTasks()
        {
            tasks.Clear();

            int rowsPerTask = bitmapHeight / numberOfThreads;
            int remainder = bitmapHeight % numberOfThreads;
            int rows, sectionSize, RGBValuesInRow = bitmapWidth * 3;
            int end = 0;

            for (int i = 0; i < numberOfThreads; i++)
            {
                int startIndex = end;  //starting index of the n+1 section is the end of the n-th section
                rows = rowsPerTask;     

                if (remainder > 0) //in case of uneven distribution of rows between threads
                {
                    rows+=1;
                    remainder -= 1;
                }

                sectionSize = rows * RGBValuesInRow;

                end = startIndex + sectionSize;
                int endIndex = end;

                tasks.Add(new Task(() => method.RunAlgorithm(dataPtr, sepiaCoefficient, startIndex, endIndex)));
            }
        }

        public void Execute(bool enableConsoleOutput)
        {
            String executedAlgorithm;
            if(chosenMethod == Method.CPP)
            {
                executedAlgorithm = "C++";
            }
            else {
                executedAlgorithm = "Assembly";
            }
            TimeSpan elapsedTime;
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            InitializeTasks();
            Parallel.ForEach(tasks, (task) => task.Start());
            Task.WaitAll(tasks.ToArray());

            stopwatch.Stop();
            elapsedTime = stopwatch.Elapsed;
            if (enableConsoleOutput)
            {
                Console.WriteLine("Method: " + executedAlgorithm);
                Console.WriteLine("Algorithm took " + elapsedTime.TotalMilliseconds + "ms to complete.");
            }

        }
    }
}
