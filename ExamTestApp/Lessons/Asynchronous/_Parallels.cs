using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExamTestApp.Lessons.Asynchronous
{
    public class _Parallels
    {
        /// <summary>
        /// Запускает указанные задачи и ждёт завершения каждой из них.
        /// Невозможно контролировать очередность их запуска и процессоры, на которых они будут выполнены.
        /// </summary>
        public void Invoke()
        {
            System.Threading.Tasks.Parallel.Invoke(this.Task_1, this.Task_2, this.Task_3);
        }
        /// <summary>
        /// Принимает 2 параметра: первый - IEnumerable коллекция, второй  - делегат Action для каждого элемента в списке.
        /// Задачи не выполняются в порядке, в котором были запущены.
        /// </summary>
        public void ForEach()
        {
            var items = Enumerable.Range(0, 200);
            System.Threading.Tasks.Parallel.ForEach(items, item =>
            {
                WorkOnItem(item);
            });
        }
        /// <summary>
        /// Принимает 3 аргумента: первый - начальный индекс элемента в цикле, второй - конечный индекс,
        /// третий - делегат Action, указывает на метод, который выполняется при каждой итерации.
        /// Не позволяет контролировать очередность запуска задач.
        /// </summary>
        public void For()
        {
            var items = Enumerable.Range(0, 200).ToArray();
            System.Threading.Tasks.Parallel.For(0, items.Length, index =>
            {
                WorkOnItem(index);
            });
        }
        /// <summary>
        /// Выполняет параллельные задачи до тех пор, пока не встретит элемент с индексом 200.
        /// Все, начавшиеся раннее задачи, будут выполнены, но новые начаты не будут.
        /// </summary>
        public void For_Loop_State_Stop()
        {
            var stopIndex = new Random().Next(75, 150);
            var items = Enumerable.Range(0, 200).ToArray();

            ParallelLoopResult result = System.Threading.Tasks.Parallel.For(0, items.Count(),
                (int i, ParallelLoopState loopState) =>
                {
                    if (i == stopIndex)
                    {
                        loopState.Stop();
                        Console.WriteLine($"--------------Equal {stopIndex}: Stop process--------------------");
                        return;
                    }

                    if (loopState.IsStopped)
                    {
                        return;
                    }

                    WorkOnItem(items[i]);
                });
        }
        /// <summary>
        /// Выполняет параллельные задачи, останавливаясь при достижении break индекса.
        /// Все итерации, индекс которых ниже break будут выполнены, выше - нет.
        /// Если итерации выше указнной уже были начаты, то они будут выполнены.
        /// </summary>
        public void For_Each_Loop_State_Break()
        {
            var breakIndex = new Random().Next(10, 20);
            var items = Enumerable.Range(0, 500);

            ParallelLoopResult result = System.Threading.Tasks.Parallel.ForEach(items, (i, state) =>
            {
                if (state.ShouldExitCurrentIteration)
                {
                    if (state.LowestBreakIteration < i)
                        return;
                }
                if (i == breakIndex)
                {
                    state.Stop();
                    Console.WriteLine($"--------------Equal {breakIndex}: Break process--------------------");
                }

                WorkOnItem(i);
            });
        }

        #region PrivateGroup
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

        private void WorkOnItem(object e)
        {
            Console.WriteLine($"Started working on: {e}");
            var timer = new Random().Next(500, 3000);
            Thread.Sleep(timer);
            Console.WriteLine($"Finished working on: {e} for {timer} ms");
        }
        #endregion
    }
}