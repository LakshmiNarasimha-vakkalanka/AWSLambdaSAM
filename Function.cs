using Amazon.Lambda.Core;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace AWSLambdaSAM;

public class Function
{
    private DynamoDBContext _dynamoDBContext;

    public Function()
    {
        _dynamoDBContext = new DynamoDBContext(new AmazonDynamoDBClient());
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/customers/{customerid}")]
    public async Task<Customer> GetCustomerAsync(string customerid, ILambdaContext context)
    {
        Guid.TryParse(customerid, out Guid id);
        return await _dynamoDBContext.LoadAsync<Customer>(id);
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/customers")]
    public async void PostCustomerAsync([FromBody]Customer objCustomer, ILambdaContext context)
    {        
        await _dynamoDBContext.SaveAsync<Customer>(objCustomer);
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Delete, "/customers/{customerid}")]
    public async void DeleteCustomerAsync(string customerid, ILambdaContext context)
    {
        Guid.TryParse(customerid, out Guid id);
        await _dynamoDBContext.DeleteAsync<Customer>(id);
    }
}

public class Customer
{
    public Guid CustomerId { get; set; }
    public string? CustomerName { get; set; }
}
