using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProyectoBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
//Esta linea nos permite obtener un valor del archivo de configuracion appsettings
var apiUrl = builder.Configuration.GetValue<string>("apiUrl");
//Http Client es la clase que se encarga de hacer conexiones con APIs y servicios externos
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiUrl) });
//Aca se hace la injeccion de dependencias para todos los componetes. Se lee asi: Para esa interfaz, el objeto que
//se va a crear internamente cuando se injecta la dependencia seria el servicio como tal
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

await builder.Build().RunAsync();