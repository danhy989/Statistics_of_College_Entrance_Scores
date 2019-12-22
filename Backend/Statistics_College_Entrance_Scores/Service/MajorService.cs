using Statistics_College_Entrance_Scores.entity;
using Statistics_College_Entrance_Scores.Dto;
using Statistics_College_Entrance_Scores.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Statistics_College_Entrance_Scores.Payload;

namespace Statistics_College_Entrance_Scores.Service
{

    public interface IMajorService
    {
        JsonMajorGroupByYears findScoreByMajorCode(string majorCode,List<int> years);
        List<MajorEntity> GetAll();
        MajorEntity findByCode(string code);
        JsonCompare findScoreByCollegeCompared(string majorCode, IList<string> collegeCodes, int year);
        List<MajorEntity> GetMajorsByGroupCode(string groupCode);
        List<MajorEntity> GetMajorsByName(string name);
    }
    public class MajorService : IMajorService
    {
        //@Autowire
        private readonly IMajorRepository _majorRepository;
        private readonly IMajorCollegeRepository _majorCollegeRepository;
        private readonly ICollegeRepository _collegeRepository;
        public MajorService(IMajorRepository majorRepository,IMajorCollegeRepository majorCollegeRepository,ICollegeRepository collegeRepository)
        {
            this._majorRepository = majorRepository;
            this._majorCollegeRepository = majorCollegeRepository;
            this._collegeRepository = collegeRepository;
        }

        public List<MajorEntity> GetAll()
        {
            return this._majorRepository.GetAll().Result.ToList();
        }

        public MajorEntity findByCode(string code)
        {
            return this._majorRepository.findByCode(code).Result;
        }

        public JsonMajorGroupByYears findScoreByMajorCode(string majorCode, List<int> years)
        {
            years.Sort((s1, s2) => s2.CompareTo(s1));
            var jsonMajorGroupByYears = new JsonMajorGroupByYears();

            var majorName = _majorRepository.findByCode(majorCode).Result.name;
            var colleges = new List<JsonCollegesInMajorGroupByYears>();

            foreach (var y in years)
            {
                var jci = new JsonCollegesInMajorGroupByYears();
                var jscgbyList = new List<JsonScoreCollegeGroupByYears>();

                var majorColleges = _majorCollegeRepository.GetMajorCollegesByMajorCodeAndYear(majorCode, y).Result;
                foreach (var mc in majorColleges)
                {
                    JsonScoreCollegeGroupByYears jscgby = new JsonScoreCollegeGroupByYears();

                    jscgby.groupCode = mc.groupCode;
                    jscgby.collegeCode = mc.CollegeEntityId;
                    jscgby.collegeName = _collegeRepository.findByCode(mc.CollegeEntityId).Result.name;
                    jscgby.score = mc.score;
                    jscgbyList.Add(jscgby);
                }

                jci.year = Convert.ToInt32(y);
                jci.colleges = jscgbyList;
                colleges.Add(jci);
            }


            jsonMajorGroupByYears.majorCode = majorCode;
            jsonMajorGroupByYears.majorName = majorName;
            jsonMajorGroupByYears.colleges = colleges;

            return jsonMajorGroupByYears;
        }

        public JsonCompare findScoreByCollegeCompared(string majorCode, IList<string> collegeCodes, int year)
        {
            var jsonCompare = new JsonCompare();
            var majorName = _majorRepository.findByCode(majorCode).Result.name;
            var jsonCompareColleges = new List<JsonCompareColleges>();

            var majorColleges = _majorCollegeRepository.findScoreByCollegeCompared(majorCode, collegeCodes, year);

            foreach(var mc in majorColleges)
            {
                if (mc == null) continue;
                var jsonCC = new JsonCompareColleges();

                jsonCC.collegeCode = mc.CollegeEntityId;
                jsonCC.collegeName = _collegeRepository.findByCode(mc.CollegeEntityId).Result.name;
                jsonCC.groupCode = mc.groupCode;
                jsonCC.score = mc.score;

                jsonCompareColleges.Add(jsonCC);
            }

            jsonCompare.majorCode = majorCode;
            jsonCompare.majorName = majorName;
            jsonCompare.year = year;
            jsonCompare.colleges = jsonCompareColleges;
            
            return jsonCompare;
        }

        public List<MajorEntity> GetMajorsByGroupCode(string groupCode)
        {
            var majors = _majorCollegeRepository.GetMajorByGroupCode(groupCode);
            return majors;
        }

        public List<MajorEntity> GetMajorsByName(string name)
        {
            var majors = _majorRepository.GetByName(name).Result;
            return majors;
        }
    }
}
