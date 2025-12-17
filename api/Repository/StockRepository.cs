using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Dtos.Stock.Comment;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository: IStockRepository
    {
        private readonly  ApplicationDBContext _context;
        // dependency injection is 99% constructor based so StockRepository is the constructor and the parameter passed is bthe DBContext it gets the db context to the application to use it further 
        public StockRepository(ApplicationDBContext context){
            _context=context;

        }
        public async Task<List<Stock>> GetAllAsync(QueryObject query){
            var stocks= _context.Stock.Include(c=>c.Comments).ThenInclude(a=>a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(query.CompanyName)){
                stocks=stocks.Where(s=>s.CompanyName.Contains(query.CompanyName));

            }
            if (!string.IsNullOrWhiteSpace(query.Symbol)){
                stocks=stocks.Where(s=>s.Symbol.Contains(query.Symbol));

            }
            if (!string.IsNullOrWhiteSpace(query.SortBy)){
               if (query.SortBy.Equals("Symbol",StringComparison.OrdinalIgnoreCase)){
                stocks=query.IsDescending ? stocks.OrderByDescending(s=>s.Symbol): stocks.OrderBy(s=>s.Symbol);

               }

            }
            var skipNumber =(query.PageNumber-1)*query.PageSize;

            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }


        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stock.Include(c=>c.Comments).FirstOrDefaultAsync(i=>i.Id==id);
           
            
        }

        public  async Task<Stock> CreateAsync(Stock stockModel)
        {
            await _context.Stock.AddAsync(stockModel);
            await _context.SaveChangesAsync();
            return stockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequest stockDto)
        {
            var stockModel = await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);
            if (stockModel==null){
            return null;
             }
                stockModel.Symbol=stockDto.Symbol;
                stockModel.CompanyName=stockDto.CompanyName;
                stockModel.Purchase=stockDto.Purchase;
                stockModel.LastDiv=stockDto.LastDiv;
                stockModel.Indistry=stockDto.Indistry;
                stockModel.MarketCap=stockDto.MarketCap;

                await _context.SaveChangesAsync();
                
                return stockModel;


        }

        public async Task<Stock?> DeleteAsync(int id)
        {
        var stockModel = await _context.Stock.FirstOrDefaultAsync(x=>x.Id==id);
        if (stockModel==null){
            return null;
        }
        _context.Stock.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
        }

        public Task<bool> StockExists(int id)
        {
            return _context.Stock.AnyAsync(s=>s.Id==id);
        }

        public async Task<Stock?> GetBySymbolAsync(string Symbol)
        {
           return await _context.Stock.FirstOrDefaultAsync(s=>s.Symbol==Symbol);
        }
    }
}