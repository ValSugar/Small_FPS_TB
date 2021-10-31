namespace Entities
{
    public interface ITakeDamage
    {
        void TakeDamage(float damage, DamageType damageType);
    }

    public enum DamageType
    {
        Common,
        Explosive
    }
}
