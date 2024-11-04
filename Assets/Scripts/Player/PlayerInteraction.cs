using System;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    Camera cam;

    [SerializeField] float checkRate = 0.05f;
    float lastCheckTime;
    [SerializeField] float maxCheckDistance = 5f;
    [SerializeField] LayerMask layerMask;

    IInteractable detectedObject;

    public event Action<ItemData> OnDetectItem;


    void Start()
    {
        cam = Camera.main;

        CharacterManager.Instance.Player.inputController.OnInteractEvent += Interact;
    }

    void Update()
    {
        if(Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Check();
        }
        
    }

    void Check()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0f));
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
        {
            if (hit.collider.TryGetComponent(out ItemObject item))
            {
                detectedObject = item;
                OnDetectItem?.Invoke(item.data);
            }
        }
        else
        {
            OnDetectItem?.Invoke(null);
        }
    }

    void Interact()
    {
        if(detectedObject == null)
            return;
        
        detectedObject.Interact();
        detectedObject = null;
    }
}
