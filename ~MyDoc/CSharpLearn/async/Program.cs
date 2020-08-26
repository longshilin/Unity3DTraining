using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace async
{
    /// <summary>
    /// Ref Doc: https://docs.microsoft.com/zh-cn/dotnet/csharp/programming-guide/concepts/async/
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            var eggsTask = FryEggsAsync(2);
            var baconTask = FryBaconAsync(3);
            var toastTask = MakeToastWithButterAndJamAsync(2);


            // 高效地等待任务
            // 方案一： 可以通过使用 Task 类的方法改进上述代码末尾的一系列 await 语句。 其中一个 API 是 WhenAll，
            // 它将返回一个其参数列表中的所有任务都已完成时才完成的 Task，如以下代码中所示：
            /*
            await Task.WhenAll(eggsTask, baconTask, toastTask);
            Console.WriteLine("eggs are ready");
            Console.WriteLine("bacon is ready");
            Console.WriteLine("toast is ready");
            Console.WriteLine("Breakfast is ready!");
            */

            // 方案二：使用 WhenAny，它将返回一个当其参数完成时才完成的 Task<Task>。 你可以等待返回的任务，了解它已经完成了。
            // 以下代码展示了可以如何使用 WhenAny 等待第一个任务完成，然后再处理其结果。 处理已完成任务的结果之后，可以从传递给
            // WhenAny 的任务列表中删除此已完成的任务。
            var breakfastTasks = new List<Task> {eggsTask, baconTask, toastTask};
            while (breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("bacon is ready");
                }
                else if (finishedTask == toastTask)
                {
                    Console.WriteLine("toast is ready");
                }

                breakfastTasks.Remove(finishedTask);
            }

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");
        }

        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }

            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }

            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }
}