using System;
using System.Threading;
using Prometheus;

namespace DataGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            MetricServer metricServer;
            Random random = new Random();

            Console.WriteLine("Metrics generator started");

            try
            {
                metricServer = new MetricServer(port: 1234);
                metricServer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while creating MetricServer: {ex.Message}");
                return;
            }

            Gauge speedGauge = Metrics.CreateGauge("speed", "Speed in km/h");
            Counter randomCounter = Metrics.CreateCounter("speed_limit_exceeded", "Speed limit exceeded");

            do
            {
                double speed = 100 * random.NextDouble();
                speedGauge.Set(speed);

                if (speed > 80)
                    randomCounter.Inc();

                Console.WriteLine($"Speed = {speed}");

                Thread.Sleep(random.Next(200, 2000));

            } while (true /* !Console.KeyAvailable */);

            metricServer.Stop();
        }
    }
}
