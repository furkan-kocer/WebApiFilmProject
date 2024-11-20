using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.DataAccess.DataTransferObjects.Film
{
    public record FilmResponse(string filmName, float price, string filmCode);
}
