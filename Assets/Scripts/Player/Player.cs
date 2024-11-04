using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    [HideInInspector] public PlayerInputController inputController;
    [HideInInspector] public PlayerStatus status;
    [HideInInspector] public PlayerInteraction interaction;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Equipment equip;


    public Action<ItemData> AddItem;
    

    void Awake()
    {
        CharacterManager.Instance.Player = this;

        inputController = GetComponent<PlayerInputController>();
        status = GetComponent<PlayerStatus>();
        interaction = GetComponent<PlayerInteraction>();

        rigidBody = GetComponent<Rigidbody>();
    }

}
