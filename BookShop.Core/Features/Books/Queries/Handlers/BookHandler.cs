using BookShop.Core.Features.Books.Queries.Models;
using BookShop.DataAccess.Entities;
using BookShop.Service.Abstract;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Features.Books.Queries.Handlers
{
    public class BookHandler : IRequestHandler<GetBookListQuery, List<Book>>
    {
        #region Fields
        private readonly IBookService _bookService;
        #endregion
        

        #region Constructors
        public BookHandler(IBookService bookService)
        {
            _bookService = bookService;
        }
        #endregion


        #region Handel Functions
        public async Task<List<Book>> Handle(GetBookListQuery request, CancellationToken cancellationToken)
        {
            return await _bookService.GetBooksListAsync();
        }
        #endregion


    }
}
