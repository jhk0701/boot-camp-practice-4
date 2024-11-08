using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerCondition condition;    

    [SerializeField] Projectile magicBall;
    [SerializeField] float manaUsageOfMagicBall = 10f;

    void Start()
    {
        condition = CharacterManager.Instance.Player.condition;

        PlayerInputController inputController = CharacterManager.Instance.Player.inputController;
        inputController.OnMagicEvent += MagicAttack;
    }


    void MagicAttack()
    {
        if (condition.UseMana(manaUsageOfMagicBall))
        {
            Projectile p = Instantiate(magicBall, transform.position + Vector3.up + transform.forward * 0.5f, Quaternion.identity);
            p.Fire(transform.forward);
        }
    }
}
