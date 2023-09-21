using System.Net.Http.Json;
using System.Text.Json;
using System.Xml;

namespace ProyectoBlazor;

public class ProductService : IProductService{
    //Para recibir injeccion de dependencias a HttpClient que sa se encuentra configurado de manera global
    private readonly HttpClient client;
    //Se utiliza para hacer el mapeo de los datos
    private readonly JsonSerializerOptions options;
    //Esta dependecia se debe pasar al constructor
    public ProductService(HttpClient httpClient){
        client = httpClient;
        //Incializar el serializer. El PropertyNameCaseInsensitive nos garantiza que se pueda mapear cada una de las propiedades que vienen dentro del Json
        options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true};
    }
    //Metodo GET - Obtener los datos
    public async Task<List<Product>?> Get(){
        //Va a recibir lo que devuelva el HttpClient
        var response = await client.GetAsync("v1/products");
        //Se va a leer como string el resultado 
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode){
            //Si llega a haber un error se muestra esta excepecion
            throw new ApplicationException(content);
        }
        //Para mapear o deserializar el producto
        return JsonSerializer.Deserialize<List<Product>>(content, options);
    }

    public async Task Add(Product product){
        var response = await client.PostAsync("v1/products", JsonContent.Create(product));
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode){
            throw new ApplicationException(content);
        }
    }

    public async Task Delete(int productId){
        var response = await client.DeleteAsync($"v1/products/{productId}");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode){
            throw new ApplicationException(content);
        }
    }
}
//La interfaz sirve para injectarse a los componentes que se van a utilizar, luego de esa configuración los servicios quedarán hablitados para cualquier componente en Blazor
public interface IProductService{
    Task<List<Product>?> Get();
    Task Add(Product product);
    Task Delete(int productId);
}