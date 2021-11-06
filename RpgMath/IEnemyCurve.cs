using Engine;

namespace RpgMath
{
    public interface IEnemyCurve
    {
        long GetAttack(int level);
        long GetAttackPercent(int level);
        long GetDefense(int level);
        long GetDefensePercent(int level);
        long GetDexterity(int level);
        long GetGold(int level);
        long GetHp(int level);
        long GetLuck(int level);
        long GetMagicAttack(int level);
        long GetMagicAttackPercent(int level);
        long GetMagicDefense(int level);
        long GetMagicDefensePercent(int level);
        long GetMp(int level);
        Vector3 GetScale(int level, EnemyType enemyType);
        long GetXp(int level);
    }
}