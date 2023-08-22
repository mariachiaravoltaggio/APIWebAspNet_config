
using APIWebAspNet_config;
using APIWebAspNet_config.Data;
using APIWebAspNet_config.Models;
using APIWebAspNet_config.Models.DTOs;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

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
//lista Coupon
app.MapGet("/api/coupon", (ApplicationDbContext _db, ILogger<Program> _logger) =>
{

    APIResponse response = new();
    _logger.Log(LogLevel.Information, "Getting all Coupons");
    response.Result = _db.Coupons;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
   
}
).WithName("GetCoupons").Produces<APIResponse>(200);

//Coupon in base all'id
app.MapGet("/api/coupon/{id:int}", (ApplicationDbContext _db, ILogger < Program > _logger, int id) =>
{
    APIResponse response = new();

    if ( _db.Coupons.FirstOrDefault(u => u.Id == id) == null|| id == 0)
    {
        response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };
        response.ErrorMessages.Add("Id cannot be zero or not exist.");
        _logger.Log(LogLevel.Error, "Non riesco a prendere il Coupon: id=0 o non esiste");
        return Results.BadRequest(response);
    }
   
    response.Result = _db.Coupons.FirstOrDefault(c => c.Id == id);
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.OK;
    return Results.Ok(response);
}
).WithName("GetCoupon").Produces<APIResponse>(200).Produces(404);

//Crea Coupon
app.MapPost("/api/coupon", (IMapper _mapper, ApplicationDbContext _db, ILogger < Program > _logger,[FromBody] CouponCreateDTO couponCreateDTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

    if(_db.Coupons.FirstOrDefault(c => c.Name.ToLower() == couponCreateDTO.Name.ToLower()) != null)
    {
        response.ErrorMessages.Add("Coupon Name already exists");
        return Results.BadRequest(response);
    }
    Coupon coupon = _mapper.Map<Coupon>(couponCreateDTO);
   
    coupon.Created = DateTime.Now;
    _db.Add(coupon);
    _db.SaveChanges();

    CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);
   
    response.Result = couponDTO;
    response.IsSuccess = true;
    response.StatusCode = HttpStatusCode.Created;
    return Results.Ok(response);

}
).WithName("CreateCoupon").Accepts<CouponCreateDTO>("application/json").Produces<APIResponse>(201).Produces(400);

//Update Coupon
app.MapPut("/api/coupon/{id:int}", (IMapper _mapper, ApplicationDbContext _db, ILogger<Program> _logger,int id, [FromBody] CouponUpdateDTO couponUpdateDTO) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };


    // Trova il coupon da modificare per ID
    var existingCoupon = _db.Coupons.FirstOrDefault(c => c.Id == id);

    if (existingCoupon != null)
    {
        // Applica le modifiche dal DTO all'oggetto esistente
        _mapper.Map(couponUpdateDTO, existingCoupon);

        // Salva le modifiche nel database
        _db.SaveChanges();

        // Prepara la risposta di successo
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.OK;
        return Results.Ok(response);
    }
    else
    {
        // Coupon non trovato, restituisci uno status 404 (Not Found)
        response.StatusCode = HttpStatusCode.NotFound;
        response.ErrorMessages.Add("Coupon not Found");
        return Results.NotFound(response);
    }

}
).WithName("UpdateCoupon").Produces<APIResponse>(200).Produces(404);

//Elimina Coupon
app.MapDelete("/api/coupon/{id:int}", (ApplicationDbContext _db, ILogger<Program> _logger, int id) =>
{
    APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };
   
    if (_db.Coupons.FirstOrDefault(u => u.Id == id) == null || id == 0)
    {
        response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };
        response.ErrorMessages.Add("Id cannot be zero or not exist.");
        _logger.Log(LogLevel.Error, "Non posso eliminare!");
        return Results.NotFound(response);
    }

    Coupon coupon = _db.Coupons.FirstOrDefault(c => c.Id == id);

    if (coupon != null)
    {
       _db.Coupons.Remove(coupon);
       _db.SaveChanges();
        response.IsSuccess = true;
        response.StatusCode = HttpStatusCode.NoContent;
        return Results.Ok(response);
    }
    else
    {
        response.ErrorMessages.Add("Invalid Id");
        return Results.BadRequest(response);
    }
}
);

//app.ConfigureEndPointBase(); Controller in cui sono definite tutte le API (vedi punto 7)
app.UseHttpsRedirection();
app.Run();

