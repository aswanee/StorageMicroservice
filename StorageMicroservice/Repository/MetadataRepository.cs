using StorageMicroservice.Model;

namespace StorageMicroservice.Repository
{
    public interface IMetadataRepository
    {
        Task SaveMetadataAsync(FileMetadata metadata);
        Task<FileMetadata> GetMetadataAsync(string fileId);
        Task<List<FileMetadata>> ListAllMetadataAsync();
        Task UpdateMetadataAsync(FileMetadata metadata);
        Task DeleteMetadataAsync(string fileId);
    }

    /// <summary>
    /// Implement is out of scop but we can replace this repo with other implementation with MS EF 
    /// </summary>
    public class MetadataRepository : IMetadataRepository
    {
        private readonly Dictionary<string, FileMetadata> _storage = new Dictionary<string, FileMetadata>();

        public Task SaveMetadataAsync(FileMetadata metadata)
        {
            _storage[metadata.FileId] = metadata;
            return Task.CompletedTask;
        }

        public Task<FileMetadata> GetMetadataAsync(string fileId)
        {
            _storage.TryGetValue(fileId, out var metadata);
            return Task.FromResult(metadata);
        }

        public Task<List<FileMetadata>> ListAllMetadataAsync()
        {
            return Task.FromResult(_storage.Values.ToList());
        }

        public Task UpdateMetadataAsync(FileMetadata metadata)
        {
            _storage[metadata.FileId] = metadata;
            return Task.CompletedTask;
        }

        public Task DeleteMetadataAsync(string fileId)
        {
            _storage.Remove(fileId);
            return Task.CompletedTask;
        }
    }

}
