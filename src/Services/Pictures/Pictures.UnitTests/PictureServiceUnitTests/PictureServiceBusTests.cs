using Contracts.Events;
using Contracts.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Pictures.Core.Model.Entities;
using Pictures.Core.Model.Requests;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Pictures.UnitTests.PictureServiceUnitTests
{
    public class PictureServiceBusTests : PictureServiceSetup
    {
        [Fact]
        public async Task AddPictureAsync_Should_PublishCarRegistered()
        {
            var request = new AddPictureRequest
            {
                Picture = new FormFile(new MemoryStream(), 0, 0, null, "form.pdf")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/pdf"
                }
            };

            var responseMock = new ResponseMock<AnalyzePictureResult>(new AnalyzePictureResultMock
            {
                PlateNumber = "112233"
            });

            var path = new Uri("http://test.path.com");
            _requestClient
                .Setup(c => c.GetResponse<AnalyzePictureResult>(It.IsAny<object>(), default, default))
                .ReturnsAsync(responseMock);

            _blobStorageService
                .Setup(c => c.UploadFileBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(path);

            _pictureRepository
                .Setup(c => c.AddAsync(It.IsAny<Picture>()));

            var result = await _pictureService.AddPictureAsync(request);

            _bus.Verify(c => c.Publish<CarRegistered>(It.IsAny<object>(), default), Times.Once());
        }

        [Fact]
        public async Task AddPictureAsync_Should_GetPlateResponse()
        {
            var request = new AddPictureRequest
            {
                Picture = new FormFile(new MemoryStream(), 0, 0, null, "form.pdf")
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "application/pdf"
                }
            };

            var responseMock = new ResponseMock<AnalyzePictureResult>(new AnalyzePictureResultMock
            {
                PlateNumber = "112233"
            });

            var path = new Uri("http://test.path.com");
            _requestClient
                .Setup(c => c.GetResponse<AnalyzePictureResult>(It.IsAny<object>(), default, default))
                .ReturnsAsync(responseMock);

            _blobStorageService
                .Setup(c => c.UploadFileBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(path);

            _pictureRepository
                .Setup(c => c.AddAsync(It.IsAny<Picture>()));

            var result = await _pictureService.AddPictureAsync(request);

            _requestClient.Verify(c => c.GetResponse<AnalyzePictureResult>(It.IsAny<object>(), default, default), Times.Once());
        }
    }
}
