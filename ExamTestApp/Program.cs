using System;
using System.Diagnostics;

namespace ExamTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region start
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            #endregion

            var Common = new Common();
            Common.Asynchronous.Task.TaskWaitAny();
          
            #region end
            stopWatch.Stop();
            Console.WriteLine($"\nTask completed in {stopWatch.ElapsedMilliseconds} ms. Press any key to end.");
            Console.ReadKey();
            #endregion

        }
    }
}
