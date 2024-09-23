namespace AspNetElsDb.options;

public class AwsOptions
{
    public string AWSProfileName { get; set; }
    public string AWSRegion { get; set; }
    public string DynamoEndPoint { get; set; }
    public string AccessKey { get; set; }
    public string SecretKey { get; set; }
}