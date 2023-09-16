using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace WeeklyReview.Server.Setup
{
    public class ExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order => int.MaxValue - 10;

        public void OnActionExecuted(ActionExecutedContext context)
        {
            UnAuthorized(context);
            KeyNotFound(context);
            InvalidOperation(context);
        }

        private void InvalidOperation(ActionExecutedContext context)
        {
            if (context.Exception is InvalidOperationException e)
            {
                context.Result = new ObjectResult(e.Message)
                {
                    StatusCode = 422
                };

                context.ExceptionHandled = true;
            }
        }

        private static void UnAuthorized(ActionExecutedContext context)
        {
            if (context.Exception is UnauthorizedAccessException e)
            {
                context.Result = new ObjectResult(e.Message)
                {
                    StatusCode = 401
                };

                context.ExceptionHandled = true;
            }
        }

        private static void KeyNotFound(ActionExecutedContext context)
        {
            if (context.Exception is KeyNotFoundException e)
            {
                context.Result = new ObjectResult(e.Message)
                {
                    StatusCode = 404
                };

                context.ExceptionHandled = true;
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}
