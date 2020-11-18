﻿using AutoMapper;
using Contracts.Commands;
using Contracts.Events;
using Contracts.Results;
using MassTransit;
using Microsoft.Extensions.Logging;
using Pictures.Core.Interfaces;
using Pictures.Core.Model.DTOs;
using Pictures.Core.Model.Entities;
using Pictures.Core.Model.Requests;
using Shared.Core.BaseModels.Requests;
using Shared.Core.BaseModels.Responses;
using Shared.Core.BaseModels.Responses.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pictures.Core.Services
{
    public interface IPictureService
    {
        Task<IResultResponse<PictureDTO>> GetPictureAsync(int id);
        Task<IPagedResponse<PictureDTO>> GetNotRecognizedPicturesAsync(SearchRequest search);
        Task<string> AddPictureAsync(AddPictureRequest pictureRequest);
    }

    public class PictureService : IPictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IBusControl _bus;
        private readonly IMapper _mapper;
        private readonly ILogger<PictureService> _logger;

        public PictureService(
            IPictureRepository pictureRepository,
            IBlobStorageService blobStorageService,
            IBusControl bus,
            IMapper mapper,
            ILogger<PictureService> logger)
        {
            _pictureRepository = pictureRepository;
            _blobStorageService = blobStorageService;
            _bus = bus;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<string> AddPictureAsync(AddPictureRequest pictureRequest)
        {
            using var pictureStream = pictureRequest.Picture.OpenReadStream();
            using var pictureMemoryStream = new MemoryStream();
            pictureStream.CopyTo(pictureMemoryStream);

            pictureStream.Seek(0, SeekOrigin.Begin);
            var uri = new Uri("rabbitmq://localhost/ai-listener");

            var requestClient = _bus.CreateRequestClient<AnalyzePicture>(uri);

            var response = await requestClient.GetResponse<AnalyzePictureResult>(new
            {
                Data = pictureMemoryStream.ToArray()
            });

            var plateNumber = response.Message.PlateNumber;
            var filename = $"{Guid.NewGuid()}{Path.GetExtension(pictureRequest.Picture.FileName)}";
            var imagePath = await _blobStorageService.UploadFileBlobAsync(
                    "picturescontainer",
                    pictureStream,
                    pictureRequest.Picture.ContentType,
                    filename);

            var picture = new Picture
            {
                IsRecognized = !string.IsNullOrWhiteSpace(plateNumber),
                PlateNumber = plateNumber,
                ImagePath = imagePath.AbsoluteUri
            };

            await _pictureRepository.AddAsync(picture);

            _logger.LogInformation($"Picture registered - [ID={picture.Id}] [PlateNumber={picture.PlateNumber}] [ImagePath={picture.ImagePath}] [IsRecognized={picture.IsRecognized}]");

            if (string.IsNullOrWhiteSpace(plateNumber))
            {
                return "Not recognized";
            }

            await _bus.Publish<CarRegistered>(new
            {
                PictureId = picture.Id,
                PlateNumber = picture.PlateNumber
            });

            return response.Message.PlateNumber;
        }

        public async Task<IPagedResponse<PictureDTO>> GetNotRecognizedPicturesAsync(SearchRequest search)
        {
            var response = new PagedResponse<PictureDTO>();

            var pictures = await _pictureRepository.GetPagedByAsync(
                c => !c.IsRecognized,
                search.PageNumber,
                search.PageSize);

            _mapper.Map(pictures, response);

            return response;
        }

        public async Task<IResultResponse<PictureDTO>> GetPictureAsync(int id)
        {
            var response = new ResultResponse<PictureDTO>();

            var pictureEntity = await _pictureRepository.GetByAsync(c => c.Id == id);

            if (pictureEntity == null)
            {
                response.AddError(new Error
                {
                    Message = "Picture not found"
                });

                return response;
            }

            response.Result = _mapper.Map<PictureDTO>(pictureEntity);

            return response;
        }
    }
}
