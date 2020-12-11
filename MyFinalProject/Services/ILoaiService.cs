using MyFinalProject.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinalProject.Services
{
    public interface ILoaiService
    {
        IEnumerable<Loai> GetAll();
        Loai GetById(int id);
        Loai AddLoai(Loai id);
    }

    public class LoaiService : ILoaiService
    {
        private readonly MyStore2020Context _context;

        public LoaiService(MyStore2020Context db)
        {
            _context = db;
        }

        public Loai AddLoai(Loai id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Loai> GetAll()
        {
            return _context.Loai.ToList();
        }

        public Loai GetById(int id)
        {
            return _context.Loai.SingleOrDefault(p => p.MaLoai == id);
        }
    }
}
