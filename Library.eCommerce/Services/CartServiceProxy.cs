using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using hw1.Models;

namespace Library.eCommerce.Services
{
    public class CartServiceProxy
    {
        private CartServiceProxy()
        {
            Items = new List<Cart?>();
        }

        private int LastKey
        {
            get
            {
                if (!Items.Any())
                {
                    return 0;
                }

                return Items.Select(p => p?.Id ?? 0).Max();
            }
        }
        private static CartServiceProxy? instance; //? means it can be null
        private static object instanceLock = new object();
        public static CartServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CartServiceProxy();
                    }
                }

                return instance;
            }
        }

        public List<Cart?> Items { get; private set; }

        
        public Product AddOrUpdate(Cart item, Product prod, int i=0)
        {
            if(i<=prod.Amount+item.Num)//making sure the i "amount" exist
            {
                if (item.Id == 0) //the id "number on side"
                {
                    item.Id = LastKey + 1;
                    Items.Add(item);
                }
                prod.Amount -=i;//subtract the amount we want for the total amount in inventory
                item.Num = i; //send i to be what is in cart
            }
            if(item.Num==0)
            {
                Delete(item.Id);
            }
            return prod;
        }

        public Product? Send(int x, List<Product?> prod)
        {
            if (x == 0)
            { 
                return null;
            }
            Cart?item = Items.FirstOrDefault(p => p.Id == x);//if id of p matches x
            var product = prod.FirstOrDefault(p => p?.Name == item?.Name);
            product.Amount += item.Num;//add the amount back
            Items.Remove(item);//remove from cart
            if(item.Num==0)
            {
                Delete(item.Id);
            }
            return product;
        }
        public Cart Delete(int id)
        {
            if (id == 0)
            {
                return null;
            }
            Cart? items = Items.FirstOrDefault(c => c.Id == id);//if matches set to items
            Items.Remove(items);//delete items
            return items;
        }
        public void Bill()
        {
            double total = 0.0;
            Console.WriteLine("Your bill:");
            Console.WriteLine("{0,-10} {1,8} {2,15:N2}", "Name", "Amount", " Unit Price");
            for (int i = 0; i < Items.Count; ++i)
                {
                    Console.WriteLine("{0,-10} {1,8} {2,15:N2}", Items[i]?.Name, Items[i]?.Price, Items[i]?.Num);

                    total += Items[i].Num * Items[i].Price;
               }
            Console.WriteLine("{0,0} {1,0:N2}", "Your total is: $", total * 1.07);

            
        }
    }
}


