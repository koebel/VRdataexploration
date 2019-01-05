using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;


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
        
        // constructor
        static CollectionData() {
            allItems = new List<CollectionItem>();
        }

        public static void addCollectionItem(CollectionItem item) {
            allItems.Add(item);
        }

        public static void displayCollectionItems() {
            foreach (CollectionItem i in allItems) {
                //Debug.Log(i.title);
            }
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
            // remove the opening syntax
            input.Remove(0, 18);

            // remove the closing syntax
            input.Remove(input.Length - 4, 4);

            // split remainder
            while (input.Length > 10 && !lastInstance) { 
                // remove opening, 10 char
                input.Remove(0, 10);
                // get index of next opening element
                index = input.ToString().IndexOf("\"object\": ");
                // if not instance is found index returns -1
                if (index != -1) {
                    // -2 is for space and comma
                    item = input.ToString(0, index - 2);
                    currentItem = JsonUtility.FromJson<CollectionItem>(item);
                    addCollectionItem(currentItem);
                    //Debug.Log(currentItem.title);
                    input.Remove(0, index);
                }
                else
                {
                    lastInstance = true;
                    item = input.ToString();
                    currentItem = JsonUtility.FromJson<CollectionItem>(item);
                    // TODO: fix this, make sure that all types are set correctly
                    addCollectionItem(currentItem);
                    //Debug.Log(currentItem.title);
                } 
            }
        }

        /*
        public static CollectionData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<CollectionData>(jsonString);
        }
        */
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
            objectID = 123;
            objectRef = "R123";
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

        // Create some collection item objects
        CollectionItem myItem = new CollectionItem("something");
        CollectionItem myItem2 = new CollectionItem("another item");
        CollectionItem myItem3 = new CollectionItem("test");

        // add them to collection data
        //CollectionData.addCollectionItem(myItem);
        //CollectionData.addCollectionItem(myItem2);
        //CollectionData.addCollectionItem(myItem3);

        // display objects
        CollectionData.displayCollectionItems();
        string x = CollectionData.SaveToJsonString();
        Debug.Log(x);
        CollectionData.CreateCollectionDataFromJsonString(x);

        // test writing and reading json
        // string s = myItem.SaveToString();
        // CollectionItem r = JsonUtility.FromJson<CollectionItem>(s);
        // Debug.Log(r.title);
        // this returns : {"objectID":123,"objectRef":"R123","title":"something","artist":"","geographyDescription":"somewhere under the rainbow","country":"CH","continent":3,"coordinates":[],"timeDescription":"","timeStart":0,"timeEnd":0,"material":"","size":"","origin":"","provenance":[],"description":"something"}
        //Debug.Log(myItem.getTitle());
    }


    public void LoadData() {
        string filePath = Path.Combine(Application.streamingAssetsPath, dataFileName);

        if (File.Exists(filePath)) {
            // read json from file into a string
            string dataAsJson = File.ReadAllText(filePath);
            dataAsJson.Trim();
            Debug.Log("trimmed data");
            Debug.Log(dataAsJson);
            // TODO: Trimming is not yet working!!!
            CollectionData.CreateCollectionDataFromJsonString(dataAsJson);
        }

    }
}



