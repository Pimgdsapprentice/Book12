using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

using Engine.Locations;
using Engine.Language;
using Engine.Language.Examples;
using Engine;
using System.Reflection;
using Book12.GameData;
using DelaunayVoronoi;
using Book12.MapStuff;


namespace Book12
{
    public partial class DnC_Screen : Form
    {
        //References the first instance of Main Screen
        public MainScreen main = Application.OpenForms.OfType<MainScreen>().FirstOrDefault();
        public static int Scale2 = 100;
        public DnC_Screen(MainScreen main)
        {
            InitializeComponent();
            this.main = main;

            //Creates a World
            World.World_Name = "New World";
            World.locationIndex = 0;
            World.w_settlements = new Dictionary<int, NL_Settlement> { };
            World.seed = 123;
            World.World_Scale = 200;

            Randomer.SetSeed(World.seed);

            //Prepares Map
            MapRenderer mapper = new MapRenderer();
            mapper.RenderMapInitial();
            mapper.RenderCities();
            mapper.RenderProvincalBorders();
            mapper.TintWorldMap();
            mapper.PlaceCities();

            MenuList[1] = new MenuControls(1, "Map Menu", ":Main Menu/Map Selection", World.map_Dict.Keys.ToArray());

            //Ensures Autoscroll
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            richTextBox1.HideSelection = false;

            //Menu();
            MenuOut();
        }

        //Menu Section
        string crntMenuDir = ":Main Menu";
        List<MenuControls> MenuList = new List<MenuControls>
        {
            new MenuControls(0, "Main Menu", ":Main Menu", "Map Selection"),
            new MenuControls(1, "Map Menu", ":Main Menu/Map Selection", World.map_Dict.Keys.ToArray())
        };
        public MenuControls menuAtAddrs(string targetdirectory)
        {
            return MenuList.SingleOrDefault(menu => menu.address == targetdirectory);
        }
        private void MenuOut()
        {
            int index = 0;
            nOut(menuAtAddrs(crntMenuDir).title + ":");
            nOut("---");
            foreach (string i in menuAtAddrs(crntMenuDir).submenus)
            {
                nOut($"{index++}: {i}");
            }
            nOut("e: Exit to Main Menu");
            nOut("x-----");
        }
        
        private void HandleMenuInput()
        {
            //Try to get int from menu input
            int menuSel = -1;
            try
            {
                menuSel = int.Parse(textBox1.Text);
            }
            catch (FormatException)
            {
                menuSel = -1;
            }
            catch (OverflowException)
            {
                menuSel = -1;
            }
            //Checks if Exit
            if (textBox1.Text == "e")
            {
                crntMenuDir = ":Main Menu";
                MenuOut();
            }
            else if (textBox1.Text == "t")
            {
                MenuOut();
            }
            else if (crntMenuDir == ":Main Menu/Map Selection")
            {
                if (menuSel >= 0 && menuSel < menuAtAddrs(crntMenuDir).submenus.Count)
                {
                    main.ChangePictureBoxImage(World.map_Dict[World.map_Dict.Keys.ToArray()[menuSel]]);
                    MenuOut();
                }
                else
                {
                    nOut("Invalid Map choice");
                    nOut("x-----");
                }
            }
            else if (menuSel >= 0 && menuSel < menuAtAddrs(crntMenuDir).submenus.Count)
            {

                crntMenuDir = crntMenuDir + "/" + menuAtAddrs(crntMenuDir).submenus[menuSel];
                MenuOut();
            }
            else
            {
                nOut("Invalid Input");
            }
        }
        //Uses Enter For Menu Entry
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is NumPad Enter (ASCII code 13) and the focused control is textBox1
            if (e.KeyChar == (char)Keys.Enter && this.ActiveControl == textBox1)
            {
                // Perform the same function as the button click
                HandleMenuInput();
                // Prevent the Enter key from being processed by the TextBox
                e.Handled = true;
                textBox1.Text = string.Empty;
            }
        }
        //Prevents closing of form
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void objOut(object obj)
        {
            nOut($"Object Name: {nameof(obj)}");

            Type objectType = obj.GetType();
            FieldInfo[] fields = objectType.GetFields();
            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(obj);
                nOut($"{field.Name}: {value}");
            }
            // Get the properties of the object using reflection
            PropertyInfo[] properties = obj.GetType().GetProperties();
            // Loop through properties and print their names and values
            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);
                nOut($"{property.Name}: {value}");
            }
        }

        public void nOut(string outLine)
        {
            richTextBox1.AppendText("\n" + outLine);
        }
        public void inva()
        {
            nOut("Invalid choice.");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HandleMenuInput();
        }

        //Relatively Unused
        private void button2_Click(object sender, EventArgs e)
        {
            Action2();
        }

        private void Action2()
        {
            MessageBox.Show("Bye!");
        }
    }
}
