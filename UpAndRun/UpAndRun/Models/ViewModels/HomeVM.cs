using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UpAndRun.Models.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Product> ListOfProducts { get; set; }
        public IEnumerable<Category> ListOfCategories { get; set; }
    }
}
