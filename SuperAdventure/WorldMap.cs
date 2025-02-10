using System.Numerics;
using System.Reflection;
using Engine;

namespace SuperAdventure
{
    public partial class WorldMap : Form
    {
        readonly Assembly _thisAssembly = Assembly.GetExecutingAssembly();
        private Player _player;
        private int xDiff, yDiff, maxXDiff, maxYDiff;
        private int xCoord, yCoord;
        private int boxesInRow, boxesInColumn, boxWidth, boxHeight;
        private int bufferWidth, bufferHeight;
        private int windowWidth, windowHeight;
        private const int windowBorder = 18;
        private List<int> LocationsAdded { get; set; }

        public WorldMap(Player player)
        {
            _player = player;
            InitializeComponent();
            //HardCodedMethod();
            ProgrammaticMethod();
        }

        private void HardCodedMethod()
        {
            SetImage(pic_3_2, _player.LocationsVisited.Contains(World.LOCATION_ID_HOME) ? "Home" : "FogLocation");
            SetImage(pic_2_2, _player.LocationsVisited.Contains(World.LOCATION_ID_TOWN_SQUARE) ? "TownSquare" : "FogLocation");
            SetImage(pic_1_2, _player.LocationsVisited.Contains(World.LOCATION_ID_ALCHEMIST_HUT) ? "AlchemistHut" : "FogLocation");
            SetImage(pic_0_2, _player.LocationsVisited.Contains(World.LOCATION_ID_ALCHEMISTS_GARDEN) ? "AlchemistsGarden" : "FogLocation");
            SetImage(pic_2_1, _player.LocationsVisited.Contains(World.LOCATION_ID_FARMHOUSE) ? "Farmhouse" : "FogLocation");
            SetImage(pic_2_0, _player.LocationsVisited.Contains(World.LOCATION_ID_FARM_FIELD) ? "FarmersFields" : "FogLocation");
            SetImage(pic_2_3, _player.LocationsVisited.Contains(World.LOCATION_ID_GUARD_POST) ? "GuardPost" : "FogLocation");
            SetImage(pic_2_4, _player.LocationsVisited.Contains(World.LOCATION_ID_BRIDGE) ? "Bridge" : "FogLocation");
            SetImage(pic_2_5, _player.LocationsVisited.Contains(World.LOCATION_ID_SPIDER_FIELD) ? "SpiderField" : "FogLocation");
        }

        private void ProgrammaticMethod()
        {
            LocationsAdded = new List<int>();
            boxesInRow = boxesInColumn = 5;
            maxXDiff = boxesInRow / 2;
            maxYDiff = boxesInColumn / 2;
            xDiff = yDiff = 0;
            boxWidth = boxHeight = 76;
            bufferWidth = bufferHeight = 6;
            windowWidth = boxesInRow * boxWidth + (boxesInRow + 2) * bufferWidth;
            windowHeight = boxesInColumn * boxHeight + (boxesInColumn + 2) * bufferHeight;
            xCoord = windowWidth / 2 - boxWidth / 2;
            yCoord = windowHeight / 2 - boxHeight / 2;
            this.Size = new Size(windowWidth + windowBorder, windowHeight + windowBorder * 2);
            HideHardcodedPictureBoxes();

            AddLocation(_player.CurrentLocation, xCoord, yCoord, xDiff, yDiff);
        }

        private void AddLocation(Location location, int x, int y, int xDifference, int yDifference)
        {
            if (xDifference > maxXDiff || xDifference < maxXDiff * -1) return;
            if (yDifference > maxYDiff || yDifference < maxYDiff * -1) return;
            if (LocationsAdded.Contains(location.ID)) return;
            LocationsAdded.Add(location.ID);

            PictureBox pictureBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            pictureBox.Size = new Size(boxWidth, boxHeight);
            pictureBox.Location = new Point(x, y);
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Name = "pic_" + x + "_" + y;
            Controls.Add(pictureBox);
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();

            SetImage(pictureBox, _player.LocationsVisited.Contains(location.ID) ? location.ImageSource : "FogLocation");

            if (location.LocationToNorth != null)
            {
                DrawLine(x + boxWidth / 2, y, x, y - bufferHeight);
                AddLocation(location.LocationToNorth, x, y - boxHeight - bufferHeight, xDifference, yDifference + 1);
            }
            if (location.LocationToEast != null)
            {
                DrawLine(x + boxWidth, y + boxHeight / 2, x + boxWidth + bufferWidth, y + boxHeight / 2);
                AddLocation(location.LocationToEast, x + boxWidth + bufferWidth, y, xDifference + 1, yDifference);
            }
            if (location.LocationToSouth != null)
            {
                DrawLine(x + boxWidth / 2, y + boxHeight, x, y + boxHeight + bufferHeight);
                AddLocation(location.LocationToSouth, x, y + boxHeight + bufferHeight, xDifference, yDifference - 1);
            }
            if (location.LocationToWest != null)
            {
                DrawLine(x, y + boxHeight / 2, x - bufferWidth, y + boxHeight / 2);
                AddLocation(location.LocationToWest, x - boxWidth - bufferWidth, y, xDifference - 1, yDifference);
            }
        }

        private void SetImage(PictureBox pictureBox, string imageName)
        {
            pictureBox.Image = (Bitmap)(Properties.Resources.ResourceManager.GetObject(imageName));
        }

        private void DrawLine(int x1, int y1, int x2, int y2)
        {
            Point point1 = new Point(x1, y1);
            Point point2 = new Point(x2, y2);
            Pen blackPen = new Pen(Color.Black, 3);

            // TODO: Figure out why this is not working
            Graphics graphics = this.CreateGraphics();
            graphics.DrawLine(blackPen, point1, point2);
        }

        private void HideHardcodedPictureBoxes()
        {
            foreach (var pb in this.Controls.OfType<PictureBox>())
            {
                pb.Visible = false;
            }
        }
    }
}
