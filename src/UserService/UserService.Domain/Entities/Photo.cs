using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Domain.Entities;

public class Photo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; }
    public string PublicId { get; set; }
    public Guid ProfileId { get; set; }
}