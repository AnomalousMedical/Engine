using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;
using Engine.ObjectManagement;
using PhysXWrapper;
using Engine.Renderer;

namespace PhysXPlugin
{
    class PhysXDebugInterface : DebugInterface
    {
        private List<DebugEntry> debugEntries = new List<DebugEntry>();

        public PhysXDebugInterface()
        {
            debugEntries.Add(new PhysXDebugEntry("World Axes", PhysParameter.NX_VISUALIZE_WORLD_AXES));
            debugEntries.Add(new PhysXDebugEntry("Body Axes", PhysParameter.NX_VISUALIZE_BODY_AXES));
            debugEntries.Add(new PhysXDebugEntry("Body Mass Axes", PhysParameter.NX_VISUALIZE_BODY_MASS_AXES));
            debugEntries.Add(new PhysXDebugEntry("Body Linear Velocity", PhysParameter.NX_VISUALIZE_BODY_LIN_VELOCITY));
            debugEntries.Add(new PhysXDebugEntry("Body Angular Velocity", PhysParameter.NX_VISUALIZE_BODY_ANG_VELOCITY));
            debugEntries.Add(new PhysXDebugEntry("Body Joint Groups", PhysParameter.NX_VISUALIZE_BODY_JOINT_GROUPS));
            debugEntries.Add(new PhysXDebugEntry("Joint Local Axes", PhysParameter.NX_VISUALIZE_JOINT_LOCAL_AXES));
            debugEntries.Add(new PhysXDebugEntry("Joint World Axes", PhysParameter.NX_VISUALIZE_JOINT_WORLD_AXES));
            debugEntries.Add(new PhysXDebugEntry("Joint Limits", PhysParameter.NX_VISUALIZE_JOINT_LIMITS));
            debugEntries.Add(new PhysXDebugEntry("Contact Points", PhysParameter.NX_VISUALIZE_CONTACT_POINT));
            debugEntries.Add(new PhysXDebugEntry("Contact Normal", PhysParameter.NX_VISUALIZE_CONTACT_NORMAL));
            debugEntries.Add(new PhysXDebugEntry("Contact Error", PhysParameter.NX_VISUALIZE_CONTACT_ERROR));
            debugEntries.Add(new PhysXDebugEntry("Contact Force", PhysParameter.NX_VISUALIZE_CONTACT_FORCE));
            debugEntries.Add(new PhysXDebugEntry("Actor Axes", PhysParameter.NX_VISUALIZE_ACTOR_AXES));
            debugEntries.Add(new PhysXDebugEntry("Collision AABB", PhysParameter.NX_VISUALIZE_COLLISION_AABBS));
            debugEntries.Add(new PhysXDebugEntry("Shapes", PhysParameter.NX_VISUALIZE_COLLISION_SHAPES));
            debugEntries.Add(new PhysXDebugEntry("Shape Axes", PhysParameter.NX_VISUALIZE_COLLISION_AXES));
            debugEntries.Add(new PhysXDebugEntry("Compound AABB", PhysParameter.NX_VISUALIZE_COLLISION_COMPOUNDS));
            debugEntries.Add(new PhysXDebugEntry("Mesh/Convex Vertex Normals", PhysParameter.NX_VISUALIZE_COLLISION_VNORMALS));
            debugEntries.Add(new PhysXDebugEntry("Mesh/Convex Face Normals", PhysParameter.NX_VISUALIZE_COLLISION_FNORMALS));
            debugEntries.Add(new PhysXDebugEntry("Mesh Active Edges", PhysParameter.NX_VISUALIZE_COLLISION_EDGES));
            debugEntries.Add(new PhysXDebugEntry("Bounding Spheres", PhysParameter.NX_VISUALIZE_COLLISION_SPHERES));
            debugEntries.Add(new PhysXDebugEntry("Static Pruning Structure", PhysParameter.NX_VISUALIZE_COLLISION_STATIC));
            debugEntries.Add(new PhysXDebugEntry("Dynamic Pruning Structure", PhysParameter.NX_VISUALIZE_COLLISION_DYNAMIC));
            debugEntries.Add(new PhysXDebugEntry("Free Pruning Structures", PhysParameter.NX_VISUALIZE_COLLISION_FREE));
            debugEntries.Add(new PhysXDebugEntry("CCD Tests", PhysParameter.NX_VISUALIZE_COLLISION_CCD));
            debugEntries.Add(new PhysXDebugEntry("CCD Skeletons", PhysParameter.NX_VISUALIZE_COLLISION_SKELETONS));
            debugEntries.Add(new PhysXDebugEntry("Fluid Emitters", PhysParameter.NX_VISUALIZE_FLUID_EMITTERS));
            debugEntries.Add(new PhysXDebugEntry("Fluid Particle Positions", PhysParameter.NX_VISUALIZE_FLUID_POSITION));
            debugEntries.Add(new PhysXDebugEntry("Fluid Particle Velocity", PhysParameter.NX_VISUALIZE_FLUID_VELOCITY));
            debugEntries.Add(new PhysXDebugEntry("Fluid Particle Kernal Radius", PhysParameter.NX_VISUALIZE_FLUID_KERNEL_RADIUS));
            debugEntries.Add(new PhysXDebugEntry("Fluid AABB", PhysParameter.NX_VISUALIZE_FLUID_BOUNDS));
            debugEntries.Add(new PhysXDebugEntry("Fluid Packets", PhysParameter.NX_VISUALIZE_FLUID_PACKETS));
            debugEntries.Add(new PhysXDebugEntry("Fluid Motion Limits", PhysParameter.NX_VISUALIZE_FLUID_MOTION_LIMIT));
            debugEntries.Add(new PhysXDebugEntry("Fluid Dynamic Mesh Collision", PhysParameter.NX_VISUALIZE_FLUID_DYN_COLLISION));
            debugEntries.Add(new PhysXDebugEntry("Avaliable Fluid Mesh Packets", PhysParameter.NX_VISUALIZE_FLUID_MESH_PACKETS));
            debugEntries.Add(new PhysXDebugEntry("Fluid Drain Shapes", PhysParameter.NX_VISUALIZE_FLUID_DRAINS));
            debugEntries.Add(new PhysXDebugEntry("Fluid Data Packets", PhysParameter.NX_VISUALIZE_FLUID_PACKET_DATA));
            debugEntries.Add(new PhysXDebugEntry("Cloth Meshes", PhysParameter.NX_VISUALIZE_CLOTH_MESH));
            debugEntries.Add(new PhysXDebugEntry("Cloth Rigid Body Collision", PhysParameter.NX_VISUALIZE_CLOTH_COLLISIONS));
            debugEntries.Add(new PhysXDebugEntry("Cloth Self Collision", PhysParameter.NX_VISUALIZE_CLOTH_SELFCOLLISIONS));
            debugEntries.Add(new PhysXDebugEntry("Cloth Clustering for PPU", PhysParameter.NX_VISUALIZE_CLOTH_WORKPACKETS));
            debugEntries.Add(new PhysXDebugEntry("Cloth Sleeping", PhysParameter.NX_VISUALIZE_CLOTH_SLEEP));
            debugEntries.Add(new PhysXDebugEntry("Cloth Sleeping With Full Information", PhysParameter.NX_VISUALIZE_CLOTH_SLEEP_VERTEX));
            debugEntries.Add(new PhysXDebugEntry("Tearable Cloth Vertices", PhysParameter.NX_VISUALIZE_CLOTH_TEARABLE_VERTICES));
            debugEntries.Add(new PhysXDebugEntry("Cloth Tearing", PhysParameter.NX_VISUALIZE_CLOTH_TEARING));
            debugEntries.Add(new PhysXDebugEntry("Cloth Attachments", PhysParameter.NX_VISUALIZE_CLOTH_ATTACHMENT));
            debugEntries.Add(new PhysXDebugEntry("Cloth Valid Bounds", PhysParameter.NX_VISUALIZE_CLOTH_VALIDBOUNDS));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Meshes", PhysParameter.NX_VISUALIZE_SOFTBODY_MESH));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Collisions With Rigid Bodies", PhysParameter.NX_VISUALIZE_SOFTBODY_COLLISIONS));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Clustering for PPU", PhysParameter.NX_VISUALIZE_SOFTBODY_WORKPACKETS));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Sleeping", PhysParameter.NX_VISUALIZE_SOFTBODY_SLEEP));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Sleeping With Full Information", PhysParameter.NX_VISUALIZE_SOFTBODY_SLEEP_VERTEX));
            debugEntries.Add(new PhysXDebugEntry("Tearable Soft Body Vertices", PhysParameter.NX_VISUALIZE_SOFTBODY_TEARABLE_VERTICES));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Tearing", PhysParameter.NX_VISUALIZE_SOFTBODY_TEARING));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Attachments", PhysParameter.NX_VISUALIZE_SOFTBODY_ATTACHMENT));
            debugEntries.Add(new PhysXDebugEntry("Soft Body Valid Bounds", PhysParameter.NX_VISUALIZE_SOFTBODY_VALIDBOUNDS));
        }

        public IEnumerable<DebugEntry> getEntries()
        {
            return debugEntries;
        }

        public void renderDebug(DebugDrawingSurface drawingSurface, SimSubScene subScene)
        {
            PhysXSceneManager sceneManager = subScene.getSimElementManager<PhysXSceneManager>();
            if (sceneManager != null)
            {
                PhysDebugRenderable data = sceneManager.PhysScene.getDebugRenderable();

                unsafe
                {
                    //Render Points
	                uint nbPoints = data.getNbPoints();
	                PhysDebugPoint* points = data.getPoints();

                    drawingSurface.begin(subScene.Name + "PhysPoints", DrawingType.PointList);
                    while (nbPoints-- != 0)
	                {
		                drawingSurface.setColor(Color.FromARGB(points->color));
                        drawingSurface.drawPoint(points->p);
		                points++;
	                }
                    drawingSurface.end();

	                //Render lines
	                uint nbLines = data.getNbLines();
	                PhysDebugLine* lines = data.getLines();

                    drawingSurface.begin(subScene.Name + "PhysLines", DrawingType.LineList);
                    while (nbLines-- != 0)
	                {
                        drawingSurface.setColor(Color.FromARGB(lines->color));
                        drawingSurface.drawLine(lines->p0, lines->p1);
		                lines++;
	                }
                    drawingSurface.end();

	                //Render triangles
	                uint nbTris = data.getNbTriangles();
	                PhysDebugTriangle* triangles = data.getTriangles();

                    drawingSurface.begin(subScene.Name + "PhysTriangles", DrawingType.TriangleList);
                    while (nbTris-- != 0)
	                {
                        drawingSurface.setColor(Color.FromARGB(triangles->color));
                        drawingSurface.drawTriangle(triangles->p0, triangles->p1, triangles->p2);
		                triangles++;
	                }
                    drawingSurface.end();
                }
            }
        }

        public void setEnabled(bool enabled)
        {
            PhysSDK.Instance.setParameter(PhysParameter.NX_VISUALIZATION_SCALE, enabled ? 1.0f : 0.0f);
        }

        public String Name
        {
            get
            {
                return "PhysX Debug";
            }
        }
    }
}
