namespace UserService.Application.DTOs;

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

public class UploadImageRequest
{
    [Required]
    [SwaggerSchema(Description = "Файл изображения", Format = "binary")]
    public IFormFile File { get; set; }
}