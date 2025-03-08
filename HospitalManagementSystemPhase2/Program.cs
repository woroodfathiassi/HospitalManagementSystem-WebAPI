using HospitalManagementSystemPhase2;
using HospitalManagementSystemPhase2.Managements;
using HospitalManagementSystemPhase2.Managers;
using HospitalManagementSystemPhase2.Services;
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
builder.Services.AddScoped<PatientDBAccess>();

builder.Services.AddScoped<DoctorManagement>();
builder.Services.AddScoped<DoctorDBAccess>();

builder.Services.AddScoped<AppointmentManagement>();
builder.Services.AddScoped<AppointmentDBAccess>();

builder.Services.AddScoped<MedicationManagement>();
builder.Services.AddScoped<MedicationDBAccess>();

builder.Services.AddScoped<BillingManagement>();

builder.Services.AddScoped<PrescriptionManagement>();

builder.Services.AddScoped<AccountManagement>();
builder.Services.AddScoped<AccountDBAccess>();

builder.Services.AddScoped<AdminManagement>();
builder.Services.AddScoped<AdminDBAccess>();


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
