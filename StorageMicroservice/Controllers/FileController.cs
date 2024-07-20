using Microsoft.AspNetCore.Mvc;
using StorageMicroservice.Model;
using StorageMicroservice.Services;

namespace StorageMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : ControllerBase
    {
        private readonly IFileManager _fileManager;

        public FileController(IFileManager fileManager)
        {
            _fileManager = fileManager;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> GetUploadUrl([FromBody] FileMetadata request)
        {
            var uploadUrl = await _fileManager.GenerateUploadUrlAsync(request);
            return Ok(new { uploadUrl });
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> GetDownloadUrl(string fileId)
        {
            var downloadUrl = await _fileManager.GenerateDownloadUrlAsync(fileId);
            return Ok(new { downloadUrl });
        }

        [HttpGet("metadata/{fileId}")]
        public async Task<IActionResult> GetMetadata(string fileId)
        {
            var metadata = await _fileManager.GetFileMetadataAsync(fileId);
            return Ok(metadata);
        }

        [HttpGet("list")]
        public async Task<IActionResult> ListFiles()
        {
            var files = await _fileManager.ListFilesAsync();
            return Ok(files);
        }

        [HttpDelete("{fileId}")]
        public async Task<IActionResult> DeleteFile(string fileId)
        {
            await _fileManager.DeleteFileAsync(fileId);
            return NoContent();
        }

    }


}
