using JWTTestproj;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var con = builder.Configuration.GetSection("ConnectionStrings")["DefaultConnection"];
builder.Services.AddDbContext<JWTContext>(options =>
{

    options.UseSqlServer(con,
        b => b.MigrationsAssembly(typeof(JWTContext).Assembly.FullName));

});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var _JwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.Configure<JWTSettings>(_JwtSettings);


var authKey = builder.Configuration.GetValue<string>("JWTSettings:SecretKey");
builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{
    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
        ValidateAudience = false,
        ValidateIssuerSigningKey = true
    };
});


builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();

//service inject
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
