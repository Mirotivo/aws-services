﻿using System;
using System.Threading.Tasks;
using Amazon.SQS.Model;

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

            // // Create CloudWatchLogger instance and append lines
            // var logger = CloudWatchLogger.Instance;
            // await logger.AppendLine("First Line");
            // await logger.AppendLine("Log message to CloudWatch");
            // Console.WriteLine("Lines appended to CloudWatch events.");

            var instance = SQS.Instance;
            await instance.CreateQueue();
            Console.WriteLine(" queue created successfully..");
            string MessageBody = "This is first message queue test";
            await instance.SendMessage(MessageBody);
            MessageBody = "This is second message queue test";
            await instance.SendMessage(MessageBody);
            List<Message> messages = await instance.ReceiveMessages();
            foreach (var message in messages)
            {
                Console.WriteLine(message.Body + " found in Queue");
                // Delete the message
                var receiptHandle = message.ReceiptHandle;
                await instance.DeleteMessage(receiptHandle);
            }
            Console.WriteLine(" queue processed successfully..");

            // Wait for key press to keep the console open
            Console.ReadKey();
        }
    }
}