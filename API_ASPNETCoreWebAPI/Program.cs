using API_ASPNETCoreWebAPI.Data;
//using API_ASPNETCoreWebAPI.Importing;
using API_ASPNETCoreWebAPI.Interfaces;
using API_ASPNETCoreWebAPI.Repository;
using Microsoft.EntityFrameworkCore;
using API_ASPNETCoreWebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("LCompDb")));

//// used during initial importation of pre-existing JSON files
//builder.Services.AddScoped<PassageImporter>();

// "register our interface" PassageRepository
builder.Services.AddScoped<IRepository, Repository>();
// Register grading service
builder.Services.AddScoped<IGradingService, GradingService>();

var app = builder.Build();

//// for importing --------------------------------------------------
//Console.WriteLine("Arguments: " + string.Join(" | ", args));

//// Usage: dotnet run -- --import "C:\QuizContent" ru
//if (args.Length > 0 && args[0] == "--import")
//{
//    if (args.Length != 3)
//    {
//        throw new ArgumentException(
//            "Usage: --import <folder> <language>");
//    }

//    await using var scope = app.Services.CreateAsyncScope();

//    var importer = scope.ServiceProvider
//        .GetRequiredService<PassageImporter>();

//    var result = await importer.ImportFolderAsync(args[1], args[2]);

//    Console.WriteLine(
//        $"Imported {result.Imported} passage(s); " +
//        $"skipped {result.Skipped} existing passage(s).");

//    return;
//}
//// end of importation --------------------------------------------

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//}

app.UseHttpsRedirection();

//app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

app.Run();

//// Expose a Program type for WebApplicationFactory in integration tests
//public partial class Program { }
