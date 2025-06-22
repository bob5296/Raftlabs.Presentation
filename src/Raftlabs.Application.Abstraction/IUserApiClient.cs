using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstractions;

public interface IUserApiClient
{
    Task<HttpResponseMessage> GetUserByIdAsync(int id, CancellationToken cancellationToken);

    Task<HttpResponseMessage> GetAllUsersAsync(int pageNumber, CancellationToken cancellationToken);
}
