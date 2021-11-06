using Engine;

namespace RpgMath
{
    public interface IEnemyCurve
    {
        long GetAttack(int level, EnemyType enemyType);
        long GetAttackPercent(int level, EnemyType enemyType);
        long GetDefense(int level, EnemyType enemyType);
        long GetDefensePercent(int level, EnemyType enemyType);
        long GetDexterity(int level, EnemyType enemyType);
        long GetGold(int level, EnemyType enemyType);
        long GetHp(int level, EnemyType enemyType);
        long GetLuck(int level, EnemyType enemyType);
        long GetMagicAttack(int level, EnemyType enemyType);
        long GetMagicAttackPercent(int level, EnemyType enemyType);
        long GetMagicDefense(int level, EnemyType enemyType);
        long GetMagicDefensePercent(int level, EnemyType enemyType);
        long GetMp(int level, EnemyType enemyType);
        Vector3 GetScale(int level, EnemyType enemyType);
        long GetXp(int level, EnemyType enemyType);
    }
}