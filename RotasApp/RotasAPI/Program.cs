using RotasService.Interfaces;
using RotasService.UseCases;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddSingleton<IRotaService, RotasMelhorCusto>();
services.AddControllers();

var app = builder.Build();

var env = app.Environment;
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();    
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:4200")    
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();