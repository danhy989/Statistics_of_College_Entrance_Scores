using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Statistics_College_Entrance_Scores;
using Statistics_College_Entrance_Scores.entity;
using Statistics_College_Entrance_Scores.Repository;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Common;
using Statistics_College_Entrance_Scores.dto;
using Statistics_College_Entrance_Scores.Service;

namespace Statistics_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }


        [HttpGet]
        public IActionResult GetMajorEntities()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._majorService.GetAll();
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpGet("{code}")]
        public IActionResult GetMajorByCode([FromRoute] string code)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._majorService.findByCode(code);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpPost]
        public IActionResult GetMajorByCode([FromBody] MajorDTO majorDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._majorService.findScoreByMajorCode(majorDTO.majorCode,majorDTO.years);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpPost("compare")]
        public IActionResult GetScoresByCollegeCompared([FromBody] ScoreCollegeComparedDTO scoreCollegeComparedDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._majorService.findScoreByCollegeCompared(scoreCollegeComparedDTO.majorCode, scoreCollegeComparedDTO.collegeCodes, scoreCollegeComparedDTO.year);
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
            var rs = this._majorService.GetMajorsByGroupCode(code);
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
            var rs = this._majorService.GetMajorsByName(name);
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