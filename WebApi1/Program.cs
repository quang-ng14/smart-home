using Microsoft.EntityFrameworkCore;
using WebApi1;
using WebApi1.Controllers;

var builder = WebApplication.CreateBuilder(args);
using (var db = new MyContext())
{
    db.Database.EnsureCreated();
    db.Database.Migrate();

}
//Add default account to database automatically
using (var _context = new MyContext())
{
    LoginInfo info = new LoginInfo();
    info.Username = "user";
    info.Password = "123456";
    bool check = true;
    try
    {
        check = _context.LoginInfos.Any(i => i.Username == info.Username);
    }
    catch (Exception ex)
    {
        string s = "CREATE TABLE LoginInfos("
                 + "Id       INTEGER PRIMARY KEY UNIQUE,"
                 + "Username STRING,"
                 + "Password STRING"
                 + ");";
        _context.Database.ExecuteSqlRaw(s);
        _context.LoginInfos.Add(info);
        _context.SaveChanges();
    }
    finally
    {
        if (!check)
        {
            _context.LoginInfos.Add(info);
            _context.SaveChanges();
        }
    }
}
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<MyContext>(ServiceLifetime.Singleton);


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
