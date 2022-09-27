# EShopContainer 
This project is based Microsoft's eShopContainer solution.

This is a study application which will cover following major technologies and concepts.
- Cloud Native Development
- Docker
- Microservice
- Messaging Queue Pattern
- Identity Server
- REST

We will be following industry standard principles and design patterns for developing this project.

This will help us to develop solutions for different system design problems.

C# and .NET technologies are primarly used for developing this application.

###### Cloud Native and Design Standards for API Development

- API Gateway Pattern (Cloud: Azure API Gateway , On-Premise: OcelotAPI Gateway )
- Security (Azure AD, OAuth and OpenID Connect)
- API versioning 
- API Documentation - Swagger Implementation
- etry Policy
- Rate Limiter
- Load Balancer
- Circuit Breaker
- Distributed Caching

###### Low Level Code Design and Approach
- Use EFCore for Database management
- epository/Service Layer Pattern
- Seed Database from Application
- MVC Design approach
- DB Concurrency Handling via EFCore.
- Custom Middleware Implementation
- Read App config data to Typed Class.
- Custom Collection Implementation
- State management 
- Caching
---------------------------------------------------------------------------------------------
###### Swagger Implementation:
- Swagger Help us to generate API meta data and description.
- Microsoft provides support for integrating swagger with .NET API application.
- Swagger uses the OpenAPI specification.
- The specification defines how a service should be discovered and how it's capabilities understood.
- Swagger metadata can be used by other application and it helps them quickly consume and integrate.
  - AutoRest - Helps to generate .NET classes based on swagger meta data description.
  - Microsoft Flow - Low code tool
  - Microsoft PowerApps - low code tool.
  - Azure App Service Logic Apps - low code tool.
  ---------------------------------------------------------------------------------------------
  
###### API Versioning: (Implementation: https://code.qburst.com/kamil.hussain/eshopcontainer/-/issues/2)
- API versioning is one of the characteristics of REST API.
- The versioning mechanism is simple and depends on the server routing the request to the appropriate endpoint.
- Use HATEOAS - HyperText as the Engine of Application State approach for implementing the versioning.
- There are different version of can be implemented.
- There are following approaches we can follow.
    - Query String
    - Request Header
    - Url segment- This is most popular and explicit approach
    - MediaType

