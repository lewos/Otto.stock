using Microsoft.EntityFrameworkCore;
using Otto.stock;
using Otto.stock.DTO;
using Otto.stock.Mapper;
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

app.MapGet("/stock", async (StockDb db) =>
{
    var aux = await db.Stocks.ToListAsync();
    return GetListStockDTO(aux);
});

app.MapGet("/stock/{id}", async (StockDb db, int id) => 
{
    var item = await db.Stocks.FindAsync(id);
    if(item is not null)
        return StockMapper.GetStockDTO(item);
    return null;
});


app.MapPost("/stock", async (StockDb db, StockDTO dto) =>
{
    var stock = StockMapper.GetStock(dto);
    await db.Stocks.AddAsync(stock);
    await db.SaveChangesAsync();
    return Results.Created($"/stock/{stock.Id}", stock);
});

app.MapPut("/stock/{id}", async (StockDb db, StockDTO dto, int id) =>
{
    var updateStock = StockMapper.GetStock(dto);
    var stock = await db.Stocks.FindAsync(id);
    if (stock is null) return Results.NotFound();
    UpdateFields(updateStock, stock);
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

static void UpdateFields(Stock request, Stock? stock)
{
    stock.Name = request.Name;
    stock.Description = request.Description;
    stock.Quantity = request.Quantity;
    stock.Origin = request.Origin;
    stock.SellerId = request.SellerId;
    stock.SellerIdMail = request.SellerIdMail;
    stock.MSellerId = request.MSellerId;
    stock.TSellerId = request.TSellerId;
    stock.MItemId = request.MItemId;
    stock.TItemId = request.TItemId;
    stock.SKU = request.SKU;
    stock.Code = request.Code;
    stock.Category = request.Category;
    stock.State = request.State;
    stock.StateDescription = request.StateDescription;
}

static List<StockDTO> GetListStockDTO(List<Stock> aux)
{
    var res = new List<StockDTO>();
    foreach (var item in aux)
        res.Add(StockMapper.GetStockDTO(item));
    return res;
}