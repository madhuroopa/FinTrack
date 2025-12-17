using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extensions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/portfolio")]
    [ApiController]
    public class PortfolioController : ControllerBase

    {

        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockrepo;

        private readonly IportfolioRepository _portfolioRepo;
         private readonly IFMPService _fmpService;
        public PortfolioController(UserManager<AppUser> userManager,IStockRepository stockRepo, IportfolioRepository portfolioRepo,IFMPService fmpService){
            _userManager=userManager;
            _stockrepo=stockRepo;
            _portfolioRepo=portfolioRepo;
            _fmpService=fmpService;
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetUserPortfolio(){
            var username =User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var userPortfolio= await  _portfolioRepo.GetUserPortfolio(appUser);
            return Ok(userPortfolio);
        }
        [HttpPost]
        [Authorize]
        public async Task <IActionResult> AddPortfolio(string Symbol){
            var username=User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);
            var stock = await _stockrepo.GetBySymbolAsync(Symbol);
             if (stock == null){
                stock = await  _fmpService.FindStockBySymbolAsync(Symbol);
                if (stock == null){
                    return BadRequest("Stock does not exist");

                }
                else{
                    await _stockrepo.CreateAsync(stock);
                }
            }
            if (stock==null) return BadRequest("Stock not found");
            var userPortfolio= await _portfolioRepo.GetUserPortfolio(appUser);
            if (userPortfolio.Any(e => e.Symbol.ToLower() == Symbol.ToLower())) return BadRequest("Cannot add same stock to portfolio");
            var portfolioModel = new Portfolio{
                StockId = stock.Id,
                AppUserId = appUser.Id
            };
            await _portfolioRepo.CreateAsync(portfolioModel);
            if (portfolioModel==null){
                return StatusCode(500,"Could not create");
            }
            else{
                return  Created();
            }
         

        }
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> DeletePortfolio(string Symbol){
            var username = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(username);

            var userPortfolio = await _portfolioRepo.GetUserPortfolio(appUser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == Symbol.ToLower()).ToList();
            if (filteredStock.Count()==1){
                await _portfolioRepo.DeletePortfolio(appUser,Symbol);
            }
            else{
                return BadRequest("Stock not in your portfolio");
            }
            return Ok();
        }
    }
}