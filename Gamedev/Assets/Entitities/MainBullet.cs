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
    public class MainBullet : BulletBase
    {

        

        public MainBullet(Vector2 position, int damage, ContentManager contentManager, Direction direction)
        {
            Position = position;
            InitPosition = position;
            Damage = damage;

            //verhoogkogels

            Position += new Vector2(0, -75);

            base.direction = direction;
            LoadContent(contentManager);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BulletTexture, Position, Color.White);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            BulletTexture = contentManager.Load<Texture2D>("Bullets/mainbullet");
        }

      

        public override void Update(GameTime gameTime)
        {
            var movementChange = new Vector2(20, 0);
            if (direction == Direction.LEFT)
            {
                Position -= movementChange;
            }
            else
            {
                Position += movementChange;

            }

        }
    }
}
