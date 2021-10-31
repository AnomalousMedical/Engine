using RpgMath;
using SceneTest.Assets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Character
    {
        public CharacterSheet CharacterSheet { get; set; }

        public IPlayerSprite PlayerSprite { get; set; }

        public ISpriteAsset PrimaryHandAsset { get; set; }

        public ISpriteAsset SecondaryHandAsset { get; set; }
    }
}
