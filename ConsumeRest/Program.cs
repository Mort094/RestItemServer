using System;

namespace ConsumeRest
{
    class Program
    {
        static void Main(string[] args)
        {
            RestWorker worker = new RestWorker();
            worker.Start();

            Console.ReadLine();
        }
    }
}
