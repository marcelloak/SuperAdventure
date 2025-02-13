namespace Engine
{
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Turns { get; set; }
        public int Value { get; set; }

        public Status(int id, string name, string description, int turns = 0, int value = 0)
        {
            ID = id;
            Name = name;
            Description = description;
            Turns = turns;
            Value = value;
        }

        internal Status NewInstanceOfStatus(int value, int turns)
        {
            Status newStatus = new Status(ID, Name, Description);
            newStatus.Value = value;
            newStatus.Turns = turns;

            return newStatus;
        }
    }
}
