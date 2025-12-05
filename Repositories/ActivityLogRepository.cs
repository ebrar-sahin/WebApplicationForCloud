using WebApplication1.Models;
namespace WebApplication1.Repositories
{
    public class ActivityLogRepository : Repository<ActivityLog>, IActivityLogRepository
    {
        public ActivityLogRepository(WarehouseDbContext context) : base(context) { }
    }
}