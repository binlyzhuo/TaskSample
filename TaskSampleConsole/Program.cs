using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

//https://www.cnblogs.com/wyy1234/p/9172467.html
//https://www.cnblogs.com/wyy1234/p/9172467.html
//https://www.cnblogs.com/doforfuture/p/6293926.html


namespace TaskSampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("主线程执行业务处理");
            //Task task = new Task(() =>
            //{
            //    Console.WriteLine("使用Task执行异步操作");
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Console.WriteLine(i);
            //    }
            //});

            //task.Start();
            //Console.WriteLine("主线程执行其他处理");
            ////Console.ReadLine();
            //Thread.Sleep(2000);

            ////TaskWait();

            //TaskContinueWith();

            //Console.WriteLine("线程池操作");

            //for (int i = 0; i < 10; i++)
            //{
            //    ThreadPool.QueueUserWorkItem(new WaitCallback((obj) =>
            //    {
            //        Console.WriteLine($"第{obj}个任务");
            //    }),i);
            //}


            //TaskCancel();

            //string content = GetContentAsync("txt.txt").Result;
            //Console.WriteLine(content);

            string content2 = GetContent("txt.txt");
            Console.WriteLine(content2);

            TaskCallBack();
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


        static void TaskCancel()
        {
            Console.WriteLine("Task Cancel...");
            CancellationTokenSource source = new CancellationTokenSource();
            
            int index = 0;
            Task task1 = new Task(() =>
            {
                while (!source.IsCancellationRequested)
                {
                    Thread.Sleep(1000);
                    Console.WriteLine($"第{++index}次执行，线程运行中...");
                }
            });

            task1.Start();
            Thread.Sleep(5000);
            source.Token.Register(() =>
            {
                Console.WriteLine("任务已取消。。");
            });
            source.Cancel();
            Console.WriteLine("任务已取消");
        }

        static async Task<string> GetContentAsync(string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            var bytes = new byte[fs.Length];
            Console.WriteLine("开始读取文件....");
            int len = await fs.ReadAsync(bytes, 0, bytes.Length);
            string result = Encoding.UTF8.GetString(bytes);
            return result;
        }

        static string GetContent(string fileName)
        {
            object locker = new object();

            lock (locker)
            {
                FileStream fs = new FileStream(fileName, FileMode.Open);
                var bytes = new byte[fs.Length];
                int len = fs.Read(bytes, 0, bytes.Length);
                string result = Encoding.UTF8.GetString(bytes);
                return result;
            }
            
        }

        static void TaskCallBack()
        {
            Console.WriteLine("主线程开始");
            Task<string> task = Task.Run(() =>
            {
                Thread.Sleep(5000);
                return Thread.CurrentThread.ManagedThreadId.ToString();
            });

            task.GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("CallBack Result:"+task.Result);
            });

            Console.WriteLine("主线程结束");
        }
    }
}
