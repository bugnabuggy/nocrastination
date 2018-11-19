using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nocrastination.Core.DTO;
using Nocrastination.Models;

namespace Nocrastination.Interfaces
{
    public interface IAccountService
    {
        Task<OperationResult> Register(RegisterUserDTO user);
        Task<OperationResult> Register(RegisterChildDTO child, string id);
    }
}
