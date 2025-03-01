using HospitalManagementSystem;
using HospitalManagementSystem.Managements;
using HospitalManagementSystemPhase2.Managers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<HMSDBContext>();
builder.Services.AddScoped<PatientManagement>();
builder.Services.AddScoped<DoctorManagement>();
builder.Services.AddScoped<AppointmentManagement>();
builder.Services.AddScoped<MedicationManagement>();
builder.Services.AddScoped<BillingManagement>();
builder.Services.AddScoped<PrescriptionManagement>();
builder.Services.AddScoped<AuthManagement>();


string connString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<HMSDBContext>(opt => opt.UseSqlServer(connString));

var secretKey = builder.Configuration["JWT:SecretKey"];
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = key
        };
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configure the HTTP request pipeline.

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
