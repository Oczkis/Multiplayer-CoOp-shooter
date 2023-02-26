using UnityEngine;

public interface IDamageable
{
    public GameObject GetHitFX()
    {
        return null;
    }

    public void ReceiveDamage(int damage, int playerID)
    {

    }

    public Helpers.CharacterType GetCharacterType()
    {
        return Helpers.CharacterType.Foe;
    }
}
