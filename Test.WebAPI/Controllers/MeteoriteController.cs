using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using Test.Core.Models;
using Test.Core.Services.Interfaces;

namespace Test.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeteoriteController : ControllerBase
    {
        private readonly IMeteoriteService _meteoriteService;

        public MeteoriteController(IMeteoriteService meteoriteService)
        {
            _meteoriteService = meteoriteService;
        }

        [HttpPost("filter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "List of meteorites")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public Task<MeteoriteContext> GetAsync([FromBody] MeteoritesFilter filter, CancellationToken cancellationToken)
        {
            return _meteoriteService.GetAsync(filter, cancellationToken);
        }
    }
}
