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
