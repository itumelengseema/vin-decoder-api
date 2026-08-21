# VIN Decoder API

A REST API built with ASP.NET Core that validates and decodes a 17-character Vehicle Identification Number (VIN).

## Problem and Solution

A VIN is a standard 17-character vehicle identifier, but the information stored in it is not easy to understand by simply reading the characters. Manually checking whether a VIN is correctly formatted and extracting details such as its manufacturer, country of origin and model year is repetitive and can lead to mistakes.

The VIN Decoder API provides one reusable endpoint that handles this process. A client application sends a VIN to the API, which validates the input, checks its check digit and returns the decoded information as structured JSON. This makes the result easier for another application or user to understand and use.

Possible clients include vehicle marketplace applications, dealership systems, workshop software and insurance applications that need to perform basic VIN validation or identification.

The current version is intended for basic VIN decoding. It does not replace a complete commercial vehicle database or provide information such as ownership and service history.

## Features

- Validates that a VIN contains exactly 17 characters
- Checks for characters that are not allowed in a VIN
- Validates the VIN check digit
- Extracts the World Manufacturer Identifier (WMI)
- Extracts the Vehicle Descriptor Section (VDS)
- Extracts the Vehicle Identifier Section (VIS)
- Identifies the country of origin
- Identifies the manufacturer
- Decodes the vehicle model year
- Provides error responses for invalid input

## Technologies Used

- C#
- .NET 8
- ASP.NET Core Web API
- xUnit
- Swagger / OpenAPI

## Project Structure

```text
VinDecoder/
├── VinDecoder.Api/
│   ├── Controllers/
│   ├── Models/
│   ├── Services/
│   └── Program.cs
└── VinDecoder.Tests/
```

The decoding logic is divided into separate services. Each service is responsible for one part of the VIN, such as the check digit, country, manufacturer, or model year.

## Getting Started

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Visual Studio, Visual Studio Code, or another C# editor

### Run the API

Clone the repository and move into the project folder:

```bash
git clone <your-repository-url>
cd VinDecoder
```

Restore the dependencies:

```bash
dotnet restore
```

Run the API:

```bash
dotnet run --project VinDecoder.Api
```

The terminal will display the local address for the API. Open the Swagger URL in your browser to test the endpoint.

## API Endpoint

### Decode a VIN

```http
GET /api/vin/{vin}
```

Example request:

```http
GET /api/vin/1HGCM82633A004352
```

Example response:

```json
{
  "vin": "1HGCM82633A004352",
  "wmi": "1HG",
  "vds": "CM8263",
  "vis": "3A004352",
  "country": "United States",
  "manufacturer": "Honda",
  "modelYear": 2003,
  "isCheckDigitValid": true
}
```

## Running the Tests

Run all the tests from the solution directory:

```bash
dotnet test
```

The test project checks the individual VIN services and the main decoding logic.

## Current Limitations

- The manufacturer and country data only covers the codes currently included in the project.
- The API provides basic VIN information and does not return a vehicle's full specification or service history.
- No external vehicle database is connected.

## Future Improvements

- Add more manufacturer and country codes
- Add integration tests for the API endpoint
- Return consistent error-response objects
- Add more Swagger examples and documentation
- Rewrite the API in Java and Spring Boot for comparison

## Author

Itumeleng Seema
