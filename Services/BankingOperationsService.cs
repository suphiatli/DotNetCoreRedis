using System.Collections.Generic;
using DotNetCoreRedis.Models;

namespace DotNetCoreRedis.Services
{
    public class BankingOperationsService : IBankingOperationService
    {
        public List<AccountItem> GetBranchUserBankAccounts(string branchId)
        {
            Dictionary<string,List<AccountItem>> dictionary=new Dictionary<string,List<AccountItem>>();
            
            List<AccountItem> UserAccounts1 = new List<AccountItem>() {
                new AccountItem(){ UserId =1, AccountBalance=6700},
                new AccountItem(){ UserId =2, AccountBalance=5300},
            };
            List<AccountItem> UserAccounts2 = new List<AccountItem>() {
                new AccountItem(){ UserId =3, AccountBalance=600},
                new AccountItem(){ UserId =4, AccountBalance=5200},
            };
            List<AccountItem> UserAccounts3 = new List<AccountItem>() {
                new AccountItem(){ UserId =5, AccountBalance=6100},
                new AccountItem(){ UserId =6, AccountBalance=5100},
            };
            dictionary.Add("1",UserAccounts1);
            dictionary.Add("2",UserAccounts2);
            dictionary.Add("3",UserAccounts3);

            dictionary.TryGetValue(branchId, out List<AccountItem> UserAccounts);

            return UserAccounts;
        }
    }
}