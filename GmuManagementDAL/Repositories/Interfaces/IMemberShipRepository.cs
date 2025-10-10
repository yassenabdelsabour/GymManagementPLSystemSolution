﻿using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface IMemberShipRepository
    {
        IEnumerable<MemberShip> GetAll();
        MemberShip? GetById(int memberId, int planId);
        int Add(MemberShip memberShip);
        int Update(MemberShip memberShip);
        int Delete(MemberShip memberShip);
    }
}
