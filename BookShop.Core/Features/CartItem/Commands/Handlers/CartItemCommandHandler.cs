using BookShop.Core.Bases;
using BookShop.Core.Features.CartItem.Commands.Models;
using BookShop.Core.Resources;
using BookShop.DataAccess.Entities;
using BookShop.Infrastructure.Data;
using BookShop.Service.Abstract;
using BookShop.Service.AuthServices.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace BookShop.Core.Features.CartItem.Commands.Handlers
{
    public class CartItemCommandHandler : ResponseHandler,
        IRequestHandler<AddCartItemCommand, Response<string>>,
        IRequestHandler<EditTheCartItemQuantityAndCheckIfItIsInStockCommand, Response<string>>,
        IRequestHandler<DeleteCartItemCommand, Response<string>>
    {
        #region Fields
        private readonly IBookService _bookService;
        private readonly ICartItemService _cartItemService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICurrentUserService _currentUserService;
        private readonly ApplicationDbContext _applicationDBContext;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public CartItemCommandHandler(ICartItemService cartItemService,
            IBookService bookService,
            IShoppingCartService shoppingCartService,
            ICurrentUserService currentUserService,
            ApplicationDbContext applicationDBContext,
            IStringLocalizer<SharedResources> stringLocalizer) : base(stringLocalizer)
        {
            _cartItemService = cartItemService;
            _bookService = bookService;
            _shoppingCartService = shoppingCartService;
            _currentUserService = currentUserService;
            _applicationDBContext = applicationDBContext;
            _localizer = stringLocalizer;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            // 1 Get the current user ID
            var currentUserId = _currentUserService.GetUserId();

            // 2 Retrieve the user's shopping cart
            var shoppingCart = await _shoppingCartService.GetByUserId(currentUserId);

            // Check if the book in stock
            var IsTheBookInStock = await _bookService.IsTheBookInStock(request.BookId);
            if (!IsTheBookInStock)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.TheBookIsNotAvailable]);

            // 3 Check if the book already exists in the shopping cart
            var isBookExistInCartItem = await _cartItemService.IsCartItemExistByBookIdAndShoppingCartId(request.BookId, shoppingCart.Id);
            if (isBookExistInCartItem)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.ThisBookAlreadyExistInTheShoppingCart]);

            // 24 Verify that the book exists
            var book = await _bookService.GetByIdAsync(request.BookId);
            if (book == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.BookIsNotExist]);

            // 5 Start a transaction to ensure data consistency
            using var transaction = await _applicationDBContext.Database.BeginTransactionAsync();

            try
            {
                // If the user does not have a shopping cart, create one
                if (shoppingCart == null)
                {
                    shoppingCart = new ShoppingCart
                    {
                        ApplicationUserId = currentUserId,
                        CreatedAt = DateTime.Now
                    };
                    await _shoppingCartService.AddAsync(shoppingCart);

                    // 🔹 Re-fetch ShoppingCart from database to ensure correct ID
                    shoppingCart = await _shoppingCartService.GetByUserId(currentUserId);
                }

                // Add the item to the cart
                var cartItem = new DataAccess.Entities.CartItem
                {
                    BookId = request.BookId,
                    Quantity = request.Quantity,
                    ShoppingCartId = shoppingCart.Id
                };

                await _cartItemService.AddAsync(cartItem);

                // Commit the transaction after all operations succeed
                await transaction.CommitAsync();

                return Created<string>("");
            }
            catch (Exception ex)
            {
                // Rollback the transaction in case of an error
                await transaction.RollbackAsync();
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
            }
        }

        public async Task<Response<string>> Handle(EditTheCartItemQuantityAndCheckIfItIsInStockCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            // 1 Ensure the item exists
            var cartItem = await _cartItemService.GetCartItemByIdAsync(request.Id);
            if (cartItem == null)
                return NotFound<string>(_localizer[SharedResourcesKeys.CartItemNotFound]);

            // 2 Retrieve the user's shopping cart
            var shoppingCart = await _shoppingCartService.GetByUserId(currentUserId);

            if (cartItem.ShoppingCartId != shoppingCart.Id)
                return Unauthorized<string>(_localizer[SharedResourcesKeys.UnAuthorized]);

            // 3 Check if the book in stock
            var IsTheBookInStock = await _bookService.IsTheBookInStock(cartItem.BookId);
            if (!IsTheBookInStock)
                return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.TheBookIsNotAvailable]);

            // 4 Start a transaction to ensure data consistency
            using var transaction = await _applicationDBContext.Database.BeginTransactionAsync();
            try
            {
                // 5 Ensure that the required quantity is available after the return.
                var isQuantityAvailable = await _bookService.IsQuantityGraterThanExist(cartItem.BookId, request.Quantity);
                if (!isQuantityAvailable)
                {
                    await transaction.RollbackAsync();
                    return UnprocessableEntity<string>(_localizer[SharedResourcesKeys.QuantityIsGreater]);
                }

                // Modify the new quantity
                cartItem.Quantity = request.Quantity;
                await _cartItemService.EditAsync(cartItem);

                // // Commit the transaction after all operations succeed
                await transaction.CommitAsync();

                return Success<string>(_localizer[SharedResourcesKeys.Updated]);
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest<string>(_localizer[SharedResourcesKeys.BadRequest]);
            }

        }

        public async Task<Response<string>> Handle(DeleteCartItemCommand request, CancellationToken cancellationToken)
        {
            var currentUserId = _currentUserService.GetUserId();

            // 1 Search for the item in the cart
            var cartItem = await _cartItemService.GetCartItemByIdAsync(request.Id);
            if (cartItem == null) return NotFound<string>(_localizer[SharedResourcesKeys.NotFound]);

            // 2 Retrieve the user's shopping cart
            var shoppingCart = await _shoppingCartService.GetByUserId(currentUserId);

            if (cartItem.ShoppingCartId != shoppingCart.Id)
                return Unauthorized<string>(_localizer[SharedResourcesKeys.UnAuthorized]);

            // 3 Delete the item from the cart
            await _cartItemService.DeleteAsync(cartItem);

            return Deleted<string>();
        }

        #endregion
    }
}