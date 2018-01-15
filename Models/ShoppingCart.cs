using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;





namespace Webstore_MyElectronics.Models
{
     public class ShoppingCart
    {
        private readonly DatabaseContext _dbcontext;
        
        private ShoppingCart(DatabaseContext dbcontext){
            _dbcontext = dbcontext;
        }

        public string ShoppingCartId{get;set;}
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public static ShoppingCart GetCart(IServiceProvider services){
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<DatabaseContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();

            session.SetString("CartId",cartId);
            return new ShoppingCart(context) {ShoppingCartId = cartId};
        }

        public void AddToCart(Product product,int amount)
        {
            var shoppingCartItem = _dbcontext.ShoppingCartItems.SingleOrDefault(s => s.Product.ProductId == product.ProductId && s.ShoppingCartId == ShoppingCartId);
            
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Amount = 1
                };

                _dbcontext.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _dbcontext.SaveChanges();
        }

        public int RemoveFromCart(Product product)
        {
            var shoppingCartItem = _dbcontext.ShoppingCartItems.SingleOrDefault(s => s.Product.ProductId == product.ProductId && s.ShoppingCartId == ShoppingCartId);
            
            var localAmount = 0;

            if(shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                    localAmount = shoppingCartItem.Amount;
                }
                else
                {
                    _dbcontext.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _dbcontext.SaveChanges();

            return localAmount;
        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ??
            (ShoppingCartItems =
                _dbcontext.ShoppingCartItems.Where(c => c.ShoppingCartId == ShoppingCartId)
                    .Include(s => s.Product)
                    .ToList());
        }

        public void ClearCart(){
            var cartItems =_dbcontext
                .ShoppingCartItems
                .Where(cart => cart.ShoppingCartId == ShoppingCartId);

            _dbcontext.ShoppingCartItems.RemoveRange(cartItems);

            _dbcontext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            var total = _dbcontext.ShoppingCartItems.Where(c=>c.ShoppingCartId == ShoppingCartId)
                .Select(c=> c.Product.ProductPrice * c.Amount).Sum();
            return total;
        }
    }

}