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
    class Life : IPipelineBase
    {

        public Life(ContentManager contentManager)
        {
            LoadContent(contentManager);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void LoadContent(ContentManager contentManager)
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
