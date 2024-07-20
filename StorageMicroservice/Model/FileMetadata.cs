using System.Reflection.Metadata;

namespace StorageMicroservice.Model
{
    public class FileMetadata
    {
        public string FileId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long FileSize { get; set; }
        public string UploadedBy { get; set; }
        public DateTime UploadedOn { get; set; }
        public DateTime? LastAccessedOn { get; set; }
        public string StorageUrl { get; set; }
    }

}
