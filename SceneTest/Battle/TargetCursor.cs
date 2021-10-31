using DiligentEngine.GltfPbr;
using DiligentEngine.GltfPbr.Shapes;
using Engine;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SceneTest.Battle
{
    class TargetCursor : IDisposable
    {
        private readonly SceneObjectManager<IBattleManager> sceneObjectManager;
        private readonly SpriteManager sprites;
        private readonly IDestructionRequest destructionRequest;
        private readonly ISpriteMaterialManager spriteMaterialManager;
        private ISpriteMaterial spriteMaterial;
        private SceneObject sceneObject;
        private Sprite sprite;
        private bool disposed;

        public uint TargetIndex { get; private set; }

        public bool TargetPlayers { get; private set; }

        public bool Targeting => getTargetTask != null;

        TaskCompletionSource<IBattleTarget> getTargetTask;

        public TargetCursor(SceneObjectManager<IBattleManager> sceneObjectManager,
            SpriteManager sprites,
            Plane plane,
            IScopedCoroutine coroutine,
            IDestructionRequest destructionRequest,
            ISpriteMaterialManager spriteMaterialManager)
        {
            this.sceneObjectManager = sceneObjectManager;
            this.sprites = sprites;
            this.destructionRequest = destructionRequest;
            this.spriteMaterialManager = spriteMaterialManager;
            this.sprite = new Sprite(new Dictionary<string, SpriteAnimation>()
            {
                { "default", new SpriteAnimation(1,
                    new SpriteFrame(0, 0, 1, 1)
                    {
                        Attachments = new List<SpriteFrameAttachment>()
                        {
                            SpriteFrameAttachment.FromFramePosition(0, 9, -0.01f, 32, 32),
                        }
                    } )
                },
                { "reverse", new SpriteAnimation(1,
                    new SpriteFrame(1, 0, 0, 1)
                    {
                        Attachments = new List<SpriteFrameAttachment>()
                        {
                            SpriteFrameAttachment.FromFramePosition(31, 9, -0.01f, 32, 32),
                        }
                    } )
                }
            })
            { BaseScale = new Vector3(0.5f, 0.5f, 1f) };

            sceneObject = new SceneObject()
            {
                vertexBuffer = plane.VertexBuffer,
                skinVertexBuffer = plane.SkinVertexBuffer,
                indexBuffer = plane.IndexBuffer,
                numIndices = plane.NumIndices,
                pbrAlphaMode = PbrAlphaMode.ALPHA_MODE_MASK,
                position = Vector3.Zero,
                orientation = Quaternion.Identity,
                scale = sprite.BaseScale * Vector3.ScaleIdentity,
                RenderShadow = false,
                Sprite = sprite,
            };

            coroutine.RunTask(async () =>
            {
                using var destructionBlock = destructionRequest.BlockDestruction(); //Block destruction until coroutine is finished and this is disposed.

                var matDesc = new SpriteMaterialDescription(
                    colorMap: "original/pointingfinger.png",
                    materials: new HashSet<SpriteMaterialTextureItem>());

                spriteMaterial = await this.spriteMaterialManager.Checkout(matDesc);

                if (disposed)
                {
                    spriteMaterialManager.Return(spriteMaterial);
                }
                else
                {
                    sceneObject.shaderResourceBinding = spriteMaterial.ShaderResourceBinding;
                }

                if (!destructionRequest.DestructionRequested)
                {
                    sprites.Add(sprite);
                    sceneObjectManager.Add(sceneObject);
                }
            });
        }

        private bool visible = true;
        public bool Visible
        {
            get
            {
                return visible;
            }
            set
            {
                if (visible != value)
                {
                    visible = value;
                    if (visible)
                    {
                        sceneObjectManager.Add(sceneObject);
                    }
                    else
                    {
                        sceneObjectManager.Remove(sceneObject);
                    }
                }
            }
        }

        public void Dispose()
        {
            disposed = true;
            sprites.Remove(sprite);
            sceneObjectManager.Remove(sceneObject);
            spriteMaterialManager.TryReturn(spriteMaterial);
        }

        internal void BattleStarted()
        {
            TargetIndex = 0;
        }

        public void SetPosition(Vector3 targetPosition)
        {
            this.sceneObject.position = targetPosition - sprite.GetCurrentFrame().Attachments[0].translate * sprite.BaseScale;
        }

        public Task<IBattleTarget> GetTarget()
        {
            getTargetTask = new TaskCompletionSource<IBattleTarget>();
            return getTargetTask.Task.ContinueWith(t =>
            {
                getTargetTask = null;
                return t.Result;
            });
        }

        public void Cancel()
        {
            if (getTargetTask != null)
            {
                SetTarget(null);
            }
        }

        public void UpdateCursor(ISharpGui sharpGui, IBattleTarget target, Vector3 enemyPos)
        {
            SetPosition(enemyPos);
            switch (sharpGui.GamepadButtonEntered)
            {
                case GamepadButtonCode.XInput_A:
                    SetTarget(target);
                    break;
                case GamepadButtonCode.XInput_B:
                    SetTarget(null);
                    break;
                case GamepadButtonCode.XInput_DPadUp:
                    NextTarget();
                    break;
                case GamepadButtonCode.XInput_DPadDown:
                    PreviousTarget();
                    break;
                case GamepadButtonCode.XInput_DPadLeft:
                case GamepadButtonCode.XInput_DPadRight:
                    ChangeRow();
                    break;
                default:
                    //Handle keyboard
                    switch (sharpGui.KeyEntered)
                    {
                        case KeyboardButtonCode.KC_RETURN:
                            SetTarget(target);
                            break;
                        case KeyboardButtonCode.KC_ESCAPE:
                            SetTarget(null);
                            break;
                        case KeyboardButtonCode.KC_UP:
                            NextTarget();
                            break;
                        case KeyboardButtonCode.KC_DOWN:
                            PreviousTarget();
                            break;
                        case KeyboardButtonCode.KC_LEFT:
                        case KeyboardButtonCode.KC_RIGHT:
                            ChangeRow();
                            break;
                    }
                    break;
            }
        }

        private void ChangeRow()
        {
            TargetPlayers = !TargetPlayers;
            if (TargetPlayers)
            {
                sprite.SetAnimation("reverse");
            }
            else
            {
                sprite.SetAnimation("default");
            }
        }

        private void PreviousTarget()
        {
            if (TargetPlayers)
            {
                ++TargetIndex;
            }
            else
            {
                --TargetIndex;
            }
        }

        private void NextTarget()
        {
            if (TargetPlayers)
            {
                --TargetIndex;
            }
            else
            {
                ++TargetIndex;
            }
        }

        private void SetTarget(IBattleTarget enemy)
        {
            getTargetTask.SetResult(enemy);
        }
    }
}
