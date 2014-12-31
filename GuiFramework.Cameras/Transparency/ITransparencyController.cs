using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Engine;

namespace Anomalous.GuiFramework.Cameras
{
    public interface ITransparencyController
    {
        void createTransparencyState(String name);

        void removeTransparencyState(String name);

        void applyTransparencyState(String name);

        /// <summary>
        /// This is the transparency state that will be modified when set alpha
        /// or blend functions are called. This does not mean that the scene is
        /// currently setup with this transparency, however. That must be
        /// modified by applyTransparencyState.
        /// </summary>
        String ActiveTransparencyState { get; set; }
    }
}
