using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Helpers;
using api.Models;

namespace api.Interfaces
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllAsync(QueryObject query);
        // we are using the question  mark in stock beacuse the GEtById can be FirstOrDefault can be null
        Task<Stock?> GetByIdAsync(int id);

        Task <Stock?> GetBySymbolAsync(string Symbol);
        Task<Stock> CreateAsync(Stock stockModel);
        Task<Stock?> UpdateAsync(int id, UpdateStockRequest stockDto);
        Task<Stock?> DeleteAsync(int id);

        Task <bool> StockExists(int id);
    }
}