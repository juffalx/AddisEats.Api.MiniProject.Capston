## Day 56 – Question 5: Handling Ambiguous Routes

### 1. Deliberately Create Ambiguous Routes

I created two identical routes in `RestaurantsController` using the same HTTP method and route template.

When I ran the application and accessed the endpoint, ASP.NET Core threw an `AmbiguousMatchException` because multiple endpoints matched the same request.

### 2. Observed Exception

```text
Microsoft.AspNetCore.Routing.Matching.AmbiguousMatchException:
The request matched multiple endpoints.

Matches:
- RestaurantsController.Test1
- RestaurantsController.Test1
```

### 3. Fix 1: Using Route Constraints

I added route constraints to make the endpoints distinguishable based on the parameter type.

### 4. Fix 2: Using Distinct Literal Paths

I assigned different literal paths to the endpoints so each request matches only one endpoint.

### 5. Conclusion

ASP.NET Core routing requires each incoming request to resolve to a single endpoint.

Ambiguous routes cause runtime exceptions. I learned how to resolve routing conflicts using route constraints and distinct literal paths.

## Day 57 – Exercise 1: Complete API Route Table

This table documents the routes currently implemented in my AddisEats API. It includes each HTTP method, route, purpose, success status code, and possible failure status codes.

| Method | Path                                | Purpose                                                                                                                        | Success Code | Failure Codes                                                                     |
| ------ | ----------------------------------- | ------------------------------------------------------------------------------------------------------------------------------ | ------------ | --------------------------------------------------------------------------------- |
| GET    | `/api/v1/restaurants`               | Returns restaurants, with optional area filtering and pagination through query parameters such as `area`, `page`, and `limit`. | 200 OK       | 400 Bad Request, 404 Not Found, 405 Method Not Allowed, 500 Internal Server Error |
| GET    | `/api/v1/restaurants/{id}`          | Returns one restaurant using its ID and maps the result to a RestaurantDTO.                                                    | 200 OK       | 400 Bad Request, 404 Not Found, 405 Method Not Allowed, 500 Internal Server Error |
| GET    | `/api/v1/restaurants/{id:int}/menu` | Returns the restaurant name and the menu value supplied through the `menu` query parameter.                                    | 200 OK       | 404 Not Found, 405 Method Not Allowed                                             |
| GET    | `/api/v1/orders/{id:int}`           | Returns an order item using its ID.                                                                                            | 200 OK       | 404 Not Found, 405 Method Not Allowed                                             |
| POST   | `/api/v1/orders`                    | Creates a new menu item from the submitted OrderCreateDTO and returns the created result.                                      | 201 Created  | 400 Bad Request, 405 Method Not Allowed, 500 Internal Server Error                |

### Failure Status Code Explanations

- **400 Bad Request:** The request contains invalid data or a value that cannot be converted to the required parameter type. The `[ApiController]` attribute automatically handles invalid model state.
- **404 Not Found:** The requested route does not exist, or the requested resource cannot be found.
- **405 Method Not Allowed:** The route exists, but the HTTP method used by the client is not supported for that route.
- **500 Internal Server Error:** An unexpected server-side exception occurs while processing the request.

### Important Implementation Notes

1. The restaurant search service returns a list. If no restaurants match the search criteria, the API currently returns `200 OK` with an empty list.
2. The restaurant-by-ID action does not currently check whether the restaurant exists before calling `ToDTO()`. An unknown ID can therefore cause a `500 Internal Server Error` instead of the expected `404 Not Found`.
3. The restaurant menu action returns `404 Not Found` when the restaurant ID does not exist.
4. The order lookup action returns `404 Not Found` when the requested ID does not exist.
5. The order creation service throws an `ArgumentException` when the submitted price is zero or negative. Since no exception-handling middleware is configured in the supplied `Program.cs`, this can result in a `500 Internal Server Error`.
6. The order creation action currently uses `CreatedAtAction(nameof(CreateOrder), ...)`. This targets the POST action rather than the GET order lookup action, so its generated Location URL does not correctly identify the GET resource route.

