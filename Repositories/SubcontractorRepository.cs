using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class SubcontractorRepository : Repository<Subcontractor>, ISubcontractorRepository
    {
        public SubcontractorRepository(WarehouseDbContext context) : base(context)
        {
        }
    }
}