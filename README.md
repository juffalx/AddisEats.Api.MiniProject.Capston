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

| Method | Path | Purpose | Success Code | Failure Codes |
|---|---|---|---|---|
| GET | `/api/v1/restaurants` | Returns restaurants, with optional area filtering and pagination through query parameters such as `area`, `page`, and `limit`. | 200 OK | 400 Bad Request, 404 Not Found, 405 Method Not Allowed, 500 Internal Server Error |
| GET | `/api/v1/restaurants/{id}` | Returns one restaurant using its ID and maps the result to a RestaurantDTO. | 200 OK | 400 Bad Request, 404 Not Found, 405 Method Not Allowed, 500 Internal Server Error |
| GET | `/api/v1/restaurants/{id:int}/menu` | Returns the restaurant name and the menu value supplied through the `menu` query parameter. | 200 OK | 404 Not Found, 405 Method Not Allowed |
| GET | `/api/v1/orders/{id:int}` | Returns an order item using its ID. | 200 OK | 404 Not Found, 405 Method Not Allowed |
| POST | `/api/v1/orders` | Creates a new menu item from the submitted OrderCreateDTO and returns the created result. | 201 Created | 400 Bad Request, 405 Method Not Allowed, 500 Internal Server Error |

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
