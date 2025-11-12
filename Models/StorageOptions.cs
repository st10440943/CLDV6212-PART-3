public class StorageOptions
{
    public string ConnectionString { get; set; } = string.Empty;
    public string BlobContainer { get; set; } = string.Empty;
    public string QueueName { get; set; } = string.Empty;
    public string CustomersTable { get; set; } = "Customers";
    public string ProductsTable { get; set; } = "Products";
    public string OrdersTable { get; set; } = "Orders";
    public string FileShare { get; set; } = string.Empty;
    public string BlobFunctionUrl { get; set; } = string.Empty;
}
