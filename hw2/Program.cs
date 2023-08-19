using System;
using System.Collections.Concurrent;
using System.Threading;

public enum Priority
{
    LOW,
    NORMAL,
    HIGH
}

public class CustomThreadPool
{
    private readonly int threadCount;
    private readonly BlockingCollection<Tuple<Action<object>, Priority, object>> taskQueue;
    private bool isStopped = false;

    public CustomThreadPool(int threadCount)
    {
        this.threadCount = threadCount;
        taskQueue = new BlockingCollection<Tuple<Action<object>, Priority, object>>();

        for (int i = 0; i < threadCount; i++)
        {
            var thread = new Thread(Worker);
            thread.Start();
        }
    }

    public bool Execute(Action<object> action, Priority priority, object value = null)
    {
        if (isStopped)
        {
            Console.WriteLine("ThreadPool is stopped. Cannot add new tasks.");
            return false;
        }

        taskQueue.Add(new Tuple<Action<object>, Priority, object>(action, priority, value));
        return true;
    }

    public void Stop()
    {
        isStopped = true;
        taskQueue.CompleteAdding();
    }

    private void Worker()
    {
        foreach (var taskInfo in taskQueue.GetConsumingEnumerable())
        {
            if (taskInfo.Item2 == Priority.LOW)
            {
                bool hasHigherPriorityTasks = taskQueue.Any(task => task.Item2 > Priority.LOW);
                if (hasHigherPriorityTasks)
                {
                    continue;
                }
            }
            else if (taskInfo.Item2 == Priority.NORMAL)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (taskQueue.TryTake(out var higherPriorityTask) && higherPriorityTask.Item2 == Priority.HIGH)
                    {
                        higherPriorityTask.Item1(higherPriorityTask.Item3);
                    }
                }
            }

            taskInfo.Item1(taskInfo.Item3);
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        CustomThreadPool threadPool = new CustomThreadPool(4);

        for (int i = 1; i <= 10; i++)
        {
            int taskNumber = i;
            threadPool.Execute(obj =>
            {
                Console.WriteLine($"Task {taskNumber} with priority {obj} is executing.");
                Thread.Sleep(1000);
            }, i % 3 == 0 ? Priority.HIGH : i % 3 == 1 ? Priority.NORMAL : Priority.LOW);
        }

        Thread.Sleep(5000);
        threadPool.Stop();
    }
}
