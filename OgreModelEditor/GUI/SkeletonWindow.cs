using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OgrePlugin;
using WeifenLuo.WinFormsUI.Docking;

namespace OgreModelEditor
{
    public partial class SkeletonWindow : DockContent
    {
        public SkeletonWindow()
        {
            InitializeComponent();
        }

        public void setSkeleton(Entity entity)
        {
            SkeletonInstance skeleton = entity.getSkeleton();
            skeletonTree.Nodes.Clear();
            for (ushort i = 0; i < skeleton.getNumBones(); i++)
            {
                Bone bone = skeleton.getBone(i);
                TreeNode skeletonNode = new TreeNode(bone.getName());
                TreeNode positionNode = new TreeNode("Position " + bone.getPosition());
                skeletonNode.Nodes.Add(positionNode);
                TreeNode rotationNode = new TreeNode("Rotation" + bone.getOrientation());
                skeletonNode.Nodes.Add(rotationNode);
                skeletonTree.Nodes.Add(skeletonNode);
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
        }

        public void clearSkeleton()
        {
            skeletonTree.Nodes.Clear();
        }
    }
}
