using Webstore_MyElectronics.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;

namespace Webstore_MyElectronics.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShoppingCart _shoppingCart;

        private readonly DatabaseContext _dbcontext;

        private readonly UserManager<ApplicationUser> _userManager;

        public OrderController(ShoppingCart shoppingCart,DatabaseContext dbcontext,UserManager<ApplicationUser> userManager)
        {
            _shoppingCart = shoppingCart;
            _dbcontext = dbcontext;
            _userManager = userManager;
        }
        
        public IActionResult unCheckout(Order order)
        {
            var prods = _shoppingCart.GetShoppingCartItems().ToList();
            var mail = order.Email;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            if(prods.Count>=1)
            {
                order.prod2name = prods[0].Product.ProductName;
            }
            if(prods.Count>=2)
            {
                order.prod2name = prods[1].Product.ProductName;
            }
            if(prods.Count>=3)
            {
                order.prod3name = prods[2].Product.ProductName;
            }
            if(prods.Count>=4)
            {
                order.prod4name = prods[3].Product.ProductName;
            }
            if(prods.Count>=5)
            {
                order.prod5name = prods[4].Product.ProductName;
            }


             if (ModelState.IsValid)
            {
                    sentMessage(order,mail);

                _shoppingCart.ClearCart();
                return RedirectToAction("unCheckoutComplete");
            }
            return View(order);
        }


        public IActionResult unCheckoutComplete()
        {
            return View();
        }

        

        [Authorize ]
        [HttpGet]
        public async Task<IActionResult> Checkout(string id)
        {
            var user = await GetUserByEmail(id);
            var model = new Order
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AddressLine1 = user.AddressLine1,
                AddressLine2 = user.AddressLine2,
                ZipCode = user.ZipCode,
                State = user.State,
                Country=user.Country,
                PhoneNumber = user.PhoneNumber
            };
            
            return View(model);
        } 

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Checkout(Order order,string id)
        {
            var items = _shoppingCart.GetShoppingCartItems();
            var user = await GetUserByEmail(id);
            
            List<int> prods = new List<int>();


            _shoppingCart.ShoppingCartItems = items;

            foreach(var i in items)
            {
                var productAmount = i.Amount;
            
                var list = _dbcontext.Products.Where(p=>p.ProductId == i.Product.ProductId);
                var product = _dbcontext.Products.FirstOrDefault(p=>p.ProductId == i.Product.ProductId);

                foreach (var x in list)
                {   
                    prods.Add(x.ProductId);
                    x.TimesBought += 1 * productAmount;
                }
            }
            if(prods.Count() >= 1)
            {
                var prod1 = _dbcontext.Products.FirstOrDefault(p=>p.ProductId == prods[0]);
                order.prod1name = prod1.ProductName;
                order.prod1img = prod1.ImgUrl;
                order.prod1price = prod1.ProductPrice;
            }
             if(prods.Count() >=2)
            {
                var prod2 = _dbcontext.Products.FirstOrDefault(p=>p.ProductId == prods[1]);
                order.prod2name = prod2.ProductName;
                order.prod2img = prod2.ImgUrl;
                order.prod2price = prod2.ProductPrice;
                System.Console.WriteLine(prod2.ProductName);
            }
             if(prods.Count() >= 3)
            {
                var prod3 = _dbcontext.Products.FirstOrDefault(p=>p.ProductId == prods[2]);
                order.prod3name = prod3.ProductName;
                order.prod3img = prod3.ImgUrl;
                order.prod3price = prod3.ProductPrice;
            }
             if(prods.Count() >= 4)
            {
                var prod4 = _dbcontext.Products.FirstOrDefault(p=>p.ProductId == prods[3]);
                order.prod4name = prod4.ProductName;
                order.prod4img = prod4.ImgUrl;
                order.prod4price = prod4.ProductPrice; 
            }
             if(prods.Count() >= 5)
            {
                var prod5 = _dbcontext.Products.FirstOrDefault(p=>p.ProductId == prods[4]);
                order.prod5name = prod5.ProductName;
                order.prod5img = prod5.ImgUrl;
                order.prod5price = prod5.ProductPrice;
            }
            
            
            order.User = user;
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = _shoppingCart.GetShoppingCartTotal();

            
            
            if (ModelState.IsValid && user!=null)
            {
                sentMessage(order,User.Identity.Name);
                _dbcontext.Orders.Add(order);
                _shoppingCart.ClearCart();
                return RedirectToAction("CheckoutComplete");
            }
            return View(order);
        }


        public void sentMessage(Order order,string email)
        {
            var itemlist = _shoppingCart.GetShoppingCartItems();
            
            var p1 = "";
            var p2 = "";
            var p3 = "";
            var p4 = "";
            var p5 = "";

            if(order.prod1name != null)
            {
                p1= "| " +order.prod1name;
            }
            if(order.prod2name != null)
            {
                p2= "| " +order.prod2name;
            }
            if(order.prod3name != null)
            {
                p3= "| " +order.prod3name;
            }
            if(order.prod4name != null)
            {
                p4="| " +order.prod4name;
            }
            if(order.prod5name != null)
            {
                p5= "| " +order.prod5name;
            }
            

            
            var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("MyElectronics", "freddydacruz90@gmail.com"));
                    message.To.Add(new MailboxAddress("Order confirmed", email));
                    message.Subject = "Order Confirmed";
                    message.Body = new TextPart("plain")
                    {
                        Text = "Your Order has been confirmed you paid €" + order.OrderTotal + " you ordered the following products:" +p1 + p2+ p3 + p4+ p5 
                        
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);  
                        client.Authenticate("freddydacruz90@gmail.com", password: "c374d1bb");
        
                        client.Send(message);
        
                        client.Disconnect(true);
                    }
        }

        public IActionResult CheckoutComplete()
        {
            ViewBag.CheckoutCompleteMessage = "Thanks for your order! :) ";
            return View();
        }


        private async Task<ApplicationUser> GetUserById(string id)=> await _userManager.FindByIdAsync(id);
        private async Task<ApplicationUser> GetUserByEmail(string id)=> await _userManager.FindByEmailAsync(id);
    }
}