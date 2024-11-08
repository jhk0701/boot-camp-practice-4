using UnityEngine;
using UnityEngine.UI;


public class UIStatBar : MonoBehaviour
{
    [SerializeField] Image bar;
    [SerializeField] StatType type;

    void Start()
    {
        PlayerCondition condition = CharacterManager.Instance.Player.condition;
        
        if (condition.stats.TryGetValue(type, out Stat stat))
            stat.OnValueChanged += ChangeBar;
        else
            Destroy(gameObject);
            
    }

    void ChangeBar(float val)
    {
        bar.fillAmount = val;
    }
}
