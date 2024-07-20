using Azure.Storage.Sas;
using StorageMicroservice.Model;
using StorageMicroservice.Repository;

namespace StorageMicroservice.Services
{
    public interface IFileManager
    {
        Task<string> GenerateUploadUrlAsync(FileMetadata request);
        Task<string> GenerateDownloadUrlAsync(string fileId);
        Task<FileMetadata> GetFileMetadataAsync(string fileId);
        Task<List<FileMetadata>> ListFilesAsync();
        Task DeleteFileAsync(string fileId);
    }

    public class FileManager : IFileManager
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IMetadataRepository _metadataRepository;

        public FileManager(IBlobRepository blobRepository, IMetadataRepository metadataRepository)
        {
            _blobRepository = blobRepository;
            _metadataRepository = metadataRepository;
        }

        public async Task<string> GenerateUploadUrlAsync(FileMetadata request)
        {
            var sasToken = _blobRepository.GenerateToken(request.FileName, BlobSasPermissions.Write);
            var metadata = new FileMetadata
            {
                FileId = Guid.NewGuid().ToString(),
                FileName = request.FileName,
                ContentType = request.ContentType,
                FileSize = request.FileSize,
                UploadedBy = request.UploadedBy,
                UploadedOn = DateTime.UtcNow
            };
            await _metadataRepository.SaveMetadataAsync(metadata);

            return $"{_blobRepository.GetBlobUrl(request.FileName)}?{sasToken}";
        }

        public async Task<string> GenerateDownloadUrlAsync(string fileId)
        {
            var fileMetadata = await _metadataRepository.GetMetadataAsync(fileId);
            var sasToken = _blobRepository.GenerateToken(fileMetadata.FileName, BlobSasPermissions.Read);

            fileMetadata.LastAccessedOn = DateTime.UtcNow;
            await _metadataRepository.UpdateMetadataAsync(fileMetadata);

            return $"{_blobRepository.GetBlobUrl(fileMetadata.FileName)}?{sasToken}";
        }

        public async Task<FileMetadata> GetFileMetadataAsync(string fileId)
        {
            return await _metadataRepository.GetMetadataAsync(fileId);
        }

        public async Task<List<FileMetadata>> ListFilesAsync()
        {
            return await _metadataRepository.ListAllMetadataAsync();
        }

        public async Task DeleteFileAsync(string fileId)
        {
            var fileMetadata = await _metadataRepository.GetMetadataAsync(fileId);
            await _blobRepository.DeleteBlobAsync(fileMetadata.FileName);
            await _metadataRepository.DeleteMetadataAsync(fileId);

        }
    }
}
