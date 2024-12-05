
using Amazon.S3;
using Amazon.S3.Model;
using AWSS3FilesCrudManager;
using System.ComponentModel.Design;


namespace AWSS3FilesCrudHelperPackage
{
    public class AwsS3Wrappers(IAmazonS3 _s3Client) : IAwsS3Wrappers
    {
        public async Task<bool> UploadFileAsync(byte[] file, string bucketName, string keyName)
        {
            try
            {
                using var inputStream = new MemoryStream(file);
                PutObjectRequest request = new()
                {
                    InputStream = inputStream,
                    BucketName = bucketName,
                    Key = keyName // <-- in S3 key represents a path
                }; 

               await _s3Client.PutObjectAsync(request);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }

        }


        public async Task<bool> DeleteFileAsync(string bucketName, string keyName)
        {
                if (await S3FileExistsAsync(bucketName, keyName))
                {
                    var deleteObjectRequest = new DeleteObjectRequest
                    {
                        BucketName = bucketName,
                        Key = keyName,
                    };

                    Console.WriteLine($"Deleting object: {keyName}");
                    await _s3Client.DeleteObjectAsync(deleteObjectRequest);
                    Console.WriteLine($"Object: {keyName} deleted from {bucketName}.");
                    return true;
                }
                else return false;
            }

        public async Task<bool> S3FileExistsAsync(string bucketName, string key)
        {
            try
            {
                var request = new GetObjectMetadataRequest()
                {
                    BucketName = bucketName,
                    Key = key,

                };

                var result = await _s3Client.GetObjectMetadataAsync(request);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<byte[]> DownloadFileAsync(string bucketName, string key)
        {


            if (await S3FileExistsAsync(bucketName, key))
            {
                GetObjectRequest getObjectRequest = new()
                {
                    BucketName = bucketName,
                    Key = key
                };

                using var objResp = await _s3Client.GetObjectAsync(getObjectRequest);
                using var memoryStream = new MemoryStream();
                await objResp.ResponseStream.CopyToAsync(memoryStream);


                return memoryStream.ToArray();
            }
            else throw new Exception("Error encountered while downaloding the file");

        }
    }
}

