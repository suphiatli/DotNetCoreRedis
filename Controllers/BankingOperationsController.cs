
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DotNetCoreRedis.Models;
using DotNetCoreRedis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace DotNetCoreRedis
{
    [ApiController]
    [Route("api/[controller]")]
    public class BankingOperationsController: ControllerBase 
    {
        private readonly IBankingOperationService bankingOperationsService;
        private readonly IDistributedCache redisDistributedCache;
        public BankingOperationsController(IBankingOperationService bankingOperationsService,IDistributedCache redisDistributedCache )
        {
            this.bankingOperationsService = bankingOperationsService;
            this.redisDistributedCache = redisDistributedCache;
        }

        [HttpGet("GetUserAccountAsync")]
        public async Task<List<AccountItem>> GetUserAccountAsync(string branchId)
        {
            List<AccountItem> branchUserAccounts;
            string cacheJsonItem;
            var userAccountsFromCache = await redisDistributedCache.GetAsync(branchId);
            if (userAccountsFromCache != null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(userAccountsFromCache);
                branchUserAccounts = JsonSerializer.Deserialize<List<AccountItem>>(cacheJsonItem);
            }
            else
            {
                branchUserAccounts = await Task.Run(() => bankingOperationsService.GetBranchUserBankAccounts(branchId));
                cacheJsonItem = JsonSerializer.Serialize(branchUserAccounts);
                userAccountsFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                var options = new DistributedCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(1))
                        .SetAbsoluteExpiration(DateTime.Now.AddMonths(1));
                await redisDistributedCache.SetAsync(branchId, userAccountsFromCache, options);

            }
            return branchUserAccounts;
        }
    }
}