using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Coti.Models.Domain;
using Coti.Models.Requests.Classes;
using Coti.Services;
using Coti.Web.Controllers;
using Coti.Web.Models.Responses;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Coti.Models;

namespace Coti.Web.Api.Controllers
{
    [Route("api/classes")]
    [ApiController]
    public class ClassApiController : BaseApiController
    {

        private IClassesService _service = null;
        private IAuthenticationService<int> _authService = null;
        //The middleman between the HTTP request provided by this interface (ClassApiController)
        public ClassApiController(IClassesService service
            , IAuthenticationService<int> authService
            , ILogger<ClassApiController> logger) : base(logger)
        {
            _service = service;
            _authService = authService;
        }

        //GET api/widgets
        [HttpGet("")]
        public ActionResult<ItemsResponse<Class>> GetAll()
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                List<Class> list = _service.GetAll();

                if (list == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resources not found.");
                }
                else
                {
                    response = new ItemsResponse<Class> { Items = list };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Class>> Get(int id)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Class @class = _service.Get(id);

                if (@class == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Class> { Item = @class };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);

        }

        [HttpGet("my-classes")]
        public ActionResult<ItemResponse<Paged<MemberClass>>> GetByUser(int pageIndex, int pageSize)
        {
            int iCode = 200;

            BaseResponse response = null;

            int userId = _authService.GetCurrentUserId();

            if(userId !> 0)
            {
                iCode = 403;
                response = new ErrorResponse("Application resource not found.");
            }
            try
            {
                Paged<MemberClass> paged = _service.GetByUser(pageIndex, pageSize, userId);

                if (paged == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Application resource not found.");
                }
                else
                {
                    response = new ItemResponse<Paged<MemberClass>> { Item = paged };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse($"Generic Error: {ex.Message}");
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);

        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> Delete(int id)
        {

            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Delete(id);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);

        }

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(ClassAddRequest model)
        {
            ObjectResult result = null;


            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);

                ItemResponse<int> response = new ItemResponse<int>()

                {
                    Item = id
                };

                result = Created201(response);

            }

            catch (Exception ex)
            {

                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
                base.Logger.LogError(ex.ToString());
            }


            return result;
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(ClassUpdateRequest model)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.Update(model);
                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                response = new ErrorResponse(ex.Message);
                base.Logger.LogError(ex.ToString());
            }

            return StatusCode(iCode, response);
        }
    }
}
