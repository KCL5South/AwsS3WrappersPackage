using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AwsS3Wrappers;
using Moq;


namespace AwsS3WrapperTests.Unit
{
    public class AwsS3WrapperTests
    {
        //public class TestAwsS3Wrappers(IAmazonS3 _S3Client) : AwsS3Wrapper(_S3Client) { }

        private readonly Mock<IAmazonS3> _s3ClientMock = new();

        [Fact]
        public async Task DeleteFileAsync_S3FileExistsAsyncReturnsTrue_ReturnsTrue()
        {
          
            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default)).ReturnsAsync(new DeleteObjectResponse());
            
            string bucketName = "";
            string key = "";

            var result = await awsS3Wrapper.DeleteFileAsync(bucketName, key);

            Assert.IsType<bool>(result);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteFileAsync_S3FileExistsAsyncReturnsFalse_ReturnsFalse()
        {

            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(),default)).ThrowsAsync(new Exception("The specified key does not exist."));

            var result = await awsS3Wrapper.DeleteFileAsync(bucketName, key);

            Assert.IsType<bool>(result);
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteFileAsync_S3FileExistsAsyncReturnsTrue_DeleteObjectAsyncIsCalledExactlyOnce()
        {
            AwsS3Wrapper awsS3Wrapper  = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default)).ReturnsAsync(new DeleteObjectResponse());

            var result = await awsS3Wrapper.DeleteFileAsync(bucketName, key);

            _s3ClientMock.Verify(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default), Times.Once);

        }

  
        [Fact]
        public async Task UploadFileAsync_NoExceptionThrown_ReturnsTrue()
        {
           
            _s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default));
            byte[] testByteArray = new byte[64];

            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";

            var result = await awsS3Wrapper.UploadFileAsync(testByteArray, bucketName, key);

            Assert.IsType<bool>(result);
            Assert.True(result);

        }
    

        [Fact]
        public async Task UploadFileAsync_PutObjectAsyncThrowsException_ReturnsFalse()
        {
            _s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default)).ThrowsAsync(new Exception("some exception"));
            byte[] testByteArray = new byte[64];
            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";

            var result = await awsS3Wrapper.UploadFileAsync(testByteArray, bucketName, key);

            Assert.IsType<bool>(result);
            Assert.False(result);

        }


        [Fact]
        public async Task DownloadFile_S3FileExistsAsyncReturnsTrue_ReturnsFileByteArray()
        {
            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            var expectedBytes = new byte[] { 1, 2, 3, 4 };
            var expResponseStream = new MemoryStream(expectedBytes);
            var expResponse = new GetObjectResponse
            {
                ResponseStream = expResponseStream
            };

            _s3ClientMock.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default)).ReturnsAsync(expResponse);
            var result = await awsS3Wrapper.DownloadFileAsync(bucketName, key);

            Assert.IsType<byte[]>(result);
            Assert.Equal(expectedBytes, result);
        }
        [Fact]
        public async Task DownloadFile_S3FileExistsAsyncReturnsFalse_ReturnsExceptionWithMessage()
        {
            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            string errorMessage = "Error encountered while downloading the file";

            _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), default)).ThrowsAsync(new Exception("The specified key does not exist."));

            var exception = await Assert.ThrowsAsync<Exception>(()=> awsS3Wrapper.DownloadFileAsync(bucketName, key));
            Assert.Equal(errorMessage, exception.Message);
        }

        [Fact]
        public async Task DownloadFileAsync_S3FileExistsAsyncReturnsTrue_GetObjectAsyncIsCalledExactlyOnce()
        {
            AwsS3Wrapper awsS3Wrapper = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            var expectedBytes = new byte[] { 1, 2, 3, 4 };
            var expResponseStream = new MemoryStream(expectedBytes);
            var expResponse = new GetObjectResponse
            {
                ResponseStream = expResponseStream
            };
            _s3ClientMock.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default)).ReturnsAsync(expResponse);
            var result = await awsS3Wrapper.DownloadFileAsync(bucketName, key);

            _s3ClientMock.Verify(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default), Times.Once);

        }

    }
}
