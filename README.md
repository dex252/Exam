# Miles R. - Exam Ref 70-483. Programming in C# - 2019

1.	Chapter 1: Manage program flow
	-	Asynchronous (Skill 1.1: Implement multithreading and asynchronous processing)
		-	Parallel
			-	Invoke
			-	ForEach
			-	For
			-	For (ParallelLoopState Stop)
			-	For (ParallelLoopState Break)
		-	LINQ
			-	AsParallel
			-	AsParallel.WithDegreeOfParallelism.WithExecutionMode
			-	AsParallel.AsOrdered
			-	AsParallel.AsSequential.AsTake
			-	AsParallel.ForAll
			-	AsParallel.ForAll (AggregateException)
		-	Task
			-	Start & Wait
			-	Run & Wait
			-	Run & Result
			-	WaitAll
			-	WaitAny
			-	ContinuationTasks
			-	ContinuationTasks (TaskContinuationOptions.OnlyOnRanToCompletion & TaskContinuationOptions.OnlyOnFaulted)
			-	ContinuationTasks (TaskCreationOptions.AttachedToParent)
		-	Thread
			-	Thread.Start
			-	ThreadStart delegate (old NET version)
			-	Thread.Start (Analog)
			-	ThreadParameterized
			-	ThreadParameterized (Analog)
			-	Thread.Abort Thread.Interrupt
			-	Thread.Abort Safe Variation
			-	Thread.Join
			-	ThreadLocal
			-	ThreadExecutionContext
			-	ThreadPools