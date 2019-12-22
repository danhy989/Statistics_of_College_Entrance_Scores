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
    public interface ICollegeService
    {
        JsonCollegeGroupByYears findScoreByCollegeCode(string code, List<int> years);
        List<CollegeEntity> GetAll();
        CollegeEntity findByCode(string code);
        List<CollegeEntity> GetCollegeByProvince(long province_id);
        List<CollegeEntity> GetCollegeByGroupCode(string groupCode);
        List<CollegeEntity> GetCollegesByName(string name);
    }
    public class CollegeService : ICollegeService
    {
        private readonly IMajorRepository _majorRepository;
        private readonly IMajorCollegeRepository _majorCollegeRepository;
        private readonly ICollegeRepository _collegeRepository;
        public CollegeService(IMajorRepository majorRepository, IMajorCollegeRepository majorCollegeRepository, ICollegeRepository collegeRepository)
        {
            this._majorRepository = majorRepository;
            this._majorCollegeRepository = majorCollegeRepository;
            this._collegeRepository = collegeRepository;
        }

        public CollegeEntity findByCode(string code)
        {
            return this._collegeRepository.findByCode(code).Result;
        }

        public JsonCollegeGroupByYears findScoreByCollegeCode(string code, List<int> years)
        {
            years.Sort((s1, s2) => s2.CompareTo(s1));

            var jsonCollegeGroupByYears = new JsonCollegeGroupByYears();

            var collegeName = _collegeRepository.findByCode(code).Result.name;
            var majors = new List<JsonMajorsInCollegeGroupByYears>();

            foreach(var y in years)
            {
                var jmi = new JsonMajorsInCollegeGroupByYears();
                var jsgbyList = new List<JsonScoreMajorGroupByYears>();

                var majorColleges = _majorCollegeRepository.GetMajorCollegesByCollegeCodeAndYear(code, y).Result;
                foreach(var mc in majorColleges)
                {
                    JsonScoreMajorGroupByYears jsgby = new JsonScoreMajorGroupByYears();

                    jsgby.groupCode = mc.groupCode;
                    jsgby.majorCode = mc.MajorEntityId;
                    jsgby.majorName = _majorRepository.findByCode(jsgby.majorCode).Result.name;
                    jsgby.score = mc.score;
                    jsgbyList.Add(jsgby);
                }

                jmi.year = Convert.ToInt32(y);
                jmi.majors = jsgbyList;
                majors.Add(jmi);
            }


            jsonCollegeGroupByYears.collegeCode = code;
            jsonCollegeGroupByYears.collegeName = collegeName;
            jsonCollegeGroupByYears.majors = majors;

            return jsonCollegeGroupByYears;
        }


        public List<CollegeEntity> GetAll()
        {
            return this._collegeRepository.GetAll().Result.ToList();
        }

        public List<CollegeEntity> GetCollegeByProvince(long province_id)
        {
            return this._collegeRepository.GetByProvince(province_id).Result;
        }
        public List<CollegeEntity> GetCollegeByGroupCode(string groupCode)
        {
            var colleges = this._majorCollegeRepository.GetCollegeByGroupCode(groupCode);
            return colleges;
        }
        public List<CollegeEntity> GetCollegesByName(string name)
        {
            var colleges = this._collegeRepository.GetByName(name).Result;
            return colleges;
        }
    }
}
