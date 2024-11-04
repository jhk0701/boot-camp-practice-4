using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingObstacle : MonoBehaviour
{
    [SerializeField] float damage = 5f;
    [SerializeField] float frequency = 1f;

    List<IDamagable> damagables = new List<IDamagable>();

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DealDamage", 0f, frequency);   
    }

    void DealDamage()
    {
        for (int i = 0; i < damagables.Count; i++)
        {
            damagables[i].TakeDamage(damage);
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagables.Add(damagable);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IDamagable damagable))
        {
            damagables.Remove(damagable);
        }
    }
}
