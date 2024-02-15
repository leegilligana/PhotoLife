public interface IProductHttpRepository
{
    Task<List<Flash>> GetFlash();
}

public class ProductHttpRepository : IProductHttpRepository
{
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _options;

    public ProductHttpRepository(HttpClient client)
    {
        _client = client;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    // Get Request
    public async Task<List<String>> GetFlash()
    {
        // add to end of the base address
        var response = await _client.GetAsync("GetFlash");
        var content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException(content);
        }

        var products = JsonSerializer.Deserialize<List<String>>(content, _options);
        return products;
    }
}

