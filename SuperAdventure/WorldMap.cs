using System.Reflection;

namespace SuperAdventure
{
    public partial class WorldMap : Form
    {
        readonly Assembly _thisAssembly = Assembly.GetExecutingAssembly();
        public WorldMap()
        {
            InitializeComponent();
            SetImage(pic_3_2, "Home");
            SetImage(pic_2_2, "TownSquare");
            SetImage(pic_2_1, "Farmhouse");
            SetImage(pic_2_0, "FarmersFields");
            SetImage(pic_1_2, "AlchemistHut");
            SetImage(pic_0_2, "AlchemistsGarden");
            SetImage(pic_2_3, "GuardPost");
            SetImage(pic_2_4, "Bridge");
            SetImage(pic_2_5, "SpiderField");
            // TODO: Add way to show current location and hide unvisited locations
        }

        private void SetImage(PictureBox pictureBox, string imageName)
        {
            pictureBox.Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(imageName));
        }
    }
}
