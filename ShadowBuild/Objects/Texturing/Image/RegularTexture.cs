﻿using ShadowBuild.Exceptions;
using System.Drawing;

namespace ShadowBuild.Objects.Texturing.Image
{
    public class RegularTexture : ImageTexture
    {
        //Empty constructor for deserialization
        public RegularTexture() { }
        public RegularTexture(string name, string imgPath)
        {
            this.Name = name;
            this.ImagePath = imgPath;
            InitializeImage();
        }

        public override System.Windows.Size GetSize()
        {
            return new System.Windows.Size(this.Image.Width, this.Image.Height);
        }

        public override void Render(Graphics g, TexturedObject obj, System.Windows.Point cameraPos)
        {

            RegularTexture tex = (RegularTexture)obj.ActualTexture;

            if (tex.Image == null) throw new RenderException("Image was not initialized in texture \"" + tex.Name);

            g.DrawImage(
                tex.Image,
                new Rectangle(
                    new Point(
                        (int)(obj.GetStartPosition().X - cameraPos.X),
                        (int)(obj.GetStartPosition().Y - cameraPos.Y)
                    ), new Size(
                        (int)(tex.Image.Width * obj.Size.Width),
                        (int)(tex.Image.Height * obj.Size.Height)
                        )
                    )
                );
        }
    }
}
