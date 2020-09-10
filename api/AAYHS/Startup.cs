using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAYHS.API.Filter;
using AAYHS.API.Logging;
using AAYHS.Core.AutoMapper;
using AAYHS.Core.Shared.Helper;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBContext;
using AAYHS.Repository.IRepository;
using AAYHS.Repository.Repository;
using AAYHS.Service.IService;
using AAYHS.Service.Service;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AAYHS
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();

            // Set all appsetting configuration in static object 
            AppSettingConfigurations.AppSettings = appSettingsSection.Get<AppSettings>();

            // DBContext
            var connection = Configuration.GetConnectionString("ApplicationContext");
            services.AddDbContext<AAYHSDBContext>(options =>
            options.UseSqlServer(connection), ServiceLifetime.Transient);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                {
                    new OpenApiSecurityScheme
                    {
                    Reference = new OpenApiReference
                        {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                    },
                    new List<string>()
                    }
                });

                // Added custom filter lists an additional "401" response for all actions that are decorated with the AuthorizeAttribute
                c.OperationFilter<AuthResponsesOperationFilter>();
            });


            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapping());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            // Handle CORS
            services.AddCors();

            // Configure jwt authentication
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });




            //Dependency injection in ASP.NET Core (Services)
            services.AddTransient<IAPIErrorLogService, APIErrorLogService>();
            services.AddTransient<IClassSponsorService, ClassSponsorService>();
            services.AddTransient<IExhibitorService, ExhibitorService>();
            services.AddTransient<ISponsorExhibitorService, SponsorExhibitorService>();
            services.AddTransient<ISponsorService, SponsorService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IClassService, ClassService>();
            services.AddTransient<IGlobalCodeService, GlobalCodeService>();
            services.AddTransient<IHorseService, HorseService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IGroupService, GroupService>();
            services.AddTransient<IStallAssignmentService, StallAssignmentService>();
            services.AddTransient<IAdvertisementService, AdvertisementService>();




            //Dependency injection in ASP.NET Core (Repositories)
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IAPIErrorLogRepository, APIErrorLogRepository>();
            services.AddTransient<IApplicationSettingRepository, ApplicationSettingRepository>();
            services.AddTransient<IAddressRepository, AddressRepository>();
            services.AddTransient<IClassSponsorRepository, ClassSponsorRepository>();
            services.AddTransient<IExhibitorRepository, ExhibitorRepository>();
            services.AddTransient<ISponsorExhibitorRepository, SponsorExhibitorRepository>();
            services.AddTransient<ISponsorRepository, SponsorRepository>();
            services.AddTransient<IClassRepository, ClassRepository>();
            services.AddTransient<IScheduleDateRepository, ScheduleDateRepository>();
            services.AddTransient<IExhibitorClassRepository, ExhibitorClassRepository>();
            services.AddTransient<ISplitClassRepository, SplitClassRepository>();
            services.AddTransient<IResultRepository, ResultRepository>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IGlobalCodeRepository, GlobalCodeRepository>();
            services.AddTransient<IHorseRepository, HorseRepository>();
            services.AddTransient<IEmailSenderRepository, EmailSenderRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IEmailSenderRepository, EmailSenderRepository>();
            services.AddTransient<IApplicationSettingRepository, ApplicationSettingRepository>();
            services.AddTransient<IStallAssignmentRepository, StallAssignmentRepository>();
            services.AddTransient<IGroupRepository, GroupRepository>();
            services.AddTransient<IGroupExhibitorRepository, GroupExhibitorRepository>();
            services.AddTransient<IGroupFinancialRepository, GroupFinancialRepository>();
            //services.AddTransient<IStallRepository, StallRepository>();
            services.AddTransient<IAdvertisementRepository, AdvertisementRepository>();
            services.AddTransient<IExhibitorHorseRepository, ExhibitorHorseRepository>();
            services.AddTransient<IZipCodeRepository, ZipCodeRepository>();
            services.AddTransient<IScanRepository, ScanRepository>();
            services.AddTransient<IExhibitorPaymentDetailRepository, ExhibitorPaymentDetailRepository>();

            services.AddControllers();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AAYHS API V1.0");
                c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureCustomApplicationMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
