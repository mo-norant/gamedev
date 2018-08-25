using Gamedev.Assets.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Entitities.Specials
{
    public abstract class SpecialItemBase : IPipelineBase
    {



        protected Texture2D texture;
        protected Vector2 origin;
        protected ContentManager contentManager;


        public SpecialItemBase(ContentManager contentManager)
        {
            this.contentManager = contentManager;
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract void LoadContent(ContentManager contentManager);
        public abstract void Update(GameTime gameTime);
    }
}
