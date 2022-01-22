using RpgMath;
using SceneTest.Assets;
using SceneTest.Battle;
using SceneTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SceneTest
{
    class Party
    {
        private readonly Persistence persistence;

        public Party(Persistence persistence)
        {
            this.persistence = persistence;
        }

        public IEnumerable<Character> ActiveCharacters => persistence.Party.Members.Select(i =>
        {
            var assemblyName = typeof(Party).Assembly.GetName().Name;

            var character = new Character()
            {
                CharacterSheet = i.CharacterSheet,
                PlayerSprite = CreateInstance<IPlayerSprite>($"SceneTest.Assets.Original.{i.PlayerSprite}"),
                PrimaryHandAsset = i.PrimaryHandAsset != null ? CreateInstance<ISpriteAsset>($"SceneTest.Assets.Original.{i.PrimaryHandAsset}") : null,
                SecondaryHandAsset = i.SecondaryHandAsset != null ? CreateInstance<ISpriteAsset>($"SceneTest.Assets.Original.{i.SecondaryHandAsset}") : null,
                Spells = i.Spells?.Select(s => CreateInstance<ISpell>($"SceneTest.Battle.Spells.{s}"))
            };

            return character;
        });

        private T CreateInstance<T>(String name)
        {
            var type = Type.GetType(name);
            var instance = (T)Activator.CreateInstance(type);
            return instance;
        }

        public IEnumerable<CharacterSheet> ActiveCharacterSheets => persistence.Party.Members.Select(i => i.CharacterSheet);

        public long Gold { get; set; }
    }
}
