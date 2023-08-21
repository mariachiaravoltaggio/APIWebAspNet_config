
using APIWebAspNet_config;
using APIWebAspNet_config.Data;
using APIWebAspNet_config.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//connessione al database
builder.Services.AddDbContext<ApplicationDbContext>(option =>
    option.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection")));
builder.Services.AddAutoMapper(typeof(MappingConfig)); 
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/coupon", (ApplicationDbContext _db) =>
{
    return Results.Ok(_db.Coupons);
}
).WithName("GetCoupons").Produces<IEnumerable<Coupon>>(200);

app.MapGet("/api/coupon/{id:int}", (ApplicationDbContext _db, int id) =>
{
    return Results.Ok(_db.Coupons.FirstOrDefault(c => c.Id == id));
}
).WithName("GetCoupon").Produces<Coupon>(200);


app.MapPost("/api/coupon", ([FromBody] Coupon coupon) =>
{

}
).WithName("CreateCoupon").Accepts<Coupon>("application/json").Produces<APIResponse>(201).Produces(400);

app.MapPut("/api/coupon", () =>
{

}
).WithName("UpdateCoupon").Produces<APIResponse>(200).Produces(400);

app.MapDelete("/api/coupon", () =>
{
  
}
);

//app.ConfigureEndPointBase(); Controller in cui sono definite tutte le API (vedi punto 7)
app.UseHttpsRedirection();
app.Run();

