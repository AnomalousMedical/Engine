using Anomalous.OSPlatform;
using DiligentEngine;
using DiligentEngine.RT;
using Engine;
using Engine.CameraMovement;
using Engine.Platform;
using SharpGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SceneTest
{
    class SceneTestUpdateListener : UpdateListener
    {
        private readonly RayTracingRenderer rayTracingRenderer;
        private readonly RTInstances rtInstances;
        private readonly ITimeClock timeClock;
        private readonly ISharpGui sharpGui;
        private readonly ISwapChain swapChain;
        private readonly IDeviceContext immediateContext;

        private readonly IObjectResolverFactory objectResolverFactory;
        private readonly CameraMover cameraMover;
        private readonly Sky sky;
        private readonly FirstPersonFlyCamera flyCamera;
        private IGameState gameState;

        public unsafe SceneTestUpdateListener
        (
            GraphicsEngine graphicsEngine,
            RayTracingRenderer rayTracingRenderer,
            RTInstances rtInstances,
            ITimeClock timeClock,
            ISharpGui sharpGui,
            IObjectResolverFactory objectResolverFactory,
            CameraMover cameraMover,
            Sky sky,
            IFirstGameStateBuilder startState,
            FirstPersonFlyCamera flyCamera
        )
        {
            flyCamera.Position = new Vector3(0, 0, -10);

            this.swapChain = graphicsEngine.SwapChain;
            this.immediateContext = graphicsEngine.ImmediateContext;
            this.rayTracingRenderer = rayTracingRenderer;
            this.rtInstances = rtInstances;
            this.timeClock = timeClock;
            this.sharpGui = sharpGui;
            this.objectResolverFactory = objectResolverFactory;
            this.cameraMover = cameraMover;
            this.sky = sky;
            this.flyCamera = flyCamera;
            this.gameState = startState.GetFirstGameState();
            this.gameState.SetActive(true);
        }

        public void exceededMaxDelta()
        {

        }

        public void loopStarting()
        {

        }

        public unsafe void sendUpdate(Clock clock)
        {
            rayTracingRenderer.SetInstances(gameState.Instances);

            //Update
            flyCamera.UpdateInput(clock);
            timeClock.Update(clock);
            sharpGui.Begin(clock);
            var nextState = this.gameState.Update(clock);
            if(nextState != this.gameState)
            {
                this.gameState.SetActive(false);
                nextState.SetActive(true);
                this.gameState = nextState;
            }
            sharpGui.End();
            sky.UpdateLight(clock);
            rtInstances.UpdateSprites(clock);

            //pbrRenderAttribs.AverageLogLum = sky.AverageLogLum;
            //Upate sun here

            rayTracingRenderer.Render(flyCamera.Position, flyCamera.Orientation, new Vector4(0, 20f, -10f, 0f), new Vector4(0f, 20f, -10f, 0f));

            var pRTV = swapChain.GetCurrentBackBufferRTV();
            var pDSV = swapChain.GetDepthBufferDSV();
            immediateContext.SetRenderTarget(pRTV, pDSV, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);

            //var ClearColor = new Color(0.350f, 0.350f, 0.350f, 1.0f);

            // Clear the back buffer
            // Let the engine perform required state transitions
            //immediateContext.ClearRenderTarget(pRTV, ClearColor, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            immediateContext.ClearDepthStencil(pDSV, CLEAR_DEPTH_STENCIL_FLAGS.CLEAR_DEPTH_FLAG, 1.0f, 0, RESOURCE_STATE_TRANSITION_MODE.RESOURCE_STATE_TRANSITION_MODE_TRANSITION);
            sharpGui.Render(immediateContext);

            this.swapChain.Present(1);

            objectResolverFactory.Flush();
        }
    }
}
