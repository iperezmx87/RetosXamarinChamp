using Microsoft.ProjectOxford.Emotion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emotions
{
    public class ServiceEmotions
    {
        static string key = "";
        public static async Task<Dictionary<string, float>> GetEmotions(Stream stream)
        {
            EmotionServiceClient client = new EmotionServiceClient(key);
            var emotions = await client.RecognizeAsync(stream);
            if (emotions == null || emotions.Count() == 0)
            {
                return null;
            }

            return emotions[0].Scores.ToRankedList().ToDictionary(x => x.Key, y => y.Value);
        }
    }
}
