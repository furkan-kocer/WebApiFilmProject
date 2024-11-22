using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.DataAccess.FilterErrors
{
    public class FilterErrorModelResponse
    {
        public List<FilterErrorModel> Errors { get; set; }  = new List<FilterErrorModel>();
    }
}
