public interface IGuaranteedDamage
{
    // Made by Daniel.

    /// <summary>
    /// Can be used to deal guaranteed damage.
    /// </summary>
    /// <param name="damage">How much damage will be dealt</param>
    /// <param name="spellType">Type of damage</param>
    /// <returns>Returns true if object was killed</returns>
    public bool GuaranteedDamage(int damage, Spell.SpellType? spellType);
}
