using Microsoft.EntityFrameworkCore;
using NetShopAPI.Common;
using NetShopAPI.Data;
using NetShopAPI.DTOs.Cart.Request;
using NetShopAPI.DTOs.Cart.Response;
using NetShopAPI.DTOs.User.ResponseDTO;
using NetShopAPI.Models;
using NetShopAPI.Models.CartModel;
using NetShopAPI.Services.ClientServices;
using NetShopAPI.Services.CurrentUserServices;

namespace NetShopAPI.Services.CartServices
{
    public class CartService : ICartService
    {

        private readonly ShopDbContext _db;
        private readonly ICurrentUserService _currentUser;
        private readonly IUserAccountService _userAccount;

        public CartService(ShopDbContext db, ICurrentUserService currentUser, IUserAccountService userAccount)
        {
            _db = db;
            _currentUser = currentUser;
            _userAccount = userAccount;
        }



        public async Task<Result<CartResponse>> GetUserCartAsync(CancellationToken ct)
        {
            if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
                return Result<CartResponse>.Unauthorized("UNAUTHORIZED", "Требуется авторизация!");

            var user = await _userAccount.TryGetUserByIdAsync(_currentUser.UserId.Value, ct);

            if (user is null)
                return Result<CartResponse>.NotFound("USER_NOT_FOUND", "Пользователь не найден в базе данных.");

            var cart = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == user.Data.Id, ct);

            if (cart is null)
                return Result<CartResponse>.Ok(new CartResponse());

            return Result<CartResponse>.Ok(Map(cart));
        }


        private static CartResponse Map(Cart cart)
        {
            return new CartResponse
            {
                Items = cart.Items.Select(i => new CartItemResponse
                {
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity
                }).ToList()
            };
        }


        private async Task<Cart> GetOrCreateUserCartAsync(Guid userId, CancellationToken ct)
        {
            var cart = await _db.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId, ct);

            if (cart is null)
            {
                cart = new Cart { UserId = userId };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync(ct);
            }

            return cart;
        }


        public async Task<Result<CartResponse>> AddProductToUserCart(AddToCartRequest req, CancellationToken ct)
        {
            var user = await GetUserMap(ct);

            if (!user.IsSuccess) return Result<CartResponse>.Unauthorized("UNAUTHORIZED", "Требуется авторизация!");

            var cart = await GetOrCreateUserCartAsync(user.Data.Id, ct);

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == req.ProductId, ct);

            if (product is null)
                return Result<CartResponse>.NotFound("PRODUCT_NOT_FOUND", "Продукт не найден.");

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == req.ProductId);

            if (existingItem is not null) existingItem.Quantity += req.Quantity;
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = product.Id,
                    Quantity = req.Quantity
                });
            }

            await _db.SaveChangesAsync(ct);

            return Result<CartResponse>.Ok(Map(cart));
        }


        private async Task<Result<UserResponse>> GetUserMap(CancellationToken ct)
        {
            if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
                return Result<UserResponse>.Unauthorized("UNAUTHORIZED", "Требуется авторизация!");

            var user = await _userAccount.TryGetUserByIdAsync(_currentUser.UserId.Value, ct);

            if (user is null)
                return Result<UserResponse>.NotFound("USER_NOT_FOUND", "Пользователь не найден в базе данных.");

            return Result<UserResponse>.Ok(user.Data);
        }


    }
}
