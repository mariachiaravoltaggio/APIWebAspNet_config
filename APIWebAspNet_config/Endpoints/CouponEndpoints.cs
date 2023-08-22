using APIWebAspNet_config.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using APIWebAspNet_config.Models.DTOs;
using APIWebAspNet_config.Repositories;
using Microsoft.IdentityModel.Tokens;

namespace APIWebAspNet_config.Endpoints
{
    public static class CouponEndpoints
    {
        public static void ConfigureCouponEndpoints(this WebApplication app)
        {

            app.MapGet("/api/coupon", GetAllCoupon)
                  .WithName("GetCoupons").Produces<APIResponse>(200);

            app.MapGet("/api/coupon/{id:int}", GetCoupon)
                .WithName("GetCoupon").Produces<APIResponse>(200).Produces(404);

            app.MapPost("/api/coupon", CreateCoupon)
                .WithName("CreateCoupon")
                .Accepts<CouponCreateDTO>("application/json")
                .Produces<APIResponse>(201)
                .Produces(400);

            app.MapPut("/api/coupon", UpdateCoupon)
                .WithName("UpdateCoupon")
                .Accepts<CouponUpdateDTO>("application/json")
                .Produces<APIResponse>(200).Produces(404);

            app.MapDelete("/api/coupon/{id:int}", DeleteCoupon);
        }
       
        //lista Coupon
            private async static Task<IResult> GetAllCoupon(ICouponRepository _couponRepo, ILogger<Program> _logger)
            {
                APIResponse response = new();
                _logger.Log(LogLevel.Information, "Getting all Coupons");
                response.Result = await _couponRepo.GetAllAsync();
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                return Results.Ok(response);
            }

        //Coupon in base all'id
        private async static Task<IResult> GetCoupon(ICouponRepository _couponRepo, ILogger<Program> _logger, int id)
        {
            APIResponse response = new();

            if (await _couponRepo.GetAsync(id) == null || id == 0)
            {
                response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };
                response.ErrorMessages.Add("Id cannot be zero or not exist.");
                _logger.Log(LogLevel.Error, "Non riesco a prendere il Coupon: id=0 o non esiste");
                return Results.BadRequest(response);
            }

            response.Result = await _couponRepo.GetAsync(id);
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.OK;
            return Results.Ok(response);
        }

        //Crea Coupon
        //[Authorize]
        private async static Task<IResult> CreateCoupon(ICouponRepository _couponRepo, IMapper _mapper,
                         [FromBody] CouponCreateDTO coupon_C_DTO)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };

            if (_couponRepo.GetAsync(coupon_C_DTO.Name).GetAwaiter().GetResult() != null)
            {
                response.ErrorMessages.Add("Coupon Name already Exists");
                return Results.BadRequest(response);
            }

            Coupon coupon = _mapper.Map<Coupon>(coupon_C_DTO);


            await _couponRepo.CreateAsync(coupon);
            await _couponRepo.SaveAsync();
            CouponDTO couponDTO = _mapper.Map<CouponDTO>(coupon);

            response.Result = couponDTO;
            response.IsSuccess = true;
            response.StatusCode = HttpStatusCode.Created;
            return Results.Ok(response);
         
        }

        //Update Coupon
        //[Authorize]
        private async static Task<IResult> UpdateCoupon(IMapper _mapper, ICouponRepository _couponRepo, ILogger<Program> _logger, int id, [FromBody] CouponUpdateDTO couponUpdateDTO)
            {
                APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.NotFound };

                // Trova il coupon da modificare per ID
                Coupon couponFromDb = await _couponRepo.GetAsync(couponUpdateDTO.Id);

                if (couponFromDb != null)
                {
                    couponFromDb.LastUpdated = DateTime.Now;
                    // Applica le modifiche dal DTO all'oggetto esistente
                    _mapper.Map(couponUpdateDTO, couponFromDb);
                    // Salva le modifiche nel database
                    await _couponRepo.SaveAsync();

                    // Prepara la risposta di successo
                    response.Result = couponFromDb.Id;
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
                }}

        //Elimina Coupon
        //[Authorize]
        private async static Task<IResult> DeleteCoupon(ICouponRepository _couponRepo, int id)
        {
            APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };


            Coupon couponFromStore = await _couponRepo.GetAsync(id);
            if (couponFromStore != null)
            {
                await _couponRepo.RemoveAsync(couponFromStore);
                await _couponRepo.SaveAsync();
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

/* [Authorize]
 private async static Task<IResult> UpdateCoupon(ICouponRepository _couponRepo, IMapper _mapper,
                   [FromBody] CouponUpdateDTO coupon_U_DTO)
  {
      APIResponse response = new() { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest };


      await _couponRepo.UpdateAsync(_mapper.Map<Coupon>(coupon_U_DTO));
      await _couponRepo.SaveAsync();

      response.Result = _mapper.Map<CouponDTO>(await _couponRepo.GetAsync(coupon_U_DTO.Id)); ;
      response.IsSuccess = true;
      response.StatusCode = HttpStatusCode.OK;
      return Results.Ok(response);
  }*/
    

    }
}
