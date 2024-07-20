using StorageMicroservice.Repository;
using StorageMicroservice.Services;

namespace StorageMicroservice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddScoped<IBlobRepository, BlobRepository>();
            builder.Services.AddScoped<IFileManager, FileManager>();
            builder.Services.AddScoped<IMetadataRepository, MetadataRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
