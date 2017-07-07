using Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Engine.Platform;
using Engine.Editing;
using OgrePlugin;
using Engine.Attributes;

namespace Anomalous.SidescrollerCore
{
    class PlayAnimation : Behavior
    {
        [Editable]
        public String Node { get; set; }

        [Editable]
        public String Entity { get; set; }

        [Editable]
        public String Animation { get; set; }

        [DoNotCopy]
        [DoNotSave]
        private AnimationState animation;

        public PlayAnimation()
        {
            Node = "Node";
            Entity = "Entity";
        }

        protected override void link()
        {
            var node = Owner.getElement(Node) as SceneNodeElement;
            if(node == null)
            {
                blacklist($"Cannot find node '{Node}'");
            }

            var entity = node.getNodeObject(Entity) as Entity;
            if(entity == null)
            {
                blacklist($"Cannot find entity '{Entity}' on node '{Node}'");
            }

            animation = entity.getAnimationState(Animation);
            if (animation == null)
            {
                blacklist($"Cannot find animation '{Animation}'");
            }

            animation.setEnabled(true);

            base.link();
        }

        public override void update(Clock clock, EventManager eventManager)
        {
            animation.addTime(clock.DeltaSeconds);
        }
    }
}