### Conclusion

A route table helps document the public API contract. It makes the supported endpoints, expected responses, and possible errors clear to developers who use or maintain the API. It also helps identify implementation problems that should be corrected in later development.

## Day 57 – Exercise 2: Identifiers and Filters in API Routes

An identifier is used to identify a specific resource. A filter is used to narrow down a collection of resources based on certain conditions.

I reviewed all the routes in my API and classified their path segments.

### 1. Route Segment Classification

| HTTP Method | Route                               | Path Segment  | Classification         |
| ----------- | ----------------------------------- | ------------- | ---------------------- |
| GET         | `/api/v1/restaurants`               | `api`         | API prefix             |
| GET         | `/api/v1/restaurants`               | `v1`          | API version            |
| GET         | `/api/v1/restaurants`               | `restaurants` | Resource collection    |
| GET         | `/api/v1/restaurants/{id}`          | `{id}`        | Identifier             |
| GET         | `/api/v1/restaurants/{id:int}/menu` | `{id:int}`    | Identifier             |
| GET         | `/api/v1/restaurants/{id:int}/menu` | `menu`        | Sub-resource           |
| GET         | `/api/v1/orders/{id:int}`           | `{id:int}`    | Identifier             |
| POST        | `/api/v1/orders`                    | `orders`      | Resource collection    |
| POST        | `/api/v1/orders`                    | `api/v1`      | API prefix and version |

The API prefix, version, resource names, and sub-resource names are structural route segments. They are not identifiers or filters.

### 2. Identifiers

The following route parameters are identifiers:

- `/api/v1/restaurants/{id}` identifies a specific restaurant.
- `/api/v1/restaurants/{id:int}/menu` identifies the restaurant whose menu is requested.
- `/api/v1/orders/{id:int}` identifies a specific order.

The `id` parameter is an identifier because it points to one specific resource.

### 3. Filters

My restaurant search uses query parameters instead of placing filters in the route path.

The `RestaurantQuery` class contains:

- `Area`: filters restaurants by area.
- `MaxFeeEtb`: filters restaurants by maximum fee.
- `Page`: selects the result page.
- `Limit`: controls the number of results returned per page.

These values are received through `[FromQuery] RestaurantQuery r`.

Example:

`GET /api/v1/restaurants?Area=Bole&MaxFeeEtb=100&Page=1&Limit=5`

The menu endpoint also receives `menu` through `[FromQuery]`, not through a dynamic path segment.

Example:

`GET /api/v1/restaurants/1/menu?menu=Breakfast`

### 4. Changes Made

After reviewing my routes, I found that no filter was incorrectly placed in a dynamic path segment.

I therefore did not need to move any existing path parameter.

The current route design separates the responsibilities:

- Identifiers belong in the route path.
- Search conditions and pagination belong in query parameters.
- Resource names and sub-resource names describe the API structure.

### 5. Conclusion

Identifiers help the API locate a specific resource, while filters help clients search and narrow down collections.

Keeping identifiers in route paths and filters in query parameters makes API routes easier to understand, maintain, and use.

## Day 57 – Exercise 3: Remove Verbs from API Paths

I reviewed all the routes in my API to identify action verbs used in URL paths.

### 1. Existing Route Review

| Existing Route                      | HTTP Method | Path Analysis                                                                    |
| ----------------------------------- | ----------- | -------------------------------------------------------------------------------- |
| `/api/v1/Restaurants`               | GET         | `Restaurants` is a resource collection name.                                     |
| `/api/v1/Restaurants/{id}`          | GET         | `Restaurants` is a resource name, and `{id}` identifies one restaurant.          |
| `/api/v1/Restaurants/{id:int}/menu` | GET         | `Restaurants` and `menu` are resource names. `{id:int}` identifies a restaurant. |
| `/api/v1/orders/{id:int}`           | GET         | `orders` is a resource name, and `{id:int}` identifies an order.                 |
| `/api/v1/orders`                    | POST        | `orders` is a resource collection name.                                          |

### 2. Identified Verbs

