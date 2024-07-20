using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOcelot();

var app = builder.Build();

app.UseHttpsRedirection();


app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});
app.UseOcelot().Wait();

app.Run();

