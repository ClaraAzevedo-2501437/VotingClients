# Voter Registration Client

A simple console application that connects to the VoterRegistrationService using REST API.

## Description

This client allows you to register voters and obtain voting credentials by submitting citizen card numbers to the registration service. It uses HTTP POST requests with JSON payloads.

## Technology

- .NET 8.0
- REST API (HTTP/JSON)

## Service Details

- **Endpoint**: `https://ken01.utad.pt:9091/api/voter/register`
- **Method**: POST
- **Request Body**: `{"citizen_card_number": "12345678"}`
- **Response**: `{"is_eligible": true, "voting_credential": "abc123"}`

## How to Execute

1. Navigate to the project directory:
```bash
cd VoterRegistrationClient
```

2. Build the project:
```bash
dotnet build
```

3. Run the application:
```bash
dotnet run
```

4. Enter citizen card numbers when prompted, or type `exit` to quit.

## Example Usage

```
Enter citizen card number (or 'exit' to quit): 12345678

Registering voter: 12345678
Request Body: {"citizen_card_number":"12345678"}
HTTP Status: 200 OK
Response Body: {"is_eligible":true,"voting_credential":"abc123"}

Parsed Response:
  Is Eligible: True
  Voting Credential: abc123
```

