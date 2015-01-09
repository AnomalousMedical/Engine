using Anomalous.GuiFramework;
using MyGUIPlugin;
using OgrePlugin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OgreModelEditor
{
    class SkeletonWindow : MDIDialog
    {
        private ScrollView scrollView;
        private Tree skeletonTree;

        public SkeletonWindow()
            :base("OgreModelEditor.GUI.SkeletonWindow.SkeletonWindow.layout")
        {
            scrollView = window.findWidget("Scroller") as ScrollView;
            skeletonTree = new Tree(scrollView);

            this.Resized += SkeletonWindow_Resized;
        }

        public override void Dispose()
        {
            skeletonTree.Dispose();
            base.Dispose();
        }

        public void setSkeleton(Entity entity)
        {
            SkeletonInstance skeleton = entity.getSkeleton();
            skeletonTree.Nodes.clear();
            for (ushort i = 0; i < skeleton.getNumBones(); i++)
            {
                Bone bone = skeleton.getBone(i);
                TreeNode skeletonNode = new TreeNode(bone.getName());
                TreeNode positionNode = new TreeNode("Position " + bone.getPosition());
                skeletonNode.Children.add(positionNode);
                TreeNode rotationNode = new TreeNode("Rotation" + bone.getOrientation());
                skeletonNode.Children.add(rotationNode);
                skeletonTree.Nodes.add(skeletonNode);
            }
            using (MeshPtr mesh = entity.getMesh())
            {
                mesh.Value._updateCompiledBoneAssignments();
                if (mesh.Value.SharedVertexData != null)
                {
                    //Logging.Log.Debug("Shared Bone Assignments {0}", mesh.Value.SharedBoneAssignmentCount);
                }
                else
                {
                    ushort numSubMeshes = mesh.Value.getNumSubMeshes();
                    for (ushort i = 0; i < numSubMeshes; ++i)
                    {
                        SubMesh subMesh = mesh.Value.getSubMesh(i);
                        VertexData vertexData = subMesh.vertexData;
                        VertexDeclaration vertexDeclaration = vertexData.vertexDeclaration;
                        VertexElement elem = vertexDeclaration.findElementBySemantic(VertexElementSemantic.VES_BLEND_WEIGHTS);
                        if (elem != null)
                        {
                            Logging.Log.Debug("Sub Mesh {0} Bone Assignments {1}", i, VertexElement.getTypeCount(elem.getType()));
                        }
                    }
                }
            }
            skeletonTree.layout();
        }

        public void clearSkeleton()
        {
            skeletonTree.Nodes.clear();
            skeletonTree.layout();
        }

        void SkeletonWindow_Resized(object sender, EventArgs e)
        {
            skeletonTree.layout();
        }
    }
}
