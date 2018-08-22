using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Levels
{
    public enum TileCollision
    {
        Passable = 0,
        Impassable = 1,
        Platform = 2,
    }


    struct Tile
    {
        public Texture2D Texture;
        public TileCollision  Collision { get; set; }
        public const int Width =64;
        public const int Height = 64;
        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Tile(Texture2D texture, TileCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}
