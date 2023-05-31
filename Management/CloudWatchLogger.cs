using Amazon;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using System;

public sealed class CloudWatchLogger
{
    private static readonly CloudWatchLogger instance = new CloudWatchLogger();
    private static readonly string _logGroup = "/Test/debug";
    private static readonly string _logStream = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
    private static string _nextSequenceToken = string.Empty;

    private IAmazonCloudWatchLogs cloudWatchClient;

    private CloudWatchLogger()
    {
        cloudWatchClient = new AmazonCloudWatchLogsClient("awsAccessKeyId", "awsSecretAccessKey", RegionEndpoint.APSoutheast1);
        CreateLogGroupAndStreamIfNotExist().GetAwaiter().GetResult();
    }

    public static CloudWatchLogger Instance
    {
        get { return instance; }
    }

    private async Task CreateLogGroupAndStreamIfNotExist()
    {
        var existingLogGroups = await cloudWatchClient.DescribeLogGroupsAsync();
        if (!existingLogGroups.LogGroups.Any(x => x.LogGroupName == _logGroup))
        {
            // Log Group
            _ = await cloudWatchClient.CreateLogGroupAsync(new CreateLogGroupRequest()
            {
                LogGroupName = _logGroup
            });

            // Log stream
            _ = await cloudWatchClient.CreateLogStreamAsync(new CreateLogStreamRequest()
            {
                LogGroupName = _logGroup,
                LogStreamName = _logStream
            });
        }
    }

    public async Task AppendLine(string logMessage)
    {
        // Create a request to put the log event into CloudWatch
        var response = await cloudWatchClient.PutLogEventsAsync(new PutLogEventsRequest()
        {
            LogGroupName = _logGroup,
            LogStreamName = _logStream,
            SequenceToken = _nextSequenceToken,
            LogEvents = new List<InputLogEvent>()
            {
                new InputLogEvent()
                {
                    Message = logMessage,
                    Timestamp = DateTime.UtcNow
                }
            }
        });

        _nextSequenceToken = response.NextSequenceToken;
    }
}
