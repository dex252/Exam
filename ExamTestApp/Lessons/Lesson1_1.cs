using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExamTestApp.Lessons
{
    public class Lesson1_1
    {
        /// <summary>
        /// Запускает указанные задачи и ждёт завершения каждой из них.
        /// Невозможно контролировать очередность их запуска и процессоры, на которых они будут выполнены
        /// </summary>
        public void _1_Parallel_Invoke()
        {
            Parallel.Invoke(Task_1, Task_2, Task_3);
            Console.WriteLine("Finish processing. Press any key to end.");
            Console.ReadKey();
        }

        #region Tasks

        private void Task_3()
        {
            Console.WriteLine("Task 3 starting");
            Thread.Sleep(7000);
            Console.WriteLine("Task 3 ending");
        }

        private void Task_2()
        {
            Console.WriteLine("Task 2 starting");
            Thread.Sleep(3000);
            Console.WriteLine("Task 2 ending");
        }

        private void Task_1()
        {
            Console.WriteLine("Task 1 starting");
            Thread.Sleep(1000);
            Console.WriteLine("Task 1 ending");
        }

        #endregion

    }
}