using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Model
{
    public class ProductModel
    {
        public int Id { get; set; }

        //Name and ProductName is the same parameter. It was just named differently in the tests,
        //so instead of making a new class I added a extra (albeit unnecessary) parameter here. 
        public string Name { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
    }
}
