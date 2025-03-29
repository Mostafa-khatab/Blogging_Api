using BlogApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApi.Core.Repos
{
    public interface ITokenServices
    {
        string CreatToken(User user);

    }
}
