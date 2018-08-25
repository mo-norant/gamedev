using Gamedev.Assets.Helpers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamedev.Assets.UI
{
    public class IntroSeq : IPipelineBase
    {
        VideoPlayer videoPlayer;
        Texture2D texture;
        Video intro;
        private const float delay = 17;
        private float remainingDelay = delay;
        public bool PlayVideo { get; set; }

        public void LoadContent(ContentManager contentManager)
        {
            videoPlayer = new VideoPlayer();
            intro = contentManager.Load<Video>("Intro/introgame");
            PlayVideo = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (PlayVideo)
            {
                texture = videoPlayer.GetTexture();
                if (texture != null)
                {
                    spriteBatch.Draw(texture, new Rectangle(0, 0, 1920, 1080),
                        Color.White);
                }
            }
          
        }

        

        public void Update(GameTime gameTime)
        {

            if (PlayVideo)
            {
                videoPlayer.Play(intro);
                videoPlayer.Volume = 0.5f;
            }

            var timer = (float)gameTime.ElapsedGameTime.TotalSeconds;

            remainingDelay -= timer;

            if (remainingDelay <= 0)
            {
                PlayVideo = false;
                videoPlayer.Stop();
                texture = null;
                remainingDelay = delay;
            }
           


            
        }
    }
}
