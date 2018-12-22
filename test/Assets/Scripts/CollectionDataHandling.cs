using UnityEngine;
using System.Collections;
using System.Collections.Generic;




    [System.Serializable]
    public class CollectionDataHandling : MonoBehaviour
    {

        
        public static class CollectionData
        {

            public static List<CollectionItem> items = new List<CollectionItem>();

            public static CollectionItem testItem = new CollectionItem("Test item");

            /*
            public static CollectionData() {
                public CollectionItem test = new CollectionItem("other item");
                items.add(test);
                return items;
            }
            */

            /*
            public static CollectionData CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<CollectionData>(jsonString);
            }
            */
        }
   

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

            
            public string getTitle() {
                return title;
            }
            

        }

        public enum Continent
        {
            AFRICA, ASIA, AMERICA, EUROPE, OZEANIA, OTHER
        }


        // Create an instance of a collection item object
        public CollectionItem myItem = new CollectionItem("something");


        void Start()
        {
            //Debug.Log(myItem.getTitle());
        }
    }



