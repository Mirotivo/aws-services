# Services

![Alt Text](AWS.png)

```
dotnet new console --name Services
```
# Compute
```
dotnet add package Amazon.Lambda.Core
dotnet add package Amazon.Lambda.Serialization.SystemTextJson
```
```
dotnet add package AWSSDK.Lambda
```

## Storage
```
dotnet add package AWSSDK.S3
```
## Management & Governance
```
dotnet add package AWSSDK.CloudWatchLogs
```
## Application Integration
```
dotnet add package AWSSDK.SQS
```
## Business Applications
```
dotnet add package AWSSDK.SimpleEmail
```