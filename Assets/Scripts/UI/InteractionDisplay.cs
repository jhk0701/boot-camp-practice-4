using UnityEngine;
using TMPro;

public class InteractionDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI message;

    void Start()
    {
        CharacterManager.Instance.Player.interaction.OnDetectItem += ShowItemData;
        
        title.text = string.Empty;
        message.text = string.Empty;
    }

    void ShowItemData(ItemData data)
    {
        if (data == null)
        {
            title.text = string.Empty;
            message.text = string.Empty;
            return;
        }

        
        title.text = data.title;
        message.text = data.description;

    }

}
