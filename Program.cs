var builder = WebApplication.CreateBuilder(args);

// Agrega servicios al contenedor
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configuración del pipeline HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // Seguridad HSTS para HTTPS
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Activa archivos estáticos como CSS, JS, imágenes, etc.

app.UseRouting();

app.UseAuthorization();

// Ruta por defecto: DashboardController -> Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
