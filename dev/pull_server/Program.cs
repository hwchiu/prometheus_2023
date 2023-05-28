using System;
using Prometheus;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var counter = Metrics.CreateCounter("my_counter", "A custom counter");
            
            var server = new MetricServer(port: 1234);
            server.Start();

            while (true)
            {
                counter.Inc();
                Console.WriteLine("Incrementing counter...");

                // Sleep for 1 second
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}
