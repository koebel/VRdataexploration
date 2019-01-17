using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CollectionItem : MonoBehaviour {

    // variables
    public string reference;
    private CollectionItem collectionItem;
    private GameObject collectionItemObject;
    private GameObject picture;
    private GameObject ui;
    private GameObject background;

    private Material pictureMaterial;
    private Material backgroundMaterial;

    private float backgroundOpacity = 0.8f;

    private Text itemText;
    private string demoText = "Demo Text... Lorem Ipsum bla bla bla... ";
    private string materialPath = "Objects/Materials/";
    private string picturePath;


    // Use this for initialization
    void Start () {

        // define materials
        backgroundMaterial = (Material)Resources.Load("Objects/Materials/BackgroundObjects", typeof(Material));

        picturePath = string.Concat(materialPath, reference);
        // TODO: test if this is a valid material?
        pictureMaterial = (Material)Resources.Load(picturePath, typeof(Material));

        // set references to child objects
        picture = this.transform.Find("picture").gameObject;
        ui = this.transform.Find("UI").gameObject;
        background = this.transform.Find("background").gameObject;

        // set materials
        // background material & opacity
        background.GetComponent<MeshRenderer>().material = backgroundMaterial;
        // reduce opacity, unity doesn't allow to directly access the alpha value because not all materials have an alpha
        var col = background.GetComponent<Renderer>().material.color;
        col.a = backgroundOpacity;
        background.GetComponent<Renderer>().material.color = col;

        // set picture
        picture.GetComponent<MeshRenderer>().material = pictureMaterial;

        // set text
        itemText = ui.GetComponentInChildren<Text>();
        itemText.GetComponent<Text>().supportRichText = true;
        itemText.text = demoText;
        //itemText.text = demoText.Replace("<br>", "\n");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
