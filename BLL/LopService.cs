using DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class LopService
    {
        Model1 db = new Model1();   
        public List<Lop> GetAll()
        {
            return db.Lop.ToList();
        }
    }
}
