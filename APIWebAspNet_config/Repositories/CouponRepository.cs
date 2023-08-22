using APIWebAspNet_config.Data;
using APIWebAspNet_config.Models;
using Microsoft.EntityFrameworkCore;

namespace APIWebAspNet_config.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext _db;
        public CouponRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task CreateAsync(Coupon coupon)
        {
            _db.Add(coupon);
        }

        public async Task<ICollection<Coupon>> GetAllAsync()
        {
            return await _db.Coupons.ToListAsync();
        }

        public async Task<Coupon> GetAsync(int id)
        {
            return await _db.Coupons.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Coupon> GetAsync(string couponName)
        {
            return await _db.Coupons.FirstOrDefaultAsync(x=> x.Name.ToLower() == couponName.ToLower());
        }

        public async Task RemoveAsync(Coupon coupon)
        {
           _db.Coupons.Remove(coupon);
        }
      

        public async Task SaveAsync()
        {
           await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Coupon coupon)
        {
             _db.Coupons.Update(coupon);
        }
    }
}
