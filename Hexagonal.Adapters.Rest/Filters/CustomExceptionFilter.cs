using Hexagonal.Application.Common.Exceptions.EntityExceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Hexagonal.Adapters.Rest.Filters
{
    /// <summary>
    /// Represents an filter attribute
    /// </summary>
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            // only handle application exceptions others should return 500 result;
            if (!(context.Exception is RepositoryError)) return;

            string message;
            int statusCode;
            string code;
            switch (context.Exception)
            {

                case EntityNotFound entityNotFound:
                    message = entityNotFound.Message;
                    statusCode = StatusCodes.Status404NotFound;
                    code = "NotFound";
                    break;

                default:
                    message = "";
                    statusCode = StatusCodes.Status500InternalServerError;
                    code = "";
                    break;
            }

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = statusCode;
            context.Result = new JsonResult(new { message, code, statusCode });
        }
    }
}
