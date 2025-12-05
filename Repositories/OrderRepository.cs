using WebApplication1.Models;
namespace WebApplication1.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(WarehouseDbContext context) : base(context) { }
    }
}