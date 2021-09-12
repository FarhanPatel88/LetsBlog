﻿using System.IdentityModel.Tokens.Jwt;
using LetsBlog.Models.BlogComments;
using LetsBlog.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LetsBlog.Web.Controllers;
[Route("api/[controller]")]
[ApiController]
public class BlogCommentController : ControllerBase
{
    private readonly IBlogCommentRepository _blogCommentRepository;

    public BlogCommentController(IBlogCommentRepository blogCommentRepository)
    {
        _blogCommentRepository = blogCommentRepository;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BlogComment>> Create(BlogCommentCreate blogCommentCreate)
    {
        int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

        var createdComment = await _blogCommentRepository.UpsertAsync(blogCommentCreate, applicationUserId);

        return Ok(createdComment);
    }

    [HttpGet("{blogId}")]
    public async Task<ActionResult<List<BlogComment>>> GetAll(int blogId)
    {
        var blogComments = await _blogCommentRepository.GetAllAsync(blogId);

        return Ok(blogComments);
    }

    [Authorize]
    [HttpDelete("{blogCommentId}")] 
    public async Task<ActionResult<int>> Delete(int blogCommentId)
    {
        int applicationUserId = int.Parse(User.Claims.First(i => i.Type == JwtRegisteredClaimNames.NameId).Value);

        var foundBlogComment = await _blogCommentRepository.GetAsync(blogCommentId);

        if (foundBlogComment == null) return BadRequest("Blog Comment does not exist.");

        if(foundBlogComment.ApplicationUserId == applicationUserId)
        {
            var affectedRows = await _blogCommentRepository.DeleteAsync(blogCommentId);

            return Ok(affectedRows);
        }
        else
        {
            return BadRequest("You did not create this Blog Comment.");
        }
    }
}