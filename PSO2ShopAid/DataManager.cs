using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace PSO2ShopAid
{
    public static class DataManager
    {
        private static string savePath = Path.Combine(Directory.GetCurrentDirectory(), "savedItems.txt");

        public static void Save()
        {
            SaveItems();
            SaveColours();
        }

        public static void SaveColours()
        {
            try
            {
                string data = JsonConvert.SerializeObject(ColourPicker.Colours);
                Properties.Settings.Default.Colours = data;
                Properties.Settings.Default.Save();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Properties.Settings.Default.Save();
            }
        }
        public static void SaveItems()
        {
            try
            {
                ObservableCollection<Item> items = MainWindow.Shop.AllItems;
                string data = JsonConvert.SerializeObject(items);
                File.WriteAllText(savePath, data);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Unable to save data.");
                Console.WriteLine(e);
            }
        }

        public static void SaveItems(ObservableCollection<Item> items)
        {
            try
            {
                string data = JsonConvert.SerializeObject(items);
                File.WriteAllText(savePath, data);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Unable to save data.");
                Console.WriteLine(e);
            }
        }

        public static ObservableCollection<Item> LoadItems()
        {
            try
            {
                string data = File.ReadAllText(savePath);
                return JsonConvert.DeserializeObject<ObservableCollection<Item>>(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("[ERROR] Unable to load save data");
                Console.WriteLine(e);
                return new ObservableCollection<Item>();
            }
        }
    }
}
