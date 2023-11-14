using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryGrid : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

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