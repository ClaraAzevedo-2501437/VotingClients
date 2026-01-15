# VotingClients

This workspace contains two independent client applications for the Voting System, each connecting to different services using different protocols.

## Client Applications

### 1. VoterRegistrationClient (REST)

Connects to the VoterRegistrationService to register voters and issue voting credentials.

- **Protocol**: REST API (HTTP/JSON)
- **Endpoint**: `https://ken01.utad.pt:9091/api/voter/register`
- **Functionality**: 
  - Submit citizen card numbers
  - Receive voting credentials for eligible voters

📄 See [VoterRegistrationClient/README.md](VoterRegistrationClient/README.md) for detailed instructions.

### 2. VotingServiceClient (gRPC)

Connects to the VotingService to manage the voting process and view results.

- **Protocol**: gRPC
- **Endpoint**: `https://ken01.utad.pt:9091`
- **Functionality**:
  - Get list of candidates
  - Submit votes using credentials
  - View election results

📄 See [VotingServiceClient/README.md](VotingServiceClient/README.md) for detailed instructions.

## Quick Start

Each client can be run independently:

```bash
# Run VoterRegistrationClient
cd VoterRegistrationClient
dotnet build
dotnet run

# Run VotingServiceClient
cd VotingServiceClient
dotnet build
dotnet run
```

## Technology Stack

- .NET 8.0
- REST API (HttpClient)
- gRPC
