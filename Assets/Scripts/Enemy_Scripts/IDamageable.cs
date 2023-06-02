public interface IDamageable
{
    // Made by Daniel.

    /// <summary>
    /// Can be used to deal damage to objects
    /// </summary>
    /// <param name="damage">Amount of damage</param>
    /// <param name="spellType">Type of damage</param>
    /// <returns>Returns true if object was destroyed</returns>
    public bool TryToDestroyDamageable(int damage, Spell.SpellType? spellType);

    /// <summary>
    /// Can be used to knock back objects.
    /// </summary>
    /// <param name="knockBack">How far the object will be knocked back</param>
    public void KnockBack(float knockBack);
}
