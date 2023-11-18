using Book12.GameData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Book12.MapStuff;

namespace Book12
{
    public partial class MainScreen : Form
    {
        DnC_Screen dnC_scrn;
        public MainScreen()
        {
            InitializeComponent();
            pictureBox1.Size = MapSettings.map_size;
            dnC_scrn = new DnC_Screen(this);
            dnC_scrn.Show();
        }
        public void ChangePictureBoxImage(Image newImage)
        {
            pictureBox1.Image = newImage;
        }
        private void dNCBtn_Click(object sender, EventArgs e)
        {
            dnC_scrn.Show();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouseEventArgs = e as MouseEventArgs;

            if (mouseEventArgs != null)
            {
                int mouseX = mouseEventArgs.X;
                int mouseY = mouseEventArgs.Y;

                bool hasCity = false;

                foreach (var settlement in World.w_settlements.Values)
                {
                    int cityX = settlement.X_Cord;  // Correct case
                    int cityY = settlement.Y_Cord;  // Correct case

                    // Calculate the distance between the mouse click and the city
                    double distance = Math.Sqrt(Math.Pow(mouseX - cityX, 2) + Math.Pow(mouseY - cityY, 2));

                    // Check if the distance is less than or equal to 10 pixels
                    if (distance <= 10)
                    {
                        dnC_scrn.nOut($"Mouse click is in the vicinity of {settlement.LocationName}");
                        hasCity = true;
                    }
                }
                if (hasCity)
                {
                }
                else
                {
                    dnC_scrn.nOut($"Mouse Click Coordinates: X={mouseX}, Y={mouseY}");
                }
            }
        }
    }
}
