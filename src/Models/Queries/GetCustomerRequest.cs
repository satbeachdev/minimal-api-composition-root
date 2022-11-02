namespace Models.Queries
{
    public class GetCustomerRequest
    {
        public Guid CustomerId { get; private set; }

        public GetCustomerRequest(Guid id)
        {
            CustomerId = id;
        }
    }
}
