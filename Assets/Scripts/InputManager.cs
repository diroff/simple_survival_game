using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Keys")]
    [SerializeField] private KeyCode _inventory = KeyCode.I;

    [Space]
    [SerializeField] private UIInventory _inventoryActions;

    private void Update()
    {
        if (Input.GetKeyDown(_inventory))
            _inventoryActions.InteractWithInventory();
    }
}