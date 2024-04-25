using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RWA_Web_API_.Data;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

        };
    });

builder.Services.AddDbContext<VideoContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));          
builder.Services.AddDbContext<TagContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));          
builder.Services.AddDbContext<NotificationContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));          
builder.Services.AddDbContext<UserContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionStringName")));          

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthentication();
app.UseHttpsRedirection();
app.UseFileServer(new FileServerOptions
{
    FileProvider=new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "Static")),
    RequestPath="/Static",
    EnableDefaultFiles=true
    
});
app.UseAuthorization();

app.MapControllers();

app.Run();
