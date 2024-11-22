using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.DataAccess.FilterErrors
{
    public class FilterErrorModel
    {
        public string fieldName { get; set; }
        public List<string> messages { get; set; } = new List<string>();
    }
}
