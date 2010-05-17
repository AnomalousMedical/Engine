using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace BulletPlugin
{
    class BulletShapeFileManager : ShapeFileManager
    {
        public BulletShapeFileManager(BulletShapeRepository repository, BulletShapeBuilder builder)
            :base(repository, builder)
        {
            ShapeBuilder = builder;
            ShapeRepository = repository;
            ShapeBuilder.setRepository(ShapeRepository);
        }

        protected override void loadStarted()
        {
            
        }

        protected override void loadEnded()
        {
            
        }

        public BulletShapeRepository ShapeRepository { get; private set; }

        public BulletShapeBuilder ShapeBuilder { get; private set; }
    }
}
