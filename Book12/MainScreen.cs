using Engine.Locations;

namespace Book12
{
    public partial class MainScreen : Form
    {
        
        public static Dictionary<string, Bitmap> map_Dict = new Dictionary<string, Bitmap>();
        public static Dictionary<string, int> idIndex = new Dictionary<string, int>();
        public static Dictionary<int, Settlement> Settlements = new Dictionary<int, Settlement>();


        public MainScreen()
        {
            InitializeComponent();
            MapRenderer mapper = new MapRenderer();
            mapper.RenderMapInitial();
            //mapper.RenderCities();
            map_Dict = MapRenderer.map_Dict;
            pictureBox1.Size = MapRenderer.size;
            //MapRender();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != null)
            {
                // Check the entered number and call the appropriate function.
                switch (textBox1.Text)
                {
                    case "1":
                        pictureBox1.Image = map_Dict["Map"];
                        break;
                    case "2":
                        pictureBox1.Image = map_Dict["is_Land_Map"];
                        break;
                    case "3":
                        MapRenderer mapper = new MapRenderer();
                        mapper.RenderCities();
                        pictureBox1.Image = map_Dict["cities_Map"];
                        break;
                    case "4":

                        break;
                    // Add more cases as needed.
                    default:
                        richTextBox1.Text = "Improper input";
                        break;
                }
            }
            else
            {
                // Handle invalid input (non-integer) or provide feedback to the user.
            }
        }
    }
}