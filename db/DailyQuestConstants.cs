using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace db
{
    public static class DailyQuestConstants
    {
        private static readonly string[] m_dailyQuestDescriptions =
        {
            "This is the first quest of the day! Bring me a {goal} and I will reward you with a fortune token! But if you can complete all the quests, there will be an added bonus for you!",
            "Ahh, you have moved on to the second quest! If you bring me a {goal} I can pull out the magic bits and make another Fortune Token! If you finish my next quest, I will up the ante a bit...",
            "You again! Excellent Since you have been so helpful, I will use some specific parts arround here and make you TWO Fortune Tokens. All I need is a {goal}"
        };

        private static readonly string[] m_dailyQuestImages =
        {
            "http://rotmg.kabamcdn.com/DailyQuest1FortuneToken.png",
            "http://rotmg.kabamcdn.com/DailyQuest1FortuneToken.png",
            "http://rotmg.kabamcdn.com/DailyQuest2FortuneToken.png"
        };

        public static string[] Descriptions { get { return m_dailyQuestDescriptions; } }
        public static string[] Images { get { return m_dailyQuestImages; } }

        public static string GetDescriptionByTier(int tier)
        {
            if (tier == -1) return null;
            if(m_dailyQuestDescriptions.Length - 1 >= tier - 1)
                return m_dailyQuestDescriptions[tier - 1];
            throw new ArgumentException("Invalid tier");
        }

        public static string GetImageByTier(int tier)
        {
            if (tier == -1) return null;
            if (m_dailyQuestImages.Length - 1 >= tier - 1)
                return m_dailyQuestImages[tier - 1];
            throw new ArgumentException("Invalid tier");
        }
    }
}
