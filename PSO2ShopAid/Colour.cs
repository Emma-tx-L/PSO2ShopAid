using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PSO2ShopAid
{
    public static class ColourPicker
    {
        public static List<string> Colours = new List<string>
            {
                "#55efc4", "#81ecec", "#74b9ff", "#a29bfe", "#00b894", "#00cec9", "#0984e3", "#6c5ce7",
                "#ffeaa7", "#fab1a0", "#ff7675", "#fd79a8", "#fdcb6e", "#e17055", "#d63031", "#e84393",
                "#f6e58d", "#ffbe76", "#ff7979", "#badc58", "#dff9fb", "#f9ca24", "#eb4d4b", "#f0932b",
                "#6ab04c", "#c7ecee", "#7ed6df", "#e056fd", "#686de0", "#22a6b3", "#30336b", "#be2edd",
                "#4834d4", "#cd84f1", "#ffcccc", "#ff4d4d", "#ffaf40", "#fffa65", "#c56cf0", "#ffb8b8",
                "#ff9f1a", "#ff3838", "#fff200", "#32ff7e", "#7efff5", "#18dcff", "#7d5fff", "#3ae374",
                "#67e6dc", "#17c0eb", "#7158e2", "#ae00ff", "#7300ff", "#9cfaff", "#9cd2ff", "#a6ffb8",
                "#deffad", "#fffeb0", "#a2ff80", "#9cb4ff", "#66fffa", "#5490ff", "#8a54ff", "#d7c4ff",
            };

        static ColourPicker()
        {
            string savedColours = Properties.Settings.Default.Colours;
            if (string.IsNullOrEmpty(savedColours))
            {
                return;
            }

            try
            {
                List<string> newColours = JsonConvert.DeserializeObject<List<string>>(savedColours);
                Colours = newColours.Count > Colours.Count ? newColours : Colours.Union(newColours).ToList();
            }
            catch
            {
                return;
            }
            
        }

        public static string GetRandomColour()
        {
            int index = new Random().Next(Colours.Count);
            return Colours[index];
        }

        public static void AddColour(string colour)
        {
            if (!string.IsNullOrEmpty(colour))
            {
                Colours.Add(colour);
            }
        }
    }
}
