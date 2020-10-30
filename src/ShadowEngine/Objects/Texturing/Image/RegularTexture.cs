﻿using ShadowBuild.Exceptions;
using System.Drawing;

namespace ShadowBuild.Objects.Texturing.Image
{
    /// <summary>
    /// Regular texture class.
    /// With this class you can create standard image textures.
    /// </summary>
    public class RegularTexture : ImageTexture
    {
        //Empty constructor for deserialization
        public RegularTexture() { }

        /// <summary>
        /// Regular texture constructor
        /// </summary>
        /// <param name="name">texture name</param>
        /// <param name="imgPath">path to image</param>
        public RegularTexture(string name, string imgPath)
        {
            this.Name = name;
            this.ImagePath = imgPath;
            InitializeImage();
        }

        public override void Render(Graphics g, TexturedObject obj, System.Windows.Point cameraPos)
        {
            RegularTexture tex = (RegularTexture)obj.ActualTexture;
            g.InterpolationMode = tex.InterpolationMode;

            if (tex.Image == null) throw new RenderException("Image was not initialized in texture \"" + tex.Name);

            g.DrawImage(
                tex.Image,
                new Rectangle(
                    new Point(
                        (int)(obj.GetStartPosition().X - cameraPos.X),
                        (int)(obj.GetStartPosition().Y - cameraPos.Y)
                    ), new Size(
                        (int)(obj.SizeMultipler.Width * obj.BaseSize.Width),
                        (int)(obj.SizeMultipler.Height * obj.BaseSize.Height)
                        )
                    )
                );
        }
    }
}
