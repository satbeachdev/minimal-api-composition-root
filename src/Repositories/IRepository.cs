namespace Repositories
{
    public interface IRepository<T>
    {
        IList<T> GetAll();
        T? GetById(Guid id);
        T? CreateNew(T newCustomer);
        T? Update(T updatedCustomer);
        T? DeleteById(Guid id);
    }
}