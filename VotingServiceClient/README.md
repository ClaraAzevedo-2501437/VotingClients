# Voting Service Client

A simple console application that connects to the VotingService using gRPC.

## Description

This client provides an interactive menu to:
- View available candidates
- Submit votes using voting credentials
- Check election results

## Technology

- .NET 8.0
- gRPC

## Service Details

- **Endpoint**: `https://ken01.utad.pt:9091`
- **Protocol**: gRPC
- **Service**: VotingService
- **Operations**:
  - `GetCandidates()`: Returns list of candidates
  - `Vote(credential, candidateId)`: Submits a vote
  - `GetResults()`: Returns election results

## How to Execute

1. Navigate to the project directory:
```bash
cd VotingServiceClient
```

2. Build the project:
```bash
dotnet build
```

3. Run the application:
```bash
dotnet run
```

4. Use the interactive menu to test different operations:
   - **Option 1**: Get list of candidates
   - **Option 2**: Submit a vote (requires credential from registration)
   - **Option 3**: View election results
   - **Option 4**: Exit

## Example Usage

```
=== Menu ===
  1 - Get Candidates
  2 - Submit Vote
  3 - Get Results
  4 - Exit

Select option: 1

Getting candidates...
Received 3 candidates:
  ID: 1, Name: Alice
  ID: 2, Name: Bob
  ID: 3, Name: Charlie
```

