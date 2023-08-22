namespace APIWebAspNet_config.Models.DTOs
{
    public class CouponUpdateDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Percent { get; set; }
        public bool IsActive { get; set; }
    }
}
