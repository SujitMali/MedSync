using MedSync_ClassLibraries.DAL;
using MedSync_ClassLibraries.Helpers;
using MedSync_ClassLibraries.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace MedSync_API.Controllers
{
    public class AccountController : ApiController
    {

        [HttpPost]
        public IHttpActionResult Login([FromBody] UserModel loginRequest)
        {
            if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.PasswordHash))
            {
                return Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = "Request body cannot be empty."
                });
            }


            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                return Content(HttpStatusCode.BadRequest, new
                {
                    success = false,
                    message = "Validation failed.",
                    errors
                });
            }


            try
            {
                User usersDal = new User();
                var user = usersDal.GetUserByEmail(loginRequest);
                if (user == null)
                {
                    return Content(HttpStatusCode.Unauthorized, new
                    {
                        success = false,
                        message = "Invalid email or password."
                    });
                }

                var token = JwtTokenManager.GenerateToken(user);

                return Ok(new
                {
                    success = true,
                    message = "Login successful.",
                    data = new
                    {
                        Token = token,
                        User = new
                        {
                            user.UserID,
                            user.Email,
                            user.RoleName,
                            user.FirstName,
                            user.LastName
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                DbErrorLogger.LogError(ex, createdBy: 1);
                return Content(HttpStatusCode.InternalServerError, new
                {
                    success = false,
                    message = $"Login failed: {ex.Message}"
                });
            }
        }


    }
}
