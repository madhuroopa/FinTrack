using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IportfolioRepository
    {
        Task <List<Stock>> GetUserPortfolio(AppUser user);
        Task <Portfolio> CreateAsync(Portfolio portfolio);

         Task<Portfolio> DeletePortfolio(AppUser appUser, string symbol);
    }
}