using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettings : MonoBehaviour
{
    [SerializeField] GameObject panel;

    // Start is called before the first frame update
    void Start()
    {
        CharacterManager.Instance.Player.inputController.OnToggleSettingEvent += Toggle;
    }

    public void Toggle()
    {
        panel.SetActive(!panel.activeInHierarchy);
    }

}
