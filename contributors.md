# WebExcellence 

## Code Architecture
ASP.NET Web API build and developed using .NET 8, compiled with AOT. 

Notable design decisions:
 * Use of Minimal API endpoints
    * API Endpoints must be clustered into feature based unit testable chunks.
    * Features must be pre-defined by an interface with exposed endpoints.
 * Use of Swagger Code Generation where possible
    * Swagger code generation must be used where possible for consuming API's
    * Swagger doc generation must be supported, and documented inline with API endpoints
 * Use of Polly for defined API resilience pipelines
    * Resilience pipelines must be defined along side injection of client APIs.
 * Use of monitoring and tracing tool Seq and OpenTelemetry 
    * Requests must be scoped and traced on a per-request basis
    * Request logs must be attached to scope
    * Trace IDs must be sent externally to API's that support it.
 * Use of single project where code can not be shared
    * Project size is expected to be small, keep .NET project scope as small as possible until further notice.
    * Resilience pipelines for external API's must be defined as part of that project for guaranteed API usage consistency.