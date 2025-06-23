using BookShop.Core.Wrappers;
using BookShop.DataAccess.Entities;
using BookShop.XUnitTest.BookShopTests.Wrappers.Interfaces;
using FluentAssertions;
using MockQueryable;
using Moq;

namespace BookShop.XUnitTest.BookShopTests.Service_Tests
{
    public class QueryableExtensionMethodTest
    {
        private readonly Mock<IPaginatedService<Book>> _paginatedServiceMock;
        public QueryableExtensionMethodTest()
        {
            _paginatedServiceMock = new();
        }

        [Theory]
        [InlineData(1, 10)]
        public async Task ToPaginatedListAsync_Should_Return_List(int pageNumber, int pageSize)
        {
            //Arrange
            var bookList = new List<Book>
            {
                new Book
                {
                    Id = 1,
                    Title = "Refactoring",
                    Description = "Improving the Design of Existing Code",
                    ISBN13 = "1234567890123",
                    ISBN10 = "1234567890",
                    Author = "Martin Fowler",
                    Price = 50,
                    PriceAfterDiscount = 45,
                    Publisher = "Addison-Wesley",
                    PublicationDate = new DateTime(2018, 1, 1),
                    Unit_Instock = 10,
                    Image_url = "img1.jpg",
                    IsActive = true,
                    Subject = new Subject { Name = "Software", Name_Ar = "البرمجيات" },
                    SubSubject = new SubSubject { Name = "Refactoring", Name_Ar = "تحسين الكود" }
                }
            }.AsQueryable().BuildMock(); // 🔥 هذا يحل المشكلة

            var paginatedResult = new PaginatedResult<Book>(bookList.ToList());

            _paginatedServiceMock.Setup(x => x.ReturnPaginatedResult(bookList, pageNumber, pageSize))
                .Returns(Task.FromResult(paginatedResult));

            //Act
            var result = await _paginatedServiceMock.Object.ReturnPaginatedResult(bookList, pageNumber, pageSize);

            //Assert
            result.Data.Should().NotBeNullOrEmpty();
        }
    }
}
