using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour {

    public TextMeshProUGUI textDisplay;
    public int itemCounter;
    public string itemTitle;

	// Use this for initialization
	void Start () {
        textDisplay.text = itemTitle + itemCounter;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItem(GameObject obj) {
        itemCounter++;
        textDisplay.text = itemTitle + itemCounter;
    }

    public void RemoveItem() {
        itemCounter--;
        textDisplay.text = itemTitle + itemCounter;
    }
}
