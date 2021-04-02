using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpgMath
{
    public class CharacterSheet : IBattleStats
    {
        public CharacterSheet()
        {

        }

        public String Name { get; set; }

        public Archetype Archetype { get; set; }

        private Equipment mainHand;
        public Equipment MainHand
        {
            get
            {
                return mainHand;
            }
            set
            {
                mainHand = value;
                if (mainHand.TwoHanded)
                {
                    OffHand = null;
                }
            }
        }

        public Equipment Body { get; set; }

        private Equipment offHand;
        public Equipment OffHand
        {
            get
            {
                return offHand;
            }
            set
            {
                offHand = value;
                if (mainHand.TwoHanded)
                {
                    mainHand = null;
                }
            }
        }

        public Equipment Accessory { get; set; }

        private IEnumerable<Equipment> EquippedItems()
        {
            if (MainHand != null)
            {
                yield return MainHand;
            }
            if (Body != null)
            {
                yield return Body;
            }
            if (OffHand != null)
            {
                yield return OffHand;
            }
            if (Accessory != null)
            {
                yield return Accessory;
            }
        }

        public long Hp => Archetype.BaseHp + Archetype.BonusHp;

        public long Mp => Archetype.BaseMp + Archetype.BonusMp;

        public long CurrentHp { get; set; }

        public long CurrentMp { get; set; }

        public long Attack => Archetype.BaseStrength + Archetype.BonusStrength + EquippedItems().Sum(i => i.Strength + i.Attack);

        public long AttackPercent => EquippedItems().Sum(i => i.AttackPercent);

        public long Defense => Archetype.BaseVitality + Archetype.BonusVitality + EquippedItems().Sum(i => i.Vitality + i.Defense);

        public long DefensePercent => EquippedItems().Sum(i => i.DefensePercent);

        public long MagicAttack => Archetype.BaseMagic + Archetype.BonusMagic + EquippedItems().Sum(i => i.Magic + i.MagicAttack);

        public long MagicDefense => Archetype.BaseSpirit + Archetype.BonusSpirit + EquippedItems().Sum(i => i.Spirit + i.MagicDefense);

        public long MagicDefensePercent => EquippedItems().Sum(i => i.MagicDefensePercent);

        public long BaseDexterity => Archetype.BaseDexterity;

        public long Dexterity => Archetype.BaseDexterity + Archetype.BonusDexterity + EquippedItems().Sum(i => i.Dexterity);

        public long Luck => Archetype.BaseLuck + Archetype.BonusLuck + EquippedItems().Sum(i => i.Luck);

        public bool AllowLuckyEvade => true;

        public long Level { get; set; } = 1;

        public long ExtraCritChance => EquippedItems().Sum(i => i.CritChance);
    }
}
