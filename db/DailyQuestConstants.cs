using System;

namespace db
{
    public static class DailyQuestConstants
    {
        public static int QuestsPerDay => Descriptions.Length;

        public static string[] Rewards { get; } =
        {
            "FortuneToken:1",
            "FortuneToken:1",
            "FortuneToken:2",
            "FortuneToken:2"
        };

        public static string[] Descriptions { get; } =
        {
            "This is the first quest of the day! Bring me a {goal} and I will reward you with a fortune token! But if you can complete all the quests, there will be an added bonus for you!",
            "Ahh, you have moved on to the second quest! If you bring me a {goal} I can pull out the magic bits and make another Fortune Token! If you finish my next quest, I will up the ante a bit...",
            "You again! Excellent Since you have been so helpful, I will use some specific parts arround here and make you TWO Fortune Tokens. All I need is a {goal}",
            "You again! Excellent Since you have been so helpful, I will use some specific parts arround here and make you TWO Fortune Tokens. All I need is a {goal}"
        };

        public static string[] Images { get; } =
        {
            "http://rotmg.kabamcdn.com/DailyQuest1FortuneToken.png",
            "http://rotmg.kabamcdn.com/DailyQuest1FortuneToken.png",
            "http://rotmg.kabamcdn.com/DailyQuest2FortuneToken.png",
            "http://rotmg.kabamcdn.com/DailyQuest2FortuneToken.png"
        };

        public static string GetDescriptionByTier(int tier)
        {
            if (tier == -1) return null;
            if(Descriptions.Length - 1 >= tier - 1)
                return Descriptions[tier - 1];
            throw new ArgumentException("Invalid tier");
        }

        public static string GetImageByTier(int tier)
        {
            if (tier == -1) return null;
            if (Images.Length - 1 >= tier - 1)
                return Images[tier - 1];
            throw new ArgumentException("Invalid tier");
        }
    }
}
