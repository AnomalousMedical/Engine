using Engine;
using RpgMath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle.Scenes
{
    class DinoSkeletonBattle : IBattleScene
    {
        public IEnumerable<Enemy> CreateEnemies(IObjectResolver objectResolver, Party party)
        {
            var level = party.ActiveCharacters.GetAverageLevel() * 4 / 5;
            if(level < 1)
            {
                level = 1;
            }

            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var location = new Vector3(-4f, 0f, -2f);
                var enemyType = EnemyType.Normal;
                var curve = new StandardEnemyCurve();
                var spriteAsset = new Assets.Original.TinyDino();
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = curve.GetHp(level),
                    Mp = curve.GetMp(level),
                    Attack = curve.GetAttack(level),
                    AttackPercent = curve.GetAttackPercent(level),
                    Defense = curve.GetDefense(level),
                    DefensePercent = curve.GetDefensePercent(level),
                    MagicAttack = curve.GetMagicAttack(level),
                    MagicAttackPercent = curve.GetMagicAttackPercent(level),
                    MagicDefensePercent = curve.GetMagicDefensePercent(level),
                    MagicDefense = curve.GetMagicDefense(level),
                    Dexterity = curve.GetDexterity(level),
                    Luck = curve.GetLuck(level),
                    Level = level,
                };
                c.Scale = curve.GetScale(level, enemyType);
                c.Translation = new Vector3(location.x, c.Sprite.BaseScale.y * c.Scale.y / 2.0f, location.z);
                c.XpReward = curve.GetXp(level);
                c.GoldReward = curve.GetGold(level);
            });
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var location = new Vector3(-5, 0, 0);
                var enemyType = EnemyType.Normal;
                var curve = new StandardEnemyCurve();
                var spriteAsset = new Assets.Original.TinyDino()
                {
                    SkinMaterial = "cc0Textures/Leather011_1K"
                };
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = curve.GetHp(level),
                    Mp = curve.GetMp(level),
                    Attack = curve.GetAttack(level),
                    AttackPercent = curve.GetAttackPercent(level),
                    Defense = curve.GetDefense(level),
                    DefensePercent = curve.GetDefensePercent(level),
                    MagicAttack = curve.GetMagicAttack(level),
                    MagicAttackPercent = curve.GetMagicAttackPercent(level),
                    MagicDefensePercent = curve.GetMagicDefensePercent(level),
                    MagicDefense = curve.GetMagicDefense(level),
                    Dexterity = curve.GetDexterity(level),
                    Luck = curve.GetLuck(level),
                    Level = level,
                };
                c.Scale = curve.GetScale(level, enemyType);
                c.Translation = new Vector3(location.x, c.Sprite.BaseScale.y * c.Scale.y / 2.0f, location.z);
                c.XpReward = curve.GetXp(level);
                c.GoldReward = curve.GetGold(level);
            });
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var location = new Vector3(0, 0, 2);
                var enemyType = EnemyType.Normal;
                var curve = new StandardEnemyCurve();
                var spriteAsset = new Assets.Original.Skeleton();
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = curve.GetHp(level),
                    Mp = curve.GetMp(level),
                    Attack = curve.GetAttack(level),
                    AttackPercent = curve.GetAttackPercent(level),
                    Defense = curve.GetDefense(level),
                    DefensePercent = curve.GetDefensePercent(level),
                    MagicAttack = curve.GetMagicAttack(level),
                    MagicAttackPercent = curve.GetMagicAttackPercent(level),
                    MagicDefensePercent = curve.GetMagicDefensePercent(level),
                    MagicDefense = curve.GetMagicDefense(level),
                    Dexterity = curve.GetDexterity(level),
                    Luck = curve.GetLuck(level),
                    Level = level,
                    Resistances = new Dictionary<Element, Resistance>() { { Element.Fire, Resistance.Weak }, { Element.Healing, Resistance.Absorb } }
                };
                c.Scale = curve.GetScale(level, enemyType);
                c.Translation = new Vector3(location.x, c.Sprite.BaseScale.y * c.Scale.y / 2.0f, location.z);
                c.XpReward = curve.GetXp(level);
                c.GoldReward = curve.GetGold(level);
            });
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var location = new Vector3(-4, 0, 4);
                var enemyType = EnemyType.Normal;
                var curve = new StandardEnemyCurve();
                var spriteAsset = new Assets.Original.TinyDino();
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = curve.GetHp(level),
                    Mp = curve.GetMp(level),
                    Attack = curve.GetAttack(level),
                    AttackPercent = curve.GetAttackPercent(level),
                    Defense = curve.GetDefense(level),
                    DefensePercent = curve.GetDefensePercent(level),
                    MagicAttack = curve.GetMagicAttack(level),
                    MagicAttackPercent = curve.GetMagicAttackPercent(level),
                    MagicDefensePercent = curve.GetMagicDefensePercent(level),
                    MagicDefense = curve.GetMagicDefense(level),
                    Dexterity = curve.GetDexterity(level),
                    Luck = curve.GetLuck(level),
                    Level = level,
                };
                c.Scale = curve.GetScale(level, enemyType);
                c.Translation = new Vector3(location.x, c.Sprite.BaseScale.y * c.Scale.y / 2.0f, location.z);
                c.XpReward = curve.GetXp(level);
                c.GoldReward = curve.GetGold(level);
            });
        }
    }
}
