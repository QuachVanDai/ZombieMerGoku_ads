using BreakInfinity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExampleProject.Gameplay.Buffs
{
    public class BuffManager : MonoBehaviour
    {
        [SerializeField] List<Buff> activeBuffs = new List<Buff>();
        public void AddBuff(Buff _buff)
        {
            activeBuffs.Add(_buff);
        }

        public void AddBuffs(List<Buff> _buffs)
        {
            if (_buffs == null || _buffs.Count == 0)
                return;

            foreach (var _buff in _buffs)
            {
                AddBuff(_buff);
            }
        }

        public void RemoveBuff(Buff _buff)
        {
            activeBuffs.Remove(_buff);
        }
        public void RemoveBuffs(List<Buff> _buffs)
        {
            if (_buffs == null || _buffs.Count == 0)
                return;
            foreach (var _buff in _buffs)
            {
                RemoveBuff(_buff);
            }
        }

        public void ClearAllBuffs()
        {
            activeBuffs.Clear();
        }

        public void ClearBuffsByType(BuffType _buffType)
        {
            activeBuffs.RemoveAll(_b => _b.buffType == _buffType);
        }

        public List<Buff> GetActiveBuffs()
        {
            return new List<Buff>(activeBuffs);
        }

        public List<Buff> GetActiveBuffsByType(BuffType _buffType)
        {
            return activeBuffs.Where(_b => _b.buffType == _buffType).ToList();
        }

        public bool HasBuff(BuffType _buffType)
        {
            return activeBuffs.Any(_b => _b.buffType == _buffType);
        }
    }
}
