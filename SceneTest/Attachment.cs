using DiligentEngine;
using DiligentEngine.RT;
using DiligentEngine.RT.Sprites;
using Engine;
using System;

namespace SceneTest
{
    class Attachment<TSceneObjectManager> : IDisposable
    {
        public class Description
        {
            public string InstanceName { get; set; } = Guid.NewGuid().ToString("N");

            public Quaternion Orientation { get; set; } = Quaternion.Identity;

            public Sprite Sprite { get; set; }

            public SpriteMaterialDescription SpriteMaterial { get; set; }

            public bool RenderShadow { get; set; } = true;
        }

        
        private const int PrimaryAttachment = 0;

        private readonly TLASBuildInstanceData instanceData;
        private readonly IDestructionRequest destructionRequest;
        private readonly RTInstances<TSceneObjectManager> rtInstances;
        private readonly SpriteInstanceFactory spriteInstanceFactory;
        private readonly Sprite sprite;

        private SpriteInstance spriteInstance;
        private bool disposed;

        private Quaternion orientation;
        private Quaternion additionalRotation = Quaternion.Identity;

        public Attachment
        (
            IDestructionRequest destructionRequest,
            RTInstances<TSceneObjectManager> rtInstances,
            SpriteInstanceFactory spriteInstanceFactory,
            IScopedCoroutine coroutine,
            Description attachmentDescription
        )
        {
            this.orientation = attachmentDescription.Orientation;
            this.sprite = attachmentDescription.Sprite;
            this.destructionRequest = destructionRequest;
            this.rtInstances = rtInstances;
            this.spriteInstanceFactory = spriteInstanceFactory;

            this.instanceData = new TLASBuildInstanceData()
            {
                InstanceName = attachmentDescription.InstanceName,
                Mask = RtStructures.OPAQUE_GEOM_MASK,
                Transform = new InstanceMatrix(Vector3.Zero, attachmentDescription.Orientation)
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until task is finished and this is disposed.

                this.spriteInstance = await spriteInstanceFactory.Checkout(attachmentDescription.SpriteMaterial);
                this.instanceData.pBLAS = spriteInstance.Instance.BLAS.Obj;

                if (this.disposed)
                {
                    this.spriteInstanceFactory.TryReturn(spriteInstance);
                    return; //Stop loading
                }

                if (!destructionRequest.DestructionRequested) //This is more to prevent a flash for 1 frame of the object
                {
                    rtInstances.AddTlasBuild(instanceData);
                    rtInstances.AddShaderTableBinder(Bind);
                }
            });
        }

        public void Dispose()
        {
            disposed = true;
            this.spriteInstanceFactory.TryReturn(spriteInstance);
            rtInstances.RemoveShaderTableBinder(Bind);
            rtInstances.RemoveTlasBuild(instanceData);
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        public void SetAdditionalRotation(in Quaternion additionalRotation)
        {
            this.additionalRotation = additionalRotation;
        }

        public void SetPosition(in Vector3 parentPosition, in Quaternion parentRotation, in Vector3 parentScale)
        {
            var frame = sprite.GetCurrentFrame();
            var primaryAttach = frame.Attachments[PrimaryAttachment];

            //Get the primary attachment out of sprite space into world space
            var scale = sprite.BaseScale * parentScale;
            var translate = scale * primaryAttach.translate;
            var fullRot = parentRotation * this.orientation * additionalRotation;
            translate = Quaternion.quatRotate(fullRot, translate);

            var finalPosition = parentPosition - translate; //The attachment point on the sprite is an offset to where that sprite attaches, subtract it
            var finalOrientation = fullRot;
            var finalScale = scale;

            this.instanceData.Transform = new InstanceMatrix(finalPosition, finalOrientation, finalScale);
        }

        private void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            spriteInstance.Bind(this.instanceData.InstanceName, sbt, tlas);
        }
    }
}
