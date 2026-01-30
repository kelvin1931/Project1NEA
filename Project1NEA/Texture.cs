using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Project1NEA
{
    class Texture
    {
        public int Handle { get; private set; }

        // Bind the texture to a texture unit
        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        // Constructor: load texture from file
        public Texture(string path)
        {
            // Generate texture handle
            int textureHandle = GL.GenTexture();
            Handle = textureHandle;

            // Bind texture
            GL.BindTexture(TextureTarget.Texture2D, Handle);

            // Set texture parameters
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Load image
            StbImage.stbi_set_flip_vertically_on_load(1);
            FileStream fileStream = File.OpenRead(path);
            ImageResult image = ImageResult.FromStream(fileStream, ColorComponents.RedGreenBlueAlpha);
            fileStream.Close();

            // Upload image to GPU
            GL.TexImage2D(TextureTarget.Texture2D,0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);

            // Generate mipmaps
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        }
    }
}
