﻿using FileHosting.Domain.Dto;
using FileHosting.Domain.Models;

namespace FileHosting.Domain.Services.Interfaces;

public interface IFileUploadService
{
    Task<FileUploadedDto> UploadFile(FullFileDto fileDto);
}