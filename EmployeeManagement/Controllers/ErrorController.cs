using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace EmployeeManagement.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
        }

        [Route("[controller]/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.Message = "Sorry, the resource you requested could not found";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.Query = statusCodeResult.OriginalQueryString;

                    logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath}." +
                        $" Query String = {statusCodeResult.OriginalQueryString}");
                    break;
            }
            return View();
        }

        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            ViewBag.ExceptionPath = exceptionDetails.Path;
            ViewBag.ExceptionMessage = exceptionDetails.Error.Message;
            ViewBag.StackTrace = exceptionDetails.Error.StackTrace;

            logger.LogError($"The path {exceptionDetails.Path} threw an exception. {exceptionDetails.Error}");

            return View();
        }
    }
}