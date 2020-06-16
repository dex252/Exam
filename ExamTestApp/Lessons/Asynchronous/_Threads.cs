using System;
using System.Threading;

namespace ExamTestApp.Lessons.Asynchronous
{
    public class _Threads
    {
        /// <summary>
        /// Инициализация и старт потока
        /// </summary>
        public void ThreadStart()
        {
            Thread thread = new Thread(ThreadHello);
            thread.Start();
        }
        /// <summary>
        /// В ранних версиях NET требовалось создать делегат ThreadStart, чтобы указать метод для потока.
        /// Теперь это не требуется.
        /// </summary>
        public void ThreadStartOld()
        {
            ThreadStart ts = new ThreadStart(ThreadHello);
            Thread thread = new Thread(ts);
            thread.Start();
        }
        /// <summary>
        /// Указать задачу для потока можно через делегат.
        /// </summary>
        public void ThreadStartAnalog()
        {
            Thread thread = new Thread(() =>
            {
                Console.WriteLine("Hello from the thread");
                Thread.Sleep(1000);
            });

            thread.Start();
        }
        /// <summary>
        /// Старт потока с параметрами (передача из основного потока).
        /// Как и в случае с ThreadStart, объявление можно упростить.
        /// </summary>
        public void ThreadParameterized()
        {
            ParameterizedThreadStart ps = new ParameterizedThreadStart(WorkOnData);
            Thread thread = new Thread(ps);
            thread.Start(99);
        }
        /// <summary>
        /// Запись, аналогичная ThreadParameterized.
        /// Данные, передаваемые в поток,  всегда передаются по ссылке, поэтому никогда нельзя быть уверенным, что данные
        /// передаются с определенным типом данных.
        /// </summary>
        public void ThreadParameterizedWithLambaExpression()
        {
            Thread thread = new Thread((data) =>
            {
                WorkOnData(data);
            });
            thread.Start(99);
        }
        /// <summary>
        /// НЕ РЕКОМЕНДУЕТСЯ
        /// Вызывает остановку потока и выдает исключение. Не являются потокобезопасными методами, кроме того
        /// Abort работает только в NET Framework. В NET Core используют Interrupt, но лучше не использовать их вовсе.
        /// https://www.infoworld.com/article/3105821/my-two-cents-on-the-thread-abort-and-thread-interrupt-methods.html
        /// </summary>
        public void ThreadAbort()
        {
            Thread tickThread = new Thread(() =>
            {
                while (true)
                {
                    Console.WriteLine("Tick");
                    Thread.Sleep(1000);
                }
            });

            tickThread.Start();

            Console.WriteLine("Press a key to stop the clock");
            Console.ReadKey();
           
            tickThread.Abort();

            Console.WriteLine("Press a key to exit");
            Console.ReadKey();
        }
        /// <summary>
        /// Потокобезопасный вариант завершения потока.
        /// </summary>
        public void ThreadAbortSafe()
        {
            bool tickRunning = true;
            Thread tickThread = new Thread(() =>
            {
                while (tickRunning)
                {
                    Console.WriteLine("Tick");
                    Thread.Sleep(1000);
                }
            });

            tickThread.Start();

            Console.WriteLine("Press a key to stop the clock");
            Console.ReadKey();
            tickRunning = false;
        }
        /// <summary>
        /// Метод Join не позволяет вызывающему потоку завершиться до окончания дочернего.
        /// Этот метод отлично подходит для синхронизации работы нескольких потоков.
        /// </summary>
        public void ThreadJoin()
        {
            Thread threadToWaitFor = new Thread(() =>
            {
                Console.WriteLine("Thread starting");
                Thread.Sleep(3000);
                Console.WriteLine("Thread done");
            });

            threadToWaitFor.Start();
            Console.WriteLine("Joining thread");
            threadToWaitFor.Join();
        }
        /// <summary>
        /// Синхронизация `случайных чисел`
        /// Для каждого потока будет передано свое значение, в данном случае изменение этого значения в одном потоке
        /// не приведет к изменению этого же значения в другом. В примере в оба потока передается один и тот же случайный генератор.
        /// Он генерирует одинаковые случайные значения для каждого потока. Если передать Random напрямую, то в каждом потоке
        /// будет сгенерированно отличное от предыдущих случайное число.
        /// </summary>
        public void ThreadLocalParams()
        {
            ThreadLocal<Random> RandomGenerator =
                new ThreadLocal<Random>(() => new Random(2));

            Thread t1 = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("t1: {0}", RandomGenerator.Value.Next(10));
                    Thread.Sleep(500);
                }
            });

            Thread t2 = new Thread(() =>
            {
                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("t2: {0}", RandomGenerator.Value.Next(10));
                    Thread.Sleep(500);
                }
            });
            t1.Start();
            t2.Start();
            Console.ReadKey();
        }
        /// <summary>
        /// Пример получения информации из контекста текущего потока.
        /// </summary>
        public void ThreadExecutionContext()
        {
            Thread.CurrentThread.Name = "Main method";
            DisplayThread(Thread.CurrentThread);

        }

        /// <summary>
        /// Подходит для создания большого числа однотипных краткосрочных задач.
        /// Не стоит использовать его, если задачи простаивают длительное время, кроме того локальные переменные состояния
        /// не очищаются при повторном использовании пула потоков. В пуле невозможно управлять приоритетом потоков, все потоки имеют приоритет фона.
        /// </summary>
        public void ThreadPools()
        {
            for (int i = 0; i < 50; i++)
            {
                int stateNumber = i;
                ThreadPool.QueueUserWorkItem(state => DoWork(stateNumber));
            }

        }
        #region PrivateGroup
        private void ThreadHello()
        {
            Console.WriteLine("Hello from the thread");
            Thread.Sleep(1000);
        }
        private void WorkOnData(object data)
        {
            Console.WriteLine("Working on: {0}", data);
            Thread.Sleep(1000);
        }
        private void DisplayThread(Thread t)
        {
            Console.WriteLine("Name: {0}", t.Name);
            Console.WriteLine("Culture: {0}", t.CurrentCulture);
            Console.WriteLine("Priority: {0}", t.Priority);
            Console.WriteLine("Context: {0}", t.ExecutionContext);
            Console.WriteLine("IsBackground?: {0}", t.IsBackground);
            Console.WriteLine("IsPool?: {0}", t.IsThreadPoolThread);

        }
        private void DoWork(object state)
        {
            Console.WriteLine("Doing work: {0}", state);
            Thread.Sleep(500);
            Console.WriteLine("Work finished: {0}", state);
        }
        #endregion
    }
}