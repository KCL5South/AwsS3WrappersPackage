
namespace AwsS3Wrappers
{
    public interface IAwsS3Wrapper
    {
        public Task<bool> UploadFileAsync(byte[] fs, string bucketName, string keyName);
        public Task<bool> DeleteFileAsync(string bucketName, string keyName);
       
        public Task<byte[]> DownloadFileAsync(string bucketName, string key);

    }
}
