using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody rigidbody;
    [SerializeField] float life = 5f;
    
    
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Invoke("Destory", life);
    }

    public void Fire(Vector3 direction)
    {
        rigidbody.AddForce(direction * 10f, ForceMode.Impulse);
    }

    void Destory()
    {
        Destroy(gameObject);
    }
}
