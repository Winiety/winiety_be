using Contracts.Results;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Moq;
using Pictures.Core.Model.DTOs;
using Pictures.Core.Model.Entities;
using Pictures.Core.Model.Requests;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Pictures.UnitTests.PictureServiceUnitTests
{
    public class PictureServiceTests : PictureServiceSetup
    {
        [Fact]
        public async Task GetPictureAsync_Should_ReturnPicture()
        {
            var picture = new Picture
            {
                Id = 1,
                ImagePath = "Test",
                IsRecognized = true,
                PlateNumber = "112233"
            };

            var pictureDto = new PictureDTO
            {
                Id = 1,
                ImagePath = "Test",
                IsRecognized = true,
                PlateNumber = "112233"
            };

            _pictureRepository
                .Setup(c => c.GetAsync(It.IsAny<int>(), false))
                .ReturnsAsync(picture);

            _mapper
                .Setup(c => c.Map<PictureDTO>(picture))
                .Returns(pictureDto);

            var result = await _pictureService.GetPictureAsync(1);

            Assert.True(result.IsSuccess);
            Assert.Equal(pictureDto, result.Result);
        }

        [Fact]
        public async Task GetPictureAsync_Should_ReturnPictureNotFound()
        {
            var picture = new Picture
            {
                Id = 1,
                ImagePath = "Test",
                IsRecognized = true,
                PlateNumber = "112233"
            };

            _pictureRepository
                .Setup(c => c.GetByAsync(It.IsAny<Expression<Func<Picture, bool>>>(), false))
                .ReturnsAsync((Picture)null);

            var result = await _pictureService.GetPictureAsync(1);

            Assert.False(result.IsSuccess);
            Assert.Equal(1, result.Errors.Count);
            Assert.Equal("Nie znaleziono zdjêcia", result.Errors.First().Message);
        }

        [Fact]
        public async Task GetNotRecognizedPicturesAsync_Should_ReturnNotRecognizedPictures()
        {
            var pictures = GetPictures();
            var searchRequest = new SearchRequest();
            var paged = new PagedList<Picture>()
            {
                TotalPages = 1,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = pictures.Count(),
                PageSize = searchRequest.PageSize,
            };

            var response = new PagedResponse<PictureDTO>()
            {
                TotalPages = paged.TotalPages,
                CurrentPage = searchRequest.PageNumber,
                TotalCount = paged.TotalCount,
                PageSize = searchRequest.PageSize,
                Results = GetPicturesDTO(pictures)
            };

            paged.AddRange(pictures);

            _pictureRepository
                .Setup(c => c.GetQueryable())
                .Returns(pictures.AsQueryable());

            _pictureRepository
                .Setup(c => c.GetPagedByAsync(It.IsAny<IQueryable<Picture>>(), searchRequest.PageNumber, searchRequest.PageSize))
                .ReturnsAsync(paged);

            _mapper
                .Setup(c => c.Map(It.IsAny<IPagedList<Picture>>(), It.IsAny<PagedResponse<PictureDTO>>()))
                .Returns(response);

            var result = await _pictureService.GetNotRecognizedPicturesAsync(searchRequest);

            Assert.True(result.IsSuccess);
            Assert.Equal(pictures.Count(), result.TotalCount);
        }

        [Fact]
        public async Task AddPictureAsync_Should_AddPictureWithPlateNumber()
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
                .Setup(c => c.GetResponse<AnalyzePictureResult>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync(responseMock);

            _blobStorageService
                .Setup(c => c.UploadFileBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(path);

            _pictureRepository
                .Setup(c => c.AddAsync(It.IsAny<Picture>()));

            var result = await _pictureService.AddPictureAsync(request);

            Assert.Equal(responseMock.Message.PlateNumber, result);
            _pictureRepository.Verify(c => c.AddAsync(It.IsAny<Picture>()), Times.Once());
        }

        [Fact]
        public async Task AddPictureAsync_Should_AddPictureWithNoLicensePlate()
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
                PlateNumber = null
            });

            var path = new Uri("http://test.path.com");
            _requestClient
                .Setup(c => c.GetResponse<AnalyzePictureResult>(It.IsAny<object>(), default, RequestTimeout.After(null, null, 5, null, null)))
                .ReturnsAsync(responseMock);

            _blobStorageService
                .Setup(c => c.UploadFileBlobAsync(It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(path);

            _pictureRepository
                .Setup(c => c.AddAsync(It.IsAny<Picture>()));

            var result = await _pictureService.AddPictureAsync(request);

            Assert.Equal("Nie rozpoznano", result);
            _pictureRepository.Verify(c => c.AddAsync(It.IsAny<Picture>()), Times.Once());
        }
    }
}
