using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] InventoryUI inventoryUI;

    public int inventorySize = 30;

    private void Start()
    {
        inventoryUI.RefreshInventory(inventorySize); 
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && inventoryUI.isActiveAndEnabled)
        {
            inventoryUI.Hide();
        }
    }

}
