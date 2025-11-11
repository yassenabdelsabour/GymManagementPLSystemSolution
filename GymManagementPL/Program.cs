using GymManagementBLL;
using GymManagementBLL.Service.AttachmentService;
using GymManagementBLL.Service.Classes;
using GymManagementBLL.Service.InterFaces;
using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Data.DataSeed;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GymManagementPL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<GymDbContext>(Options =>
            {
                //Options.UseSqlServer("Server=DESKTOP-T8S46TQ;Database=GymManagementGroup01;Trusted_Connection=True;TrustServerCertificate=True;");
                //Options.UseSqlServer(builder.Configuration["ConnectionStrings : DefaultConnection"]);
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
        });
            //builder.Services.AddScoped<GenericRepository<Member> , GenericRepository<Member>>();
            //builder.Services.AddScoped(typeof(GenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddScoped<IPlanRepository, PlanRepository>();
            builder.Services.AddScoped<ISessionRepository, SessionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(X=>X.AddProfile(new MappingProfile()));
            builder.Services.AddScoped<IAnalyticsService, AnalyticsService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<ITrainerService, TrainerService>();
            builder.Services.AddScoped<IPlanService, PlanService>();
            builder.Services.AddScoped<ISessionService, SessionService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(Config =>
            {
                Config.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<GymDbContext>();

            builder.Services.ConfigureApplicationCookie(Config =>
            {
                Config.LoginPath = "/Account/Login";
                Config.AccessDeniedPath = "/Account/AccessDenied";
            });
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<GymDbContext>();

            var app = builder.Build();

            #region Seed Data - Migrate Database
           using var scope = app.Services.CreateScope();
           var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var PendingMigrations = dbContext.Database.GetPendingMigrations();
            if (PendingMigrations?.Any() ?? false)
                dbContext.Database.Migrate();
            GymDbContextDataSeeding.SeedData(dbContext);
            IdentityDbContextSeeding.SeedDate(roleManager,userManager);
            #endregion

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id:int?}")
                .WithStaticAssets();

               app.Run();
        }
    }
}