I did not find any action verbs in my existing route paths.

The action names are represented by HTTP methods instead:

- GET retrieves resources.
- POST creates a resource.

For example, the `BySearch` action method does not introduce a verb into the URL. The actual route remains `/api/v1/Restaurants`, and search conditions are supplied through query parameters.

### 3. Three Naming Approaches

Since my existing API routes do not contain action verbs, there are no incorrect verb-based paths that need to be rewritten.

For comparison, if a restaurant search endpoint were designed using a verb-based path such as `/api/v1/restaurants/search`, it could be represented in three ways:

| Approach                       | Example                                    | Explanation                                                       |
| ------------------------------ | ------------------------------------------ | ----------------------------------------------------------------- |
| Verb in the path               | `GET /api/v1/restaurants/search`           | Places the search action in the URL.                              |
| Resource-based path            | `GET /api/v1/restaurants`                  | Uses the restaurant collection as the resource.                   |
| Resource with query parameters | `GET /api/v1/restaurants?area=Bole&page=1` | Uses the collection route and query parameters to filter results. |

### 4. My Choice

I choose the resource-based route with query parameters:

`GET /api/v1/restaurants?area=Bole&page=1`

This approach keeps the URL focused on the resource, while query parameters describe how the client wants to filter the results.

### 5. Conclusion

API paths should describe resources rather than actions. HTTP methods communicate what operation the client wants to perform.

My existing routes already follow this principle, so I did not need to change any controller routes for this exercise.

## Day 57 – Exercise 4: API Error Catalogue

An error catalogue defines the errors that an API can return. Each error has a unique code, an HTTP status code, and a clear message.

The error code helps the client identify the specific problem, the HTTP status communicates the general result, and the message explains what went wrong.

### 1. Error Catalogue

| Error Code                | HTTP Status               | Message                                                                                      |
| ------------------------- | ------------------------- | -------------------------------------------------------------------------------------------- |
| `VALIDATION_ERROR`        | 400 Bad Request           | One or more submitted fields contain invalid values. Please check the request and try again. |
| `INVALID_QUERY_PARAMETER` | 400 Bad Request           | One or more query parameters contain invalid values. Please provide valid parameters.        |
| `PRICE_MUST_BE_POSITIVE`  | 400 Bad Request           | The price must be greater than zero.                                                         |
| `RESOURCE_NOT_FOUND`      | 404 Not Found             | The requested resource could not be found.                                                   |
| `ROUTE_NOT_FOUND`         | 404 Not Found             | The requested API endpoint does not exist.                                                   |
| `METHOD_NOT_ALLOWED`      | 405 Method Not Allowed    | The HTTP method is not supported for this endpoint.                                          |
| `UNAUTHORIZED`            | 401 Unauthorized          | Authentication is required. Please provide a valid access token.                             |
| `FORBIDDEN`               | 403 Forbidden             | You do not have permission to perform this operation.                                        |
| `RESOURCE_CONFLICT`       | 409 Conflict              | The request conflicts with the current state of the resource.                                |
| `INTERNAL_SERVER_ERROR`   | 500 Internal Server Error | An unexpected server error occurred. Please try again later.                                 |
| `SERVICE_UNAVAILABLE`     | 503 Service Unavailable   | The service is temporarily unavailable. Please try again later.                              |

### 2. Applying the Catalogue to AddisEats

The following examples show how these errors relate to my current API:

**Example 1: Invalid order price**

When a client submits an order with a price of zero or a negative value, the `OrderService.Add()` method throws an `ArgumentException`.

The intended API response is:

- Error code: `PRICE_MUST_BE_POSITIVE`
- HTTP status: `400 Bad Request`
- Message: `The price must be greater than zero.`

**Example 2: Restaurant not found**

When a client requests a restaurant that does not exist through the menu endpoint, the controller returns `NotFound()`.

The corresponding error is:

- Error code: `RESOURCE_NOT_FOUND`
- HTTP status: `404 Not Found`
- Message: `The requested resource could not be found.`

**Example 3: Order not found**

