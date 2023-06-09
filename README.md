# Genetics Challenge

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)

The Genetics Challenge project is a console application coded in **C# using the .NET framework**. It interacts with a genetic data API to perform various operations, including encoding and decoding DNA strands and gene activation analysis. This README provides an overview of the project, instructions for installation, usage, authentication, and guidelines for contributing.

## Table of Contents

- [Installation](#installation)
- [Usage](#usage)
- [Authentication](#authentication)
- [API Endpoints](#api-endpoints)
- [Contributing](contributing)

## Instalation

To run the Genetics Challenge project on your local machine, follow these steps:

1. Clone the repository to your local machine using the following command:
```bash
git clone https://github.com/your-username/genetics-challenge.git
```
2. Open the project in your preferred development environment (e.g., Visual Studio).
3. Build the project to restore dependencies and compile the code.

## Usage

1. Open the Program.cs file in the project.
2. Locate the Main method, which serves as the entry point for the application.
3. In the Main method, replace the placeholder values in the user object with your desired username, email, and password. These credentials will be used for user creation and login operations.
4. Run the application.
5. The application will make HTTP requests to interact with the genetic data API hosted at https://gene.lacuna.cc/.
6. The application performs the following tasks:
- Creates a new user by sending a POST request to /api/users/create.
- Logs in the user by sending a POST request to /api/users/login.
- Requests information about DNA jobs by sending a GET request to /api/dna/jobs.
- Decodes or encodes DNA strands based on the type of job received from the API.
- Performs gene activation analysis and sends the results to the API.
7. The application will display the obtained results in the console output.

## Authentication

The Genetics Challenge project uses OAuth 2.0 for authentication with the genetic data API. To authenticate your requests, you need to include an Authorization header with the value Bearer <access_token>, where <access_token> is the access token obtained after a successful login.

## API Endpoints

The following endpoints are used in the Genetics Challenge project:

```markdown
POST /api/users/create - Creates a new user.

POST /api/users/login - Logs in the user.

GET /api/dna/jobs - Retrieves information about DNA jobs.

POST /api/dna/jobs/{jobId}/decode - Decodes a DNA strand for a specific job.

POST /api/dna/jobs/{jobId}/encode - Encodes a DNA strand for a specific job.

POST /api/dna/jobs/{jobId}/gene - Sends gene activation analysis results for a specific job.
```

Please ensure that you have a valid access token and include it in the Authorization header for the appropriate endpoints.







