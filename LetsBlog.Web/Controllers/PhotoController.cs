using System.Linq;
using LetsBlog.Models.Photo;
using LetsBlog.Repository;
using LetsBLog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace LetsBlog.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoService;
    private readonly IPhotoRepository _photoRepository;
    private readonly IBlogRepository _blogRepository;

    public PhotoController(IPhotoService photoService, IPhotoRepository photoRepository, IBlogRepository blogRepository)
    {
        _photoService = photoService;
        _photoRepository = photoRepository;
        _blogRepository = blogRepository;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Photo>> UploadPhoto(IFormFile file)
    {
        int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

        var uploadResult = await _photoService.AddPhotoAsync(file);

        if (uploadResult.Error != null)
        {
            return BadRequest(uploadResult.Error.Message);
        }

        var photoCreate = new PhotoCreate
        {
            PublicId = uploadResult.PublicId,
            ImageUrl = uploadResult.SecureUrl.AbsoluteUri,
            Description = file.FileName
        };

        var photo = await _photoRepository.InsertAsync(photoCreate, applicationUserId);

        return Ok(photo);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<Photo>>> GetByApplicationUserId()
    {
        int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

        var photos = await _photoRepository.GetAllByUserIdAsync(applicationUserId);

        return Ok(photos);
    }

    [HttpGet("{photoId}")]
    public async Task<ActionResult<Photo>> Get(int photoId)
    {
        var photo = await _photoRepository.GetAsync(photoId);

        return Ok(photo);
    }

    [Authorize]
    [HttpDelete("{photoId}")]
    public async Task<ActionResult<int>> Delete(int photoId)
    {
        int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

        var foundPhoto = await _photoRepository.GetAsync(photoId);

        if (foundPhoto != null)
        {
            if(foundPhoto.ApplicationUserId == applicationUserId)
            {
                var blogs = await _blogRepository.GetAllByUserIdAsync(applicationUserId);

                var usedInBlog = blogs.Any(i => i.PhotoId == photoId);

                if(usedInBlog)
                {
                    return BadRequest("Photo is being used in one or more blogs, hence can't be deleted.");
                }

                var deleteResult = await _photoService.DeletePhotoAsync(foundPhoto.PublicId);

                if(deleteResult.Error != null)
                {
                    return BadRequest(deleteResult.Error.Message);
                }

                var affectedRows = await _photoRepository.DeleteAsync(foundPhoto.PhotoId);

                return Ok(affectedRows);
            }
            else
            {
                return BadRequest("Application Id of Photo does not match with the current user");
            }
        }

        return BadRequest("Photo does not exist.");
    }
}
