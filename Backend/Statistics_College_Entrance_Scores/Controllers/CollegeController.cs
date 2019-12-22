using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Statistics_College_Entrance_Scores.dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Statistics_College_Entrance_Scores.Common;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Service;

namespace Statistics_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CollegeController : ControllerBase
    {
        private readonly ICollegeService _collegeService;

        public CollegeController(ICollegeService collegeService)
        {
            _collegeService = collegeService;
        }


        [HttpGet]
        public IActionResult GetCollegeEntities()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._collegeService.GetAll();
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpGet("{code}")]
        public IActionResult GetCollegeByCode([FromRoute] string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._collegeService.findByCode(code);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpPost]
        public IActionResult GetCollegeByCode([FromBody] CollegeDTO collegeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._collegeService.findScoreByCollegeCode(collegeDTO.collegeCode, collegeDTO.years);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }
        
        [HttpGet("province/{code}")]
        public IActionResult GetCollegeByProvince([FromRoute] long code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._collegeService.GetCollegeByProvince(code);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }
        [HttpGet("groupCode/{code}")]
        public IActionResult GetCollegeByGroupCode([FromRoute]string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._collegeService.GetCollegeByGroupCode(code);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpGet("find/{name}")]
        public IActionResult GetMajorsByName([FromRoute]string name)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._collegeService.GetCollegesByName(name);
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