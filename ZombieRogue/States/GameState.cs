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

using Comora;

namespace ZombieRogue.States
{
    public class GameState : State
    {
        private Map _currentMap;

        private Camera _camera;

        public GameState(GameInstance game, GraphicsDevice graphicsDevice, ContentManager content) : base(game, graphicsDevice, content)
        {
            // Load Map
            _currentMap = new Map(content, graphicsDevice);

            _camera = new Camera(graphicsDevice);
            _camera.Zoom = 2.45f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(_camera);

            // draw game state
            _currentMap.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // post update game state
            Vector2 prevCamPosition = _camera.Position;

            _camera.Update(gameTime);
            _camera.Position = new Vector2(_currentMap.Player.Position.X, _currentMap.BackgroundRect.Y + 100);

            if ((_camera.GetBounds().Left <= _currentMap.BackgroundRect.Left) || (_camera.GetBounds().Right >= _currentMap.BackgroundRect.Right))
            {
                _camera.Position = new Vector2(prevCamPosition.X, _currentMap.BackgroundRect.Y + 100);
            }
            else
            {
                _currentMap.ParallaxPosition.X = _currentMap.Player.Position.X - (_currentMap.ParallaxRect.Width / 2);
            }

            _currentMap.Update(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            // update game state
        }
    }
}
