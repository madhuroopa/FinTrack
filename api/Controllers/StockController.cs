using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/stock")]
    //Controllers decorated with this attribute are configured with features and 
    //behavior targeted at improving the developer experience for building APIs.
    [ApiController]
    public class StockController:ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly IStockRepository _stockRepo;
        public StockController(ApplicationDBContext context, IStockRepository stockRepo)
        {
            _stockRepo=stockRepo;
            _context=context;
            
        }
        
        [HttpGet]
        [Authorize]
//Execution of Query: In Entity Framework (EF) Core, LINQ queries are not executed until they are enumerated. By default, LINQ queries return IQueryable<T> which represents a query that has not yet been executed against the database.

        public async Task<IActionResult> GetAll([FromQuery] QueryObject query){
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var stocks = await _stockRepo.GetAllAsync(query);
            var stockDto=stocks.Select(s=>s.ToStockDto()).ToList();
            return Ok(stocks);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id){
            var stock =await _stockRepo.GetByIdAsync(id);
            if (stock==null){
                return NotFound();
            } 
            return Ok(stock.ToStockDto());
        }
[HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto ){
        
        var stockModel=  stockDto.ToStockFromCreateDto();
        await _stockRepo.CreateAsync(stockModel);
        return CreatedAtAction(nameof(GetById), new{id = stockModel.Id}, stockModel.ToStockDto() );
    }
[HttpPut]
[Route("{id:int}")]  
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequest updateDto){
        if(!ModelState.IsValid)
                return BadRequest(ModelState);
        var stockModel = await _stockRepo.UpdateAsync(id,updateDto);
        if (stockModel==null){
            return NotFound();
        }
       
        return Ok(stockModel.ToStockDto());
        }
[HttpDelete]
[Route("{id:int}")]  
    public async Task<IActionResult> Delete([FromRoute] int id){
        if(!ModelState.IsValid)
                return BadRequest(ModelState);
        var stockModel = await _stockRepo.DeleteAsync(id);
        if (stockModel==null){
            return NotFound();
        }
        
        return NoContent();
    }

    }



}