using Microsoft.AspNetCore.Mvc;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Service;
using Statistics_College_Entrance_Scores.Common;

namespace Statistics_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorCollegeController : ControllerBase
    {
        private readonly IMajorCollegeService _majorCollegeService;

        public MajorCollegeController(IMajorCollegeService majorCollegeService)
        {
            _majorCollegeService = majorCollegeService;
        }

        [HttpGet("years")]
        public IActionResult GetYears()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._majorCollegeService.GetYears();
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
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
            var rs = this._majorCollegeService.FindMajorAndCollegeByName(name);
            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            if (rs == null)
            {
                return NotFound(new JsonResponse(took, MessagesResponse.MESSAGE_NOT_FOUND, null));
            }
            return Ok(new JsonResponse(took, null, rs));
        }

        [HttpPost("statistic")]
        public IActionResult GetScoreThroughYears([FromBody] MajorCollegeDTO majorCollegeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var rs = this._majorCollegeService.getScoreThroughYears(majorCollegeDTO.collegeId,majorCollegeDTO.majorId);
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