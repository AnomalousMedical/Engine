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
        SharpImGuiVertex[] quadVerts;
        private int currentQuad = 0;
        private int maxNumberOfQuads;

        public SharpGuiBuffer(OSWindow osWindow, ILogger<SharpGuiBuffer> logger, SharpGuiOptions options)
        {
            this.maxNumberOfQuads = options.MaxNumberOfQuads;
            quadVerts = new SharpImGuiVertex[maxNumberOfQuads * 4];

            this.osWindow = osWindow;
            this.logger = logger;
        }

        public void Begin()
        {
            currentQuad = 0;
            NumQuadIndices = 0;
        }

        public void DrawQuad(int x, int y, int width, int height, Color color)
        {
            if(currentQuad >= quadVerts.Length)
            {
                logger.LogWarning($"Exceeded maximum number of quads '{quadVerts.Length / 4}'.");
                return;
            }

            float left = x / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float right = (x + width) / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float top = y / (float)osWindow.WindowHeight * -2.0f + 1.0f;
            float bottom = (y + height) / (float)osWindow.WindowHeight * -2.0f + 1.0f;

            float z = 1.0f - (float)currentQuad / (float)maxNumberOfQuads - 1.0f / maxNumberOfQuads;
            quadVerts[currentQuad].pos = new Vector3(left, top, z);
            quadVerts[currentQuad + 1].pos = new Vector3(right, top, z);
            quadVerts[currentQuad + 2].pos = new Vector3(right, bottom, z);
            quadVerts[currentQuad + 3].pos = new Vector3(left, bottom, z);

            quadVerts[currentQuad].color = color;
            quadVerts[currentQuad + 1].color = color;
            quadVerts[currentQuad + 2].color = color;
            quadVerts[currentQuad + 3].color = color;

            currentQuad += 4;
            NumQuadIndices += 6;
        }

        public uint NumQuadIndices { get; private set; }

        internal SharpImGuiVertex[] QuadVerts => quadVerts;
    }
}
