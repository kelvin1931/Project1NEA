using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Project1NEA
{
    class Texture 
    {
        public int Handle {  get; set; }

        public void Use()
        {
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public Texture(string path)
        {
            Handle = GL.GenTexture();

            Use();

            StbImage.stbi_set_flip_vertically_on_load(1);
            ImageResult image = ImageResult.FromStream(File.OpenRead(path), ColorComponents.RedGreenBlueAlpha);


            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }


    }
}
