using System;
using System.Threading.Tasks;

namespace Services
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // // Create S3Logger instance and append lines
            // var logger = S3Logger.Instance;
            // await logger.AppendLine("Third line");
            // await logger.AppendLine("Fourth line");
            // Console.WriteLine("Lines appended to S3 file.");

            // Create CloudWatchLogger instance and append lines
            var logger = CloudWatchLogger.Instance;
            await logger.AppendLine("Log message to CloudWatch");

            // Wait for key press to keep the console open
            Console.ReadKey();
        }
    }
}