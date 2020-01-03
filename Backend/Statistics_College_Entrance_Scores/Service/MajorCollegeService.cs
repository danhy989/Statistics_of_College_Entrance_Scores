using Statistics_College_Entrance_Scores.Repository;
using Statistics_College_Entrance_Scores.entity;
using System.Collections.Generic;
using Statistics_College_Entrance_Scores.Payload;
using System;
using Statistics_College_Entrance_Scores.Dto;

namespace Statistics_College_Entrance_Scores.Service
{
    public interface IMajorCollegeService
    {
        int[] GetYears();
        List<MajorEntity> FindMajorAndCollegeByName(string name);
		JsonMajorCollege getScoreThroughYears(string collegeId, string majorId);
        HashSet<string> getGroupCodeBy(string collegeCode, string majorCode);
        string[] GetGroupCode();
    }
    public class MajorCollegeService : IMajorCollegeService
    {
        private string[] GROUP_CODE = { "A", "A1", "B", "C", "D", "D1", "D2","D3", "D4", "D5", "D6", "H",
                        "M", "N","N1", "S", "T", "K", "R", "V" };
        private readonly IMajorCollegeRepository _majorCollegeRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly ICollegeRepository _collegeRepository;
        public MajorCollegeService(IMajorCollegeRepository majorCollegeRepository, IMajorRepository majorRepository, ICollegeRepository collegeRepository)
        {
            this._majorCollegeRepository = majorCollegeRepository;
            this._majorRepository = majorRepository;
            this._collegeRepository = collegeRepository;
        }

        public int[] GetYears()
        {
            return this._majorCollegeRepository.GetYears();
        }

        public List<MajorEntity> FindMajorAndCollegeByName(string name)
        {
            var majors = _majorCollegeRepository.FindMajorAndCollegeByName(name).Result;
            return majors;
        }

        public JsonMajorCollege getScoreThroughYears(string collegeId, string majorId)
        {

            List<JsonScore> jsonScores = new List<JsonScore>();
            double[] years = _majorCollegeRepository.GetPastYearsTrainData(collegeId, majorId);

            foreach(var y in years)
            {
                JsonScore jc = new JsonScore();

                jc.year = Convert.ToInt32(y);


                MajorCollege majorCollegeScore = this._majorCollegeRepository.GetMajorCollegesByCollegeCodeAndMajorAndYear(collegeId, majorId, y).Result;

                jc.groupCode = majorCollegeScore.groupCode;

                jc.score = majorCollegeScore.score;

                jsonScores.Add(jc);
            }


            var collegeName = this._collegeRepository.findByCode(collegeId).Result.name;
            var majorName = this._majorRepository.findByCode(majorId).Result.name;
            JsonMajorCollege jsonMajorCollege = new JsonMajorCollege();
            jsonMajorCollege.collegeCode = collegeId;
            jsonMajorCollege.collegeName = collegeName;
            jsonMajorCollege.majorCode = majorId;
            jsonMajorCollege.majorName = majorName;
            jsonMajorCollege.scores = jsonScores;
            return jsonMajorCollege;
        }
       
        public HashSet<string> getGroupCodeBy(string collegeCode, string majorCode)
        {
            var rs = this._majorCollegeRepository.getGroupCodeBy(collegeCode,majorCode);
            HashSet<string> listRsSet = new HashSet<string>();
            for(int i = 0; i < rs.Length; i++)
            {
                char[] spearator = { ',', ' '};
                string[] strlist = rs[i].Split(spearator, StringSplitOptions.RemoveEmptyEntries);
                listRsSet.UnionWith(strlist);
            }
            return listRsSet;
        }

        public string[] GetGroupCode()
        {
            return this.GROUP_CODE;

        }
    }
}
