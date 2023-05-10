using Business.Concrete;
using DataAccess.Dapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
