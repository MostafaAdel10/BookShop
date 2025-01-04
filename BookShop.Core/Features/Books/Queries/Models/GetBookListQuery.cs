using BookShop.DataAccess.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Core.Features.Books.Queries.Models
{
    public class GetBookListQuery : IRequest<List<Book>> 
    {

    }
}
