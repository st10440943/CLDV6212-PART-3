using ABC_Retail.Models;

namespace ABCRetail.Web.Models;

public record BlobItemVm(string Name, string Url);
public record QueueItemVm(string MessageId, string Text, DateTimeOffset? InsertionTime);
public record FileItemVm(string Name);


