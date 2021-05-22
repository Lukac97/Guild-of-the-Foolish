using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Combat Spell", menuName = "Spells/Combat Spell")]
public class CombatSpell : Spell
{
    public CombatSpellType combatSpellType;
    [Header("Intensity")]
    public float physicalIntensityRatio;
    public float magicalIntensityRatio;

    public float UseSpell(CombatHandler caster, CombatHandler target)
    {
        if(combatSpellType == CombatSpellType.HARMFUL)
        {
            float dmg = caster.combatStats.physicalDamage * physicalIntensityRatio
                + magicalIntensityRatio * magicalIntensityRatio; 
            float damageDealt = target.CalculateDamageReceived(dmg);
            return damageDealt;

        }
        else if(combatSpellType == CombatSpellType.BENEFICIAL)
        {

        }
        return 0.0f;
    }
}
