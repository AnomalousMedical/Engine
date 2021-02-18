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
        private uint currentQuad = 0;
        private uint maxNumberOfQuads;

        SharpImGuiTextVertex[] textVerts;
        private uint currentText = 0;
        private uint maxNumberOfTextQuads;

        public SharpGuiBuffer(OSWindow osWindow, ILogger<SharpGuiBuffer> logger, SharpGuiOptions options)
        {
            this.maxNumberOfQuads = options.MaxNumberOfQuads;
            this.maxNumberOfTextQuads = options.MaxNumberOfTextQuads;

            quadVerts = new SharpImGuiVertex[maxNumberOfQuads * 4];
            textVerts = new SharpImGuiTextVertex[maxNumberOfTextQuads * 4];

            this.osWindow = osWindow;
            this.logger = logger;
        }

        public void Begin()
        {
            currentText = 0;
            currentQuad = 0;
            NumQuadIndices = 0;
            NumTextIndices = 0;
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

        public void DrawTextQuad(int x, int y, int width, int height, Color color, GlyphRect uvRect)
        {
            if (currentText >= textVerts.Length)
            {
                logger.LogWarning($"Exceeded maximum number of text quads '{textVerts.Length / 4}'.");
                return;
            }

            float left = x / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float right = (x + width) / (float)osWindow.WindowWidth * 2.0f - 1.0f;
            float top = y / (float)osWindow.WindowHeight * -2.0f + 1.0f;
            float bottom = (y + height) / (float)osWindow.WindowHeight * -2.0f + 1.0f;

            float z = 1.0f - (float)currentText / (float)maxNumberOfTextQuads - 1.0f / maxNumberOfTextQuads; //This won't be right, need to work on z index
            textVerts[currentText].pos = new Vector3(left, top, z);
            textVerts[currentText + 1].pos = new Vector3(right, top, z);
            textVerts[currentText + 2].pos = new Vector3(right, bottom, z);
            textVerts[currentText + 3].pos = new Vector3(left, bottom, z);
            
            textVerts[currentText].color = color;
            textVerts[currentText + 1].color = color;
            textVerts[currentText + 2].color = color;
            textVerts[currentText + 3].color = color;

            textVerts[currentText].uv = new Vector2(uvRect.Left, uvRect.Top);
            textVerts[currentText + 1].uv = new Vector2(uvRect.Right, uvRect.Top);
            textVerts[currentText + 2].uv = new Vector2(uvRect.Right, uvRect.Bottom);
            textVerts[currentText + 3].uv = new Vector2(uvRect.Left, uvRect.Bottom);

            currentText += 4;
            NumTextIndices += 6;
        }

        public uint NumQuadIndices { get; private set; }

        public uint NumTextIndices { get; private set; }

        internal SharpImGuiVertex[] QuadVerts => quadVerts;

        internal SharpImGuiTextVertex[] TextVerts => textVerts;

        // vertices

        // -1, +1 --------------- +1, +1
        //    |  0               1   |
        //    |                      |
        //    |                      |
        //    |                      |
        //    |                      |
        //    |  3               2   |
        // -1, -1 --------------- +1, -1
    }
}