When the requested order ID does not exist, the `OrdersController` returns `NotFound()`.

The corresponding error is:

- Error code: `RESOURCE_NOT_FOUND`
- HTTP status: `404 Not Found`
- Message: `The requested resource could not be found.`

### 3. Error Response Design

A consistent error response should provide the same structure across API endpoints.

Example:

```json
{
  "code": "PRICE_MUST_BE_POSITIVE",
  "status": 400,
  "message": "The price must be greater than zero."
}
```

The client can use the error code to identify the problem programmatically, while the message provides information that can be displayed to the user.

### 4. Important Design Rules

1. Each error should have a clear and stable error code.
2. The HTTP status code should correctly represent the type of failure.
3. Error messages should be understandable and should not expose internal implementation details.
4. The same error should use the same code and response structure across different endpoints.
5. Unexpected server exceptions should return a general error message rather than exposing stack traces or sensitive information.

### 5. Current Implementation Status

This catalogue defines the intended API error contract. It is not yet fully implemented in the current project.

The current project has some existing error handling, including `NotFound()` responses and an `ArgumentException` for invalid prices. However, it does not yet have a centralized error response format or global exception-handling middleware.

These improvements can be implemented in the next development stage.

### Conclusion

A consistent error catalogue makes an API easier to understand, debug, and integrate with frontend applications and other services. It also separates internal implementation details from the information returned to API clients.

````

## Day 57 – Exercise 4: API Error Catalogue

An API error catalogue defines the errors that an API can return. Each error has a unique code, an HTTP status code, and a clear message.

Using a consistent error catalogue makes the API easier to understand, debug, maintain, and integrate with frontend applications.

### Error Catalogue

| Error Code | HTTP Status | Message | When It Is Returned |
|---|---|---|---|
| `VALIDATION_ERROR` | 400 Bad Request | The request contains invalid data. Please check the submitted values. | The request contains missing, invalid, or incorrectly formatted values. |
| `UNAUTHORIZED` | 401 Unauthorized | Authentication is required to access this resource. | The client does not provide a valid authentication token. |
| `FORBIDDEN` | 403 Forbidden | You do not have permission to perform this operation. | The user is authenticated but does not have the required permission. |
| `RESOURCE_NOT_FOUND` | 404 Not Found | The requested resource could not be found. | The requested restaurant, order, or other resource does not exist. |
| `METHOD_NOT_ALLOWED` | 405 Method Not Allowed | The requested HTTP method is not supported for this endpoint. | The client uses an HTTP method that the endpoint does not support. |
| `RESOURCE_CONFLICT` | 409 Conflict | The request conflicts with the current state of the resource. | The requested operation cannot be completed because of the resource's current state. |
| `RATE_LIMIT_EXCEEDED` | 429 Too Many Requests | Too many requests have been sent. Please try again later. | The client sends more requests than the API allows within a specified period. |
| `INTERNAL_SERVER_ERROR` | 500 Internal Server Error | An unexpected server error occurred. Please try again later. | An unexpected error occurs while the server processes a request. |

### Design Rules

1. Every error has a unique, consistent code.
2. Each error code is associated with an appropriate HTTP status.
3. Error messages should be clear and understandable to API clients.
4. Error messages must not expose passwords, access tokens, database details, stack traces, or other sensitive internal information.
5. The same error should use the same code and message format across different endpoints.
6. The API should return the appropriate HTTP status instead of returning `200 OK` for failed requests.

### Example Error Response

When a client requests a restaurant that does not exist, the API can return:

