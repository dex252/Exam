using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExamTestApp.Lessons
{
    /// <summary>
    /// Skill 1-1
    /// Когда использовать Parallel.ForEach, а когда PLINQ
    /// https://habr.com/ru/post/135942/
    /// </summary>
    public class Parallel
    {
        public IEnumerable IEnumerab1e => new IEnumerable();
        public LINQ Linq => new LINQ();
        public Tasked Task => new Tasked();
        public class Tasked
        {

        }
        public class LINQ
        {
            /// <summary>
            /// Асинхронный запрос к коллекции. Порядок элементов не соблюдается.
            /// </summary>
            public void LINQ_Query()
            {
                var people = Service.GetPersonList();

                var result = people.AsParallel().Where(e => e.City == "London");
                //  Analog:
                //  var result = from person in people.AsParallel() where person.City == "London" select person;

                foreach (var person in result)
                {
                    Console.WriteLine($"{person.Name}: {person.City}");
                }
            }

            /// <summary>
            /// Асинхронный запрос к коллекции.
            /// WithDegreeOfParallelism(4) - максимально число одновременно выполняемых задач.
            /// WithExecutionMode - устанавливает режим выполнения асинхронного запроса (Default - запрос будет распараллелен только в том случае,
            /// если AsParallel решит, что операция будет ускорена. ForceParallelism - операция в любом случае будет выполнена
            /// асинхронно, независимо от решения AsParallel).
            /// </summary>
            public void LINQ_Query_With_Parallelism()
            {
                var people = Service.GetPersonList();

                var result = people.AsParallel()
                    .WithDegreeOfParallelism(4)
                    .WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                    .Where(e => e.City == "London");

                foreach (var person in result)
                {
                    Console.WriteLine($"{person.Name}: {person.City}");
                }
            }
            /// <summary>
            /// Асинхронный запрос к коллекции. Порядок элементов соблюдается.
            /// Выполнение может замедлить вопрос, так как в этом случае организовывается вывод реультатов в том порядке,
            /// что и исходные данные. Но само выполнение запроса происходит не по порядку.
            /// </summary>
            public void LINQ_Query_As_Order()
            {
                var people = Service.GetPersonList();

                var result = people.AsParallel().AsOrdered().Where(e => e.City == "London");
                //  Analog:
                //  var result = from person in people.AsParallel() where person.City == "London" select person;

                foreach (var person in result)
                {
                    Console.WriteLine($"{person.Name}: {person.City}");
                }
            }
            /// <summary>
            /// Выполняет асинхронный запрос на выборку и сортировку.
            /// AsSequential говорит, что оставшаяся часть запроса должна быть выполнена синхронно.
            /// Take (4) - выбирает 4 первых элемента созданной коллекции.
            /// </summary>
            public void LINQ_Query_As_Sequential()
            {
                var people = Service.GetPersonList();

                var result = people.AsParallel()
                    .Where(e => e.City == "London")
                    .OrderBy(e => e.Name).
                    Select(e => new { Name = e.Name }).
                    AsSequential().Take(4);

                foreach (var person in result)
                {
                    Console.WriteLine($"{person.Name}");
                }
            }
            /// <summary>
            /// ForAll предназначен для применения к каждому элементу коллекции асинхронного вызова функции без
            /// учета порядка элементов в коллекции. В данном случае элементы выводятся на экран, но может быть вызван
            /// любой делегат Action. Кроме того, итерация начнется ДО завершения запроса.
            /// </summary>
            public void LINQ_Query_For_All()
            {
                var people = Service.GetPersonList();
                people.AsParallel()
                    .Where(e => e.City == "London")
                    .ForAll(e => Console.WriteLine($"{e.Name}: {e.City}"));
            }
            /// <summary>
            /// Использование AggregateException в LINQ запросе, возвращает число ошибок при заданном условии.
            /// Запрос будет выполнен в любом случае.
            /// </summary>
            public void LINQ_Query_For_All_With_Exception()
            {
                var people = Service.GetPersonList();
                try
                {
                    people.AsParallel()
                        .Where(e => CheckCity(e.City))
                        .ForAll(e => Console.WriteLine($"{e.Name}: {e.City}"));
                }
                catch (AggregateException e)
                {
                    Console.WriteLine(e.InnerExceptions.Count() + " exceptions.");
                }
            }

            #region PrivateGroup

            private bool CheckCity(string name)
            {
                if (string.IsNullOrEmpty(name)) throw new ArgumentException();
                return name == "London";
            }
            #endregion
        }

        public class IEnumerable
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
}