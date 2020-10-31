using System.Collections.Generic;
using DotNetCoreRedis.Models;

namespace DotNetCoreRedis.Services
{
    public interface IBankingOperationService
    {
         List<AccountItem> GetBranchUserBankAccounts(string branchId);
    }
}