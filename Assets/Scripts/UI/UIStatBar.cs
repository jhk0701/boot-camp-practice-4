using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIStatBar : MonoBehaviour
{
    [SerializeField] Image bar;
    PlayerStat stat;

    void Start()
    {
        stat = GetComponent<PlayerStat>();
        
        if(stat == null)
            Destroy(gameObject);
        else
            stat.OnValueChanged += ChangeBar;
    }

    void ChangeBar(float val)
    {
        bar.fillAmount = val;
    }
}
