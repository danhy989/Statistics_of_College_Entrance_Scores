using Statistics_College_Entrance_Scores.entity;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MathNet.Numerics;
using System.Text.RegularExpressions;
using Statistics_College_Entrance_Scores.Common;

namespace Statistics_College_Entrance_Scores.Repository
{
    public interface IMajorCollegeRepository
    {
        Task<List<MajorCollege>> GetMajorCollegesByMajorCodeAndYear(string code, int year);
        Task<List<MajorCollege>> GetMajorCollegesByCollegeCodeAndYear(string code, int year);
        List<MajorCollege> findScoreByCollegeCompared(string majorCode, IList<string> collegeCodes, int year);
        double[] GetPastYearsTrainData(string collegeCode,string majorCode);
        double[] GetPastYearsTrainData(string collegeCode, string majorCode,string groupCode);
        List<CollegeEntity> GetCollegeByGroupCode(string groupCode);
        List<MajorEntity> GetMajorByGroupCode(string groupCode);
        double[] GetScores(string majorCode, string collegeCode, string groupCode ,double[] years);
        int[] GetYears();
        Task<List<MajorEntity>> FindMajorAndCollegeByName(string name);
		Task<MajorCollege> GetMajorCollegesByCollegeCodeAndMajorAndYear(string collegeId, string majorId, double y);
        List<MajorCollege> GetMajorCollegesLikeGroupCode(string collegeCode, string majorCode, string groupCode);
        string[] getGroupCodeBy(string collegeCode, string majorCode);
    }
    public class MajorCollegeRepository : IMajorCollegeRepository
    {
        private readonly EntranceScoresContext _context;
        public MajorCollegeRepository(EntranceScoresContext context)
        {
            this._context = context;
        }

        public async Task<List<MajorCollege>> GetMajorCollegesByCollegeCodeAndYear(string code, int year)
        {
            return await Task.Run(() => this._context.majorColleges.Where(c => c.CollegeEntityId.Equals(code) && c.year.Equals(year)).ToList());
        }

        public async Task<List<MajorCollege>> GetMajorCollegesByMajorCodeAndYear(string code, int year)
        {
            return await Task.Run(() =>  this._context.majorColleges.Where(c => c.MajorEntityId.Equals(code) && c.year.Equals(year)).ToList());
        }

        public List<MajorCollege> findScoreByCollegeCompared(string majorCode, IList<string> collegeCodes, int year)
        {
            var rs = new List<MajorCollege>();
            foreach (var code in collegeCodes)
            {
                var majorCollegeTemp = this._context.majorColleges.Where(c => c.MajorEntityId.Equals(majorCode) && c.CollegeEntityId.Equals(code) && c.year.Equals(year)).FirstOrDefault() ;
                rs.Add(majorCollegeTemp);
            }

            return rs;
        }

        public double[] GetPastYearsTrainData(string collegeCode, string majorCode)
        {
            var yearsQuery = _context.majorColleges
                .Where(s=>s.MajorEntityId.Equals(majorCode) && s.CollegeEntityId.Equals(collegeCode))
                .GroupBy( c => new { c.year })
                .Select(g => new
                {
                    g.Key.year
                })
                .OrderBy(x => x.year).ToList();

            double[] years = new double[yearsQuery.Count];

            for(int i = 0; i < yearsQuery.Count; i++)
            {
                years[i] = yearsQuery[i].year;
            }

            return years;
        }

        public double[] GetScores(string majorCode, string collegeCode, string groupCode, double[] years)
        {
            double[] rs = new double[years.Length];
            var majorColleges = this.GetMajorCollegesLikeGroupCode(collegeCode, majorCode, groupCode.Trim());
            for(int i=0;i<majorColleges.Count;i++)
            {
                try
                {
                    rs[i] = majorColleges[i].score;
                }catch(IndexOutOfRangeException e) {
                    throw new IndexOutOfRangeException();
                }
               
            }   

            return rs;
        }

