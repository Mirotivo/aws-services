using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

public sealed class S3Logger
{
    private static readonly S3Logger instance = new S3Logger();
    private static readonly string bucketName = "bucketName";
    private static readonly string fileName = "fileName";
    private static readonly string region = "ap-southeast-2";
    private static readonly AmazonS3Client s3Client = new AmazonS3Client("awsAccessKeyId", "awsSecretAccessKey", RegionEndpoint.GetBySystemName(region));

    private S3Logger()
    {
    }

    public static S3Logger Instance
    {
        get { return instance; }
    }

    public async Task AppendLine(string line)
    {
        // Append timestamp to the line
        string lineWithTimestamp = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {line}";

        using (MemoryStream stream = new MemoryStream())
        {
            var getRequest = new GetObjectRequest
            {
                BucketName = bucketName,
                Key = fileName
            };

            GetObjectResponse getResponse = await s3Client.GetObjectAsync(getRequest);
            using (StreamReader reader = new StreamReader(getResponse.ResponseStream))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                string existingContent = reader.ReadToEnd();
                writer.Write(existingContent);
                writer.WriteLine(lineWithTimestamp);
                writer.Flush();
                stream.Position = 0;

                var putRequest = new PutObjectRequest
                {
                    BucketName = bucketName,
                    Key = fileName,
                    InputStream = stream
                };

                PutObjectResponse putResponse = await s3Client.PutObjectAsync(putRequest);
            }
        }
    }
}