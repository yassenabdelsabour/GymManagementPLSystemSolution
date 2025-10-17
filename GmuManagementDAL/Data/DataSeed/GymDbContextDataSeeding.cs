﻿using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextDataSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlan = dbContext.Plans.Any();
                var HasCategory = dbContext.Categories.Any();
                if (HasPlan && HasCategory)
                    return false;

                if (!HasCategory)
                {
                    var categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (categories.Any())
                        dbContext.Categories.AddRange(categories);
                }
                if (!HasPlan)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        dbContext.Plans.AddRange(plans);
                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Seeding Failed {ex}");
                return false;
            }
        }
        private static List<T> LoadDataFromJsonFile<T>(string fileName)
        {
          var FilePath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot\\Files", fileName);
            if (!File.Exists(FilePath))
                throw new FileNotFoundException();
           
            string Data = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
            };
            return JsonSerializer.Deserialize<List<T>>(Data, Options) ?? new List<T>();



        }
    }
}
