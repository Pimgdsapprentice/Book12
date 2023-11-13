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

namespace Book12
{
    public partial class DnC_Screen : Form
    {
        public MainScreen main = Application.OpenForms.OfType<MainScreen>().FirstOrDefault();

        public static Dictionary<string, Bitmap> map_Dict = new Dictionary<string, Bitmap>();
        public static Dictionary<string, int> idIndex = new Dictionary<string, int>();
        public static Dictionary<int, Settlement> Settlements = new Dictionary<int, Settlement>();


        public static string[] mapKeyMappings = new string[] { "Map", "is_Land_Map", "cities_Ex_Map", "cities_Map" };

        public MenuControls menuAtAddrs(string targetdirectory)
        {
            return MenuList.SingleOrDefault(menu => menu.address == targetdirectory);
        }


        List<MenuControls> MenuList = new List<MenuControls>
        {
            new MenuControls(0, "Main Menu", ":Main Menu", "Map Selection"),
            new MenuControls(1, "Map Menu", ":Main Menu/Map Selection", mapKeyMappings)
        };
        string crntMenuDir = ":Main Menu";
        string[] menuSpecials = new string[] { ":Main Menu/Map Selection" };

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
            if (textBox1.Text == "e")
            {
                crntMenuDir = ":Main Menu";
                MenuOut();
            }
            else if (crntMenuDir == ":Main Menu/Map Selection")
            {
                if (menuSel >= 0 && menuSel < menuAtAddrs(crntMenuDir).submenus.Count)
                {
                    main.ChangePictureBoxImage(map_Dict[mapKeyMappings[menuSel]]);
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

        //Prevents closing of form
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
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

        public DnC_Screen(MainScreen main)
        {
            InitializeComponent();
            this.main = main;

            //Prepares Map
            MapRenderer mapper = new MapRenderer();
            mapper.RenderMapInitial();
            mapper.RenderCities();
            map_Dict = MapRenderer.map_Dict;
            //Ensures Autoscroll
            richTextBox1.ScrollBars = RichTextBoxScrollBars.Both;
            richTextBox1.HideSelection = false;

            //Menu();
            MenuOut();
        }




        private void nOut(string outLine)
        {
            richTextBox1.AppendText("\n" + outLine);
        }
        private void inva()
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
