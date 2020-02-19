using GApiProto.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GApiProto.Dto.Responce
{
    public class LoginDto
    {
        public UserProfile UserProfile { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
