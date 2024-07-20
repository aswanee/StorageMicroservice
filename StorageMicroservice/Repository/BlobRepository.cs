using Azure.Storage.Blobs;
using Azure.Storage.Sas;

namespace StorageMicroservice.Repository
{
    public interface IBlobRepository
    {
        /// <summary>
        /// Generate Token
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        string GenerateToken(string blobName, BlobSasPermissions permissions);
        string GetBlobUrl(string blobName);
        Task<string> DeleteBlobAsync(string FileName);
    }

    public class BlobRepository : IBlobRepository
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _containerClient;

        public BlobRepository(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("AzureBlobStorage:ConnectionString").Value;
            var containerName = configuration.GetSection("AzureBlobStorage:ContainerName").Value;
            _blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        }

        /// <summary>
        /// Implement Generate Token By 
        /// </summary>
        /// <param name="blobName"></param>
        /// <param name="permissions"></param>
        /// <returns></returns>
        public string GenerateToken(string blobName, BlobSasPermissions permissions)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = _containerClient.Name,
                BlobName = blobName,
                Resource = "b" 
                // b  for blob 
                // c  for blob container 
                // bs for blob snapshot 
                // bv for blob version
            };
            sasBuilder.SetPermissions(permissions);
            sasBuilder.ExpiresOn = DateTimeOffset.UtcNow.AddHours(1);
            return blobClient.GenerateSasUri(sasBuilder).Query;
        }

        public string GetBlobUrl(string blobName)
        {
            return _containerClient.GetBlobClient(blobName).Uri.ToString();
        }

        public async Task<string> DeleteBlobAsync(string FileName)
        {
            throw new NotImplementedException();
        }
    }

}
