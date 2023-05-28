using System;
using Prometheus;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            var jobName = "my_job";
            var metricName = "first_entry";

            var httpClient = new HttpClient();
            var counter = Metrics.CreateCounter(metricName, "A custom counter");

            var pusher = new MetricPusher(new MetricPusherOptions
            {
                Endpoint = "https://prometheus-gw.hwchiu.com/metrics",
                Job = jobName
            });

            pusher.Start();

            while (true)
            {
                Console.WriteLine("Incrementing counter...");
                counter.Inc();
                // Sleep for 1 second
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
