namespace restaurantAPI.UnitOfWork
{
    using restaurantAPI.Models;
    using restaurantAPI.Repostiories;

    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IRepository<Category> Categories { get; }
        OrderRepository Orders { get; }
        IRepository<OrderDetail> OrderDetails { get; }

        Task<int> CompleteAsync();
    }
}
