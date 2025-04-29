using UnityEngine;

public class PlayerAnimTrigger : MonoBehaviour
{
    public Soward weapon;


    public void OnSwordAttack()
    {
        weapon.TryAttack();
    }
}
