using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using static Project1NEA.Shaders;

namespace Project1NEA
{
    /// <summary>
    /// Draws strings using a bitmap font atlas. The atlas holds the printable
    /// ASCII characters in a fixed grid, so the cell for a character can be
    /// calculated directly from its character code rather than searched for.
    /// </summary>
    public class TextRenderer
    {
        // The atlas covers ASCII 32 (space) to 127, laid out left to right and
        // top to bottom, which is exactly 96 cells.
        private const int FirstCharacter = 32;
        private const int Columns = 16;
        private const int Rows = 6;
        private const int CellPixels = 64;

        private const int AtlasWidth = Columns * CellPixels;
        private const int AtlasHeight = Rows * CellPixels;

        // Consolas is monospaced, so every glyph steps forward by the same
        // amount. The font was drawn at 48pt into a 64px cell, giving a 26px
        // advance, and this ratio keeps the spacing correct at any scale.
        private const float AdvanceRatio = 0.40625f;

        // Half a texel. Without this a glyph samples the very edge of its cell
        // and picks up a sliver of the neighbouring character.
        private const float Inset = 0.5f;

        private const int FloatsPerVertex = 4;   // x, y, u, v
        private const int VerticesPerGlyph = 6;  // two triangles

        private readonly Shader shader;
        private readonly Texture atlas;

        private readonly int vao;
        private readonly int vbo;

        // Reused between draw calls so that rendering text does not allocate
        // a new array every frame.
        private float[] vertices = new float[0];

        private Matrix4 projection;

        public TextRenderer(string atlasPath)
        {
            shader = new Shader("text.vert", "text.frag");
            atlas = new Texture(atlasPath, generateMipmaps: false);

            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            int stride = FloatsPerVertex * sizeof(float);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            shader.Use();
            shader.SetInt("fontAtlas", 0);
        }

        /// <summary>
        /// Rebuilds the screen-space projection. Text is positioned in pixels
        /// with the origin at the top left, so it keeps its shape when the
        /// window is resized rather than stretching with the aspect ratio.
        /// </summary>
        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                return;
            }

            projection = Matrix4.CreateOrthographicOffCenter(0.0f, width, height, 0.0f, -1.0f, 1.0f);
        }

        public float LineHeight(float scale)
        {
            return CellPixels * scale;
        }

        public float MeasureWidth(string text, float scale)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0.0f;
            }

            return text.Length * CellPixels * AdvanceRatio * scale;
        }

        public void DrawCentred(string text, float centreX, float y, float scale, Vector4 colour)
        {
            Draw(text, centreX - MeasureWidth(text, scale) / 2.0f, y, scale, colour);
        }

        public void Draw(string text, float x, float y, float scale, Vector4 colour)
        {
            if (string.IsNullOrEmpty(text))
            {
                return;
            }

            int requiredFloats = text.Length * VerticesPerGlyph * FloatsPerVertex;
            if (vertices.Length < requiredFloats)
            {
                vertices = new float[requiredFloats];
            }

            float advance = CellPixels * AdvanceRatio * scale;
            float size = CellPixels * scale;
            int floatCount = 0;

            foreach (char character in text)
            {
                int index = character - FirstCharacter;

                // Anything outside the atlas is skipped, but still advances the
                // cursor so the rest of the line stays aligned.
                if (index < 0 || index >= Columns * Rows)
                {
                    x += advance;
                    continue;
                }

                int column = index % Columns;
                int row = index / Columns;

                float u0 = (column * CellPixels + Inset) / AtlasWidth;
                float u1 = ((column + 1) * CellPixels - Inset) / AtlasWidth;

                // Textures are flipped vertically when they are loaded, so the
                // top row of the atlas ends up at v = 1.
                float v0 = 1.0f - (row * CellPixels + Inset) / AtlasHeight;
                float v1 = 1.0f - ((row + 1) * CellPixels - Inset) / AtlasHeight;

                float left = x;
                float right = x + size;
                float top = y;
                float bottom = y + size;

                AddVertex(ref floatCount, left, top, u0, v0);
                AddVertex(ref floatCount, right, top, u1, v0);
                AddVertex(ref floatCount, right, bottom, u1, v1);

                AddVertex(ref floatCount, right, bottom, u1, v1);
                AddVertex(ref floatCount, left, bottom, u0, v1);
                AddVertex(ref floatCount, left, top, u0, v0);

                x += advance;
            }

            if (floatCount == 0)
            {
                return;
            }

            // Glyphs are drawn from an alpha mask, so blending has to be on or
            // the transparent part of every cell is drawn as a solid block.
            bool depthWasEnabled = GL.IsEnabled(EnableCap.DepthTest);
            GL.Disable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            shader.Use();
            shader.SetMatrix4("projection", projection);
            shader.SetVector4("textColour", colour);
            atlas.Use(TextureUnit.Texture0);

            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, floatCount * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            // The whole string is one draw call rather than one call per letter.
            GL.DrawArrays(PrimitiveType.Triangles, 0, floatCount / FloatsPerVertex);

            GL.Disable(EnableCap.Blend);
            if (depthWasEnabled)
            {
                GL.Enable(EnableCap.DepthTest);
            }
        }

        private void AddVertex(ref int floatCount, float x, float y, float u, float v)
        {
            vertices[floatCount++] = x;
            vertices[floatCount++] = y;
            vertices[floatCount++] = u;
            vertices[floatCount++] = v;
        }
    }
}
