using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Dtos.Stock.Comment;
using api.Models;

namespace api.Mappers
{
    public static class StockMapper
    {
        //The this keyword before the Stock type indicates that this method is an extension method for the Stock class.
//Stock stockModel is the parameter of the method. It represents an instance of the Stock class that will be converted to a StockDto.
//An extension method allows you to "add" new methods to existing types without modifying the original type or creating a new derived type. Extension methods are static methods, but they are called as if they were instance methods on the extended type.
//In this case, ToStockDto is an extension method for the Stock class. This means you can call ToStockDto on any instance of Stock as if it were a regular instance method of the Stock class.
        public static StockDto ToStockDto(this Stock stockModel){
            return new StockDto {
                Id= stockModel.Id,
                Symbol=stockModel.Symbol,
                CompanyName=stockModel.CompanyName,
                Purchase=stockModel.Purchase,
                LastDiv=stockModel.LastDiv,
                Indistry=stockModel.Indistry,
                MarketCap=stockModel.MarketCap,
                Comments = stockModel.Comments.Select(c=>c.ToCommentDto()).ToList()
            };
        }
        public static Stock ToStockFromCreateDto(this CreateStockRequestDto stockDto){
            return new Stock{
               
                Symbol=stockDto.Symbol,
                CompanyName=stockDto.CompanyName,
                Purchase=stockDto.Purchase,
                LastDiv=stockDto.LastDiv,
                Indistry=stockDto.Indistry,
                MarketCap=stockDto.MarketCap
            } ;
        }
         public static Stock ToStockFromFMP(this FMPStock fmpStock)
        {
            return new Stock
            {
                Symbol = fmpStock.symbol,
                CompanyName = fmpStock.companyName,
                Purchase = (decimal)fmpStock.price,
                LastDiv = (decimal)fmpStock.lastDiv,
                Indistry = fmpStock.industry,
                MarketCap = fmpStock.mktCap
            };
        }
    }
}