namespace RpgMath
{
    public interface ICharacterStats
    {
        ulong Attack { get; }
        ulong AttackPercent { get; }
        ulong Defense { get; }
        ulong DefensePercent { get; }
        ulong MagicAttack { get; }
        ulong MagicDefense { get; }
        ulong MagicDefensePercent { get; }
        ulong Dexterity { get; }
        ulong Luck { get; }
        bool AllowLuckyEvade { get; }
    }
}