using CoreModule.DbContextConfig;
using CoreModule.Src;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VillaApi;
using VillaApi.DBMapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddResponseCaching();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
    options.Password.RequireDigit = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.SignIn.RequireConfirmedEmail = false;

});
builder.Services.AddControllers(options =>
{
    options.CacheProfiles.Add("Default30", new CacheProfile { Duration = 30 });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                },
                Scheme= "oauth2",
                In = ParameterLocation.Header,
                Name ="Bearer"

            },
            new string[]{}
        }
    });
    opt.SwaggerDoc("v1", new OpenApiInfo {
    Version ="v1.0",
    Title = "Villa Api Version 1",
    Description = "Api with rest pattern",
    TermsOfService = new Uri("https://examples.com"),
    Contact = new OpenApiContact { 
    Name="Roshan Gurung",
    Url=new Uri("https://examples.com")
    },
    License =  new OpenApiLicense
    {
        Name = "Example License",
        Url= new Uri("https://license.com")
    }

    });

    opt.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2.0",
        Title = "Villa Api Version 2",
        Description = "Api with rest pattern",
        TermsOfService = new Uri("https://examples.com"),
        Contact = new OpenApiContact
        {
            Name = "Roshan Gurung",
            Url = new Uri("https://examples.com")
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://license.com")
        }

    });
});

builder.Services.UseVillaDIConfig();
var key = builder.Configuration.GetValue<string>("JwtConfig:Key");
builder.Services.AddAuthentication(a =>
{
    a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = false;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options=>{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Villa_Api_V1");
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Villa_Api_V2");
    });
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
DBSeed.Seed(app).Wait();
app.Run();
