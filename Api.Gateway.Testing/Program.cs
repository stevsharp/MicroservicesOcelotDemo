using System.Net.Http.Json;
using System.Text.Json;

try
{
    await Task.Delay(2000); // wait for services to start

    var gatewayBaseUrl = "http://localhost:5123"; // your ApiGateway port
    var http = new HttpClient { BaseAddress = new Uri(gatewayBaseUrl) };

    Console.WriteLine("=== Microservices Test Client ===\n");

    // 1️⃣ Request token from AuthService via Gateway
    var loginPayload = new
    {
        username = "demo",
        password = "demo"
    };

    Console.WriteLine("Requesting token...");
    var tokenResponse = await http.PostAsJsonAsync("/auth/auth/token", loginPayload);

    if (!tokenResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Token request failed: {tokenResponse.StatusCode}");
      
        Console.WriteLine("Token response:");

        Console.ReadLine();

        return;
    }

    var tokenJson = await tokenResponse.Content.ReadAsStringAsync();
    using var doc = JsonDocument.Parse(tokenJson);
    var token = doc.RootElement.GetProperty("access_token").GetString();

    Console.WriteLine($"Token received: {token?.Substring(0, 40)}...\n");

    http.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

    Console.WriteLine("Requesting data from /data/data/products ...");
    var dataResponse = await http.GetAsync("/data/data/products");

    if (!dataResponse.IsSuccessStatusCode)
    {
        Console.WriteLine($"Data request failed: {dataResponse.StatusCode}");
        var body = await dataResponse.Content.ReadAsStringAsync();
        Console.WriteLine(body);
        return;
    }

    var data = await dataResponse.Content.ReadFromJsonAsync<object>();
    Console.WriteLine("Data received:");
    Console.WriteLine(JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));

    Console.WriteLine("\nDone.");

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    throw;
}

Console.ReadLine();

