using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Assets
{
    interface ISpriteAsset
    {
        Quaternion GetOrientation() => Quaternion.Identity;

        Sprite CreateSprite();

        SpriteMaterialDescription CreateMaterial();
    }
}
