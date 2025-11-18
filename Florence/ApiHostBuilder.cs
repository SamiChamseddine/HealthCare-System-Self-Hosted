using Florence.Data;
using Microsoft.EntityFrameworkCore;

namespace Florence.ApiHost
{
    public static class ApiHostBuilder
    {
        public static WebApplication BuildApi(string[]? args = null)
        {
            args ??= Array.Empty<string>();

            var builder = WebApplication.CreateBuilder(args);

            builder.Environment.EnvironmentName = "Development";

            var connectionString =
    "Host=ep-long-feather-ag32rfl6-pooler.c-2.eu-central-1.aws.neon.tech;" +
    "Username=neondb_owner;" +
    "Password=npg_ryXSwxP5IBo3;" +
    "Database=neondb;" +
    "SSL Mode=Require;" +
    "Trust Server Certificate=true;";

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString, npgsql =>
                {
                    npgsql.EnableRetryOnFailure(
                        maxRetryCount: 10,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorCodesToAdd: null
                    );
                })
            );

            builder.Services.AddControllers()
    .AddApplicationPart(typeof(Florence.Controllers.InvoicesController).Assembly);


            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseRouting();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapControllers();

            app.Urls.Clear();
            app.Urls.Add("http://localhost:5005");

            return app;
        }
    }
}
