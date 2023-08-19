using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

// Определение перечисления для уровней приоритетов задач
public enum Priority
{
    LOW,     // Низкий
    NORMAL,  // Обычный
    HIGH     // Высокий
}

// Класс для собственной реализации пула потоков
public class CustomThreadPool
{
    private readonly int threadCount; // Количество потоков в пуле
    private readonly BlockingCollection<Tuple<Action<object>, Priority, object>> taskQueue; // Очередь задач
    private bool isStopped = false; // Флаг для обозначения остановки пула

    // Конструктор, принимающий количество потоков в пуле
    public CustomThreadPool(int threadCount)
    {
        this.threadCount = threadCount;
        taskQueue = new BlockingCollection<Tuple<Action<object>, Priority, object>>(); // Инициализация очереди

        // Создание и запуск потоков в пуле
        for (int i = 0; i < threadCount; i++)
        {
            var thread = new Thread(Worker); // Создание нового потока, который будет выполнять задачи
            thread.Start(); // Запуск потока
        }
    }

    // Метод для добавления задачи в очередь на выполнение
    public bool Execute(Action<object> action, Priority priority, object value = null)
    {
        if (isStopped)
        {
            Console.WriteLine("Пул потоков остановлен. Невозможно добавлять новые задачи.");
            return false;
        }

        // Добавление задачи в очередь с указанной операцией, приоритетом и значением
        taskQueue.Add(new Tuple<Action<object>, Priority, object>(action, priority, value));
        return true;
    }

    // Метод для остановки пула
    public void Stop()
    {
        isStopped = true; // Установка флага остановки
        taskQueue.CompleteAdding(); // Отметка очереди как завершенной для добавления
    }

    // Метод для выполнения работы потоками пула
    private void Worker()
    {
        // Перебор задач в очереди
        foreach (var taskInfo in taskQueue.GetConsumingEnumerable())
        {
            if (taskInfo.Item2 == Priority.LOW)
            {
                // Если приоритет низкий, проверяем наличие задач более высокого приоритета
                bool hasHigherPriorityTasks = taskQueue.Any(task =>
                    task.Item2 == Priority.HIGH || task.Item2 == Priority.NORMAL);
                if (hasHigherPriorityTasks)
                {
                    continue; // Пропуск задачи, если есть задачи более высокого приоритета
                }
            }
            else if (taskInfo.Item2 == Priority.NORMAL)
            {
                // Если приоритет обычный, выполняем 3 задачи с более высоким приоритетом
                for (int i = 0; i < 3; i++)
                {
                    if (taskQueue.TryTake(out var higherPriorityTask) &&
                        higherPriorityTask.Item2 == Priority.HIGH)
                    {
                        higherPriorityTask.Item1(higherPriorityTask.Item3);
                    }
                }
            }

            // Выполнение задачи из текущей очереди
            taskInfo.Item1(taskInfo.Item3);
        }
    }
}

// Класс для демонстрации работы пула потоков
class Program
{
    static void Main(string[] args)
    {
        CustomThreadPool threadPool = new CustomThreadPool(4);

        for (int i = 1; i <= 10; i++)
        {
            int taskNumber = i;
            Priority taskPriority = i % 3 == 0 ? Priority.HIGH : i % 3 == 1 ? Priority.NORMAL : Priority.LOW;
            threadPool.Execute(obj =>
            {
                Console.WriteLine($"Задача {taskNumber} с приоритетом {taskPriority} выполняется.");
                Thread.Sleep(1000);
            }, taskPriority);
        }

        Console.WriteLine("Все задачи добавлены.");
        Thread.Sleep(5000);
        threadPool.Stop();

        Console.WriteLine("Пул потоков остановлен.");
    }
}
