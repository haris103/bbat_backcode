using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.Graph;
using Microsoft.Graph.Models.ExternalConnectors;
using Azure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using WEBAPIVersion5.DAL;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container

// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddMicrosoftIdentityWebApi(builder.Configuration);

// CRUD
//builder.Services.AddDbContext<UserDBContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("UserData")));

builder.Services.AddDbContext<BusinessUnitContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("BusinessData")));

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});

// Get users GRAPH CLIENT
builder.Services.AddScoped(serviceProvider =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    string tenantId = "06b0bc3b-8b4e-467f-a6d5-0d2db90af7dd";
    string clientId = "3f0d5f74-c3b4-4c2c-a3dd-73d471be3a9c";
    string clientSecretId = "TGW8Q~O4AXOkfB.eRIjamsbA1i3y_ekseZEpOaLG";
    var clientSecretCredential = new ClientSecretCredential(tenantId, clientId, clientSecretId);
    return new GraphServiceClient(clientSecretCredential);
});


builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
{
    policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:3000");
}));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
