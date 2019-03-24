using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipUIRework.ViewModels 
{
    public class LoginViewModel : ViewModelBase
    {

        private string _username;

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        
    }
}
