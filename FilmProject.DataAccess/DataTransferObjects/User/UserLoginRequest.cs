using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmProject.DataAccess.DataTransferObjects.User
{
    public record UserLoginRequest(string Field,string Password);
}
