using UnityEngine;

public enum BonusType
{
    Heal,
    Speed,
    Invulnerability
}

public class Bonus : MonoBehaviour
{
    [SerializeField] private BonusType bonusType;
    [SerializeField] private int healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            switch (bonusType)
            {
                case BonusType.Heal:
                    player.Heal(healAmount);
                    break;
                case BonusType.Speed:
                    player.ActivateSpeedBonus();
                    break;
                case BonusType.Invulnerability:
                    player.ActivateInvulnerability();
                    break;
            }

            Destroy(gameObject);
        }
    }

}
