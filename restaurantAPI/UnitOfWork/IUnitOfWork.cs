namespace restaurantAPI.UnitOfWork
{
    using restaurantAPI.Models;
    using restaurantAPI.Repostiories;

    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        IRepository<Category> Categories { get; }

        IRepository<Order> Orders { get; }
        IOrderRepository OrderQuery { get; }
        IRepository<OrderDetail> OrderDetails { get; }

        Task<int> CompleteAsync();
    }
}
