using Statistics_College_Entrance_Scores;
using Statistics_College_Entrance_Scores.entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Statistics_College_Entrance_Scores.Repository
{
    public interface ICollegeRepository
    {
        Task<CollegeEntity> findByCode(string code);
        Task<IEnumerable<CollegeEntity>> GetAll();
        Task<List<CollegeEntity>> GetByProvince(long province_id);
        Task<List<CollegeEntity>> GetByName(string text);
    }
    public class CollegeRepository : ICollegeRepository
    {
        private readonly EntranceScoresContext _context;

        public CollegeRepository(EntranceScoresContext context)
        {
            this._context = context;
        }

        public async Task<CollegeEntity> findByCode(string code)
        {
            return await Task.Run(() => this._context.collegeEntities.FindAsync(code));
        }

        public async Task<IEnumerable<CollegeEntity>> GetAll()
        {
            return await Task.Run(() => _context.collegeEntities);
        }

        public async Task<List<CollegeEntity>> GetByProvince(long province_id)
        {
            var listColleges = await Task.Run(() => _context.collegeEntities.Where(x => x.province_id == province_id));
            return listColleges.ToList();
        }

        public async Task<List<CollegeEntity>> GetByName(string name)
        {
            var param = name.Replace(" ", "&");
            RawSqlString rawSqlString = new RawSqlString("select  * from \"Entrance_Scores\".\"collegeEntities\"" +
            "where to_tsvector(convertnonunicode(name) || ' ' || code) @@ to_tsquery(convertnonunicode({0}))");
            var listColleges = await Task.Run(()=> _context.collegeEntities
                .FromSql(rawSqlString, param)
                .ToList());
            return listColleges;
        }
    }
}
