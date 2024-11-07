using UnityEngine;

public class EquipWeapon : Equip
{
    [SerializeField] float damage;

    public override void OnHit()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (hit.collider.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(damage);
            }
        }
    }
    
}