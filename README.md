# SampleDotNet Project

ShortURLService is a URL shortening service that allows users to shorten long URLs into more manageable, shorter ones. This service includes a command-line interface for interacting with the URL shortener.

The project was developed using a clean architecture pattern to reduce tight coupling, enhance testability, and provide the flexibility to change the project's implementation. The main business logic is in the Domain layer, and the GenerateShortUrl function is responsible for creating a unique name for a long URL. The project can be enhanced by using proper storage technology, such as caching distributed storage, to support the required SLO for the project.

### Build
To build the project, navigate to the root directory and run:
```
dotnet build
```
 ### Run
 ```
 dotnet run --project ./Console/Console.csproj
 ```

## Usage

The project will give the following options to manage a short URL

```
<===== Welcome to URL Shortener Service =====>
Add a new long URL               (A):
Delete a new long URL            (D):
Get a long URL                   (R):
Get statistics of the short URL  (S):
Please Enter your choice:         
```

A - Ask for a long URL and an optional custom UTL suffix. The system will generate a short URL adding the short suffix to ```www.example.com ```
It gives an error if the short url is already in use.
D - Prompt for a long URL and delete it from the system. The system provides an appropriate error if it can't find the URL.
R - Prompt for a short URL and provide the corresponding long URL. 
S - Provide statistics of the short URL ues, the number of times the URL is accessed as well as the last time stamp when the URL was requested.

## Collision resolution

The application uses SHA256 to create a hash for the long URL. The algorithm generates a 32-byte (32-character) string with a very low probability of collision. However, for usability, this app only uses 10 characters. If a collision is detected, it tries to resolve it by appending 1, 2, 3, 4, 5 at the end. If more collisions are detected, the system may use more characters (up to 32) or increase the options for appending suffixes

## Contributing

Contributions are welcome! If you would like to contribute to this project, please follow these guidelines:

1. Fork the repository.
2. Create a new branch.
3. Make your changes.
4. Test your changes.
5. Submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
