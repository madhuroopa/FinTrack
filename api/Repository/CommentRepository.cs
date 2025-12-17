using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
   
    public class CommentRepository:ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context=context;
        }

        public async Task<Comment> CreateAsync(Comment commentModel)
        {
            await _context.Comments.AddAsync(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel= await _context.Comments.FirstOrDefaultAsync(x=>x.Id==id);
            if (commentModel==null){
                return null;

            }
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;

        }

        public async Task<List<Comment>> GetAllAsync(CommentQueryObject queryObject)
        {
            var comments = _context.Comments.Include(a=>a.AppUser).AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryObject.symbol)){
                comments = comments.Where(s=>s.Stock.Symbol==queryObject.symbol);
            }
            if (queryObject.IsDescending== true){
                comments=comments.OrderByDescending(c=>c.CreatedOn);
            }
           return await comments.ToListAsync();

        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
           return await _context.Comments.Include(a=>a.AppUser).FirstOrDefaultAsync(c=>c.Id==id);


        }

        public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
        {
            var existingComments = await _context.Comments.FindAsync(id);
            if (existingComments==null){
                return null;
            }
            existingComments.Title=commentModel.Title;
            existingComments.Content=commentModel.Content;
            await _context.SaveChangesAsync();
            return existingComments;
        }
    }
}