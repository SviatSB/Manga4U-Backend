namespace DataInfrastructure.Interfaces
{
    public interface IAvatarStorage
    {
        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
    }
}
