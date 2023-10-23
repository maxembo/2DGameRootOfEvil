using UnityEngine;

public class TransportableItem : MonoBehaviour
{
    private bool _isNotTaken = true;

    public bool IsNotTaken
    {
        get => _isNotTaken;
        set => _isNotTaken = value;
    }

    public void PickUp(Transform parent)
    {
        if (IsNotTaken)
        {
            transform.SetParent(parent.transform);

            IsNotTaken = false;
        }
    }

    public void Put()
    {
        if (!IsNotTaken)
        {
            transform.SetParent(null);

            IsNotTaken = true;
        }
    }
}