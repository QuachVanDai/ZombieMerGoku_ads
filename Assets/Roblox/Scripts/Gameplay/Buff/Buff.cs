using BreakInfinity;
using System;
using UnityEngine;

namespace ExampleProject.Gameplay.Buffs
{
    [Serializable]
    public class Buff
    {
        public BuffType buffType;
        public ModifierType modifierType;
        public float value;
        public object source;
        public double ValueToDouble => value;

        public BigDouble CalculateModifier(BigDouble _baseValue)
        {
            BigDouble buffValue = value;
            switch (modifierType)
            {
                case ModifierType.Flat:
                    return _baseValue + buffValue;
                case ModifierType.Percentage:
                    return _baseValue + (_baseValue * buffValue / 100f);
                case ModifierType.Multiplier:
                    return _baseValue * buffValue;
                default:
                    return _baseValue;
            }
        }
        public float CalculateModifier(float _baseValue)
        {
            switch (modifierType)
            {
                case ModifierType.Flat:
                    return _baseValue + value;
                case ModifierType.Percentage:
                    return _baseValue + (_baseValue * value / 100f);
                case ModifierType.Multiplier:
                    return _baseValue * value;
                default:
                    return _baseValue;
            }
        }

        public string GetDisplayText()
        {
            string _sign = value >= 0 ? "+" : "";
            string _valueText;

            switch (modifierType)
            {
                case ModifierType.Flat:
                    switch (buffType)
                    {
                        case BuffType.CriticalDamage:
                            _valueText = $"{_sign}{value * 100}%";
                            break;
                        case BuffType.CriticalChance:
                            _valueText = $"{_sign}{value}%";
                            break;
                        default:
                            _valueText = $"{_sign}{value}";
                            break;
                    }
                    break;
                case ModifierType.Percentage:
                    _valueText = $"{_sign}{value}%";
                    break;
                case ModifierType.Multiplier:
                    _valueText = $"x{value}";
                    break;
                default:
                    _valueText = value.ToString();
                    break;
            }

            return $"{_valueText}";
        }
    }
}
