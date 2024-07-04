using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grocery_App.Controls
{
    public class UserSession
    {
        private static UserSession _instance;
        public static UserSession Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserSession();
                }
                return _instance;
            }
        }

        public string CurrentUsername { get; private set; }
        public string CurrentEmployeeName { get; private set; }

        private UserSession() { }

        public void SetCurrentUser(string username, string employeeName)
        {
            CurrentUsername = username;
            CurrentEmployeeName = employeeName;
        }
    }

}
