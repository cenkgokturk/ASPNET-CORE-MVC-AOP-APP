using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNETAOP_Session;

namespace ASPNETAOP.Models
{
    public class ActiveUser
    {
        public int id;

        public static readonly ActiveUser UserInfo = new ActiveUser();

        public ActiveUser(int id){ this.id = id;}

        public void setID(int id){ this.id = id;}

        public ActiveUser(){}
    }
}
