using Statistics_College_Entrance_Scores.entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Statistics_College_Entrance_Scores.Repository
{
    public interface IMajorRepository
    {
        Task<IEnumerable<MajorEntity>> GetAll();
        Task<MajorEntity> findByCode(string code);
        Task<List<MajorEntity>> GetByName(string name);
    }
    public class MajorRepository : IMajorRepository
    {
        private readonly EntranceScoresContext _context;

        public MajorRepository(EntranceScoresContext context)
        {
            this._context = context;
        }

        public async Task<MajorEntity> findByCode(string code)
        {
            return await Task.Run(() => this._context.majorEntities.FindAsync(code));
        }

        public async Task<IEnumerable<MajorEntity>> GetAll()
        {
            return await Task.Run(() => _context.majorEntities);
        }

        public async Task<List<MajorEntity>> GetByName(string name)
        {
            var param = name.Replace(" ", "&");
            RawSqlString rawSqlString = new RawSqlString("select  * from \"Entrance_Scores\".\"majorEntities\"" +
            "where to_tsvector(convertnonunicode(name) || ' ' || code) @@ to_tsquery(convertnonunicode({0}))");
            var listMajors = await Task.Run(() => _context.majorEntities
                .FromSql(rawSqlString, param)
                .ToList());
            return listMajors;
        }
    }
}
