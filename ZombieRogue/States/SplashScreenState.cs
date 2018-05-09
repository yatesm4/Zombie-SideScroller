using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using ZombieRogue.Objects;

namespace ZombieRogue.States
{
    public class SplashScreenState : State
    {
        private Animation _animSplash;
        private AnimationPlayer _animPlayer;

        private int _countdown = 200;

        public SplashScreenState(GameInstance game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            _animSplash = new Animation(content.Load<Texture2D>("Sprites/Branding/YD_Logo"), 0.15f, false);
            _animPlayer.Scale = 3.0f;
            _animPlayer.PlayAnimation(_animSplash);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.Wheat);
            spriteBatch.Begin();
            _animPlayer.Draw(gameTime, spriteBatch, new Vector2(_graphicsDevice.Viewport.Width / 2, _graphicsDevice.Viewport.Height), SpriteEffects.None);
            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // post update
        }

        public override void Update(GameTime gameTime)
        {
            // update
            if (_countdown > 0)
            {
                _countdown--;
            }
            if (_countdown.Equals(0))
            {
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content));
            }
        }
    }
}
