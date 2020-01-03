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
        JsonGuessScore guessMajorScoreById(string majorCode, string collegeCod, string groupCode ,IList<int> yearsGuess);
    }
    public class GuessService : IGuessService
    {
        const string INFO_TEXT_LINEAR = "Linear Regression";
        const string INFO_TEXT_POLY = "Polynomial Regression";

        private readonly IMajorCollegeRepository _majorCollegeRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly ICollegeRepository _collegeRepository;
        public GuessService(IMajorCollegeRepository majorCollegeRepository,IMajorRepository majorRepository,ICollegeRepository collegeRepository)
        {
            this._majorCollegeRepository = majorCollegeRepository;
            this._majorRepository = majorRepository;
            this._collegeRepository = collegeRepository;
        }

        public JsonGuessScore guessMajorScoreById(string majorCode,string collegeCode,string groupCode,  IList<int> yearsGuess)
        {
            var collegeName = this._collegeRepository.findByCode(collegeCode).Result.name;

            var majorName = this._majorRepository.findByCode(majorCode).Result.name;

            var yearsPastTrainData = this._majorCollegeRepository.GetPastYearsTrainData(collegeCode,majorCode,groupCode);
            double[] scoresPastTrainData;
            try
            {
                scoresPastTrainData = this._majorCollegeRepository.GetScores(majorCode, collegeCode, groupCode, yearsPastTrainData);
            }catch(IndexOutOfRangeException e)
            {
                throw new ArgumentNullException("Dữ liệu trường: " + collegeName + ", ngành:" + majorName + " gặp một số vấn đề nên không thể dự đoán được, vui lòng thử lại sau!");
            }


            var guessScoreYearList = new List<JsonScore>();
           
            if (yearsPastTrainData.Length < 3 || scoresPastTrainData.Length <3)
            {
                throw new ArgumentNullException("Dữ liệu trong quá khứ quá ít để dự đoán ");
            }

            foreach (var y in yearsGuess)
            {
                var scoreGuessLinear = RegressionHelper.LinearRegression(yearsPastTrainData, scoresPastTrainData, y);
                guessScoreYearList.Add(new JsonScore(y, scoreGuessLinear, groupCode,INFO_TEXT_LINEAR));
                var scoreGuessPolys = RegressionHelper.PolynomialRegression(scoresPastTrainData, yearsPastTrainData, y);
                foreach (var SGP in scoreGuessPolys)
                {
                    guessScoreYearList.Add(new JsonScore(y, SGP, groupCode, INFO_TEXT_POLY));
                }
            }

            return new JsonGuessScore(collegeCode,collegeName,majorCode,majorName,groupCode,guessScoreYearList);
        }
    }
}
