using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerStatus status;    

    [SerializeField] Projectile magicBall;
    [SerializeField] float manaUsageOfMagicBall = 10f;

    void Start()
    {
        status = CharacterManager.Instance.Player.status;

        PlayerInputController inputController = CharacterManager.Instance.Player.inputController;
        inputController.OnMagicEvent += MagicAttack;
    }


    void MagicAttack()
    {
        if (status.UseMana(manaUsageOfMagicBall))
        {
            Projectile p = Instantiate(magicBall, transform.position + Vector3.up + transform.forward * 0.5f, Quaternion.identity);
            p.Fire(transform.forward);
        }
    }
}
