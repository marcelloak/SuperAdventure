using System.Reflection;
using Engine;

namespace SuperAdventure
{
    public partial class WorldMap : Form
    {
        readonly Assembly _thisAssembly = Assembly.GetExecutingAssembly();
        public WorldMap(Player player)
        {
            InitializeComponent();
            SetImage(pic_3_2, player.LocationsVisited.Contains(World.LOCATION_ID_HOME) ? "Home" : "FogLocation");
            SetImage(pic_2_2, player.LocationsVisited.Contains(World.LOCATION_ID_TOWN_SQUARE) ? "TownSquare" : "FogLocation");
            SetImage(pic_1_2, player.LocationsVisited.Contains(World.LOCATION_ID_ALCHEMIST_HUT) ? "AlchemistHut" : "FogLocation");
            SetImage(pic_0_2, player.LocationsVisited.Contains(World.LOCATION_ID_ALCHEMISTS_GARDEN) ? "AlchemistsGarden" : "FogLocation");
            SetImage(pic_2_1, player.LocationsVisited.Contains(World.LOCATION_ID_FARMHOUSE) ? "Farmhouse" : "FogLocation");
            SetImage(pic_2_0, player.LocationsVisited.Contains(World.LOCATION_ID_FARM_FIELD) ? "FarmersFields" : "FogLocation");
            SetImage(pic_2_3, player.LocationsVisited.Contains(World.LOCATION_ID_GUARD_POST) ? "GuardPost" : "FogLocation");
            SetImage(pic_2_4, player.LocationsVisited.Contains(World.LOCATION_ID_BRIDGE) ? "Bridge" : "FogLocation");
            SetImage(pic_2_5, player.LocationsVisited.Contains(World.LOCATION_ID_SPIDER_FIELD) ? "SpiderField" : "FogLocation");
        }

        private void SetImage(PictureBox pictureBox, string imageName)
        {
            pictureBox.Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(imageName));
        }
    }
}
