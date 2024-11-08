
using Amazon.S3;
using Amazon.S3.Model;
using AWSS3FilesCrudManager;


namespace AWSS3FilesCrudHelperPackage
{
    public abstract class AWSS3CrudManager(IAmazonS3 _S3Client) : IAWSS3CrudManager
    {
        public async Task<bool> UploadFileAsync(byte[] fs, string bucketName, string keyName)
        {
            try
            {
                using (var inputStream = new MemoryStream(fs))
                {
                    PutObjectRequest request = new PutObjectRequest()
                    {
                        InputStream = inputStream,
                        BucketName = bucketName,
                        Key = keyName // <-- in S3 key represents a path
                    };

                    await _S3Client.PutObjectAsync(request);

                    return true;
                }
            }
            catch (AmazonS3Exception e)
            {

                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
                return false;
            }

        }


        public async Task<bool> DeleteFileAsync(string bucketName, string keyName)
        {
            try
            {
                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = bucketName,
                    Key = keyName,
                };

                Console.WriteLine($"Deleting object: {keyName}");
                await _S3Client.DeleteObjectAsync(deleteObjectRequest);
                Console.WriteLine($"Object: {keyName} deleted from {bucketName}.");
                return true;
            }
            catch (AmazonS3Exception ex)
            {
                Console.WriteLine($"Error encountered on server. Message:'{ex.Message}' when deleting an object.");
                return false;
            }
        }
        public async Task<bool> IsS3FileExists(string bucketName, string key)
        {
            try
            {

                var request = new GetObjectMetadataRequest()
                {
                    BucketName = bucketName,
                    Key = key,

                };

                var result = await _S3Client.GetObjectMetadataAsync(request);

                return true;
            }
            catch
            {

                return false;
            }
        }

        public async Task<byte[]> DownloadFileAsync(string bucketName, string key)
        {

            try
            {
                GetObjectRequest getObjectRequest = new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = key
                };

                using var objResp = await _S3Client.GetObjectAsync(getObjectRequest);
                using var ms = new MemoryStream();
                await objResp.ResponseStream.CopyToAsync(ms);  


                return ms.ToArray();
            }
            catch
            {
                throw;
            }
        }
    }
}

