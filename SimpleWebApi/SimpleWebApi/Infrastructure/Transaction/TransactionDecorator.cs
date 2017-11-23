using SimpleWebApi.Entities;
using System;
using System.Threading.Tasks;

namespace SimpleWebApi.Infrastructure.Transaction
{
    public class TransactionDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IRequestHandler<TRequest, TResponse> _handler;
        private readonly SimpleWebApiContext _context;

        public TransactionDecorator(IRequestHandler<TRequest, TResponse> handler, SimpleWebApiContext context)
        {
            _handler = handler;
            _context = context;
        }

        public async Task<ApiResult<TResponse>> Handle(TRequest request)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result =  await _handler.Handle(request);
                    transaction.Commit();
                    return result;
                }
                catch(Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}