using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    [SerializeField] private Character _character;
    [SerializeField] private SpriteRenderer _playerSpriteRenderer;

    private TransportableItem _selectedItem;
    private TransportableItem _holdedItem;
    private int _itemSortingOrder;

    public TransportableItem HoldedItem  => _holdedItem;

    // ReSharper disable Unity.PerformanceAnalysis
    public void PickUpItem()
    {
        if (_selectedItem != null)
        {
            _selectedItem.PickUp(transform);
            _holdedItem = _selectedItem;
            _selectedItem = null;
            SetCharacterItem(_holdedItem);

            var tempSprite = GetFirstChildRenderer(_holdedItem.transform);
            _itemSortingOrder = tempSprite.sortingOrder;
            tempSprite.sortingOrder = _playerSpriteRenderer.sortingOrder + 1;
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    public void PutItem()
    {
        if (_holdedItem != null)
        {
            GetFirstChildRenderer(_holdedItem.transform).sortingOrder = _itemSortingOrder;
            
            _holdedItem.Put();
            _holdedItem = null;
            
            SetCharacterItem(null);
        }
    }

    private SpriteRenderer GetFirstChildRenderer(Transform t) => t.GetChild(0).GetComponent<SpriteRenderer>();
    private void SetCharacterItem(TransportableItem item) => _character.holdedItem = item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TransportableItem item))
            _selectedItem = item;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out TransportableItem item) && item == _selectedItem)
            _selectedItem = null;
    }
}