using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;
using Webstore_MyElectronics.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;

namespace Webstore_MyElectronics.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly ShoppingCart _shoppingCart;
        // UserManager toegevoegd om de data van de ingelogde gebruiker te kunnen gebruiken.
        private readonly UserManager<ApplicationUser> _userManager;

        public  ShoppingCartController (DatabaseContext context,ShoppingCart shoppingCart, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _shoppingCart = shoppingCart;
            // Usermanager toegevoegd als paramater in de constructor en een waarde eraan meegegeven in de constructor.
            // Met User. kan je gebruik maken van de UserManager.
            _userManager = userManager;
        }

        public ViewResult Index()
        {

            var items = _shoppingCart.GetShoppingCartItems();

            _shoppingCart.ShoppingCartItems = items;
            
            var sCVM = new ShoppingCartViewModel
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
        
            return View(sCVM);
        }

        public RedirectToActionResult AddToShoppingCart(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p=> p.ProductId == productId);
            if(selectedProduct != null)
            {
                selectedProduct.Stock -= 1;
                _context.SaveChanges();
                _shoppingCart.AddToCart(selectedProduct,1);
            }

            return RedirectToAction("Index");
        }

        public async Task<RedirectToActionResult> AddToAllShoppingCart(int productId,string id)
        {
            var user = await _userManager.FindByEmailAsync(id);
            var selecteditems = _context.UserWishLists.Where(i=>i.User.Email == user.UserName).Include(i=>i.product);
            var wishlistItems = _context.UserWishLists.Where(i=>i.User.Email == user.UserName);

            if(selecteditems != null)
            {
                foreach(var i in selecteditems)
                {
                    var product = _context.Products.FirstOrDefault(p=> p.ProductId == i.product.ProductId);
                    product.Stock -= 1;
                    _shoppingCart.AddToCart(product,1);
                }
            }

            foreach(var x in wishlistItems){
                _context.UserWishLists.Remove(x);
            }


            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        public RedirectToActionResult IncrementProduct(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p=> p.ProductId == productId);

        
            if(selectedProduct != null)
            {
                selectedProduct.Stock -= 1;
                _context.SaveChanges();
                _shoppingCart.AddToCart(selectedProduct,1);

            }


            return RedirectToAction("Index");
        }



        public RedirectToActionResult RemoveFromShoppingCart(int productId)
        {
            var selectedProduct = _context.Products.FirstOrDefault(p=> p.ProductId == productId);
            if(selectedProduct != null)
            {
                selectedProduct.Stock += 1;
                _context.SaveChanges();
                _shoppingCart.RemoveFromCart(selectedProduct);
            }

            return RedirectToAction("Index");
        }

        public RedirectToActionResult RemoveAllFromShoppingCart()
        {   var items = _shoppingCart.GetShoppingCartItems();

            foreach (var item in items)
            {
                var amount = item.Amount;
                var selectedProduct = _context.Products.FirstOrDefault(p=> p.ProductId == item.Product.ProductId);
                selectedProduct.Stock += 1 * amount;
                _context.SaveChanges();
                for (int i = amount; i >= 0 ; i--)
                {
                    _shoppingCart.RemoveFromCart(selectedProduct);
                }
            }


            return RedirectToAction("Index");
        }

        public RedirectToActionResult SendEmail()
        { 
            // lege string maken voor de producten in het winkelmandje.
            string items= "";
            // Alle items in de shoppingcart verzamelen in een variabel.
            var itemsPurchased = _shoppingCart.GetShoppingCartItems();
            // De naam en prijs van elk product toevoegen aan de lege string.
            foreach(var item in itemsPurchased){
                items = items + item.Product.ProductName+" : €"+item.Product.ProductPrice+",-\n";
            }           

            var total = _shoppingCart.GetShoppingCartTotal();
            var totalitems = _shoppingCart.GetShoppingCartItems();
            var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Order Confirmed", "freddydacruz90@gmail.com"));
                    // Email wordt verzonden naar de mailadres van de ingelogde gebruiker.
                    // Er moet nog een manier gevonden worden om de naam van de gebruiker zelf toe te voegen.
                    message.To.Add(new MailboxAddress("Freddy da Cruz", User.Identity.Name));
                    message.Subject = "Order Confirmed";
                    message.Body = new TextPart("plain")
                    {
                        // email naar de gebruiker sturen met de naam en bedrag van elk product, maar ook het totaal bedrag
                        Text = ("Thank you "+User.Identity.Name+" for shopping at MyElectronics.com. This is a confirmation that we recieved your order.\nThe order was placed on "+DateTime.Today.ToString("dd-MM-yyyy")+"\n\nYou ordered the following item(s):\n\n"
                        +items+" \nYou paid the total of: €" 
                        +total+",-.\n\nIn case you need additional information please contact our customer service at www.MyElectronics.com.\n\nEnjoy you product(s),\n\nMyElectronics")
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);  
                        client.Authenticate("freddydacruz90@gmail.com", password: "c374d1bb");
        
                        client.Send(message);
        
                        client.Disconnect(true);
                    }

            _shoppingCart.ClearCart();

            return RedirectToAction("Index");
        }


    }
}
