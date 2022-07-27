using Microsoft.EntityFrameworkCore;
using Otto.stock;
using Otto.stock.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsql<StockDb>(ConfigHelper.GetConnectionString());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.MapGet("/stock", async (StockDb db) => await db.Stocks.ToListAsync());

app.MapGet("/stock/{id}", async (StockDb db, int id) => await db.Stocks.FindAsync(id));


app.MapPost("/stock", async (StockDb db, Stock stock) =>
{
    await db.Stocks.AddAsync(stock);
    await db.SaveChangesAsync();
    return Results.Created($"/pizza/{stock.Id}", stock);
});

app.MapPut("/stock/{id}", async (StockDb db, Stock updateStock, int id) =>
{
    var stock = await db.Stocks.FindAsync(id);
    if (stock is null) return Results.NotFound();
    stock.Name = updateStock.Name;
    stock.Description = updateStock.Description;
    stock.Quantity = updateStock.Quantity;
    stock.Origin = updateStock.Origin;
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/stock/{id}", async (StockDb db, int id) =>
{
    var stock = await db.Stocks.FindAsync(id);
    if (stock is null)
    {
        return Results.NotFound();
    }
    db.Stocks.Remove(stock);
    await db.SaveChangesAsync();
    return Results.Ok();
});


app.Run();
