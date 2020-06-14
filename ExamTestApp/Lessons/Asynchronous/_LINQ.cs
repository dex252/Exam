using System;
using System.Linq;

namespace ExamTestApp.Lessons.Asynchronous
{
    public class _LINQ
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
}