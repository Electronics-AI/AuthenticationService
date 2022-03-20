# C# .NET 5 Authenitcation Service
This repository presents the C# .NET 5 Authentication Service implementation. Authentication Service provides the following features:
* Refresh and Access JWTs support
* Three implementations of the main database: 
  1. Entity Framework Core 5.0 compatible with any SQL database provider 
  2. Dapper for the PostgreSQL database
  3. Mongo.Driver for the MongoDB database
* Token blacklist implementation using Redis for an immediate logout opportunity
* Serilog logging with the Seq server support
