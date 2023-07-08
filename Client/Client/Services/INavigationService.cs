using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface INavigationService
    {
        void NavigateToMain();
        void NavigateToGame();
        void NavigateToLogin();
    }

}
