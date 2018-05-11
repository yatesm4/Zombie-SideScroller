using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ZombieRogue.Objects;

namespace ZombieRogue.Objects
{
    public class Map
    {
        public PlayableCharacter Player;

        private Texture2D _background;
        private Vector2 _backgroundPosition;
        public Rectangle BackgroundRect;

        public Map(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _background = content.Load<Texture2D>("Sprites/Environment/Backgrounds/00");
            _backgroundPosition = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            BackgroundRect = new Rectangle(new Point((int)_backgroundPosition.X, (int)_backgroundPosition.Y), new Point(_background.Width, _background.Height));

            Player = new PlayableCharacter(content, new Vector2(BackgroundRect.X + (BackgroundRect.Width / 2), BackgroundRect.Y + 200), new int[] { 0, 0, 0, 0, 0, 0 });
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime, Keyboard.GetState(), this);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_background, BackgroundRect, Color.White);
            Player.Draw(gameTime, spriteBatch);
        }
    }
}
