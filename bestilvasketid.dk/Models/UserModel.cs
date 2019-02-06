using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bestilvasketid.dk.Models
{
    public class UserModel
    {
        public string Email { get; set; }
        public string Address{ get; set; }
        public DateTime Created { get; set; }
        public DateTime LastLogin { get; set; }
        public DateTime Deleted { get; set; }
    }
}