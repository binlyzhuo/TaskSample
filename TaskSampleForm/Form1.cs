using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskSampleForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //TaskWait();
            TaskWait2();
        }
        void TaskWait()
        {
            Task<int> task = new Task<int>(() =>
            {
                //Thread.Sleep(5000);
                int sum = 0;
                //Console.WriteLine("使用Task执行异步操作");
                for (int i = 0; i < 9000000; i++)
                {
                    sum += i;
                }
                return sum;
            });

            task.Start();
            int result = task.Result;

            //Console.WriteLine("主线程执行其他处理");
            task.Wait();
            label1.Text = task.Result.ToString();
            //Console.WriteLine("任务执行结果:{0}", task.Result);
        }

        void TaskWait2()
        {
            HttpClient client = new HttpClient();
            Task<string> t2 =  client.GetStringAsync("http://www.taobao.com");
            t2.Wait();
            int len = t2.Result.Length;
            label1.Text = len.ToString();
        }
    }
}
