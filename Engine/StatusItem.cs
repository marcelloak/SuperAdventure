namespace Engine
{
    public class StatusItem : UsableItem
    {
        public Status StatusApplied { get; set; }

        public StatusItem(int id, string name, string namePlural, int price, int minimumLevel = 1, Status statusApplied = null) : base(id, name, namePlural, price, minimumLevel)
        {
            StatusApplied = statusApplied;
        }
    }
}
