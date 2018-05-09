using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ZombieRogue.States;

namespace ZombieRogue
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameInstance : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private State _currentState;
        private State _nextState;

        public GameInstance()
        {
            _graphics = new GraphicsDeviceManager(this);
            //_graphics.ToggleFullScreen();
            Content.RootDirectory = "Content";
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.ForestGreen);

            _currentState.Draw(gameTime, _spriteBatch);

            base.Draw(gameTime);
        }

        protected override void Initialize()
        {
            IsMouseVisible = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new SplashScreenState(this, GraphicsDevice, Content);
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;
                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }
    }
}
