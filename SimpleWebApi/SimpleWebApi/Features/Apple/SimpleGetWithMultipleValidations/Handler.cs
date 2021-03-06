﻿using SimpleWebApi.Infrastructure;

namespace SimpleWebApi.Features.Apple.SimpleGetWithMultipleValidations
{
    public class Handler : IRequestHandler<Request, Response>
    {
        public ApiResult<Response> Handle(Request request)
        {
            var response = new Response
            {
                Name = request.Name
            };

            return ApiResult<Response>.Ok(response);
        }
    }
}