using System;
using System.Collections.Generic;

namespace GApiProto.UserModel
{
    public partial class UserProfile
    {
        public ulong Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public uint Crystal { get; set; }
        public uint CrystalFree { get; set; }
        public uint FriendCoin { get; set; }
        public ushort TutorialProgress { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
