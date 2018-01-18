using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Webstore_MyElectronics.Models;

namespace Webstore_MyElectronics.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public  HomeController (DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public  async Task<ActionResult> Index(int? page)
        {
            var products = from s in _context.Products select s;
            products = products.Where(s => s.OnSale == true);

           List<Product> toremove = new List<Product>();
            List<Product> toremove1 = new List<Product>();

            ViewBag.products = _context.Products.OrderByDescending(x=>x.TimesBought).Take(4);

            Product p2 = new Product{
                CategoryId = 4,
                ProductName ="Asus G11CD-K-NL011T",
                ImgUrl = "https://image.coolblue.io/products/689197?width=500&height=500" ,
                ProductPrice = 3000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p3 = new Product{
                CategoryId = 4,
                ProductName ="HP ProDesk 400 G4 1JJ60EA",
                ImgUrl = "https://image.coolblue.io/products/712893?width=500&height=500" ,
                ProductPrice = 3000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p4 = new Product{
                CategoryId = 4,
                ProductName ="Acer Predator Orion 9000-900 i7X-SLI NL",
                ImgUrl = "https://image.coolblue.io/products/957728?height=390&width=422" ,
                ProductPrice = 3000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p5 = new Product{
                CategoryId = 4,
                ProductName ="MSI Vortex G25 8RE-014NL",
                ImgUrl = "https://image.coolblue.io/products/956490?height=390&width=422" ,
                ProductPrice = 3000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p9 = new Product{
                CategoryId = 4,
                ProductName ="Dell OptiPlex 3050 MFXX8",
                ImgUrl = "https://image.coolblue.io/products/900467?width=500&height=500" ,
                ProductPrice = 2900,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p6 = new Product{
                CategoryId = 4,
                ProductName ="MSI Infinite X VR8RF-019EU",
                ImgUrl = "https://image.coolblue.io/products/882788?height=390&width=422" ,
                ProductPrice = 3500,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p7 = new Product{
                CategoryId = 4,
                ProductName ="HP Omen 880-179nd",
                ImgUrl = "https://image.coolblue.io/products/833558?height=390&width=422" ,
                ProductPrice = 300,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p8 = new Product{
                CategoryId = 4,
                ProductName ="Lenovo ThinkCentre M710q 10MR0052MH",
                ImgUrl = "https://image.coolblue.io/products/786030?height=390&width=422" ,
                ProductPrice = 1000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p10 = new Product{
                CategoryId = 4,
                ProductName ="Apple Mac Mini 1.4GHz",
                ImgUrl = "https://image.coolblue.io/products/580962?height=390&width=422" ,
                ProductPrice = 2000,
                Stock = 20,
                spec1 = "1000Gb HDD",
                spec2 = "500gb SSD",
                spec3 = "intel core i8",
                spec4 = "Nvidia gtx1080",
                spec5 = "17inch screen",
                OldPrice = 0,
                ProductDescription =""

            };

            Product p11 = new Product{
                CategoryId = 5,
                ProductName ="Apple iPad (2017) 128 GB Wifi Space Gray",
                ImgUrl = "https://image.coolblue.io/products/946000?width=500&height=500" ,
                ProductPrice = 999,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p12 = new Product{
                CategoryId = 5,
                ProductName ="Samsung Galaxy Tab S2 9,7 inch 32GB Zwart 2016",
                ImgUrl = "https://image.coolblue.io/products/664277?height=390&width=422sss" ,
                ProductPrice = 1000,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p13 = new Product{
                CategoryId = 5,
                ProductName ="Samsung Galaxy Tab A 10.1 Wifi Zwart",
                ImgUrl = "https://image.coolblue.io/products/664269?height=390&width=422" ,
                ProductPrice = 2000,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p14 = new Product{
                CategoryId = 5,
                ProductName ="Samsung Galaxy Tab S2 9,7 inch 32GB Goud 2016t",
                ImgUrl = "https://image.coolblue.io/products/665218?height=390&width=422" ,
                ProductPrice = 200,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p15 = new Product{
                CategoryId = 5,
                ProductName ="Samsung Galaxy Tab S3 Wifi Zilver",
                ImgUrl = "https://image.coolblue.io/products/702659?height=390&width=422" ,
                ProductPrice =390,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p16 = new Product{
                CategoryId = 5,
                ProductName ="Apple iPad Pro 10,5 inch 256 GB Wifi Rose GoldSamsung Galaxy Tab S3 Wifi Zilver",
                ImgUrl = "https://image.coolblue.io/products/946103?height=390&width=422" ,
                ProductPrice = 290,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p17 = new Product{
                CategoryId = 5,
                ProductName ="Lenovo Tab 4 8 Plus 3GB 16GB Zwart",
                ImgUrl = "https://image.coolblue.io/products/932053?width=500&height=500" ,
                ProductPrice = 2999,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p18 = new Product{
                CategoryId = 5,
                ProductName ="Lenovo Yoga Book YB1-X90F Goud",
                ImgUrl = "https://image.coolblue.io/products/801746?height=390&width=422" ,
                ProductPrice = 1000,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p19 = new Product{
                CategoryId = 5,
                ProductName ="Apple iPad Pro 10,5 inch 256 GB Wifi + 4G Silver",
                ImgUrl = "https://image.coolblue.io/products/947079?height=390&width=422" ,
                ProductPrice = 200,
                Stock = 20,
                spec1 = "100Gb HDD",
                spec2 = "iOS10",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "9,7inch screen",
                OldPrice = 0,
                ProductDescription =""

            };

            Product p22 = new Product{
                CategoryId = 6,
                ProductName ="Samsung Galaxy A5 (2017) Zwart",
                ImgUrl = "https://image.coolblue.io/products/656931?height=390&width=422" ,
                ProductPrice = 500,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p23 = new Product{
                CategoryId = 6,
                ProductName ="Apple iPhone X 64GB Space Grey",
                ImgUrl = "https://image.coolblue.io/products/884812?height=390&width=422" ,
                ProductPrice = 1180,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p24 = new Product{
                CategoryId = 6,
                ProductName ="Samsung Galaxy S8 Zwart",
                ImgUrl = "https://image.coolblue.io/products/820328?height=390&width=422" ,
                ProductPrice = 780,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };Product p25 = new Product{
                CategoryId = 6,
                ProductName ="Apple iPhone 6 32GB Grijs",
                ImgUrl = "https://image.coolblue.io/products/818870?height=390&width=422" ,
                ProductPrice = 200,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };Product p26 = new Product{
                CategoryId = 6,
                ProductName ="Apple iPhone 8 64GB Space Grey",
                ImgUrl = "https://image.coolblue.io/products/911335?height=390&width=422" ,
                ProductPrice = 999,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p27 = new Product{
                CategoryId = 6,
                ProductName ="Samsung Galaxy A5 (2017) Goud",
                ImgUrl = "https://image.coolblue.io/products/656932?height=390&width=422" ,
                ProductPrice = 200,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };
            Product p28 = new Product{
                CategoryId = 6,
                ProductName ="Apple iPhone SE 32GB Space Gray",
                ImgUrl = "https://image.coolblue.io/products/503511?height=390&width=422" ,
                ProductPrice = 800,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };  Product p29 = new Product{
                CategoryId = 6,
                ProductName ="Samsung Galaxy Xcover 4",
                ImgUrl = "https://image.coolblue.io/products/736320?height=390&width=422" ,
                ProductPrice = 600,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };Product p30 = new Product{
                CategoryId = 6,
                ProductName ="Motorola Moto G5 Plus Grijs",
                ImgUrl = "https://image.coolblue.io/products/703559?height=390&width=422" ,
                ProductPrice = 290,
                Stock = 20,
                spec1 = "32gb storage",
                spec2 = "Android 6.0 Marshmallow",
                spec3 = "intel core i8",
                spec4 = "intergrated graphics",
                spec5 = "5.2inch screen",
                OldPrice = 0,
                ProductDescription =""

            };



            // _context.Products.Add(p22);
            //  _context.Products.Add(p23);
            //   _context.Products.Add(p24);
            //    _context.Products.Add(p25);
            //     _context.Products.Add(p26);
            //      _context.Products.Add(p27);
            //       _context.Products.Add(p28);
            //        _context.Products.Add(p29);
            //       _context.Products.Add(p30);


            _context.SaveChanges();
            


            var allprods = _context.Products;

            // foreach (var item in allprods)
            // {
            //     var to = _context.Products.First(x=>x.ProductName == item.ProductName);
            //     if(toremove.Contains(to))
            //     {
            //        System.Console.WriteLine("na");
            //     }
            //     else{
            //         toremove.Add(to);
            //     }


            // }

            // foreach(var i in allprods)
            // {
            //     _context.Remove(i);
            // }
            
            // _context.SaveChanges();

            // System.Console.WriteLine(allprods.Count());
            // System.Console.WriteLine(toremove.Count());
            
            // foreach (var item in toremove)
            // {
            //     _context.Products.Add(item);
            // }
    


            // _context.SaveChanges();
            int pageSize = 3;
            return View(await PaginatedList<Product>.CreateAsync(products, page ?? 1, pageSize));
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
