using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AWSS3FilesCrudHelperPackage;
using AWSS3FilesCrudManager;
using Moq;


namespace BaseProposalTests.Unit
{
    public class AWSS3CrudManagerTests
    {
        public class TestAWSS3CrudManager(IAmazonS3 _S3Client) : AWSS3CrudManager(_S3Client) { }

        [Fact]
        public async Task IsS3FileExists_ValidContext_ReturnsTrue()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), default)).ReturnsAsync(new GetObjectMetadataResponse());
            
            var result = await _testAWSS3CrudManager.IsS3FileExists(bucketName, key);
        
            Assert.IsType<bool>(result);
            Assert.True(result);
            
        }

        [Fact]
        public async Task IsS3FileExists_GetObjectMetadataAsync_isCalledExactlyOnce()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), default)).ReturnsAsync(new GetObjectMetadataResponse());

            var result = await _testAWSS3CrudManager.IsS3FileExists(bucketName, key);

            _s3ClientMock.Verify(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), default), Times.Once);

        }
        [Fact]
        public async Task IsS3FileExists_InvalidKey_ReturnsFalse()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            _s3ClientMock.Setup(x => x.GetObjectMetadataAsync(It.IsAny<GetObjectMetadataRequest>(), default)).ThrowsAsync(new Exception("The specified key does not exist.")); 
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "" ;
            
            var result = await _testAWSS3CrudManager.IsS3FileExists(bucketName, key);
      
            Assert.IsType<bool>(result);
            Assert.False(result);

        }

        [Fact]
        public async Task DeleteFileAsync_ValidContext_ReturnsTrue()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default)).ReturnsAsync(new DeleteObjectResponse());
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";

            var result = await _testAWSS3CrudManager.IsS3FileExists(bucketName, key);

            Assert.IsType<bool>(result);
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteFileAsync_DeleteObjectAsync_isCalledExactlyOnce()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default)).ReturnsAsync(new DeleteObjectResponse());

            var result = await _testAWSS3CrudManager.DeleteFileAsync(bucketName, key);

            _s3ClientMock.Verify(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default), Times.Once);

        }

        [Fact]
        public async Task DeleteFileAsync_InvalidKey_ReturnsFalse()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            _s3ClientMock.Setup(x => x.DeleteObjectAsync(It.IsAny<DeleteObjectRequest>(), default)).ThrowsAsync(new AmazonS3Exception("The specified key does not exist."));
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "invalid";

            var result = await _testAWSS3CrudManager.DeleteFileAsync(bucketName, key);

            Assert.IsType<bool>(result);
            Assert.False(result);
        }

        [Fact]
        public async Task UploadFileAsync_ValidContext_ReturnsTrue()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            _s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default));
            byte[] testByteArray = new byte[64];

            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";

            var result = await _testAWSS3CrudManager.UploadFileAsync(testByteArray, bucketName, key);

            Assert.IsType<bool>(result);
            Assert.True(result);

        }
        [Fact]
        public async Task UploadFileAsync_PutObjectAsync_isCalledExactlyOnce()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            byte[] testByteArray = new byte[64];
            _s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default)).ReturnsAsync(new PutObjectResponse());

            var result = await _testAWSS3CrudManager.UploadFileAsync(testByteArray, bucketName, key);

            _s3ClientMock.Verify(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default), Times.Once);

        }

        [Fact]
        public async Task UploadFileAsync_InvalidContext_ReturnsFalseInCaseOfAmazonException()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            _s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default)).ThrowsAsync(new AmazonS3Exception("some exception"));
            byte[] testByteArray = new byte[64];
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";

            var result = await _testAWSS3CrudManager.UploadFileAsync(testByteArray, bucketName, key);

            Assert.IsType<bool>(result);
            Assert.False(result);

        }

        [Fact]
        public async Task UploadFileAsync_InvalidContext_ReturnsFalseInCaseOfGenericException()
        {
            Mock<IAmazonS3> _s3ClientMock = new();
            _s3ClientMock.Setup(x => x.PutObjectAsync(It.IsAny<PutObjectRequest>(), default)).ThrowsAsync(new Exception(""));
            byte[] testByteArray = new byte[64];

            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";

            var result = await _testAWSS3CrudManager.UploadFileAsync(testByteArray, bucketName, key);

            Assert.IsType<bool>(result);
            Assert.False(result);
        }

        [Fact]
        public async Task DownloadFile_ValidKey_ReturnsFileByteArray()
        {
            Mock<IAmazonS3> _s3ClientMock = new();

            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            var expectedBytes = new byte[] { 1, 2, 3, 4 };
            var expResponseStream = new MemoryStream(expectedBytes);
            var expResponse = new GetObjectResponse
            {
                ResponseStream = expResponseStream
            };

            _s3ClientMock.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default)).ReturnsAsync(expResponse);
            var result = await _testAWSS3CrudManager.DownloadFileAsync(bucketName, key);

            Assert.IsType<byte[]>(result);
           
        }

        [Fact]
        public void DownloadFileAsync_InvalidKey_ReturnsException()
        {
            Mock<IAmazonS3> _s3ClientMock = new();

            _s3ClientMock.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default)).ThrowsAsync(It.IsAny<Exception>());
            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
          
            Assert.ThrowsAsync<Exception>(() => _testAWSS3CrudManager.DownloadFileAsync(bucketName, key));
        }

        [Fact]
        public async Task DownloadFileAsync_GetObjectAsync_isCalledExactlyOnce()
        {
            Mock<IAmazonS3> _s3ClientMock = new();

            TestAWSS3CrudManager _testAWSS3CrudManager = new(_s3ClientMock.Object);
            string bucketName = "";
            string key = "";
            var expectedBytes = new byte[] { 1, 2, 3, 4 };
            var expResponseStream = new MemoryStream(expectedBytes);
            var expResponse = new GetObjectResponse
            {
                ResponseStream = expResponseStream
            };
            _s3ClientMock.Setup(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default)).ReturnsAsync(expResponse);
            var result = await _testAWSS3CrudManager.DownloadFileAsync(bucketName, key);

            _s3ClientMock.Verify(x => x.GetObjectAsync(It.IsAny<GetObjectRequest>(), default), Times.Once);

        }

    }
}
