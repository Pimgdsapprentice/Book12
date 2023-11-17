using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Randomer
    {
        // Declare a static Random instance with a seed
        private static Random random = new Random();

        // Create a property to access the Random instance
        public static Random Instance
        {
            get { return random; }
        }

        // Declare a static Random instance with a seed
        private static Random seedRandom = new Random();

        // Create a property to access the Random instance
        public static Random seedInstance
        {
            get { return seedRandom; }
        }


        // Method to set a new seed for the Random instance
        public static void SetSeed(int seed)
        {
            seedRandom = new Random(seed);
        }
    }
}
