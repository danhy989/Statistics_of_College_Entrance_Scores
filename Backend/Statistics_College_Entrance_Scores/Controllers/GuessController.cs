using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Statistics_College_Entrance_Scores.Common;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Service;

namespace Statistics_College_Entrance_Scores.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuessController : ControllerBase
    {

        private readonly IGuessService _guessService;
        public GuessController(IGuessService guessService)
        {
            this._guessService = guessService;
        }

        [HttpPost]
        public IActionResult GuessMajorScore([FromBody] GuessScoreDTO guessScoreDTO)
        {

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();

			var currentYear = DateTime.Now.Year;

			var checkGuessYear = guessScoreDTO.years.Where(c => c <= currentYear).Count();

			if(checkGuessYear >0)
			{
				return BadRequest(MessagesResponse.MESSAGE_BAD_REQUEST_GUESS_YEAR);
			}

			if(guessScoreDTO.collegeCode == null || guessScoreDTO.majorCode == null)
			{
				return BadRequest(MessagesResponse.MESSAGE_BAD_REQUEST_GUESS);
			}

			var rs = this._guessService.guessMajorScoreById(guessScoreDTO.majorCode, guessScoreDTO.collegeCode, guessScoreDTO.years);

            watch.Stop();
            var took = watch.ElapsedMilliseconds;
            return Ok(new JsonResponse(took, null, rs));
        }

	}
}