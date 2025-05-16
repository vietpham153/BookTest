﻿using BookTest.Models;

namespace BookTest
{
    public interface ICartRepository
    {
        Task<int> AddItem(int bookId, int quantity);
        Task<int> RemoveItem(int bookId);
        Task<ShoppingCart> GetUserCart();
        Task<int> GetCartItemCount(string userId = "");
        Task<ShoppingCart> GetCart(string userId);
        Task<CartDetail?> GetCartItem(int bookId, int cartId);
    }
}