using Engine.Locations;

namespace Book12
{
    public partial class MainScreen : Form
    {
        DnC_Screen dnC_scrn;

        public MainScreen()
        {
            InitializeComponent();
            pictureBox1.Size = MapRenderer.size;
            dnC_scrn = new DnC_Screen(this);
            dnC_scrn.Show();

        }

        public void ChangePictureBoxImage(Image newImage)
        {
            pictureBox1.Image = newImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dnC_scrn.Show();
        }
    }
}