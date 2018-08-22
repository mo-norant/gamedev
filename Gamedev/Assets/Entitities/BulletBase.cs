using Gamedev.Assets.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Entitities
{
    public abstract  class BulletBase
    {
        private Animation idleAnimation;
        private AnimationPlayer sprite;
        private Rectangle localBounds;
        public Direction direction;
        public Texture2D BulletTexture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 InitPosition { get; set; }
        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - sprite.Origin.X) + localBounds.X;
                int top = (int)Math.Round(Position.Y - sprite.Origin.Y) + localBounds.Y;

                return new Rectangle(left, top, localBounds.Width, localBounds.Height);
            }
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void Update(GameTime gameTime);
        public abstract void LoadContent(ContentManager contentManager);


        public int Damage { get; set; }

        private ContentManager contentManager;
    }
}
