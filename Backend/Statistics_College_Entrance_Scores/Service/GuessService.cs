using Statistics_College_Entrance_Scores.Common;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Service
{
    public interface IGuessService
    {
        JsonGuessScore guessMajorScoreById(string majorCode, string collegeCod, IList<int> yearsGuess);
    }
    public class GuessService : IGuessService
    {
        const string INFO_TEXT_LINEAR = "Using Linear Regression";
        const string INFO_TEXT_POLY = "Using Polynomial Regression";

        private readonly IMajorCollegeRepository _majorCollegeRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly ICollegeRepository _collegeRepository;
        public GuessService(IMajorCollegeRepository majorCollegeRepository,IMajorRepository majorRepository,ICollegeRepository collegeRepository)
        {
            this._majorCollegeRepository = majorCollegeRepository;
            this._majorRepository = majorRepository;
            this._collegeRepository = collegeRepository;
        }

        public JsonGuessScore guessMajorScoreById(string majorCode,string collegeCode, IList<int> yearsGuess)
        {
            var yearsPastTrainData = this._majorCollegeRepository.GetPastYearsTrainData(collegeCode,majorCode);
            var scoresPastTrainData = this._majorCollegeRepository.GetScores(majorCode, collegeCode, yearsPastTrainData);
            var guessScoreYearList = new List<JsonScore>();
            foreach(var y in yearsGuess)
            {
                var scoreGuessLinear = RegressionHelper.LinearRegression(yearsPastTrainData, scoresPastTrainData, y);
                guessScoreYearList.Add(new JsonScore(y, scoreGuessLinear, null,INFO_TEXT_LINEAR));

                var scoreGuessPolys = RegressionHelper.PolynomialRegression(scoresPastTrainData, yearsPastTrainData, y);
                foreach(var SGP in scoreGuessPolys)
                {
                    guessScoreYearList.Add(new JsonScore(y, SGP, null,INFO_TEXT_POLY));
                }
            }

            var collegeName = this._collegeRepository.findByCode(collegeCode).Result.name;

            var majorName = this._majorRepository.findByCode(majorCode).Result.name;

            return new JsonGuessScore(collegeCode,collegeName,majorCode,majorName, guessScoreYearList);
        }
    }
}
