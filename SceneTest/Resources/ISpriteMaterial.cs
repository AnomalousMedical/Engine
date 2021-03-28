using DiligentEngine;
using System;

namespace SceneTest
{
    public interface ISpriteMaterial
    {
        int ImageHeight { get; }
        int ImageWidth { get; }
        IShaderResourceBinding ShaderResourceBinding { get; }
    }
}