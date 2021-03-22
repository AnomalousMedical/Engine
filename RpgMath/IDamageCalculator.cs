namespace RpgMath
{
    public interface IDamageCalculator
    {
        long ApplyDamage(long damage, long currentHp, long maxHp);
        long ApplyResistance(long damage, Resistance resistance);
        bool CriticalHit(IBattleStats attacker, IBattleStats target);
        long Cure(IBattleStats caster, long power);
        long Item(IBattleStats target, long power);
        long Magical(IBattleStats attacker, IBattleStats target, long power);
        bool MagicalHit(IBattleStats attacker, IBattleStats target, Resistance resistance, long magicAttackPercent);
        long Physical(IBattleStats attacker, IBattleStats target, long power);
        bool PhysicalHit(IBattleStats attacker, IBattleStats target);
        long RandomVariation(long damage);
    }
}