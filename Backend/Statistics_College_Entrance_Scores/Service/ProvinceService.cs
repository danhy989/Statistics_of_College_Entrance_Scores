using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Statistics_College_Entrance_Scores.Repository;
using Statistics_College_Entrance_Scores.entity;

namespace Statistics_College_Entrance_Scores.Service
{
    public interface IProvinceService
    {
        List<Province> GetAll();
        List<Province> FindByName(string name);
        Province FindByID(long id);
    }
    public class ProvinceService : IProvinceService
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceService(IProvinceRepository provinceRepository)
        {
            this._provinceRepository = provinceRepository;
        }

        public List<Province> GetAll()
        {
            return this._provinceRepository.GetAll().Result.ToList();
        }

        public Province FindByID(long id)
        {
            return this._provinceRepository.FindByID(id).Result;
        }

        public List<Province> FindByName(string name)
        {
            return this._provinceRepository.FindByName(name).Result.ToList();
        }
    }
}
