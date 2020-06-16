namespace ExamTestApp.Lessons.Asynchronous
{
    /// <summary>
    /// Skill 1-1
    /// Когда использовать Parallel.ForEach, а когда PLINQ
    /// https://habr.com/ru/post/135942/
    /// </summary>
    public class Asynchronous
    {
        public _Parallels Parallel => new _Parallels();
        public _LINQ Linq => new _LINQ();
        public _Task Task => new _Task();
        public _Threads Thread => new _Threads();
    }
}