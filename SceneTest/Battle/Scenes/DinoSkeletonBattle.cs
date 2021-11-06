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
        public IEnumerable<Enemy> CreateEnemies(IObjectResolver objectResolver)
        {
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var level = 1;
                var curve = new StandardEnemyCurve();
                var spriteAsset = new Assets.Original.TinyDino();
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = curve.GetHp(level),
                    Mp = 54,
                    Attack = curve.GetAttack(level),
                    AttackPercent = curve.GetAttackPercent(level),
                    Defense = curve.GetDefense(level),
                    DefensePercent = curve.GetDefensePercent(level),
                    MagicAttack = 2,
                    MagicAttackPercent = curve.GetMagicAttackPercent(level),
                    MagicDefensePercent = curve.GetMagicDefensePercent(level),
                    MagicDefense = 2,
                    Dexterity = curve.GetDexterity(level),
                    Luck = 14,
                    Level = level,
                };
                c.Translation = new Vector3(-4, 0.55f, -2);
                c.XpReward = 50;
                c.GoldReward = 50;
            });
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var spriteAsset = new Assets.Original.TinyDino()
                {
                    SkinMaterial = "cc0Textures/Leather011_1K"
                };
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = 40,
                    Mp = 54,
                    Attack = 12,
                    AttackPercent = 100,
                    Defense = 10,
                    DefensePercent = 4,
                    MagicAttack = 2,
                    MagicAttackPercent = 100,
                    MagicDefensePercent = 0,
                    MagicDefense = 2,
                    Dexterity = 6,
                    Luck = 14,
                    Level = 1,
                };
                c.Sprite.BaseScale *= new Vector3(1.5f, 1.5f, 1f);
                c.Translation = new Vector3(-5, c.Sprite.BaseScale.y / 2.0f, 0);
                c.XpReward = 50;
                c.GoldReward = 50;
            });
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var spriteAsset = new Assets.Original.Skeleton();
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = 40,
                    Mp = 54,
                    Attack = 12,
                    AttackPercent = 100,
                    Defense = 10,
                    DefensePercent = 4,
                    MagicAttack = 2,
                    MagicAttackPercent = 100,
                    MagicDefensePercent = 0,
                    MagicDefense = 2,
                    Dexterity = 6,
                    Luck = 14,
                    Level = 1,
                    Resistances = new Dictionary<Element, Resistance>() { { Element.Fire, Resistance.Weak }, { Element.Healing, Resistance.Absorb } }
                };
                c.Translation = new Vector3(0, 0.55f, 2);
                c.XpReward = 50;
                c.GoldReward = 50;
            });
            yield return objectResolver.Resolve<Enemy, Enemy.Desc>(c =>
            {
                var spriteAsset = new Assets.Original.TinyDino();
                c.Sprite = spriteAsset.CreateSprite();
                c.SpriteMaterial = spriteAsset.CreateMaterial();
                c.BattleStats = new BattleStats()
                {
                    Hp = 40,
                    Mp = 54,
                    Attack = 12,
                    AttackPercent = 100,
                    Defense = 10,
                    DefensePercent = 4,
                    MagicAttack = 2,
                    MagicAttackPercent = 100,
                    MagicDefensePercent = 0,
                    MagicDefense = 2,
                    Dexterity = 6,
                    Luck = 14,
                    Level = 1,
                };
                c.Translation = new Vector3(-4, 0.55f, 4);
                c.XpReward = 50;
                c.GoldReward = 50;
            });
        }
    }
}
