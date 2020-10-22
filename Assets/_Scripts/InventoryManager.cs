using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour {

    public TextMeshProUGUI keycardDisplay;
    public string keycardTitle;

    private Dictionary<PickupObjectScript.PickupObjectType, int> items;

    // Use this for initialization
    void Start() {
        InitInventory();
        UpdateGUI(PickupObjectScript.PickupObjectType.KEYCARD);
    }

    private void InitInventory() {
        items = new Dictionary<PickupObjectScript.PickupObjectType, int>();
        InitItem(PickupObjectScript.PickupObjectType.KEYCARD);
    }

    private void InitItem(PickupObjectScript.PickupObjectType type) {
        if (!items.ContainsKey(type)) {
            items.Add(type, 0);
        }
    }

    private void _TEST_ADD_ITEM(PickupObjectScript.PickupObjectType type, int total) {
        items[type] += total;
        UpdateGUI(type);
    }

    // Update is called once per frame
    void Update() {

    }

    public void AddItem(PickupObjectScript.PickupObjectType type) {
        items[type] = items[type] + 1;
        UpdateGUI(type);
    }

    public void RemoveItem(PickupObjectScript.PickupObjectType type) {
        if (HasItem(type)) {
            items[type] = items[type] - 1;
            UpdateGUI(type);
        }
    }

    public bool HasItem(PickupObjectScript.PickupObjectType type) {
        if (items.ContainsKey(type)) {
            return !(items[type] == 0);
        }
        return false;
    }

    private void UpdateGUI(PickupObjectScript.PickupObjectType type) {
        switch (type) {
            case PickupObjectScript.PickupObjectType.KEYCARD:
            keycardDisplay.text = keycardTitle + items[type];
            break;
        }
    }

}
