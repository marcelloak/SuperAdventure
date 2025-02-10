namespace Engine
{
    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Item ItemRequiredToEnter { get; set; }
        public int LevelRequiredToEnter { get; set; }
        public Quest QuestAvailableHere { get; set; }
        public bool HasAQuest { get { return QuestAvailableHere != null; } }
        public bool QuestHereHasAPrerequisite { get { return QuestAvailableHere.Prerequisite != null; } }
        private readonly SortedList<int, int> _monstersAtLocation = new SortedList<int, int>();
        public bool HasAMonster { get { return _monstersAtLocation.Count > 0; } }
        public Location LocationToNorth { get; set; }
        public Location LocationToEast { get; set; }
        public Location LocationToSouth { get; set; }
        public Location LocationToWest { get; set; }
        public Vendor VendorWorkingHere { get; set; }
        public string ImageSource { get; set; }

        public Location(int id, string name, string description, string imageSource, Item itemRequiredToEnter = null, Quest questAvailableHere = null, Monster monsterLivingHere = null, int levelRequiredToEnter = 1)
        {
            ID = id;
            Name = name;
            Description = description;
            ImageSource = imageSource;
            ItemRequiredToEnter = itemRequiredToEnter;
            QuestAvailableHere = questAvailableHere;
            LevelRequiredToEnter = levelRequiredToEnter;
        }

        public void AddMonster(int monsterID, int percentageOfAppearance)
        {
            if (_monstersAtLocation.ContainsKey(monsterID)) _monstersAtLocation[monsterID] = percentageOfAppearance;
            else _monstersAtLocation.Add(monsterID, percentageOfAppearance);
        }

        public void RemoveMonster(int monsterID)
        {
            _monstersAtLocation.Remove(monsterID);
        }

        public Monster NewInstanceOfMonsterLivingHere()
        {
            if (!HasAMonster) return null;

            int totalPercentages = _monstersAtLocation.Values.Sum();
            int randomNumber = RandomNumberGenerator.NumberBetween(1, totalPercentages);

            int runningTotal = 0;

            foreach (var monsterKeyValuePair in _monstersAtLocation)
            {
                runningTotal += monsterKeyValuePair.Value;
                if (randomNumber <=  runningTotal) return World.MonsterByID(monsterKeyValuePair.Key).NewInstanceOfMonster();
            }

            return World.MonsterByID(_monstersAtLocation.Keys.Last()).NewInstanceOfMonster();
        }
    }
}
