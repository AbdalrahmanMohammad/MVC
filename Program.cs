using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<MyDbContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyDefault"));
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IClubRepository, ClubRepository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();

var app = builder.Build();
if (args.Length == 1 && args[0].ToLower() == "seeddata")
{ 
Seed.SeedData(app);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
