using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HackVH.Data;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Services.Interfaces
{
    public interface IStockService
    {
        public Task<bool> PurchaseStock(string symbol, int shares, IdentityUser user);
        public Task<bool> SellStockAsync(string symbol, DateTime buyDate, IdentityUser user);
        public IEnumerable<StockOrder> GetOrdersForUser(IdentityUser user);
    }
}