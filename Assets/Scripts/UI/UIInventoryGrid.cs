using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryGrid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;
    [SerializeField] private UIInventorySlot _slotPrefab;

    public void CreateSlot()
    {
        Instantiate(_slotPrefab, transform);
    }

    public void SortInventory()
    {
        StartCoroutine(GridSorting());
    }

    private IEnumerator GridSorting()
    {
        _gridLayoutGroup.enabled = true;

        yield return new WaitForEndOfFrame();

        _gridLayoutGroup.enabled = false;
    }
}