using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Randomer
    {
        // Declare a static Random instance
        private static Random random = new Random();

        // Create a property to access the Random instance
        public static Random Instance
        {
            get { return random; }
        }
    }
}
