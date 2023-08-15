using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

public enum Priority
{
    LOW,
    NORMAL,
    HIGH
}

public class CustomThreadPool
{
    private int threadCount;
    private BlockingCollection<Tuple<Priority, Action<object>, object>> taskQueue;
    private CancellationTokenSource cancellationTokenSource;
    private Task[] workerThreads;

    public CustomThreadPool(int threadCount)
    {
        this.threadCount = threadCount;
        taskQueue = new BlockingCollection<Tuple<Priority, Action<object>, object>>();
        cancellationTokenSource = new CancellationTokenSource();
        workerThreads = new Task[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            workerThreads[i] = Task.Run(() => Worker(cancellationTokenSource.Token));
        }
    }

    public bool Execute(Action<object> action, Priority priority, object value = null)
    {
        var task = new Tuple<Priority, Action<object>, object>(priority, action, value);
        return taskQueue.TryAdd(task);
    }

    public void Stop()
    {
        cancellationTokenSource.Cancel();
    }

    private void Worker(CancellationToken cancellationToken)
    {
        foreach (var task in taskQueue.GetConsumingEnumerable(cancellationToken))
        {
            if (cancellationToken.IsCancellationRequested)
                break;

            if (task.Item1 == Priority.LOW)
            {
                foreach (var higherPriorityTask in taskQueue)
                {
                    if (higherPriorityTask.Item1 == Priority.HIGH)
                    {
                        return;
                    }
                }
            }
            else if (task.Item1 == Priority.NORMAL)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (taskQueue.TryTake(out var highPriorityTask))
                    {
                        highPriorityTask.Item2(highPriorityTask.Item3);
                    }
                }
            }

            task.Item2(task.Item3);
        }
    }
}

public class Program
{
    static void Main(string[] args)
    {
        CustomThreadPool threadPool = new CustomThreadPool(threadCount: 3);

        threadPool.Execute(TaskMethod, Priority.LOW, "Low priority task 1");
        threadPool.Execute(TaskMethod, Priority.NORMAL, "Normal priority task 1");
        threadPool.Execute(TaskMethod, Priority.HIGH, "High priority task 1");
        threadPool.Execute(TaskMethod, Priority.LOW, "Low priority task 2");
        threadPool.Execute(TaskMethod, Priority.NORMAL, "Normal priority task 2");
        threadPool.Execute(TaskMethod, Priority.HIGH, "High priority task 2");

        Thread.Sleep(5000);

        threadPool.Stop();
        Console.WriteLine("Thread pool stopped.");

        Console.ReadLine();
    }

    static void TaskMethod(object value)
    {
        Console.WriteLine($"Executing task: {value}");
    }
}
