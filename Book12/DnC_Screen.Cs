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

namespace Book12
{
    public partial class DnC_Screen : Form
    {
        public MainScreen main = Application.OpenForms.OfType<MainScreen>().FirstOrDefault();

        public static Dictionary<string, Bitmap> map_Dict = new Dictionary<string, Bitmap>();
        public static Dictionary<string, int> idIndex = new Dictionary<string, int>();
        public static Dictionary<int, Settlement> Settlements = new Dictionary<int, Settlement>();

        public static List<int> MenuIndex = new List<int>();

        Dictionary<int, string> mapKeyMappings = new Dictionary<int, string>
                    {
                        { 1, "Map" },
                        { 2, "is_Land_Map" },
                        { 3, "cities_Ex_Map" },
                        { 4, "cities_Map" }
                    };

        public DnC_Screen(MainScreen main)
        {
            InitializeComponent();
            this.main = main;
            MapRenderer mapper = new MapRenderer();
            mapper.RenderMapInitial();
            mapper.RenderCities();
            map_Dict = MapRenderer.map_Dict;

            richTextBox1.Text = "---";
            nOut("Input:");
            MenuIndex.Add(1);

            LanguageGenerator lg = new LanguageGenerator();

            //Found in PLGL.Examples. Set this to your own language (derived from Language).
            Qen qen = new Qen();
            lg.Language = qen;

            richTextBox1.AppendText(lg.GenerateClean("Your sentence goes here!"));

        }

        //Prevents closing of form
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        //Keybindings
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {                
                return true;
            }
            else if (keyData == Keys.Add)
            {
                Action2();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private int ParseInt(string str)
        {
            if (int.TryParse(str, out int selectedOption))
            {
                return selectedOption;
            }
            else
            {
                return 0;
            }
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
            Menu();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Action2();
        }

        private void Menu()
        {
            if (MenuIndex.Count == 0)
            {
                DisplayMainMenu();
            }
            else if (MenuIndex.Count == 1)
            {
                HandleMainMenuInput();
            }
            else if (MenuIndex.Count == 2)
            {
                HandleMapSubMenuInput();
            }
        }

        private void DisplayMainMenu()
        {
            nOut("---");
            nOut("Input:");
            MenuIndex.Add(1);
        }

        private void HandleMainMenuInput()
        {
            switch (textBox1.Text)
            {
                case "1":
                    nOut("Choose Map To display:");
                    MenuIndex.Add(1);
                    break;
                case "2":
                    nOut("b");
                    MenuIndex.Add(2);
                    break;
                default:
                    inva();
                    break;
            }
        }

        private void HandleMapSubMenuInput()
        {
            switch (MenuIndex[1])
            {
                case 1:
                    if (textBox1.Text == "e")
                    {
                        MenuIndex.Clear();
                        DisplayMainMenu();
                    }
                    else if (mapKeyMappings.TryGetValue(ParseInt(textBox1.Text), out var imageKey))
                    {
                        main.ChangePictureBoxImage(map_Dict[imageKey]);
                    }
                    else
                    {
                        nOut("Invalid Map Call. (e - exit)");
                    }
                    break;
                case 2:

                    if (textBox1.Text == "e")
                    {
                        MenuIndex.Clear();
                        DisplayMainMenu();
                    }
                    else
                    {
                        richTextBox1.AppendText("\nmade it you foker");
                        nOut("Invalid Call. (e - exit)");
                    }
                    break;
                default:
                    inva();
                    break;
            }
        }


        private void Action2()
        {
            MessageBox.Show("Bye!");
        }
    }
}
