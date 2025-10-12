using Discount.Grpc.Data;
using Discount.Grpc.Models;
using Grpc.Core;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService
    (
        DiscountContext discountContext,
        ILogger<DiscountService> logger
    )
    : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await discountContext.Coupons
            .FirstOrDefaultAsync(c => c.ProductName == request.ProductName);
        
        if(coupon is null)
            coupon = new Coupon
            {
                ProductName = "No Discount",
                Amount = 0,
                Description = "No Discount Desc"
            };

        logger.LogInformation($"Discount retrieved for product {request.ProductName}: {coupon.Description}");

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();
        
        if(coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request Object"));
        
        discountContext.Coupons.Add(coupon);
        await discountContext.SaveChangesAsync();

        logger.LogInformation($"Discount created for product {coupon.ProductName}: {coupon.Description}");

        return coupon.Adapt<CouponModel>();

    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>();

        if (coupon is null)
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request Object"));

        discountContext.Coupons.Update(coupon);
        await discountContext.SaveChangesAsync();

        logger.LogInformation($"Discount updated for product {coupon.ProductName}: {coupon.Description}");


        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = discountContext.Coupons
            .FirstOrDefault(c => c.ProductName == request.ProductName);
        
        if (coupon is null)
            throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} not found."));
        
        discountContext.Coupons.Remove(coupon);

        await discountContext.SaveChangesAsync();

        logger.LogInformation($"Discount deleted for product {request.ProductName}");

        return new DeleteDiscountResponse
        {
            Success = true
        };
    }
}