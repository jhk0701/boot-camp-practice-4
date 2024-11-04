using UnityEngine;

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    public void Interact()
    {
        CharacterManager.Instance.Player.AddItem?.Invoke(data);

        Destroy(gameObject);
    }
}
