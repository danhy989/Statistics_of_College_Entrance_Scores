using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Statistics_College_Entrance_Scores.entity;
using Microsoft.EntityFrameworkCore;

namespace Statistics_College_Entrance_Scores.Repository
{
    public interface IProvinceRepository
    {
        Task<IEnumerable<Province>> GetAll();
        Task<Province> FindByID(long id);
        Task<List<Province>> FindByName(string name);
    }
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly EntranceScoresContext _context;

        public ProvinceRepository(EntranceScoresContext context)
        {
            this._context = context;
        }

        public async Task<Province> FindByID(long id)
        {
            return await Task.Run(()=> this._context.provinces.FindAsync(id));
        }

        public async Task<IEnumerable<Province>> GetAll()
        {
            return await Task.Run(() => _context.provinces);
        }

        public async Task<List<Province>> FindByName(string name)
        {
            var param = name.Replace(" ", "&");
            RawSqlString rawSqlString = new RawSqlString("select  * from \"Entrance_Scores\".\"provinces\"" +
            "where to_tsvector(convertnonunicode(name)) @@ to_tsquery(convertnonunicode({0}))");
            var listProvinces = await Task.Run(() => _context.provinces
                .FromSql(rawSqlString, param)
                .ToList());
            return listProvinces;
        }
    }
}
