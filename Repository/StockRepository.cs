using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateStock(Stock stock)
        {
            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<Stock?> DeleteStock(int id)
        {
            var stock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null) 
            {
                return null;
            }

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();
            return stock;
        }

        public async Task<List<Stock>> GetAllStocks(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comments).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(query.SortBY))
            {
                if (query.SortBY.Equals( "Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            // Pagination
            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            stocks = stocks.Skip(skipNumber).Take(query.PageSize);

            return await stocks.ToListAsync();
        }

        public async Task<Stock?> GetStockById(int id)
        {
            var stock = await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(x => x.Id == id);
            return stock;
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stocks.AnyAsync(x => x.Id == id);
        }

        public async Task<Stock?> UpdateStock(int id, UpdateStockRequestDto updateStockDto)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingStock == null)
            {
                return null;
            }

            existingStock.Symbol = updateStockDto.Symbol;
            existingStock.CompanyName = updateStockDto.CompanyName;
            existingStock.Purchase = updateStockDto.Purchase;
            existingStock.LastDiv = updateStockDto.LastDiv;
            existingStock.Industry = updateStockDto.Industry;
            existingStock.MarketCap = updateStockDto.MarketCap;

            await _context.SaveChangesAsync();

            return existingStock;
        }
    }
}