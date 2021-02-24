using DiligentEngine;
using System;

namespace SceneTest
{
    public interface ISpriteMaterial : IDisposable
    {
        int ImageHeight { get; }
        int ImageWidth { get; }
        IShaderResourceBinding ShaderResourceBinding { get; }
    }
}