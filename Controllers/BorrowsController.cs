using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace LibraryManagementAPI.Controllers
{
    public class BorrowRequst
    {
        public int BookId { get; set; }
        public int ReaderId { get; set; }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BorrowsController : ControllerBase
    {
        private readonly libraryContext _context;

        public BorrowsController(libraryContext context)
        {
            _context = context;
        }

        // GET: api/Borrows
        [HttpGet]
        public async Task<ActionResult<Result<Borrow>>> GetBorrows([FromQuery] string search = "", [FromQuery] int pageSize=10, [FromQuery] int pageNumber=1)
        {
            IQueryable<Borrow> query = _context.Borrows;

            if (!string.IsNullOrEmpty(search))
            {
                search = search.ToLower();
                query = query.Where(borrow => borrow.Reader.Name.ToLower().Contains(search) || borrow.Book.Title.ToLower().Contains(search));
            }

            var totalRecords = await query.CountAsync();

            var skipCount = (pageNumber - 1) * pageSize;

            var borrows = await query.Skip(skipCount).Take(pageSize).ToListAsync();

            return new Result<Borrow>
            {
                Data = borrows,
                TotalCount = totalRecords,
            };
        }

        // GET: api/Borrows/5
        [HttpGet("{rid}")]
        public async Task<ActionResult<Result<Borrow>>> GetBorrowsByReaderID(int rid, [FromQuery] int pageSize = 10, [FromQuery] int pageNumber = 1, [FromQuery] Boolean isHistory = false)
        {
            IQueryable<Borrow> query = _context.Borrows;

            query = isHistory ? query.Where(borrow => borrow.ReaderID == rid && borrow.ReturnDate != null) : query.Where(borrow => borrow.ReaderID == rid && borrow.ReturnDate == null); 
            
            var totalRecords = await query.CountAsync();

            var skipCount = (pageNumber - 1) * pageSize;

            var borrows = await query.Skip(skipCount).Take(pageSize).ToListAsync();

            return new Result<Borrow>
            {
                Data = borrows,
                TotalCount = totalRecords,
            };
        }

        // PUT: api/Borrows/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBorrow(int id, Borrow borrow)
        {
            if (id != borrow.BorrowID)
            {
                return BadRequest();
            }

            _context.Entry(borrow).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BorrowExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Borrows
        [HttpPost]
        public async Task<ActionResult<Borrow>> PostBorrow([FromBody] BorrowRequst borrowRequest)
        {
            var query = _context.Borrows;

            var result = query.Where(borrow => borrow.ReaderID == borrowRequest.ReaderId && borrow.IsOverdue == true);

            var isOverdue = result.Count() > 0;

            if (isOverdue)
            {
                return Problem("当前账户存在逾期，请归还逾期图书后尝试");
            }

            var book = await _context.Books.FindAsync(borrowRequest.BookId);
            if (book == null || book.AvailabilityStatus == "借出")
            {
                return Problem("该图书不存在或已借出，请重新操作");
            }

            var borrow = new Borrow
            {
                BookID = book.BookID,
                ReaderID = borrowRequest.ReaderId,
                IsOverdue = false,
                BorrowDate = DateOnly.FromDateTime(DateTime.Now),
                ReturnDate = null,
                DueDate = DateOnly.FromDateTime(DateTime.Now.AddMonths(1)),
            };
            _context.Borrows.Add(borrow);
            await _context.SaveChangesAsync();

            book.AvailabilityStatus = "借出";
            await _context.SaveChangesAsync();

            return Ok("借阅成功");
        }

        // DELETE: api/Borrows/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBorrow([FromBody] BorrowRequst borrowRequest, int id)
        {
            var query = _context.Borrows;

            var result = query.Where(borrow => borrow.ReaderID == borrowRequest.ReaderId && borrow.IsOverdue == true);

            var isOverdue = result.Count() > 0;

            if (isOverdue)
            {
                return Problem("当前账户存在逾期，请归还逾期图书后尝试");
            }
            var book = await _context.Books.FindAsync(borrowRequest.BookId);
            if (book == null || book.AvailabilityStatus == "在馆")
            {
                return Problem("该图书不存在或未借出，请重新操作");
            }
            var borrow = await _context.Borrows.FindAsync(id);
            if (borrow == null)
            {
                return Problem("借阅信息不存在，请重新操作");
            }
            borrow.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            await _context.SaveChangesAsync();

            book.AvailabilityStatus = "在馆";
            await _context.SaveChangesAsync();

            return Ok("归还成功");
        }

        private bool BorrowExists(int id)
        {
            return _context.Borrows.Any(e => e.BorrowID == id);
        }
    }
}
