﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class MemberSession: BaseEntity
    {
        // BookingDate = CreatedAt of BaseEntity
        public bool IsAttended { get; set; }
        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
    }
}
