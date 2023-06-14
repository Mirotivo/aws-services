using System;
using System.Text.Json;
using Amazon;
using Amazon.Lambda;
using Amazon.Lambda.Core;
using Amazon.Lambda.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

// Create Lambda Function in AWS
// Handler: [Assembly]::[Namespace.Class]::[Function]
public class Function
{

    // Services::Function::FunctionHandler
    public string FunctionHandler(string input, ILambdaContext context)
    {
        return input?.ToUpper();
    }

    // Services::Function::Calculate
    public JsonElement Calculate(JsonElement input, ILambdaContext context)
    {
        string operation = input.GetProperty("operation").GetString();
        int num1 = input.GetProperty("num1").GetInt32();
        int num2 = input.GetProperty("num2").GetInt32();
        int result = 0;

        switch (operation)
        {
            case "add":
                result = num1 + num2;
                break;
            case "subtract":
                result = num1 - num2;
                break;
            case "multiply":
                result = num1 * num2;
                break;
            case "divide":
                result = num1 / num2;
                break;
            default:
                throw new ArgumentException("Invalid operation. Supported operations are: add, subtract, multiply, divide.");
        }

        using (MemoryStream stream = new MemoryStream())
        {
            using (Utf8JsonWriter outputJson = new Utf8JsonWriter(stream))
            {
                outputJson.WriteStartObject();
                outputJson.WriteNumber("result", result);
                outputJson.WriteEndObject();
                outputJson.Flush();

                stream.Position = 0;
                using (JsonDocument document = JsonDocument.Parse(stream))
                {
                    return document.RootElement.Clone();
                }
            }
        }
    }
}


public class LambdaInvoker
{
    private readonly IAmazonLambda _lambdaClient;

    public LambdaInvoker()
    {
        // Configure the AWS credentials and region
        var credentials = new Amazon.Runtime.BasicAWSCredentials("awsAccessKeyId", "awsSecretAccessKey");
        var region = RegionEndpoint.APSoutheast2;

        // Create an instance of the AmazonLambdaClient
        _lambdaClient = new AmazonLambdaClient(credentials, region);
    }

    public async Task<string> InvokeLambdaFunction(string functionName, string payload)
    {
        var request = new InvokeRequest
        {
            FunctionName = functionName,
            Payload = payload
        };

        try
        {
            // Invoke the Lambda function
            var response = await _lambdaClient.InvokeAsync(request);

            // Read the response payload
            var payloadString = System.Text.Encoding.UTF8.GetString(response.Payload.ToArray());
            return payloadString;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error invoking Lambda function: " + ex.Message);
            return null;
        }
    }
}
