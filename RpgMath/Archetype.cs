using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    public class Archetype
    {
        public long BaseHp { get; set; }

        public long BonusHp { get; set; }

        public long BaseMp { get; set; }

        public long BonusMp { get; set; }

        public long BaseStrength { get; set; }

        public long BonusStrength { get; set; }

        public long BaseVitality { get; set; }

        public long BonusVitality { get; set; }

        public long BaseMagic { get; set; }

        public long BonusMagic { get; set; }

        public long BaseSpirit { get; set; }

        public long BonusSpirit { get; set; }

        /// <summary>
        /// This is used to determine base battle timing. Raise it with the dex equation but don't ever modify it by anything.
        /// </summary>
        public long BaseDexterity { get; set; }

        public long BonusDexterity { get; set; }

        public long BaseLuck { get; set; }

        public long BonusLuck { get; set; }

        public int HpGrade { get; set; }

        public int MpGrade { get; set; }

        public int StrengthGrade { get; set; }

        public int VitalityGrade { get; set; }

        public int MagicGrade { get; set; }

        public int SpiritGrade { get; set; }

        public int DexterityGrade { get; set; }

        public int LuckGrade { get; set; }

        public long Xp02to11 { get; set; }

        public long Xp12to21 { get; set; }

        public long Xp22to31 { get; set; }

        public long Xp32to41 { get; set; }

        public long Xp42to51 { get; set; }

        public long Xp52to61 { get; set; }

        public long Xp62to81 { get; set; }

        public long Xp82to99 { get; set; }

        public static Archetype CreateHero()
        {
            return new Archetype()
            {
                BaseHp        = 314,
                BaseMp        = 54,
                BaseStrength  = 20,
                BaseVitality  = 16,
                BaseMagic     = 19,
                BaseSpirit    = 17,
                BaseDexterity = 6,
                BaseLuck      = 14,

                HpGrade        = 0,
                MpGrade        = 0,
                LuckGrade      = 0,
                StrengthGrade  = 1,
                VitalityGrade  = 6,
                MagicGrade     = 3,
                SpiritGrade    = 4,
                DexterityGrade = 26,

                Xp02to11 = 68,
                Xp12to21 = 71,
                Xp22to31 = 73,
                Xp32to41 = 74,
                Xp42to51 = 74,
                Xp52to61 = 74,
                Xp62to81 = 75,
                Xp82to99 = 77,
            };
        }

        public static Archetype CreateTank()
        {
            return new Archetype()
            {
                BaseHp        = 222,
                BaseMp        = 15,
                BaseStrength  = 15,
                BaseVitality  = 13,
                BaseMagic     = 11,
                BaseSpirit    = 9,
                BaseDexterity = 5,
                BaseLuck      = 13,

                HpGrade        = 1,
                MpGrade        = 1,
                LuckGrade      = 1,
                StrengthGrade  = 5,
                VitalityGrade  = 2,
                MagicGrade     = 18,
                SpiritGrade    = 14,
                DexterityGrade = 29,

                Xp02to11 = 70,
                Xp12to21 = 73,
                Xp22to31 = 75,
                Xp32to41 = 76,
                Xp42to51 = 77,
                Xp52to61 = 77,
                Xp62to81 = 77,
                Xp82to99 = 77,
            };
        }

        public static Archetype CreateBrawler()
        {
            return new Archetype()
            {
                BaseHp        = 219,
                BaseMp        = 16,
                BaseStrength  = 11,
                BaseVitality  = 11,
                BaseMagic     = 11,
                BaseSpirit    = 10,
                BaseDexterity = 7,
                BaseLuck      = 14,

                HpGrade        = 2,
                MpGrade        = 2,
                LuckGrade      = 2,
                StrengthGrade  = 6,
                VitalityGrade  = 18,
                MagicGrade     = 16,
                SpiritGrade    = 9,
                DexterityGrade = 25,

                Xp02to11 = 68,
                Xp12to21 = 71,
                Xp22to31 = 73,
                Xp32to41 = 74,
                Xp42to51 = 75,
                Xp52to61 = 75,
                Xp62to81 = 75,
                Xp82to99 = 76,
            };
        }

        public static Archetype CreateSage()
        {
            return new Archetype()
            {
                BaseHp        = 177,
                BaseMp        = 23,
                BaseStrength  = 10,
                BaseVitality  = 11,
                BaseMagic     = 13,
                BaseSpirit    = 14,
                BaseDexterity = 5,
                BaseLuck      = 14,

                HpGrade        = 3,
                MpGrade        = 3,
                LuckGrade      = 3,
                StrengthGrade  = 23,
                VitalityGrade  = 20,
                MagicGrade     = 0,
                SpiritGrade    = 1,
                DexterityGrade = 28,

                Xp02to11 = 67,
                Xp12to21 = 70,
                Xp22to31 = 72,
                Xp32to41 = 74,
                Xp42to51 = 74,
                Xp52to61 = 75,
                Xp62to81 = 76,
                Xp82to99 = 78,
            };
        }

        public static Archetype CreatePhilosopher()
        {
            return new Archetype()
            {
                BaseHp        = 221,
                BaseMp        = 17,
                BaseStrength  = 10,
                BaseVitality  = 12,
                BaseMagic     = 11,
                BaseSpirit    = 10,
                BaseDexterity = 10,
                BaseLuck      = 14,

                HpGrade        = 4,
                MpGrade        = 4,
                LuckGrade      = 4,
                StrengthGrade  = 12,
                VitalityGrade  = 11,
                MagicGrade     = 13,
                SpiritGrade    = 9,
                DexterityGrade = 23,

                Xp02to11 = 68,
                Xp12to21 = 71,
                Xp22to31 = 74,
                Xp32to41 = 75,
                Xp42to51 = 75,
                Xp52to61 = 75,
                Xp62to81 = 76,
                Xp82to99 = 76,
            };
        }

        public static Archetype CreateTrickster()
        {
            return new Archetype()
            {
                BaseHp        = 100,
                BaseMp        = 5,
                BaseStrength  = 1,
                BaseVitality  = 1,
                BaseMagic     = 1,
                BaseSpirit    = 1,
                BaseDexterity = 1,
                BaseLuck      = 1,

                HpGrade        = 5,
                MpGrade        = 5,
                LuckGrade      = 5,
                StrengthGrade  = 16,
                VitalityGrade  = 19,
                MagicGrade     = 11,
                SpiritGrade    = 10,
                DexterityGrade = 24,

                Xp02to11 = 69,
                Xp12to21 = 72,
                Xp22to31 = 75,
                Xp32to41 = 75,
                Xp42to51 = 76,
                Xp52to61 = 76,
                Xp62to81 = 77,
                Xp82to99 = 77,
            };
        }

        public static Archetype CreateBlackguard()
        {
            return new Archetype()
            {
                BaseHp        = 224,
                BaseMp        = 18,
                BaseStrength  = 10,
                BaseVitality  = 11,
                BaseMagic     = 13,
                BaseSpirit    = 11,
                BaseDexterity = 5,
                BaseLuck      = 15,

                HpGrade        = 6,
                MpGrade        = 6,
                LuckGrade      = 6,
                StrengthGrade  = 19,
                VitalityGrade  = 22,
                MagicGrade     = 6,
                SpiritGrade    = 4,
                DexterityGrade = 28,

                Xp02to11 = 69,
                Xp12to21 = 72,
                Xp22to31 = 75,
                Xp32to41 = 75,
                Xp42to51 = 75,
                Xp52to61 = 76,
                Xp62to81 = 76,
                Xp82to99 = 76,
            };
        }

        public static Archetype CreateGuardian()
        {
            return new Archetype()
            {
                BaseHp        = 178,
                BaseMp        = 18,
                BaseStrength  = 9,
                BaseVitality  = 10,
                BaseMagic     = 11,
                BaseSpirit    = 11,
                BaseDexterity = 5,
                BaseLuck      = 14,

                HpGrade        = 7,
                MpGrade        = 7,
                LuckGrade      = 7,
                StrengthGrade  = 21,
                VitalityGrade  = 22,
                MagicGrade     = 6,
                SpiritGrade    = 4,
                DexterityGrade = 28,

                Xp02to11 = 70,
                Xp12to21 = 72,
                Xp22to31 = 75,
                Xp32to41 = 76,
                Xp42to51 = 76,
                Xp52to61 = 76,
                Xp62to81 = 76,
                Xp82to99 = 76,
            };
        }

        public static Archetype CreateLancer()
        {
            return new Archetype()
            {
                BaseHp        = 223,
                BaseMp        = 15,
                BaseStrength  = 12,
                BaseVitality  = 12,
                BaseMagic     = 11,
                BaseSpirit    = 10,
                BaseDexterity = 6,
                BaseLuck      = 14,

                HpGrade        = 8,
                MpGrade        = 8,
                LuckGrade      = 8,
                StrengthGrade  = 11,
                VitalityGrade  = 7,
                MagicGrade     = 17,
                SpiritGrade    = 15,
                DexterityGrade = 27,

                Xp02to11 = 69,
                Xp12to21 = 72,
                Xp22to31 = 75,
                Xp32to41 = 75,
                Xp42to51 = 76,
                Xp52to61 = 76,
                Xp62to81 = 77,
                Xp82to99 = 77,
            };
        }
    }
}
