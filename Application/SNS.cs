using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.Runtime;
using Amazon.SimpleNotificationService.Model;

public class OTPSender
{
    private readonly IAmazonSimpleNotificationService _snsClient;

    public OTPSender()
    {
        var credentials = new BasicAWSCredentials("awsAccessKeyId", "awsSecretAccessKey");
        _snsClient = new AmazonSimpleNotificationServiceClient(credentials, RegionEndpoint.APSoutheast2);
    }

    public async void SendOTP(string phoneNumber, string otpCode)
    {
        var message = $"Your OTP code is: {otpCode}";

        var request = new PublishRequest
        {
            PhoneNumber = phoneNumber,
            Message = message
        };

        var response = await _snsClient.PublishAsync(request);

        // Handle the response here, check for errors, and log the result
        if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("OTP sent successfully.");
        }
        else
        {
            Console.WriteLine($"OTP sending failed. Status code: {response.HttpStatusCode}");
        }
    }
}
