using GApiProto.UserModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GApiProto.DataAccess
{
    public class UserProfileService
    {
        private readonly userContext _userContext;

        public UserProfileService(userContext context)
        {
            _userContext = context;
        }

        public async Task<UserProfile> GetByUserId(string userId)
        {
            return await _userContext.UserProfile
                .Where(x => x.UserId == userId)
                .FirstOrDefaultAsync();
        }
    }
}
