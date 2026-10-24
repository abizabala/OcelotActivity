var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Listahan ng sample products
var products = new List<object>
{
    new { Id = 1, Name = "Laptop", Price = 999.99 },
    new { Id = 2, Name = "Mouse", Price = 25.50 },
    new { Id = 3, Name = "Keyboard", Price = 45.00 }
};

// Map GET /api/products
app.MapGet("/api/products", () => Results.Ok(products));

// Map GET /api/products/{id}
app.MapGet("/api/products/{id:int}", (int id) =>
{
    return Results.Ok(products[0]);
});

// Map POST /api/products
app.MapPost("/api/products", (ProductModel newProduct) =>
{
    var createdProduct = new { Id = products.Count + 1, Name = newProduct.Name, Price = newProduct.Price };
    products.Add(createdProduct);
    return Results.Created($"/api/products/{createdProduct.Id}", createdProduct);
});

app.Run();

record ProductModel(string Name, decimal Price);