using BookTest.Data;
using BookTest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Threading.Tasks;

namespace BookTest.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<CartRepository> _logger;
        public CartRepository(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            ILogger<CartRepository> logger
            )
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<int> AddItem(int bookId, int quantity)
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId) || quantity <= 0)
                throw new Exception("User is not Log-in");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy hoặc tạo ShoppingCart cho user
                var cart = await GetCart(userId);

                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                        IsDeleted = false
                    };
                    _context.ShoppingCarts.Add(cart);
                    await _context.SaveChangesAsync(); // Cần save để cart.Id được sinh ra
                }

                // Tìm CartDetail tương ứng
                var cartItem = await GetCartItem(bookId,cart.Id);

                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = quantity
                    };
                    _context.CartDetails.Add(cartItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                //throw new Exception($"Lỗi khi thêm item vào giỏ hàng: {ex.Message}", ex);
                _logger.LogError(ex, $"Lỗi khi thêm item vào giỏ hàng: BookId={bookId}, UserId={userId}");
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int bookId)
        {
            string userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not Log-in");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Lấy hoặc tạo ShoppingCart cho user
                var cart = await GetCart(userId);
                if (cart == null)
                    throw new Exception("Giỏ hàng không tồn tại");

                var cartItem = await GetCartItem(bookId, cart.Id);
                if (cartItem == null)
                {
                    var itemCount = await GetCartItemCount(userId);
                    if (itemCount == 0)
                        throw new Exception("Giỏ hàng trống, không có sản phẩm nào trong giỏ hàng");
                }else if(cartItem.Quantity == 1)
                {
                    _context.CartDetails.Remove(cartItem);
                }
                else
                {
                    cartItem.Quantity -= 1;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, $"Lỗi khi xóa item khỏi giỏ hàng: BookId={bookId}, UserId={userId}");
                throw new Exception("Đã có lỗi khi xóa item khỏi giỏ hàng", ex);
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<IEnumerable<ShoppingCart>> GetUserCart()
        {
            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("Invalid User");
            var shoppingCart = await _context.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Book)
                .ThenInclude(a => a.Genre)
                .Where(a => a.UserId == userId && !a.IsDeleted)
                .ToListAsync();
            return shoppingCart;
        } 
        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _context.ShoppingCarts
                .FirstOrDefaultAsync(c => c.UserId == userId);
            return cart;
        }
        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }
            var data = await (from cart in _context.ShoppingCarts
                              join cartDetail in _context.CartDetails on cart.Id equals cartDetail.ShoppingCartId
                              where cart.UserId == userId && !cart.IsDeleted
                              select new
                              {
                                  cartDetail.Id
                              }).ToListAsync();
            return data.Count();
        }
        public async Task<CartDetail?> GetCartItem(int bookId,int cartId)
        {
            return await _context.CartDetails
                .FirstOrDefaultAsync(c => c.BookId == bookId && c.ShoppingCartId == cartId);
        }
        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
