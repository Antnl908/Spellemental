public interface IMagicEffect
{
    // Made by Daniel.

    /// <summary>
    /// Can be used to apply a magic effect.
    /// </summary>
    /// <param name="effectDamage">Damage of the effect</param>
    /// <param name="effectBuildUp">Effect build up</param>
    /// <param name="spellType">Type of damage</param>
    public void ApplyMagicEffect(int effectDamage, int effectBuildUp, Spell.SpellType spellType);

    /// <summary>
    /// Can be used to deal fir damage.
    /// </summary>
    /// <param name="fireDamage"></param>
    /// <param name="effectBuildUp"></param>
    public void FireEffect(int fireDamage, int effectBuildUp);

    /// <summary>
    /// Can be used to deal ice damage
    /// </summary>
    /// <param name="iceDamage"></param>
    /// <param name="effectBuildUp"></param>
    public void FrostEffect(int iceDamage, int effectBuildUp);

    /// <summary>
    /// Can be used to deal lightning damage.
    /// </summary>
    /// <param name="lightningDamage"></param>
    /// <param name="effectBuildUp"></param>
    public void LightningEffect(int lightningDamage, int effectBuildUp);

    /// <summary>
    /// Can be used to deal earth damage.
    /// </summary>
    /// <param name="earthDamage"></param>
    /// <param name="effectBuildUp"></param>
    public void EarthEffect(int earthDamage, int effectBuildUp);

    /// <summary>
    /// Can be used to deal wind damage.
    /// </summary>
    /// <param name="windDamage"></param>
    /// <param name="effectBuildUp"></param>
    public void WindEffect(int windDamage, int effectBuildUp);

    /// <summary>
    /// Can be used to apply a slow down effect.
    /// </summary>
    public void SlowDownEffect();
}
