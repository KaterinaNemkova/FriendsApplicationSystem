using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace UserService.Application.DTOs;

public class UploadImageRequest
{
    [Required]
    [SwaggerSchema(Description = "Файл изображения", Format = "binary")]
    public IFormFile File { get; set; }
}