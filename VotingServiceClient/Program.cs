using Grpc.Net.Client;
using VotingSystem.Voting;

namespace VotingServiceClient;

class Program
{
    private const string ServiceUrl = "https://ken01.utad.pt:9091";

    static async Task Main(string[] args)
    {
        Console.WriteLine("=== Voting Service Client (gRPC) ===");
        Console.WriteLine($"Service URL: {ServiceUrl}");
        Console.WriteLine($"Protocol: gRPC\n");

        // Create gRPC channel with TLS (ignore certificate validation)
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };

        var channelOptions = new GrpcChannelOptions
        {
            HttpHandler = httpHandler,
            MaxReceiveMessageSize = null,
            MaxSendMessageSize = null
        };

        var channel = GrpcChannel.ForAddress(ServiceUrl, channelOptions);
        var client = new VotingService.VotingServiceClient(channel);

        try
        {
            await InteractiveMode(client);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
        finally
        {
            channel.Dispose();
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }

    static async Task GetCandidates(VotingService.VotingServiceClient client)
    {
        try
        {
            Console.WriteLine("\nGetting candidates...");
            
            var request = new GetCandidatesRequest();
            var response = await client.GetCandidatesAsync(request);

            Console.WriteLine($"Received {response.Candidates.Count} candidates:");
            foreach (var candidate in response.Candidates)
            {
                Console.WriteLine($"  ID: {candidate.Id}, Name: {candidate.Name}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    static async Task SubmitVote(VotingService.VotingServiceClient client, string credential, int candidateId)
    {
        try
        {
            Console.WriteLine($"\nSubmitting vote...");
            Console.WriteLine($"  Credential: {credential}");
            Console.WriteLine($"  Candidate ID: {candidateId}");
            
            var request = new VoteRequest
            {
                VotingCredential = credential,
                CandidateId = candidateId
            };

            var response = await client.VoteAsync(request);

            Console.WriteLine($"\nResponse:");
            Console.WriteLine($"  Success: {response.Success}");
            Console.WriteLine($"  Message: {response.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    static async Task GetResults(VotingService.VotingServiceClient client)
    {
        try
        {
            Console.WriteLine("\nGetting election results...");
            
            var request = new GetResultsRequest();
            var response = await client.GetResultsAsync(request);

            Console.WriteLine($"\nElection Results:");
            Console.WriteLine($"{"Rank",-6} {"ID",-6} {"Name",-30} {"Votes",-10}");
            Console.WriteLine(new string('-', 55));
            
            int rank = 1;
            foreach (var result in response.Results.OrderByDescending(r => r.Votes))
            {
                Console.WriteLine($"{rank,-6} {result.Id,-6} {result.Name,-30} {result.Votes,-10}");
                rank++;
            }

            var totalVotes = response.Results.Sum(r => r.Votes);
            Console.WriteLine(new string('-', 55));
            Console.WriteLine($"{"Total:",-42} {totalVotes,-10}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR: {ex.Message}");
        }
    }

    static async Task InteractiveMode(VotingService.VotingServiceClient client)
    {
        while (true)
        {
            Console.WriteLine("\n=== Menu ===");
            Console.WriteLine("  1 - Get Candidates");
            Console.WriteLine("  2 - Submit Vote");
            Console.WriteLine("  3 - Get Results");
            Console.WriteLine("  4 - Exit");
            Console.Write("\nSelect option: ");
            
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    await GetCandidates(client);
                    break;
                
                case "2":
                    Console.Write("\nEnter voting credential: ");
                    var credential = Console.ReadLine() ?? "";
                    Console.Write("Enter candidate ID: ");
                    if (int.TryParse(Console.ReadLine(), out int candidateId))
                    {
                        await SubmitVote(client, credential, candidateId);
                    }
                    else
                    {
                        Console.WriteLine("Invalid candidate ID");
                    }
                    break;
                
                case "3":
                    await GetResults(client);
                    break;
                
                case "4":
                    return;
                
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
    }
}

