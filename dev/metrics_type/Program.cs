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
	    var gauge = Metrics.CreateGauge("first_gauge", "First Guage Example");
	    var histo = Metrics.CreateHistogram("first_histogram", "First Histogram Example",
                       new HistogramConfiguration
                       {
                           // We divide measurements in 10 buckets of $100 each, up to $1000.
                           Buckets = Histogram.LinearBuckets(start: 0, width: 10, count: 10)
                       });
	    var summary = Metrics.CreateSummary("first_summary", "First Summary Example",
			 new SummaryConfiguration
			{
		       		Objectives = new[]
		         	{
		         	new QuantileEpsilonPair(0.5, 0.05),
		     	  	new QuantileEpsilonPair(0.9, 0.05),
		       		new QuantileEpsilonPair(0.95, 0.01),
		       		new QuantileEpsilonPair(0.99, 0.005),
		       		}
			});

            var pusher = new MetricPusher(new MetricPusherOptions
            {
                Endpoint = "https://prometheus-gw.hwchiu.com/metrics",
                Job = jobName
            });

            pusher.Start();

            while (true)
            {
                counter.Inc();
		Random random = new Random();
                var number = random.Next(1,100);
                Console.WriteLine("Random is " + number);
		gauge.Set(number);
		histo.Observe(number);
		summary.Observe(number);
                // Sleep for 10 second
                System.Threading.Thread.Sleep(10000);
            }
        }
    }
}
