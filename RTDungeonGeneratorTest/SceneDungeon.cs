﻿using DiligentEngine;
using DiligentEngine.RT;
using DungeonGenerator;
using Engine;
using RogueLikeMapBuilder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTDungeonGeneratorTest
{
    internal class SceneDungeon : IDisposable, IShaderTableBinder
    {
        public class Desc
        {
            public string InstanceName { get; set; } = Guid.NewGuid().ToString();

            public uint TextureIndex { get; set; } = 0;

            public byte Mask { get; set; } = RtStructures.OPAQUE_GEOM_MASK;

            public InstanceMatrix Transform = InstanceMatrix.Identity;

            public int Seed { get; set; }
        }

        private TLASBuildInstanceData wallInstanceData;
        private TLASBuildInstanceData floorInstanceData;
        private readonly RayTracingRenderer renderer;
        private readonly IDestructionRequest destructionRequest;
        private MapMesh mapMesh;
        private Task loadingTask;

        public SceneDungeon
        (
            Desc description,
            ICoroutineRunner coroutineRunner,
            RayTracingRenderer renderer,
            IDestructionRequest destructionRequest,
            MeshBLAS floorMesh,
            MeshBLAS wallMesh
        )
        {
            this.renderer = renderer;
            this.destructionRequest = destructionRequest;
            coroutineRunner.RunTask(async () =>
            {
                var completion = new TaskCompletionSource();
                loadingTask = completion.Task;
                try
                {
                    await Task.Run(() =>
                    {
                        using var destructionBlock = destructionRequest.BlockDestruction();
                        var sw = new Stopwatch();
                        sw.Start();
                        //Quick test with the console
                        var random = new Random(description.Seed);
                        var mapBuilder = new csMapbuilder(random, 50, 50);
                        mapBuilder.CorridorSpace = 10;
                        mapBuilder.RoomDistance = 3;
                        mapBuilder.Room_Min = new IntSize2(2, 2);
                        mapBuilder.Room_Max = new IntSize2(6, 6); //Between 3-6 is good here, 3 for more cityish with small rooms, 6 for more open with more big rooms, sometimes connected
                        mapBuilder.Corridor_Max = 4;
                        mapBuilder.Horizontal = false;
                        mapBuilder.Build_ConnectedStartRooms();
                        mapBuilder.AddNorthConnector();
                        mapBuilder.AddSouthConnector();
                        mapBuilder.AddWestConnector();
                        mapBuilder.AddEastConnector();
                        sw.Stop();
                        var map = mapBuilder.map;
                        var mapWidth = mapBuilder.Map_Size.Width;
                        var mapHeight = mapBuilder.Map_Size.Height;

                        for (int mapY = mapBuilder.Map_Size.Height - 1; mapY > -1; --mapY)
                        {
                            for (int mapX = 0; mapX < mapWidth; ++mapX)
                            {
                                switch (map[mapX, mapY])
                                {
                                    case csMapbuilder.EmptyCell:
                                        Console.Write(' ');
                                        break;
                                    case csMapbuilder.MainCorridorCell:
                                        Console.Write('M');
                                        break;
                                    case csMapbuilder.RoomCell:
                                        Console.Write('S');
                                        break;
                                    case csMapbuilder.RoomCell + 1:
                                        Console.Write('E');
                                        break;
                                    default:
                                        Console.Write('X');
                                        break;
                                }
                            }
                            Console.WriteLine();
                        }

                        for (int mapY = mapBuilder.Map_Size.Height - 1; mapY > -1; --mapY)
                        {
                            for (int mapX = 0; mapX < mapWidth; ++mapX)
                            {
                                if (map[mapX, mapY] == csMapbuilder.EmptyCell)
                                {
                                    Console.Write(' ');
                                }
                                else
                                {
                                    Console.Write(map[mapX, mapY]);
                                }
                            }
                            Console.WriteLine();
                        }
                        Console.WriteLine($"Level seed {description.Seed}");
                        Console.WriteLine($"Created in {sw.ElapsedMilliseconds}");
                        Console.WriteLine(mapBuilder.StartRoom);
                        Console.WriteLine(mapBuilder.EndRoom);
                        Console.WriteLine("--------------------------------------------------");

                        mapMesh = new MapMesh(mapBuilder, random, floorMesh, wallMesh, mapUnitY: 0.1f);
                    });

                    mapMesh.CreateMesh();

                    this.wallInstanceData = new TLASBuildInstanceData()
                    {
                        InstanceName = Guid.NewGuid().ToString(),
                        CustomId = 0, //Texture index
                        pBLAS = mapMesh.WallMesh.Instance.BLAS.Obj,
                        Mask = RtStructures.OPAQUE_GEOM_MASK,
                        Transform = new InstanceMatrix(Vector3.Zero, Quaternion.Identity)
                    };
                    this.floorInstanceData = new TLASBuildInstanceData()
                    {
                        InstanceName = Guid.NewGuid().ToString(),
                        CustomId = 0, //Texture index
                        pBLAS = mapMesh.FloorMesh.Instance.BLAS.Obj,
                        Mask = RtStructures.OPAQUE_GEOM_MASK,
                        Transform = new InstanceMatrix(Vector3.Zero, Quaternion.Identity)
                    };
                    renderer.AddTlasBuild(wallInstanceData);
                    renderer.AddTlasBuild(floorInstanceData);
                    renderer.AddShaderTableBinder(this);

                    completion.SetResult();
                }
                catch (Exception ex)
                {
                    completion.SetException(ex);
                }
            });
        }

        public void RequestDestruction()
        {
            destructionRequest.RequestDestruction();
        }

        public void Dispose()
        {
            renderer.RemoveShaderTableBinder(this);
            renderer.RemoveTlasBuild(floorInstanceData);
            renderer.RemoveTlasBuild(wallInstanceData);
        }

        public void SetTransform(InstanceMatrix matrix)
        {
            this.wallInstanceData.Transform = matrix;
            this.floorInstanceData.Transform = matrix;
        }

        public void Bind(IShaderBindingTable sbt, ITopLevelAS tlas)
        {
            sbt.BindHitGroupForInstance(tlas, wallInstanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, mapMesh.FloorMesh.ShaderGroupName, IntPtr.Zero);
            sbt.BindHitGroupForInstance(tlas, floorInstanceData.InstanceName, RtStructures.PRIMARY_RAY_INDEX, mapMesh.WallMesh.ShaderGroupName, IntPtr.Zero);
        }

        public Task LoadingTask => loadingTask;
    }
}
