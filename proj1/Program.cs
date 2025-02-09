
using hw1.Models;
using Library.eCommerce.Services;
using System.ComponentModel;
namespace MyApp
{
    internal class Program
    {

        static void Main(string[] args) //main is always static
        {
            Console.WriteLine("Welcome to Amazon!");

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory items");
            Console.WriteLine("U. Update an inventory item");
            Console.WriteLine("D. Delete an inventory item");

            Console.WriteLine("A. Add an item to shopping cart");
            Console.WriteLine("S. Read all shopping cart");
            Console.WriteLine("N. Update the number of item in cart");
            Console.WriteLine("G. Return all of a product");
            
            Console.WriteLine("Q. Quit and see your price");
            List<Product?> list = ProductServiceProxy.Current.Products; //shallow copy
            List<Cart?> cart = CartServiceProxy.Current.Items;
            char choice;
            do
            {
                string? input = Console.ReadLine().ToUpper();
                choice = input[0];
                switch (choice)
                {
                    case 'C':
                        Console.WriteLine("What is the name, price and amount of items?");
                        
                        ProductServiceProxy.Current.AddOrUpdate(new Product
                        {
                            Name = Console.ReadLine() ?? "UNC",
                            Price = double.Parse(Console.ReadLine()??"-1"),//setting the member data
                            Amount = int.Parse(Console.ReadLine() ?? "-1"),
                        });
                        break;
                    case 'R':
                        list.ForEach(Console.WriteLine); //go through list instance and for each read them
                        break;
                    case 'U': 
                        Console.WriteLine("Which prodcut would you like to update?");
                        int selection = int.Parse(Console.ReadLine() ?? "-1"); //the "index"
                        Console.WriteLine("Please tell us the new name, price, and amount");
                        var selectedProd = list.FirstOrDefault(p => p.Id == selection); //effectively a shallow copy
                        var selectedCart = cart.FirstOrDefault(p => p?.Name == selectedProd?.Name); //putting data if name is smae as prod.name into the variable selectedcart
                        if (selectedCart != null) //exist
                        {
                            selectedProd.Name = Console.ReadLine() ?? "ERROR";
                            selectedProd.Amount = int.Parse(Console.ReadLine() ?? "-1");
                            selectedProd.Price = double.Parse(Console.ReadLine() ?? "-1");
                            selectedCart.Name = selectedProd.Name; //this changes name when updating in the inventory, not sure if supposed to but without it i get error
                            selectedCart.Price = selectedProd.Price;
                            ProductServiceProxy.Current.AddOrUpdate(
                            CartServiceProxy.Current.AddOrUpdate(selectedCart, selectedProd, selectedCart.Num));

                        }
                        else if (selectedProd != null)//exist
                        {
                            selectedProd.Name = Console.ReadLine() ?? "ERROR"; //changes the name
                            selectedProd.Price = double.Parse(Console.ReadLine() ?? "-1");//changes price
                            selectedProd.Amount = int.Parse(Console.ReadLine() ?? "-1");//change amount
                            ProductServiceProxy.Current.AddOrUpdate(selectedProd); //update with new info
                        }

                        break;
                    case 'D': 
                        Console.WriteLine("Which prodcut would you like to update?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");//choice
                        ProductServiceProxy.Current.Delete(selection);//deelte the choice

                        break;
                    case 'A': //add item to shopping cart
                        Console.WriteLine("What item would you like to add to your cart and how many?");
                        selection = int.Parse(Console.ReadLine() ?? "-1"); //item index you want to return
                        selectedProd = list.FirstOrDefault(p => p.Id == selection); //finds it
                        selection = int.Parse(Console.ReadLine() ?? "-1"); //how many
                        if(selectedProd!=null)
                        {
                            ProductServiceProxy.Current.AddOrUpdate(//prdocuts verson
                                CartServiceProxy.Current.AddOrUpdate(new Cart//calling on carts version to update carts
                                { 
                                    Name = selectedProd.Name,
                                    Price = selectedProd.Price,
                                }, selectedProd, selection));//the location and the amount
                        }

                        break;
                    case 'S': 
                            cart.ForEach(Console.WriteLine);
                        break;
                    case 'N':
                        Console.WriteLine("What item do you want to update amount?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedProd = list.FirstOrDefault(p => p?.Id == selection); 
                        selectedCart = cart.FirstOrDefault(p => p?.Name == selectedProd.Name);
                        if (selectedCart != null)
                        {
                            Console.WriteLine("What amount?");
                            selection = int.Parse(Console.ReadLine() ?? "-1");

                            ProductServiceProxy.Current.AddOrUpdate(
                                CartServiceProxy.Current.AddOrUpdate(selectedCart, selectedProd, selection));
                        }
                        else
                            Console.WriteLine("Does not Exist");//from u this is where error pops up

                        break;
                    case 'G':
                        Console.WriteLine("Which item do you want to send back?");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedCart = cart.FirstOrDefault(p => p?.Id == selection);
                        selectedProd = list.FirstOrDefault(p => p?.Name == selectedCart?.Name);
                        if (selectedCart != null)
                        {
                            ProductServiceProxy.Current.AddOrUpdate(
                                CartServiceProxy.Current.Send(selection, list));//put send inside as it returns the newly update product and we want to update it
                        }

                        break;
                    case 'Q':
                        CartServiceProxy.Current.Bill();

                        break;
                    default:
                        Console.WriteLine("Error: Unknown Command");

                        break;
                }
            } while (choice != 'Q');
        }
    }
}
