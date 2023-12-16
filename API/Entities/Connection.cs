using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Connection
    {
        public Connection()// need to give and empty constructor bcz of entity framworks, cant just start with param const.//otherwise complain while migration and schema creation
        {
            
        }
        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId {get;set;}

        public string Username {get;set;}
    }
}