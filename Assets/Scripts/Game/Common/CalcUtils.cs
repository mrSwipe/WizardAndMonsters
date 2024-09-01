using System;
using Spells;
using UnityEngine;

namespace Common
{
    public static class CalcUtils
    {
        public static int PercentRange(this int input)
        {
            return Math.Max(0, Math.Min(100, input));
        }
        
        public static int MinMaxRange(this int input, int max, int min = 0)
        {
            if (min > max)
            {
                throw new ArgumentException("Error. The min value is greater than the max value");
            }
            return Math.Max(min, Math.Min(max, input));
        }
        
        public static int CalcHealthOnDamage(this int currentHealth, float protection, int damage, int max = 100, int min = 0)
        {
            var damageWithProtection = protection > 0 ? damage * protection : damage;
            var resultHealth = currentHealth - Mathf.RoundToInt(damageWithProtection);
            return resultHealth.MinMaxRange(max, min);
        }
        
        public static int CalcHealthOnHeal(this int currentHealth, int heal, int max = 100, int min = 0)
        {
            var resultHealth = currentHealth + heal;
            return resultHealth.MinMaxRange(max, min);
        }
        
        public static SpellType GetNextSpellType(this SpellType currentSpellType)
        {
            if (currentSpellType == SpellType.Blue)
            {
                return SpellType.Red;
            }

            var v = (int) currentSpellType;
            return (SpellType) v + 1;
        }
        
        public static SpellType GetPrevSpellType(this SpellType currentSpellType)
        {
            if (currentSpellType == SpellType.Red)
            {
                return SpellType.Blue;
            }

            var v = (int) currentSpellType;
            return (SpellType) v - 1;
        }
    }
}