        public List<CollegeEntity> GetCollegeByGroupCode(string groupCode)
        {
            var rs = this._context.majorColleges.Where(c=>c.groupCode.Replace(",", " ").IndexOf(groupCode) >= 0).Select(c => c.CollegeEntity).Distinct();
            var colleges = rs.ToList();
            return colleges;
        }
        public List<MajorEntity> GetMajorByGroupCode(string groupCode)
        {
            var rs = this._context.majorColleges.Where(c=>c.groupCode.Replace(",", " ").IndexOf(groupCode) >= 0).Select(c => c.MajorEntity).Distinct();
            var majors = rs.ToList();
            return majors;
        }

        public int[] GetYears()
        {
            var listColleges = _context.majorColleges;
            var rs = listColleges.Select(y => y.year).Distinct().ToArray();
            Array.Sort(rs);
            Array.Reverse(rs);
            return rs;
        }

        public async Task<List<MajorEntity>> FindMajorAndCollegeByName(string name)
        {
            var param = name.Replace(" ", "&");
            RawSqlString rawSqlString = new RawSqlString("select  * from \"college_major\"" +
            "where to_tsvector(convertnonunicode(name) || ' ' || code) @@ to_tsquery(convertnonunicode({0}))");
            var listMajors = await Task.Run(() => _context.majorEntities
                .FromSql(rawSqlString, param)
                .ToList());
            return listMajors;
        }

        public async Task<MajorCollege> GetMajorCollegesByCollegeCodeAndMajorAndYear(string collegeId, string majorId, double y)
        {
            return await Task.Run(() => _context.majorColleges.Where(rs => rs.CollegeEntityId.Equals(collegeId)
            && rs.MajorEntityId.Equals(majorId) && rs.year.Equals(Convert.ToInt32(y))).FirstOrDefault());
        }

        public List<MajorCollege> GetMajorCollegesLikeGroupCode(string collegeCode, string majorCode, string groupCode)
        {
            string sqlLike = RegexHelper.getRegexLikeGroupCode(groupCode);

            string sql = "select * from \"Entrance_Scores\".\"majorColleges\" f " +
                  "where \"CollegeEntityId\" = '" + collegeCode + "' " +
                  "AND \"MajorEntityId\" = '" + majorCode + "' " +
                  "AND " +
                  sqlLike;
            RawSqlString rawSqlString = new RawSqlString(sql);

            var rs =  _context.majorColleges
               .FromSql(rawSqlString)
               .ToList();
            return rs;
        }

        public double[] GetPastYearsTrainData(string collegeCode, string majorCode, string groupCode)
        {
            string sqlLike = RegexHelper.getRegexLikeGroupCode(groupCode);
            string sql = "select year from \"Entrance_Scores\".\"majorColleges\" f " +
                  "where \"CollegeEntityId\" = '" + collegeCode + "' " +
                  "AND \"MajorEntityId\" = '" + majorCode + "' " +
                  "AND " +
                  sqlLike + " " +
                  "GROUP BY year order by  year";
            RawSqlString rawSqlString = new RawSqlString(sql);

            var rs = _context.majorColleges
               .FromSql(rawSqlString).Select(c => Convert.ToDouble(c.year)).ToArray();
            return rs;
        }

        public string[] getGroupCodeBy(string collegeCode, string majorCode)
        {
            string sql = "select \"groupCode\" from \"Entrance_Scores\".\"majorColleges\" f " +
                  "where \"CollegeEntityId\" = '" + collegeCode + "' " +
                  "AND \"MajorEntityId\" = '" + majorCode + "' " +
                  "GROUP BY \"groupCode\"";
            RawSqlString rawSqlString = new RawSqlString(sql);
            var rs = _context.majorColleges
               .FromSql(rawSqlString).Select(c=> c.groupCode).ToArray();
            var result = new string[rs.Length];
            for(int i=0;i<rs.Length;i++)
            {
                rs[i] = rs[i].Replace(";",",");
                result[i] = rs[i];
            }
            return rs;
        }
    }
}
