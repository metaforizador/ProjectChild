using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarCanvas : MonoBehaviour {

    public GameObject buttonLayout;
    public Sprite defaultImage;
    public Color defaultColor;

    public int hotbarButtonAmount { get; private set; }

    private GameObject[] hotbarButtons;
    private ConsumableSO[] hotbarItems;

    public GameObject changingItemsPanel;
    private ConsumableSO incomingItem;

    void Awake() {
        changingItemsPanel.SetActive(false);
    }

    void Start() {
        hotbarButtonAmount = buttonLayout.transform.childCount;
        hotbarButtons = new GameObject[hotbarButtonAmount];
        hotbarItems = new ConsumableSO[hotbarButtonAmount];

        for (int i = 0; i < hotbarButtonAmount; i++) {
            GameObject btn = buttonLayout.transform.GetChild(i).gameObject;
            hotbarButtons[i] = btn;

            // Set onClickListener for button
            int index = i;
            btn.GetComponent<Button>().onClick.AddListener(() => HotbarButtonClicked(index));
        }

        RefreshHotbarImages();
    }

    public void HotbarButtonClicked(int index) {
        GameMaster gm = GameMaster.Instance;
        ConsumableSO clickedItem = hotbarItems[index];
        // Play sound
        CanvasSounds sounds = CanvasMaster.Instance.canvasSounds;

        if (gm.gameState.Equals(GameState.Hotbar)) {
            // Swap hotbar button item
            SwapHotbarItem(index);
            sounds.PlaySound(sounds.BUTTON_SELECT);
        } else if (gm.gameState.Equals(GameState.Menu)) {
            // Show item info in InventoryCanvas
            if (clickedItem != null)
                CanvasMaster.Instance.inventoryCanvas.GetComponent<InventoryCanvas>().ShowHotbarItemInfo(clickedItem);
        } else if (gm.gameState.Equals(GameState.Movement)) {
            // Use item
            if (clickedItem != null) {
                Inventory.Instance.UseConsumable(clickedItem);
                sounds.PlaySound(sounds.BUTTON_SELECT);
            }
        }
    }

    private void SwapHotbarItem(int index) {
        hotbarItems[index] = incomingItem;
        RefreshHotbarImages();
        DisableIncomingItem();
    }

    public void RefreshHotbarImages() {
        for (int i = 0; i < hotbarItems.Length; i++) {
            Image img = hotbarButtons[i].GetComponent<Image>();
            ConsumableSO consumable = hotbarItems[i];

            if (consumable != null) {
                // If all selected consumables are used, set value to null
                if (consumable.quantity <= 0) {
                    consumable = null;
                } else {
                    img.sprite = consumable.sprite;
                    img.color = Color.white;
                }
            }
            
            if (consumable == null) {
                img.sprite = defaultImage;
                img.color = defaultColor;
            }

            // Type changes to Simple for some reason when changing sprites
            // If type stays Simple, the default image changes shape, which we don't want
            img.type = Image.Type.Sliced;
        }
    }

    public void SetIncomingItem(ConsumableSO consumable) {
        GameMaster.Instance.SetState(GameState.Hotbar);
        changingItemsPanel.SetActive(true);
        incomingItem = consumable;
    }

    public void DisableIncomingItem() {
        GameMaster.Instance.SetState(GameState.Menu);
        changingItemsPanel.SetActive(false);
        incomingItem = null;
    }

    public void LoadHotbar(Save save) {
        // Convert serialized consumables to ConsumableSO
        SOCreator creator = SOCreator.Instance;
        for (int i = 0; i < hotbarButtonAmount; i++) {
            SerializablePickableSO serialized = save.hotbarConsumables[i];
            ConsumableSO con = null;
            if (serialized != null) {
                con = creator.CreateConsumable(serialized);
            }
            hotbarItems[i] = con;
        }

        RefreshHotbarImages();
    }

    public void SaveHotbar(Save save) {
        // Convert hotbar consumables to a serializable format
        SerializablePickableSO[] serializableConsumables = new SerializablePickableSO[hotbarButtonAmount];
        for (int i = 0; i < hotbarButtonAmount; i++) {
            ConsumableSO con = hotbarItems[i];
            if (con != null) {
                serializableConsumables[i] = new SerializablePickableSO(con);
            } else {
                serializableConsumables[i] = null;
            }
            
        }
        save.hotbarConsumables = serializableConsumables;
    }
}
