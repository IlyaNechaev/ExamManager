﻿using ExamManager.Extensions;
using ExamManager.Models;
using AutoMapper;
using ExamManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ExamManager.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ExamManager.Controllers
{

    [Route("admin")]
    [ApiController]
    [Authorize]
    //[NotDefaultUser("Home", nameof(HomeController.ChangeUserData), "pageId", 1)]
    public class AdminController : ControllerBase
    {
        IUserService _userService { get; set; }
        IMapper _mapper { get; set; }
        IGroupService _groupService { get; set; }
        public AdminController(IUserService userService,
            IMapper mapper,
            IGroupService groupService)
        {
            _userService = userService;
            _mapper = mapper;
            _groupService = groupService;
        }

        [HttpPost("group/create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
        {
            var createdGroup = new Group();
            try
            {
                createdGroup = await _groupService.CreateGroup(request.GroupName);
            }
            catch (Exception ex)
            {
                return Ok(ResponseFactory.CreateResponse(ex));
            }

            return Ok(ResponseFactory.CreateResponse(createdGroup));
        }
    }
}
