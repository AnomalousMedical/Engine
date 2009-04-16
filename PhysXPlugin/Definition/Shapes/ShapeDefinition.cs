using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysXWrapper;
using Engine.Editing;
using Engine.Reflection;
using Engine.Saving;

namespace PhysXPlugin
{
    public class ShapeDefinition : Saveable
    {
        private PhysShapeDesc shapeDesc;
        private static MemberScanner shapeMemberScanner;

        static ShapeDefinition()
        {
            shapeMemberScanner = new MemberScanner();
            shapeMemberScanner.ProcessFields = false;
        }

        protected ShapeDefinition(PhysShapeDesc shapeDesc)
        {
            this.shapeDesc = shapeDesc;
        }

        public EditInterface getEditInterface()
        {
            return ReflectedEditInterface.createEditInterface(shapeDesc, shapeMemberScanner, shapeDesc.GetType().Name, null);
        }

        public PhysShapeDesc PhysShapeDesc
        {
            get
            {
                return shapeDesc;
            }
        }

        #region Saveable Members

        protected void restoreShape(LoadInfo info)
        {
            ReflectedSaver.RestoreObject(shapeDesc, info, shapeMemberScanner);
        }

        public void getInfo(SaveInfo info)
        {
            ReflectedSaver.SaveObject(shapeDesc, info, shapeMemberScanner);
        }

        #endregion
    }
}
