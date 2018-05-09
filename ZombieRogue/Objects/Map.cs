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

        private Animation _background;
        private AnimationPlayer _backgroundPlayer;
        private Vector2 _backgroundPosition;

        public Map(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _background = new Animation(content.Load<Texture2D>("Sprites/Environment/Backgrounds/00"), 0.15f, false);
            _backgroundPlayer.PlayAnimation(_background);
            _backgroundPosition = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);

            Player = new PlayableCharacter(content, new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2), new int[] { 0, 0, 0, 0, 0, 0 });
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime, Keyboard.GetState());
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _backgroundPlayer.Draw(gameTime, spriteBatch, _backgroundPosition, SpriteEffects.None);
            Player.Draw(gameTime, spriteBatch);
        }
    }
}
