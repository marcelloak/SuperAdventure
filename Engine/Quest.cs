namespace Engine
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardGold { get; set; }
        public int RewardExperiencePoints { get; set; }
        public Item RewardItem { get; set; }
        public List<QuestCompletionItem> QuestCompletionItems { get; set; }
        public bool IsRepeatable { get; set; }
        public Quest Prerequisite { get; set; }

        public Quest(int id, string name, string description, int rewardGold, int rewardExperiencePoints, bool isRepeatable = false, Quest prerequisite = null)
        {
            ID = id;
            Name = name;
            Description = description;
            RewardGold = rewardGold;
            RewardExperiencePoints = rewardExperiencePoints;
            QuestCompletionItems = new List<QuestCompletionItem>();
            IsRepeatable = isRepeatable;
            Prerequisite = prerequisite;
        }
    }
}
