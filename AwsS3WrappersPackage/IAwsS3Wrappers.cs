using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwsS3WrappersPackage
{
    public interface IAwsS3Wrappers
    {
        public Task<bool> UploadFileAsync(byte[] fs, string bucketName, string keyName);
        public Task<bool> DeleteFileAsync(string bucketName, string keyName);
       
        public Task<byte[]> DownloadFileAsync(string bucketName, string key);

    }
}
