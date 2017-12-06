using System;
using System.Threading.Tasks;
using SimpleWebApi.Entities;

namespace SimpleWebApi.Infrastructure.Transaction
{
    public class TransactionDecorator<TRequest, TResponse> : IRequestHandlerDecorator<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly SimpleWebApiContext _context;

        public TransactionDecorator(SimpleWebApiContext context)
        {
            _context = context;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request, RequestHandlerDelegate<TResponse> next)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var response = await next();

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    return response;
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}