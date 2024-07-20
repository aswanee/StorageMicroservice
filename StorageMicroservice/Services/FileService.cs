using Azure.Storage.Sas;
using StorageMicroservice.Model;
using StorageMicroservice.Repository;

namespace StorageMicroservice.Services
{
    public interface IFileService
    {
        Task<string> GenerateUploadUrlAsync(FileMetadata request);
        Task<string> GenerateDownloadUrlAsync(string fileId);
    }

    public class FileService : IFileService
    {
        private readonly IBlobRepository _blobRepository;
        private readonly IMetadataRepository _metadataRepository;

        public FileService(IBlobRepository blobRepository, IMetadataRepository metadataRepository)
        {
            _blobRepository = blobRepository;
            _metadataRepository = metadataRepository;
        }

        public async Task<string> GenerateUploadUrlAsync(FileMetadata request)
        {
            var sasToken = _blobRepository.GenerateToken(request.FileName, BlobSasPermissions.Write);
            await _metadataRepository.SaveMetadataAsync(new FileMetadata { 
                FileName = request.FileName,
                FileId = request.FileId , 
                ContentType = request.ContentType ,
                FileSize = request.FileSize ,
                LastAccessedOn = request.LastAccessedOn ,
                UploadedBy = request.UploadedBy ,
                StorageUrl = request.StorageUrl ,
                UploadedOn = request.UploadedOn,
            });
            return _blobRepository.GetBlobUrl(request.FileName) + sasToken;
        }

        public async Task<string> GenerateDownloadUrlAsync(string fileId)
        {
            var fileMetadata = await _metadataRepository.GetMetadataAsync(fileId);
            var sasToken = _blobRepository.GenerateToken(fileMetadata.FileName, BlobSasPermissions.Read);
            return _blobRepository.GetBlobUrl(fileMetadata.FileName) + sasToken;
        }
    }


}