```json
{
  "code": "RESOURCE_NOT_FOUND",
  "message": "The requested resource could not be found."
}
````

The HTTP response status should be `404 Not Found`.

### Conclusion

An error catalogue creates a predictable error-handling contract between the backend and its clients. Frontend applications can use error codes to identify problems and respond appropriately without depending on inconsistent message text.

This catalogue will serve as the foundation for implementing centralized API error handling in Day 58.

## Day 57 – Exercise 5: Choosing Correct HTTP Status Codes

### Scenario 1: A search returns no rows

**Status Code: 200 OK**

Justification:

The search request was valid and successfully processed. An empty result does not mean the request failed. The API should return `200 OK` with an empty array because no matching resources were found.

Example response:

```json
{
  "data": []
}
```

### Scenario 2: Cancelling an order that has already been delivered

**Status Code: 409 Conflict**

Justification:

The request may be valid, but cancelling an order that has already been delivered conflicts with the current state of that order. The API should return `409 Conflict` because the requested operation cannot be performed in the resource's current state.

### Scenario 3: A request contains a negative ETB total

**Status Code: 400 Bad Request**

Justification:

A negative order total is invalid input because an order cannot have a negative price. The API should reject the request with `400 Bad Request` and explain that the submitted total must be zero or greater, or strictly positive if the business rules require it.

### Scenario 4: A valid token belonging to a courier attempts to edit a menu

**Status Code: 403 Forbidden**

Justification:

The token is valid, so the courier is authenticated. However, the courier does not have permission to edit menu information. The API should return `403 Forbidden` because the authenticated user is not authorized to perform this operation.

### Conclusion

Choosing the correct HTTP status code helps API clients understand the result of their requests.

- `200 OK` means the request succeeded, even if the result is empty.
- `400 Bad Request` means the submitted request contains invalid data.
- `403 Forbidden` means the authenticated user lacks permission.
- `409 Conflict` means the requested operation conflicts with the current resource state.

Exercise 7 — Choose REST, GraphQL or gRPC

1. Public mobile API — REST

I would use REST because it is simple and easy for mobile applications to communicate with. It also supports standard HTTP methods and status codes.

2. Internal service-to-service communication — gRPC

I would use gRPC because it provides fast communication between services. It also uses a clear contract to define how services communicate.

3. Dashboard needing six resources — GraphQL

I would use GraphQL because the dashboard needs data from different resources. It allows the client to request the exact fields it needs in one request.
Exercise 8 — Unknown query parameters
I would ignore unknown query parameters and process the parameters that the API supports.

This makes the API easier to use and prevents extra parameters from breaking valid requests. I will use this behaviour consistently across the API.

## Day 58 — Mini-Project Exercise 1: Audit Existing API Validation

### 1. Existing API Endpoints

#### OrdersController

| HTTP Method | API Path                  | Input               |
| ----------- | ------------------------- | ------------------- |
| GET         | `/api/v1/orders/{id:int}` | Route parameter: id |
| POST        | `/api/v1/orders`          | OrderCreateDTO      |

#### RestaurantsController

| HTTP Method | API Path                        | Input                                      |
| ----------- | ------------------------------- | ------------------------------------------ |
| GET         | `/api/v1/Restaurants/{id}`      | Route parameter: id                        |
| GET         | `/api/v1/Restaurants`           | RestaurantQuery                            |
| GET         | `/api/v1/Restaurants/{id}/menu` | Route parameter: id, query parameter: menu |

### 2. Existing Validation

- Both controllers use `[ApiController]`.
- Orders GET endpoint uses an integer route constraint.
- Orders GET endpoint checks whether the requested order exists.
- Restaurant search uses `RestaurantQuery` with Area, MaxFeeEtb, Page, and Limit.
- Restaurant GET endpoints check for missing restaurants in some actions.
- No explicit data annotation validation was found in the inspected request types.

### 3. Missing Validation

- Add `[Required]` and appropriate length limits to required string inputs.
- Validate positive quantities and monetary values.
- Validate `RestaurantQuery` pagination values: Page must be at least 1 and Limit must be within an acceptable range.
- Ensure MaxFeeEtb cannot be negative.
- Validate Area entries and other query parameters.
- Add an integer route constraint to the restaurant GET-by-ID endpoint.
- Review the purpose of OrderCreateDTO and ensure client input cannot control trusted menu prices.

### 4. Conclusion

The existing API has basic route and resource-existence checks, but it needs stronger request validation, custom validation attributes, and consistent error handling. These improvements will be implemented in the following exercises.
