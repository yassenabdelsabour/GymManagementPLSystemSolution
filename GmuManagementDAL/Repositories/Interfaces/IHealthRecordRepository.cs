﻿using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface IHealthRecordRepository
    {
        IEnumerable<HealthRecord> GetAll();
        HealthRecord? GetById(int id);
        int Add(HealthRecord healthRecord);
        int Update(HealthRecord healthRecord);
        int Delete(HealthRecord healthRecord);
    }
}
