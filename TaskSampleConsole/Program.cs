using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TaskSampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程执行业务处理");
            Task task = new Task(() =>
            {
                Console.WriteLine("使用Task执行异步操作");
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                }
            });

            task.Start();
            Console.WriteLine("主线程执行其他处理");
            //Console.ReadLine();
            Thread.Sleep(2000);

            //TaskWait();

            TaskContinueWith();

            Console.ReadLine();

        }

        static void TaskWait()
        {
            Task<int> task = new Task<int>(() =>
            {
                int sum = 0;
                Console.WriteLine("使用Task执行异步操作");
                for (int i = 0; i < 100; i++)
                {
                    sum += i;
                }
                return sum;
            });

            TaskScheduler ts = TaskScheduler.Default;
            
            task.Start();
            Console.WriteLine("主线程执行其他处理");

            // wait是同步方法，会堵塞主线程
            task.Wait();
            Console.WriteLine("任务执行结果:{0}",task.Result);

            
        }

        static void TaskContinueWith()
        {
            Task<int> task = new Task<int>(() =>
            {
                int sum = 0;
                Console.WriteLine("使用Task执行异步操作");
                for (int i = 0; i < 100; i++)
                {
                    sum += i;
                }

                return sum;
            });

            task.Start();
            Console.WriteLine("主线程执行其他处理");
            Task tk = task.ContinueWith(t =>
            {
                Console.WriteLine("任务完成结果:{0}",t.Result.ToString());
            });
        }
    }
}
