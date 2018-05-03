using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TR.Classes
{
    public class Filter 
    {
        public static ObservableCollection<String> FiltersList { get; set; }

        static Filter()
        {
            FiltersList = new ObservableCollection<string>()
            {
               "Фамилии", "Номеру телефона", "Почтовому ящику", "Логину"
            };
        }
        public void Search()
        {

        }
    }
}
