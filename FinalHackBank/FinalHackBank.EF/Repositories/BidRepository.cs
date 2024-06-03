using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.EF.Repositories
{
    public class BidRepository : IBidRepository
    {
        private readonly ApplicationDbContext _context;

        public BidRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<
            
            
            Bid> AddAsync(Bid entity)
        {
            _context.Set<Bid>().Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Bid> DeleteAsync(Bid entity)
        {
            _context.Set<Bid>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> Exists(int id)
        {
            return await _context.Set<Bid>().AnyAsync(e => e.BidId == id);
        }

        public async Task<Bid> FindAsync(Expression<Func<Bid, bool>> match)
        {
            return await _context.Set<Bid>().SingleOrDefaultAsync(match);
        }


        public async Task<Bid> GetByIdAsync(int id)
        {
            var bid = await _context.Set<Bid>().FirstOrDefaultAsync(e => e.BidId == id);

            return bid;
        }

        public async Task<Bid> UpdateAsync(Bid entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

 

        public async Task<ICollection<Bid>> GetAllAsync()
        {
            var bids = await _context.Set<Bid>().ToListAsync();
            return (bids);
        }


        public async Task<Bid> FirstOrDefaultAsync(Expression<Func<Bid, bool>> predicate)
        {
            return await _context.Set<Bid>().FirstOrDefaultAsync(predicate);
        }



    }
}
