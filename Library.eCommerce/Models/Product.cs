using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }
        public int Amount { get; set; }
        public string? Display
        {
            get
            {
                return $"{Id}. {Name} ${Price}  #{Amount} ";
            }
        }
        public Product()
        {
            Name = string.Empty;
        }
        public override string ToString()
        {
            return Display ?? string.Empty;
        }

    }
}
