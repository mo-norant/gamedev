using Gamedev.Assets.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.Entitities
{
    public class MainBullet : BulletBase, IPlayerBullet
    {

        public SoundEffect ShotSound { get; set; }

        public MainBullet(Vector2 position, int damage, ContentManager contentManager, Direction direction, GraphicsDevice graphicsDevice): base(graphicsDevice)
        {
            Position = position;
            InitPosition = position;
            Damage = damage;

            //verhoog kogels

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
            animatedSprite = new AnimatedSprite(BulletTexture, 1, 1, true);
            ShotSound = contentManager.Load<SoundEffect>("sound/Gun/bullet1");
        }

      
        public void CheckCollisionWithEnemyBullets(List<BulletBase> enemybullets)
        {
            foreach (var b in enemybullets)
            {
                if (b.BoundingRectangle.Intersects(BoundingRectangle))
                {
                    b.IsNotEffective = true;
                }
            }
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

        public override void CheckCollisionWithPlayer(Player player)
        {
            throw new NotImplementedException();
        }
    }
}
