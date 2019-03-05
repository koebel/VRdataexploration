using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CollectionDataHandlingSpace { 

    public class CollectionDataHandling : MonoBehaviour
    {
        // json file
        private string dataFileName = "data.json";

    
        // json syntax
        string jsonOpeningSyntax = "{\n\"collection\": {\n";
        string jsonClosingSyntax = "\n}\n}";


        [System.Serializable]
        public static class CollectionData
        {
            // attributes
            public static List<CollectionItem> allItems;
            public static List<CollectionItem> selectedItems;
            public static Dictionary<string, int> countryStats;
            public static Dictionary<string, int> countryStatsSelection;

            // constructor
            static CollectionData() {
                allItems = new List<CollectionItem>();
                selectedItems = new List<CollectionItem>();
                countryStats = new Dictionary<string, int>();
                countryStatsSelection = new Dictionary<string, int>();
            }

            // methods
            public static void addCollectionItem(CollectionItem item) {
                allItems.Add(item);

                // add country to countryStats
                if (countryStats.ContainsKey(item.country))
                {
                    countryStats[item.country] += 1;
                }
                else {
                    countryStats.Add(item.country, 1);
                }
            }

            public static void updateCountryStatsSelection() {
                countryStatsSelection = new Dictionary<string, int>();

                foreach (CollectionItem item in selectedItems)
                {
                    // add country to countryStats
                    if (countryStatsSelection.ContainsKey(item.country))
                    {
                        countryStatsSelection[item.country] += 1;
                    }
                    else
                    {
                        countryStatsSelection.Add(item.country, 1);
                    }
                }
            }

            public static void displayCollectionItems() {
                foreach (CollectionItem i in allItems) {
                }
            }

            public static CollectionItem getItemByReference(string reference) {
                foreach (CollectionItem i in allItems) {
                    if (i.objectRef == reference) {
                        return i;
                    }
                }
                return null;
            }

            public static int getHighestDictionaryValue() {
                var max = 0;
                foreach (KeyValuePair<string, int> p in countryStats)
                {
                    if (p.Value > max) {
                        max = p.Value;
                    }
                }
                return max;
            }

            public static int getHighestSelectionDictionaryValue()
            {
                var max = 0;
                foreach (KeyValuePair<string, int> p in countryStatsSelection)
                {
                    if (p.Value > max)
                    {
                        max = p.Value;
                    }
                }
                return max;
            }

            public static void selectItemsByRegion(bool america, bool africa, bool europe, bool asia, bool oceania) {
                selectedItems = new List<CollectionItem>();

                foreach (CollectionItem i in allItems)
                {
                    if (america && i.continent == "AMERICA")
                    {
                        selectedItems.Add(i);
                    }
                    else if (africa && i.continent == "AFRICA")
                    {
                        selectedItems.Add(i);
                    }
                    else if (europe && i.continent == "EUROPE")
                    {
                        selectedItems.Add(i);
                    }
                    else if (asia && i.continent == "ASIA")
                    {
                        selectedItems.Add(i);
                    }
                    else if (oceania && i.continent == "OCEANIA")
                    {
                        selectedItems.Add(i);
                    }
                }

                updateCountryStatsSelection();
            }

            public static void deselectAll()
            {
                selectedItems = new List<CollectionItem>();
                updateCountryStatsSelection();
            }

            public static string SaveToJsonString() {
                StringBuilder json = new StringBuilder();

                // open json syntax structure
                json.Append("{");
                json.Append("\n");

                json.Append("\"collection\": {");
                json.Append("\n");
            
                foreach (CollectionItem i in allItems)
                {
                    json.Append("\"object\": ");
                    json.Append(i.SaveToString());
                    json.Append(",");
                    json.Append("\n");
                }
                // remove last Comma and Linebreak ==> last 2 Characters
                json.Remove(json.Length - 2, 2);

                // close json syntax structure
                json.Append("\n");
                json.Append("}");

                json.Append("\n");
                json.Append("}");
            
                return json.ToString();
            }

            public static void CreateCollectionDataFromJsonString(string json) {
                // delete existing items in collection
                allItems = new List<CollectionItem>();

                StringBuilder input = new StringBuilder(json);
                int index = 0;
                string item = "";
                bool lastInstance = false;
                CollectionItem currentItem;

                // start conversion of json string

                // remove linebreaks
                input.Replace("\n", "");

                // remove tabs
                input.Replace("\t", "");

                // remove double spaces
                while (input.ToString().IndexOf("  ") > 0)
                {
                    input = input.Replace("  ", " ");
                }

                // remove empty space after doublepoint and comma and curly braces
                input.Replace("\": \"", "\":\"");
                input.Replace("\", \"", "\",\"");
                input.Replace("{ ", "{");
                input.Replace(" }", "}");

                // remove the opening syntax, seems to be of variing lenght...
                // input.Remove(0, 18);
                index = input.ToString().IndexOf("\"object\": ");
                input.Remove(0, index);

                // remove the closing syntax
                input.Remove(input.Length - 2, 2);

                // split remainder
                while (input.Length > 10 && !lastInstance) {
                    // remove opening, always 10 characters?
                    // input.Remove(0, 10);
                    index = input.ToString().IndexOf("{");
                    input.Remove(0, index);
                    // get index of next opening element
                    index = input.ToString().IndexOf("\"object\": ");
                    // if not instance is found index returns -1
                    if (index != -1) {
                        // -2 is for space and comma
                        item = input.ToString(0, index-2);
                        //Debug.Log(item);
                        currentItem = JsonUtility.FromJson<CollectionItem>(item);
                        addCollectionItem(currentItem);
                        //Debug.Log(currentItem.title);
                        input.Remove(0, index);
                    }
                    else
                    {
                        //Debug.Log("last item");
                        lastInstance = true;
                        item = input.ToString();
                        //Debug.Log(item);
                        currentItem = JsonUtility.FromJson<CollectionItem>(item);
                        addCollectionItem(currentItem);
                        //Debug.Log(currentItem.title);
                    } 
                }
                //Debug.Log("all Items count: " + allItems.Count);
            }
        }


        [System.Serializable]
        public class CollectionItem
        {

            // attributes
            public int objectID;
            public string objectRef;
            public string title;
            public string artist;
            public string geographyDescription;
            public string country;
            public string continent;
            public string[] coordinates;
            public string timeDescription;
            public int timeStart;
            public int timeEnd;
            public string material;
            public string size;
            public string origin;
            public string[] provenance;
            public string description;

            // constructor
            public CollectionItem(string _title)
            {
                title = _title;

                // add more dummy data
                objectID = 721;
                objectRef = "RAF721";
                geographyDescription = "somewhere under the rainbow";
                country = "CH";
                //continent = Continent.EUROPE;
                description = "something";
            }

            public string SaveToString() {
                return JsonUtility.ToJson(this);
            }
            
            public string getTitle() {
                return title;
            }
            
        }

        public enum Continent
        {
            AFRICA, ASIA, AMERICA, EUROPE, OZEANIA, OTHER
        }


        public void Start()
        {
            LoadData();
        }


        public void LoadData() {
            string filePath = Path.Combine(Application.streamingAssetsPath, dataFileName);

            if (File.Exists(filePath)) {

                // read json from file into a string
                string dataAsJson = File.ReadAllText(filePath);
                CollectionData.CreateCollectionDataFromJsonString(dataAsJson);

                // print country Stats
                /*
                foreach (KeyValuePair<string, int> p in CollectionData.countryStats) {
                    Debug.Log(p);
                }
                */
            }
        }
    }
}



