using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Helpers
{
    public interface IDebugble
    {
        bool IsDebug { get; set; }
        GraphicsDevice GraphicsDevice { get; set; }
        void DrawBorder(Texture2D pixel, Rectangle rectangleToDraw, int thicknessOfBorder, Color borderColor, SpriteBatch spriteBatch);

    }
}
