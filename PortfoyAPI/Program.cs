using Business.Concrete;
using DataAccess.Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MLDataAccess;
using System.Text;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//DB
builder.Services.AddTransient<IProjeDal, ProjeDal>();
builder.Services.AddTransient<IDepartmentDal, DepartmanDal>();
builder.Services.AddTransient<IGorevDal, GorevDal>();
builder.Services.AddTransient<IKaynakDal, KaynakDal>();
builder.Services.AddTransient<IKPIDal, KPIDal>();
builder.Services.AddTransient<IProjeDetayDal, ProjeDetayDal>();
builder.Services.AddTransient<IProjeKategoriDal, ProjeKategoriDal>();
builder.Services.AddTransient<IProjeKPIDal, ProjeKPIDal>();
builder.Services.AddTransient<IRiskDal, RiskDal>();
builder.Services.AddTransient<IUserDal, UserDal>();


//Manager
builder.Services.AddTransient<IProjeService, ProjeManager>();
builder.Services.AddTransient<IDepartmanService, DepartmanManager>();
builder.Services.AddTransient<IGorevService, GorevManager>();
builder.Services.AddTransient<IKaynakService, KaynakManager>();
builder.Services.AddTransient<IKPIService, KPIManager>();
builder.Services.AddTransient<IProjeDetayService, ProjeDetayManager>();
builder.Services.AddTransient<IProjeKategoriService, ProjeKategoriManager>();
builder.Services.AddTransient<IProjeKPIService, ProjeKPIManager>();
builder.Services.AddTransient<IRiskService, RiskManager>();
builder.Services.AddTransient<IUserService, UserManager>();

builder.Services.AddTransient<ISelectPortfoy, SelectPortfoy>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddCors();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


