using System;
using Gamedev.Assets.Players;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Unity;

namespace Gamedev.Assets
{

    //bewegingsalgoritme werd gebaseerd op http://xnafan.net/2013/04/simple-platformer-game-in-xna-tutorial-part-three/

    class Player : AnimatedSprite, IPlayer
    {
        Texture2D texture;
        ContentManager contentManager;
      
        public Player(ContentManager manager) : base (manager.Load<Texture2D>("playerspritelong"), 1, 4)
        {
            contentManager = manager;
            LoadContent();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Position);
        }

        public void LoadContent()
        {
            
        }

        public void UnloadContent()
        {
            throw new NotImplementedException();
        }

        public void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

       
    }
}
