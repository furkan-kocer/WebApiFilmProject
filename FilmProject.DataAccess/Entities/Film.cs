using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.DataAccess.Entities
{
    public class Film : BaseEntity
    {
        public string filmName { get; set; }
        public float price { get; set; }
        public string filmCode { get; set; }
    }
}
