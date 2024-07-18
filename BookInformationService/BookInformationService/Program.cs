using BookInformationService;
using BookInformationService.BusinessLayer;
using BookInformationService.DataAccessLayer;
using BookInformationService.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Formatting.Display;
using Serilog.Sinks.Email;
using System.Net;
using System.Reflection;

try
{
    var configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();

    var defaultConnectionString = configuration.GetConnectionString("DefaultConnection");


    AppSettings? appSettings = configuration.GetRequiredSection("AppSettings").Get<AppSettings>();

    var emailInfo = new EmailSinkOptions
    {
        Subject = new MessageTemplateTextFormatter(appSettings.EmailSubject, null),
        Port = appSettings.Port,
        From = appSettings.FromEmail,
        To = new List<string>() { appSettings.ToEmail },
        Host = appSettings.MailServer,
        //EnableSsl = appSettings.EnableSsl,
        Credentials = new NetworkCredential(appSettings.FromEmail, appSettings.EmailPassword)
    };

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(configuration)
        .WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Serilog\\log_.txt"), rollOnFileSizeLimit: true, fileSizeLimitBytes: 1000000, rollingInterval: RollingInterval.Month, retainedFileCountLimit: 24, flushToDiskInterval: TimeSpan.FromSeconds(1))
        //.WriteTo.Email(emailInfo)                           
        .CreateLogger();





    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "Book Information API",
            Description = "This services for the book informations."
        });

        c.EnableAnnotations();


        var mainAssembly = Assembly.GetEntryAssembly();
        //var referencedAssembly = Assembly.LoadFrom(Path.Combine(AppContext.BaseDirectory, "BookInfoReservationModel.dll")); // Adjust the DLL name as needed

        var mainXmlFile = $"{mainAssembly.GetName().Name}.xml";
        //var referencedXmlFile = $"{referencedAssembly.GetName().Name}.xml";

        var mainXmlPath = Path.Combine(AppContext.BaseDirectory, mainXmlFile);
        //var referencedXmlPath = Path.Combine(AppContext.BaseDirectory, referencedXmlFile);

        c.IncludeXmlComments(mainXmlPath);
        //c.IncludeXmlComments(referencedXmlPath);

        /* An error occurs if XML documentation generation is not enabled.
         Ensure that XML documentation generation is enabled for your project.
            1. Right-click on your project in Visual Studio.
            2. Select Properties.
            3. Go to the Build tab.
            4. Check the documentation file checkbox.
            5. Verify that the path specified matches what you expect (bin\Debug\net8.0\BookInformationService.xml)
         */
    });

    // EF
    builder.Services.AddDbContext<SystemDbContext>(options =>
                options.UseSqlite(defaultConnectionString));

    // Models
    builder.Services.AddScoped<IBookInformationDL, BookInformationDL>();
    builder.Services.AddScoped<IBookInformationBL, BookInformationBL>();

    //Auto mapper
    builder.Services.AddAutoMapper(typeof(Program).Assembly);

    Log.Information("BookInfo Service is started.");

    var app = builder.Build();

    // Apply migrations at startup
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SystemDbContext>();
        dbContext.Database.Migrate();
    }

    //app.Urls.Add("http://192.168.56.1:3101"); // Windows IP (ipconfig) http://192.168.56.1:3101/swagger/index.html (Development Environment)
    //app.Urls.Add("http://localhost:3101"); // 127.0.0.1 

    // Configure the HTTP request pipeline.
    // Command for Windows using PowerShell
    // $env:ASPNETCORE_ENVIRONMENT="Development"
    // dotnet BookInformationService.dll
    // Command for Ubuntu using Terminal
    // export ASPNETCORE_ENVIRONMENT=Development
    // dotnet BookInformationService.dll
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    Log.Information("BookInfo Service is stopped.");
}
catch (Exception ex)
{
    Log.Fatal(ex, "BookInfo Service is failed to run correctly.");
}
finally
{
    Log.CloseAndFlush();
}

