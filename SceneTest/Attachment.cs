using DiligentEngine;
using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest
{
    class Attachment<TSceneObjectManager> : IDisposable
    {
        public class Description
        {
            public Quaternion Orientation { get; set; } = Quaternion.Identity;

            public Sprite Sprite { get; set; }

            public SpriteMaterialDescription SpriteMaterial { get; set; }
        }

        private const int PrimaryAttachment = 0;

        private ISpriteMaterial spriteMaterial;
        private SceneObjectManager<TSceneObjectManager> sceneObjectManager;
        private SpriteManager sprites;
        private IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private SceneObject sceneObject;
        private Sprite sprite;
        private bool disposed;

        private Quaternion orientation;

        public Attachment(
            SceneObjectManager<TSceneObjectManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IDestructionRequest destructionRequest,
            IScopedCoroutine coroutine,
            ISpriteMaterialManager spriteMaterialManager,
            Description attachmentDescription)
        {
            this.orientation = attachmentDescription.Orientation;
            this.sprite = attachmentDescription.Sprite;

            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;

            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = Vector3.Zero,
                orientation = this.orientation,
                scale = sprite.BaseScale,
                RenderShadow = true,
                Sprite = sprite,
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until task is finished and this is disposed.

                spriteMaterial = await this.spriteMaterialManager.Checkout(attachmentDescription.SpriteMaterial);

                if (this.disposed)
                {
                    this.spriteMaterialManager.Return(spriteMaterial);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested) //This is more to prevent a flash for 1 frame of the object
                {
                    sceneObject.shaderResourceBinding = spriteMaterial.ShaderResourceBinding;
                    sprites.Add(sprite);
                    sceneObjectManager.Add(sceneObject);
                }
            });
        }

        public void SetPosition(ref Vector3 parentPosition, ref Quaternion parentRotation, ref Vector3 parentScale)
        {
            var frame = sprite.GetCurrentFrame();
            var primaryAttach = frame.Attachments[PrimaryAttachment];

            //Get the primary attachment out of sprite space into world space
            var scale = sprite.BaseScale * parentScale;
            var translate = scale * primaryAttach.translate;
            var fullRot = parentRotation * this.orientation;
            translate = Quaternion.quatRotate(fullRot, translate);

            this.sceneObject.position = parentPosition - translate; //The attachment point on the sprite is an offset to where that sprite attaches, subtract it
            this.sceneObject.orientation = fullRot;
            this.sceneObject.scale = scale;
        }

        public void Dispose()
        {
            disposed = true;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
        }
    }
}
