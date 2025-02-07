namespace Engine
{
    public class QuestCompletionItem
    {
        public Item Details { get; set; }
        public int Quantity { get; set; }
        public string Description { get { return Quantity > 1 ? Details.NamePlural : Details.Name; } }

        public QuestCompletionItem(Item details, int quantity)
        {
            Details = details;
            Quantity = quantity;
        }
    }
}
