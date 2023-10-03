using Amazon;
using Amazon.SimpleEmail;
using Amazon.Runtime;
using Amazon.SimpleEmail.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class SES
{
    private readonly IAmazonSimpleEmailService _sesClient;
    private readonly string region = "ap-southeast-2";

    public SES()
    {
        var credentials = new BasicAWSCredentials("awsAccessKeyId", "awsSecretAccessKey");

        var config = new AmazonSimpleEmailServiceConfig
        {
            RegionEndpoint = RegionEndpoint.APSoutheast2
        };

        _sesClient = new AmazonSimpleEmailServiceClient(credentials, config);
    }

    public async Task SendEmailAsync(string sender, string recipient, string subject, string htmlBody, string textBody)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = sender,
            Destination = new Destination
            {
                ToAddresses = new List<string> { recipient }
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content
                    {
                        Charset = "UTF-8",
                        Data = htmlBody
                    },
                    Text = new Content
                    {
                        Charset = "UTF-8",
                        Data = textBody
                    }
                }
            }
        };

        var response = await _sesClient.SendEmailAsync(sendRequest);
        Console.WriteLine("Email sent!");
    }
}
