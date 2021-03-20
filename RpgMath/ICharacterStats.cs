﻿namespace RpgMath
{
    public interface ICharacterStats
    {
        long Attack { get; }
        long AttackPercent { get; }
        long Defense { get; }
        long DefensePercent { get; }
        long MagicAttack { get; }
        long MagicDefense { get; }
        long MagicDefensePercent { get; }
        long Dexterity { get; }
        long Luck { get; }
        bool AllowLuckyEvade { get; }
        long Level { get; }
    }
}