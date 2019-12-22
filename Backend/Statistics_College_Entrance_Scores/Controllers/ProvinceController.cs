using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Statistics_College_Entrance_Scores.Service;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Common;

namespace Statistics_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _provinceService;

        public ProvinceController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }
        [HttpGet]
        public IActionResult GetProvinces()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._provinceService.GetAll();
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            return Ok(new JsonResponse(took, null, rs));
        }
        
        [HttpGet("{id}")]
        public IActionResult GetProvinceByID([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._provinceService.FindByID(id);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpGet("find/{name}")]
        public IActionResult GetProvinceByName([FromRoute] string name)
        {
             if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._provinceService.FindByName(name);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }
    }
}