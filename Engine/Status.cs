namespace Engine
{
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Value { get; set; }
        public int Turns { get; set; }
        public int ChanceToActivate { get; set; }
        public int ChanceToCure { get; set; }

        public Status(int id, string name, string description, int value = 0, int turns = 1, int chanceToActivate = 100, int chanceToCure = 0)
        {
            ID = id;
            Name = name;
            Description = description;
            Value = value;
            Turns = turns;
            ChanceToActivate = chanceToActivate;
            ChanceToCure = chanceToCure;
        }

        internal Status NewInstanceOfStatus(int value = 0, int turns = 1, int chanceToActivate = 100, int chanceToCure = 0)
        {
            Status newStatus = new Status(ID, Name, Description);
            newStatus.Value = value;
            newStatus.Turns = turns;
            newStatus.ChanceToActivate = chanceToActivate;
            newStatus.ChanceToCure = chanceToCure;

            return newStatus;
        }
    }
}
