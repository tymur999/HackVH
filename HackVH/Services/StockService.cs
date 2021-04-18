using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CsvHelper;
using HackVH.Data;
using HackVH.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HackVH.Services
{
    public class StockService : IStockService
    {
        private readonly HackDbContext _context;
        private readonly IMoneyService _moneyService;

        public StockService(HackDbContext context, IMoneyService moneyService)
        {
            _context = context;
            _moneyService = moneyService;
        }
        
        public async Task<bool> PurchaseStock(string symbol, int shares, IdentityUser user)
        {
            var price = await GetStockPriceAsync(symbol);
            if (price.Equals(-1))
                return false;
            if (await _moneyService.GetBalanceAsync(user) < shares * price)
                return false;
            
            await _context.StockOrders.AddAsync(new StockOrder
            {
                DateOfPurchase = DateTime.UtcNow,
                Shares = shares,
                StockCode = symbol,
                StockPriceOnPurchase = (decimal) price
            });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SellStockAsync(string symbol, DateTime buyDate, IdentityUser user)
        {
            var order = await _context.StockOrders.FirstOrDefaultAsync(s =>
                s.UserId == user.Id && s.StockCode == symbol && s.DateOfPurchase == buyDate);
            if (order == null || order.Shares == 0)
                return false;
            
            
            var newPrice = await GetStockPriceAsync(symbol);
            
            //Can't sell or buy when market is closed
            if (newPrice == 0)
                return false;
            
            var money = order.Shares * newPrice;
            
            await _moneyService.AddBalanceAsync(user, money);

            _context.StockOrders.Remove(order);
            await _context.SaveChangesAsync();
            return true;
        }

        public IEnumerable<StockOrder> GetOrdersForUser(IdentityUser user)
        {
            return _context.StockOrders.Where(s => s.UserId == user.Id).AsEnumerable();
        }

        private static async Task<double> GetStockPriceAsync(string symbol)
        {
            if (!StockIsOpen(DateTime.UtcNow))
                return 0;
            var date1 = ConvertToUnixTimestamp(DateTime.UtcNow.AddDays(-7));
            var date2 = ConvertToUnixTimestamp(DateTime.UtcNow);

            using var client = new HttpClient();
            var response = client.GetAsync(@$"https://query1.finance.yahoo.com/v7/finance/download/{symbol}
                ?period1={date1}&period2={date2}")
                .Result;
            if (!response.IsSuccessStatusCode)
                return -1;

            using var reader = new StreamReader(await response.Content.ReadAsStreamAsync());
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var latestStock = csv.GetRecord<StockCsvData>();
            return latestStock.Close;
        }

        private static bool StockIsOpen(DateTime time)
        {
            time = time.ToUniversalTime();
            //If it is a weekend, can't trade
            if (time.Day is (int)DayOfWeek.Saturday or (int)DayOfWeek.Sunday)
                return false;
            
            //If trading time is before 9:30 AM, do not trade
            if (time.Hour < 9 || (time.Hour >= 9 && time.Minute < 30))
                return false;
            
            //If trading time is after 4 PM, do not trade
            if (time.Hour > 4)
                return false;
            
            return true;
        }
        
        private static double ConvertToUnixTimestamp(DateTime date)
        {
            var origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var diff = date.ToUniversalTime() - origin;
            return Math.Floor(diff.TotalSeconds);
        }
    }

    internal class StockCsvData
    {
        [CsvHelper.Configuration.Attributes.Index(0)]
        public DateTime Date { get; set; }
        [CsvHelper.Configuration.Attributes.Index(1)]
        public double Open { get; set; }
        [CsvHelper.Configuration.Attributes.Index(2)]
        public double High { get; set; }
        [CsvHelper.Configuration.Attributes.Index(3)]
        public double Low { get; set; }
        [CsvHelper.Configuration.Attributes.Index(4)]
        public double Close { get; set; }
        [CsvHelper.Configuration.Attributes.Index(5)]
        public double AdjClose { get; set; }
        [CsvHelper.Configuration.Attributes.Index(6)]
        public long Volume { get; set; }
    }
}