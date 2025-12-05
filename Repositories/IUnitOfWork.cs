using System;
using System.Threading.Tasks;

namespace WebApplication1.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ISubcontractorRepository Subcontractors { get; }
        IOrderRepository Orders { get; }
        IActivityLogRepository ActivityLogs { get; } // <-- BU SATIR ŞART

        Task<int> CompleteAsync();
    }
}