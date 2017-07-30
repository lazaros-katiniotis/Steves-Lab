using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour {

    public TextMeshProUGUI keycardDisplay;
    public string keycardTitle;

    public Dictionary<PickupObjectScript.PickupObjectType, int> items;

    // Use this for initialization
    void Start() {
        items = new Dictionary<PickupObjectScript.PickupObjectType, int>();
        UpdateGUI(PickupObjectScript.PickupObjectType.KEYCARD);
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddItem(PickupObjectScript.PickupObjectType type) {
        Debug.Log("Adding item...");
        if (!items.ContainsKey(type)) {
            items.Add(type, 1);
            Debug.Log("here? why?");
        } else {
            items[type] = items[type] + 1;
        }
        UpdateGUI(type);
    }

    public void RemoveItem(PickupObjectScript.PickupObjectType type) {
        if (HasItem(type)) {
            items[type] = items[type] - 1;
        }
        UpdateGUI(type);
    }

    public bool HasItem(PickupObjectScript.PickupObjectType type) {
        if (items.ContainsKey(type)) {
            return !(items[type] == 0);
        }
        return false;
    }

    private void UpdateGUI(PickupObjectScript.PickupObjectType type) {
        if (!items.ContainsKey(type)) {
            Debug.Log("Does not contain type, adding...");
            items.Add(type, 0);
        }
        switch (type) {
            case PickupObjectScript.PickupObjectType.KEYCARD:
            keycardDisplay.text = keycardTitle + items[type];
            break;
        }
    }

}
