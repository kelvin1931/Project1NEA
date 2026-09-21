using System.IO;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace Project1NEA
{
    class Texture
    {
        public int Handle { get; private set; }

        public void Use(TextureUnit unit)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }


        public Texture(string path, bool generateMipmaps = true)
        {

            int textureHandle = GL.GenTexture();
            Handle = textureHandle;

            GL.BindTexture(TextureTarget.Texture2D, Handle);



            TextureWrapMode wrapMode = generateMipmaps ? TextureWrapMode.Repeat : TextureWrapMode.ClampToEdge;
            TextureMinFilter minFilter = generateMipmaps ? TextureMinFilter.LinearMipmapLinear : TextureMinFilter.Linear;

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapMode);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)minFilter);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            StbImage.stbi_set_flip_vertically_on_load(1);
            FileStream fileStream = File.OpenRead(path);
            ImageResult image = ImageResult.FromStream(fileStream, ColorComponents.RedGreenBlueAlpha);
            fileStream.Close();




            GL.TexImage2D(TextureTarget.Texture2D,0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);




            if (generateMipmaps)
            {
                GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            }

        }
    }
}
