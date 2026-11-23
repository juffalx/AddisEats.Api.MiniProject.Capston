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
