using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using System;

public sealed class SQS
{
    private static readonly SQS instance = new SQS();
    private AmazonSQSClient client;
    private readonly string region = "ap-southeast-2";
    private readonly string account_id = "account_id";
    public string QueueName = "TestingQueue";
    public string QueueUrl = "";

    private SQS()
    {
        client = new AmazonSQSClient("awsAccessKeyId", "awsSecretAccessKey", RegionEndpoint.APSoutheast2);
        QueueUrl = $"https://sqs.{region}.amazonaws.com/{account_id}/{QueueName}";
    }

    public static SQS Instance
    {
        get { return instance; }
    }

    public async Task CreateQueue()
    {
        var createQueueRequest = new CreateQueueRequest();
        createQueueRequest.QueueName = QueueName;

        await client.CreateQueueAsync(createQueueRequest);
    }

    public async Task SendMessage(string MessageBody)
    {
        // sending the message to the message queue
        var sendMessageRequest = new SendMessageRequest
        {
            QueueUrl = QueueUrl,
            MessageBody = MessageBody
        };
        var sendMessageResponse = await client.SendMessageAsync(sendMessageRequest);
    }

    public async Task<List<Message>> ReceiveMessages()
    {
        //receiving the message from message queue
        var receiveMessageRequest = new ReceiveMessageRequest
        {
            QueueUrl = QueueUrl
        };
        var receiveMessageResponse = await client.ReceiveMessageAsync(receiveMessageRequest);
        var messages = receiveMessageResponse.Messages;

        return messages;
    }

    public async Task DeleteMessage(string receiptHandle)
    {
        var deleteMessageRequest = new DeleteMessageRequest
        {
            QueueUrl = QueueUrl,
            ReceiptHandle = receiptHandle
        };
        await client.DeleteMessageAsync(deleteMessageRequest);
    }

}
