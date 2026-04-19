using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MindMapManager.Core.Entities;
using MindMapManager.Core.ServiceContracts;
using MindMapManager.Core.Services;
using MindMapManager.Infrastructure.DatabaseContext;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using Asp.Versioning;
using MindMapManager.Core.Configuration;
using MindMapManager.Core.RepositoryContracts;
using MindMapManager.Infrastructure.Repository;



namespace MindMapManager.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                // Authorization policy (apply authrize global filter)
                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //options.Filters.Add(new AuthorizeFilter(policy));
            });

            // Register services
            builder.Services.Configure<EmailConfiguration>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddTransient<IJwtService, JwtService>();
            builder.Services.AddScoped<ITrackRepository, TrackRepository>();
            builder.Services.AddScoped<ITrackService, TrackService>();
            builder.Services.AddScoped<IRoadmapRepository, RoadmapRepository>();
            builder.Services.AddScoped<IRoadmapService,RoadmapService>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<ITopicService,TopicService>();
            builder.Services.AddScoped<ILevelService, LevelService>();
            builder.Services.AddScoped<ILevelRepository,LevelRepository>();
            builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
            builder.Services.AddScoped<IResourceService, ResourceService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<IProgressRepository, ProgressRepository>();
            builder.Services.AddScoped<IUserTrackRepository, UserTrackRepository>();
            builder.Services.AddScoped<IProgressService, ProgressService>();
            builder.Services.AddScoped<ICompletedTopicRepository, CompletedTopicRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();

            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            // Add Identity Services
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            //authentication cofiguration
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
                };
            });

            // Cors Policy Configuration
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builderPolicy =>
                {
                    builderPolicy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST", "PUT", "DELETE");
                });
            });

            // Versioning
            //builder.Services.AddApiVersioning(config =>
            //{
            //    config.ApiVersionReader = new UrlSegmentApiVersionReader();
            //});

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer(); // read all endpoints
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlPath = Path.Combine(AppContext.BaseDirectory, "api.xml");
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            }); // responsible of documetation for Api's endpoints

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "my api");
                });
            }

            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.ContentType = "text/plain";
                    var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
                    if (error != null)
                    {
                        await context.Response.WriteAsync(error.Error.ToString());
                    }
                });
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
