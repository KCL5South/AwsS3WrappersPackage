using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AWSS3FilesCrudManager
{
    public interface IAWSS3CrudManager
    {
        public Task<bool> UploadFileAsync(byte[] fs, string bucketName, string keyName);
        public Task<bool> DeleteFileAsync(string bucketName, string keyName);
        public Task<bool> IsS3FileExists(string bucketName, string key);

        public Task<byte[]> DownloadFileAsync(string bucketName, string key);

    }
}
