using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using PhysXWrapper;
using EngineMath;

namespace PhysXPlugin.Commands
{
    /// <summary>
    /// This command will build a new PhysScene.
    /// </summary>
    class CreatePhysSceneCommand : EngineCommand
    {
        #region Delegates

        delegate PhysXSceneManager CreateDefaultScene();

        #endregion Delegates

        #region Static

        private const String NAME = "createPhysScene";
        private const String PRETTY_NAME = "Create PhysX Scene";
        private const String HELP = "Creates a new PhysX scene with the given parameters.";

        #endregion Static

        #region Fields

        private PhysSDK physSDK;
        private PhysFactory factory;

        #endregion Fields

        #region Constructors

        public CreatePhysSceneCommand(PhysSDK physSDK, PhysFactory factory)
            :base(NAME, PRETTY_NAME, HELP)
        {
            this.physSDK = physSDK;
            this.factory = factory;
            this.addDelegate(new CreateDefaultScene(createScene));
        }

        #endregion Constructors

        #region Functions

        private PhysXSceneManager createScene()
        {
            using (PhysSceneDesc sceneDesc = new PhysSceneDesc())
            {
                sceneDesc.Gravity = new Vector3(0.0f, -9.8f, 0.0f);
                PhysScene scene = physSDK.createScene(sceneDesc);
                return new PhysXSceneManager(scene, physSDK, factory);
            }
        }

        #endregion Functions
    }
}
