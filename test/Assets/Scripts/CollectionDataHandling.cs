using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;



public class CollectionDataHandling : MonoBehaviour
{

    [System.Serializable]
    public static class CollectionData
    {
        // attributes
        public static List<CollectionItem> allItems;
        //public static CollectionItem testItem = new CollectionItem("Test item");

        // constructor
        static CollectionData() {
            allItems = new List<CollectionItem>();
        }

        public static void addCollectionItem(CollectionItem item) {
            allItems.Add(item);
        }

        public static void displayCollectionItems() {
            foreach (CollectionItem i in allItems) {
                Debug.Log(i.title);
            }
        }

        public static string SaveToString() {
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

        public static void ReadCollectionDataFromString(string json) {


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
        public Continent continent;
        public float[] coordinates;
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
            continent = Continent.EUROPE;
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


    void Start()
    {
        // Create some collection item objects
        CollectionItem myItem = new CollectionItem("something");
        CollectionItem myItem2 = new CollectionItem("another item");
        CollectionItem myItem3 = new CollectionItem("test");

        // add them to collection data
        CollectionData.addCollectionItem(myItem);
        CollectionData.addCollectionItem(myItem2);
        //CollectionData.addCollectionItem(myItem3);

        // display objects
        CollectionData.displayCollectionItems();
        Debug.Log(CollectionData.SaveToString());

        // test writing and reading json
        // string s = myItem.SaveToString();
        // CollectionItem r = JsonUtility.FromJson<CollectionItem>(s);
        // Debug.Log(r.title);
        // this returns : {"objectID":123,"objectRef":"R123","title":"something","artist":"","geographyDescription":"somewhere under the rainbow","country":"CH","continent":3,"coordinates":[],"timeDescription":"","timeStart":0,"timeEnd":0,"material":"","size":"","origin":"","provenance":[],"description":"something"}
        //Debug.Log(myItem.getTitle());
    }
}



