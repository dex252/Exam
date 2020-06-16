using System;
using System.Threading;
using System.Threading.Tasks;

namespace ExamTestApp.Lessons.Asynchronous
{
    public class _Task
    {
        /// <summary>
        /// Запускает задачу Task методом Start. Wait ожидает завершения задачи.
        /// При отсутствии Wait основная задача продолжит выполнение, не дожидаясь завершения асинхронного вызова.
        /// </summary>
        public void TaskStart()
        {
            Task task = new Task(DoWork);
            task.Start();
            task.Wait();
        }
        /// <summary>
        /// Объявить задачу и запустить его сразу после объявления.
        /// </summary>
        public void TaskRun()
        {
            Task task = System.Threading.Tasks.Task.Run(DoWork);
            //  Analog
            //  Task task = System.Threading.Tasks.Task.Run(()=>DoWork());

            task.Wait();
        }
        /// <summary>
        /// Запуск задачи и возврат результата его выполнения. Программма продолжит выполнение только после завершения задачи.
        /// </summary>
        public void TaskResult()
        {
            Task<int> task = System.Threading.Tasks.Task.Run(CalculateResult);
            Console.WriteLine(task.Result);
        }
        /// <summary>
        /// Запускает асинхронно задачи, не сохраняя порядка выполнения задач.
        /// Метод WaitAll ждет завершения всех этих задач.
        /// Метод WaitAll перехватывает любые исключения, которые могут быть вызваны во время выполнения задач.
        /// </summary>
        public void TaskWaitAll()
        {
            Task[] tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                //  Не стоит передавать итератор в качестве параметра, он всегда будет равен пределу цикла.
                //  Это происходит из-за передачи итератора по ссылке, это легко обойти с помощью копирования итератора 
                //  в локальную переменную
                int taskNum = i;
                tasks[i] = System.Threading.Tasks.Task.Run(() => DoWork(taskNum));
            }
            System.Threading.Tasks.Task.WaitAll(tasks);
        }
        /// <summary>
        /// Запускает асинхронно задачи, не сохраняя порядка выполнения задач.
        /// Метод WaitAny ждет завершения только первой завершенной задачи, остальные продолжат выполнение (как в гонке лошадей).
        /// </summary>
        public void TaskWaitAny()
        {
            Task[] tasks = new Task[10];

            for (int i = 0; i < 10; i++)
            {
                //  Не стоит передавать итератор в качестве параметра, он всегда будет равен пределу цикла.
                //  Это происходит из-за передачи итератора по ссылке, это легко обойти с помощью копирования итератора 
                //  в локальную переменную
                int taskNum = i;
                tasks[i] = System.Threading.Tasks.Task.Run(() => DoWork(taskNum));
            }
            System.Threading.Tasks.Task.WaitAny(tasks);
        }
        /// <summary>
        /// Начало следующей задачи по завершению предыдущей.
        /// </summary>
        public void ContinuationTasks()
        {

            Task task = Task.Run(HelloTask);
            task.ContinueWith(prev => WorldTask()).Wait();
        }
        /// <summary>
        /// Параметр TaskContinuationOptions определяет поведение задачи при выполнении.
        /// В данном случае: OnlyOnRanToCompletion - выполнение будет продолжено, если задача завершилась успешно.
        /// OnlyOnFaulted - задача будет выполнена только при возникновении исключений соответственно.
        /// Подробности: https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.taskcontinuationoptions?view=netcore-3.1
        /// </summary>
        public void ContinuationTasksWithException()
        {

            Task task = Task.Run(HelloTask);
            task.ContinueWith(prev => WorldTask(),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(prev => ExceptionTask(),
                TaskContinuationOptions.OnlyOnFaulted);
        }
        /// <summary>
        /// Создание задачи, порождающего 10 дочерних, каждый из которых с помощью TaskCreationOptions.AttachedToParent
        /// присоединяется к родителю. После основная задача ждет завершения всех дочерних.
        /// При отсутствии AttachedToParent все задачи будут независимы относительно друг друга.
        /// </summary>
        public void ContinuationChildTasks()
        {
            var parent = Task.Factory.StartNew(() => {
                Console.WriteLine("Parent starts");
                for (int i = 0; i < 10; i++)
                {
                    int taskNo = i;
                    Task.Factory.StartNew(
                        (x) => DoChild(x), // lambda expression
                        taskNo, // state object
                        TaskCreationOptions.AttachedToParent);
                }
            });

            parent.Wait(); // will wait for all the attached children to complete
        }
        #region PrivateGroup
        private void DoChild(object state)
        {
            Console.WriteLine("Child {0} starting", state);
            Thread.Sleep(new Random().Next(1000));
            Console.WriteLine("Child {0} finished", state);
        }
        private void DoWork(int i)
        {
            Console.WriteLine($"Task {i} starting");
            Thread.Sleep(new Random().Next(1000, 2000));
            Console.WriteLine($"Task {i} finished");
        }
        private void DoWork()
        {
            Console.WriteLine($"Task starting");
            Thread.Sleep(new Random().Next(1000, 2000));
            Console.WriteLine($"Task finished");
        }
        private int CalculateResult()
        {
            Console.WriteLine("Work starting");
            Thread.Sleep(2000);
            Console.WriteLine("Work finished"); ;
            return new Random().Next(0, 100);
        }

        private void HelloTask()
        {
            Thread.Sleep(1000);
            Console.WriteLine("Привет");
        }

        private void WorldTask()
        {
            Thread.Sleep(1000);
            Console.WriteLine(" Мир");
        }

        private void ExceptionTask()
        {
            Thread.Sleep(1000);
            Console.WriteLine(" Exception");
        }
        #endregion
    }
}