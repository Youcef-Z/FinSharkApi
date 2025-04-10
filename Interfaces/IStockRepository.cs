using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;
using api.Dtos.Stock;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocks();
        Task<Stock?> GetStockById(int id);
        Task<Stock> CreateStock(Stock stock);
        Task<Stock?> UpdateStock(int id, UpdateStockRequestDto updateStockDto);
        Task<Stock?> DeleteStock(int id);

        Task<bool> StockExists(int id);
    }
}