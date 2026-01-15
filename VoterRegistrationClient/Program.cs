using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace VoterRegistrationClient;

class Program
{
    private const string ServiceUrl = "https://ken01.utad.pt:9091";
    private const string RegisterEndpoint = "/api/voter/register";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Voter Registration Client (REST) ===");
        Console.WriteLine($"Service URL: {ServiceUrl}{RegisterEndpoint}");
        Console.WriteLine($"Method: POST");
        Console.WriteLine($"Content-Type: application/json\n");

        // Create HttpClient with certificate validation disabled
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        using var httpClient = new HttpClient(httpHandler)
        {
            BaseAddress = new Uri(ServiceUrl)
        };

        try
        {
            while (true)
            {
                Console.Write("Enter citizen card number (or 'exit' to quit): ");
                var input = Console.ReadLine();
                
                if (string.IsNullOrEmpty(input) || input.ToLower() == "exit")
                    break;

                await RegisterVoter(httpClient, input);
                Console.WriteLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    static async Task RegisterVoter(HttpClient httpClient, string citizenCardNumber)
    {
        try
        {
            Console.WriteLine($"\nRegistering voter: {citizenCardNumber}");
            
            var request = new VoterRequest
            {
                CitizenCardNumber = citizenCardNumber
            };

            var jsonContent = JsonSerializer.Serialize(request);
            Console.WriteLine($"Request Body: {jsonContent}");
            
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(RegisterEndpoint, content);
            
            Console.WriteLine($"HTTP Status: {(int)response.StatusCode} {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Body: {responseBody}");
                
                var voterResponse = JsonSerializer.Deserialize<VoterResponse>(responseBody);

                if (voterResponse != null)
                {
                    Console.WriteLine($"\nParsed Response:");
                    Console.WriteLine($"  Is Eligible: {voterResponse.IsEligible}");
                    Console.WriteLine($"  Voting Credential: {voterResponse.VotingCredential}");
                }
            }
            else
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error Response: {errorBody}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }
}

// Request model
public class VoterRequest
{
    [JsonPropertyName("citizen_card_number")]
    public string CitizenCardNumber { get; set; } = string.Empty;
}

// Response model
public class VoterResponse
{
    [JsonPropertyName("is_eligible")]
    public bool IsEligible { get; set; }

    [JsonPropertyName("voting_credential")]
    public string VotingCredential { get; set; } = string.Empty;
}
