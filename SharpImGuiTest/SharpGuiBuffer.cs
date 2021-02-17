using DiligentEngine;
using Engine;
using Engine.Platform;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpImGuiTest
{
    public class SharpGuiBuffer
    {
        private readonly OSWindow osWindow;
        private readonly ILogger<SharpGuiBuffer> logger;
        public const int MaxNumberOfQuads = 1000;
        SharpImGuiVertex[] verts;
        private int currentQuad = 0;

        public SharpGuiBuffer(OSWindow osWindow, ILogger<SharpGuiBuffer> logger)
        {
            verts = new SharpImGuiVertex[MaxNumberOfQuads * 4];

            this.osWindow = osWindow;
            this.logger = logger;
        }

        public void Begin()
        {
            currentQuad = 0;
            NumIndices = 0;
        }

        public void DrawQuad(int x, int y, int width, int height, Color color)
        {
            if(currentQuad >= verts.Length)
            {
                logger.LogWarning($"Exceeded maximum number of quads '{verts.Length / 4}'.");
                return;
            }

            float left = x / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float right = (x + width) / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float top = y / (float)osWindow.WindowHeight * -2.0f + 1.0f;
            float bottom = (y + height) / (float)osWindow.WindowHeight * -2.0f + 1.0f;

            float z = 1.0f - (float)currentQuad / (float)MaxNumberOfQuads;
            verts[currentQuad].pos = new Vector3(left, top, z);
            verts[currentQuad + 1].pos = new Vector3(right, top, z);
            verts[currentQuad + 2].pos = new Vector3(right, bottom, z);
            verts[currentQuad + 3].pos = new Vector3(left, bottom, z);

            verts[currentQuad].color = color;
            verts[currentQuad + 1].color = color;
            verts[currentQuad + 2].color = color;
            verts[currentQuad + 3].color = color;

            currentQuad += 4;
            NumIndices += 6;
        }

        public uint NumIndices { get; private set; }

        internal SharpImGuiVertex[] Verts => verts;
    }
}
