using System;
using System.Collections.Generic;
using System.Text;

namespace RpgMath
{
    class Archetype
    {
        public long Hp { get; set; }

        public long Mp { get; set; }

        public long Strength { get; set; }

        public long Vitality { get; set; }

        public long Magic { get; set; }

        public long Spirit { get; set; }

        public long Dexterity { get; set; }

        public long Luck { get; set; }

        public long HpGrade { get; set; }

        public long MpGrade { get; set; }

        public long StrengthGrade { get; set; }

        public long VitalityGrade { get; set; }

        public long MagicGrade { get; set; }

        public long SpiritGrade { get; set; }

        public long DexterityGrade { get; set; }

        public long LuckGrade { get; set; }

        public static Archetype CreateHero()
        {
            return new Archetype()
            {
                Hp        = 314,
                Mp        = 54,
                Strength  = 20,
                Vitality  = 16,
                Magic     = 19,
                Spirit    = 17,
                Dexterity = 6,
                Luck      = 14,

                HpGrade        = 0,
                MpGrade        = 0,
                LuckGrade      = 0,
                StrengthGrade  = 1,
                VitalityGrade  = 6,
                MagicGrade     = 3,
                SpiritGrade    = 4,
                DexterityGrade = 26
            };
        }

        public static Archetype CreateTank()
        {
            return new Archetype()
            {
                Hp        = 222,
                Mp        = 15,
                Strength  = 15,
                Vitality  = 13,
                Magic     = 11,
                Spirit    = 9,
                Dexterity = 5,
                Luck      = 13,

                HpGrade        = 1,
                MpGrade        = 1,
                LuckGrade      = 1,
                StrengthGrade  = 5,
                VitalityGrade  = 2,
                MagicGrade     = 18,
                SpiritGrade    = 14,
                DexterityGrade = 29
            };
        }

        public static Archetype CreateBrawler()
        {
            return new Archetype()
            {
                Hp        = 219,
                Mp        = 16,
                Strength  = 11,
                Vitality  = 11,
                Magic     = 11,
                Spirit    = 10,
                Dexterity = 7,
                Luck      = 14,

                HpGrade        = 2,
                MpGrade        = 2,
                LuckGrade      = 2,
                StrengthGrade  = 6,
                VitalityGrade  = 18,
                MagicGrade     = 16,
                SpiritGrade    = 9,
                DexterityGrade = 25
            };
        }

        public static Archetype CreateSage()
        {
            return new Archetype()
            {
                Hp        = 177,
                Mp        = 23,
                Strength  = 10,
                Vitality  = 11,
                Magic     = 13,
                Spirit    = 14,
                Dexterity = 5,
                Luck      = 14,

                HpGrade        = 3,
                MpGrade        = 3,
                LuckGrade      = 3,
                StrengthGrade  = 23,
                VitalityGrade  = 20,
                MagicGrade     = 0,
                SpiritGrade    = 1,
                DexterityGrade = 28
            };
        }

        public static Archetype CreatePhilosopher()
        {
            return new Archetype()
            {
                Hp        = 221,
                Mp        = 17,
                Strength  = 10,
                Vitality  = 12,
                Magic     = 11,
                Spirit    = 10,
                Dexterity = 10,
                Luck      = 14,

                HpGrade        = 4,
                MpGrade        = 4,
                LuckGrade      = 4,
                StrengthGrade  = 12,
                VitalityGrade  = 11,
                MagicGrade     = 13,
                SpiritGrade    = 9,
                DexterityGrade = 23
            };
        }

        public static Archetype CreateTrickster()
        {
            return new Archetype()
            {
                Hp        = 100,
                Mp        = 5,
                Strength  = 1,
                Vitality  = 1,
                Magic     = 1,
                Spirit    = 1,
                Dexterity = 1,
                Luck      = 1,

                HpGrade        = 5,
                MpGrade        = 5,
                LuckGrade      = 5,
                StrengthGrade  = 16,
                VitalityGrade  = 19,
                MagicGrade     = 11,
                SpiritGrade    = 10,
                DexterityGrade = 24
            };
        }

        public static Archetype CreateBlackguard()
        {
            return new Archetype()
            {
                Hp        = 224,
                Mp        = 18,
                Strength  = 10,
                Vitality  = 11,
                Magic     = 13,
                Spirit    = 11,
                Dexterity = 5,
                Luck      = 15,

                HpGrade        = 6,
                MpGrade        = 6,
                LuckGrade      = 6,
                StrengthGrade  = 19,
                VitalityGrade  = 22,
                MagicGrade     = 6,
                SpiritGrade    = 4,
                DexterityGrade = 28
            };
        }

        public static Archetype CreateGuardian()
        {
            return new Archetype()
            {
                Hp        = 178,
                Mp        = 18,
                Strength  = 9,
                Vitality  = 10,
                Magic     = 11,
                Spirit    = 11,
                Dexterity = 5,
                Luck      = 14,

                HpGrade        = 7,
                MpGrade        = 7,
                LuckGrade      = 7,
                StrengthGrade  = 21,
                VitalityGrade  = 22,
                MagicGrade     = 6,
                SpiritGrade    = 4,
                DexterityGrade = 28
            };
        }

        public static Archetype CreateLancer()
        {
            return new Archetype()
            {
                Hp        = 223,
                Mp        = 15,
                Strength  = 12,
                Vitality  = 12,
                Magic     = 11,
                Spirit    = 10,
                Dexterity = 6,
                Luck      = 14,

                HpGrade        = 8,
                MpGrade        = 8,
                LuckGrade      = 8,
                StrengthGrade  = 11,
                VitalityGrade  = 7,
                MagicGrade     = 17,
                SpiritGrade    = 15,
                DexterityGrade = 27
            };
        }
    }
}
