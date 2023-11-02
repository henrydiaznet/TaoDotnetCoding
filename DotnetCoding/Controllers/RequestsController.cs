using DotnetCoding.Core.Models;
using Microsoft.AspNetCore.Mvc;
using DotnetCoding.Services.Interfaces;

namespace DotnetCoding.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        
        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }
        
        /// <summary>
        /// Get list unprocessed requests
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRequests(CancellationToken cancellationToken)
        {
            var result = await _requestService.GetAllAsync(cancellationToken);
            
            if(!result.Any())
            {
                return NotFound();
            }
            
            return Ok(result);
        }
        
        /// <summary>
        /// Creates new request handling action
        /// </summary>
        /// 
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ProcessRequest([FromBody]RequestResolutionModel requestResolution, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var existing = await _requestService.GetByIdAsync(requestResolution.RequestId, cancellationToken);
            if (existing is null)
            {
                return NotFound();
            }

            await _requestService.ProcessRequestAsync(requestResolution, cancellationToken);
            return Ok();
        }
    }
}
