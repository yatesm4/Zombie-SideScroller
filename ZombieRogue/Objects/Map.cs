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

        public List<NonPlayableCharacter> NPCs = new List<NonPlayableCharacter>();

        private Texture2D _background;
        private Texture2D _parallaxBackground;

        private Vector2 _backgroundPosition;

        public Vector2 ParallaxPosition;

        public Rectangle BackgroundRect;
        public Rectangle ParallaxRect;

        public Map(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _background = content.Load<Texture2D>("Sprites/Environment/Backgrounds/00");
            _parallaxBackground = content.Load<Texture2D>("Sprites/Environment/Backgrounds/01");

            _backgroundPosition = new Vector2(graphicsDevice.Viewport.Width / 2, graphicsDevice.Viewport.Height / 2);
            ParallaxPosition = _backgroundPosition;

            BackgroundRect = new Rectangle(new Point((int)_backgroundPosition.X, (int)_backgroundPosition.Y), new Point(_background.Width, _background.Height));

            Player = new PlayableCharacter(content,
                new Vector2(BackgroundRect.X + (BackgroundRect.Width / 2), BackgroundRect.Y + 200),
                new int[] {0, 0, 0, 0, 0, 0})
            {
                GraphDevice = graphicsDevice,
                IsDebugging = true
            };

            NPCs.Add(new NonPlayableCharacter(content, new Vector2(BackgroundRect.X + (BackgroundRect.Width / 2) + 80, BackgroundRect.Y + 200), new int[] { 1, 1, 0, 0, 0, 0 })
            {
                GraphDevice = graphicsDevice,
                IsDebugging = true
            });
        }

        public void Update(GameTime gameTime)
        {
            Player.Update(gameTime, Keyboard.GetState(), this);
            foreach (var n in NPCs)
            {
                n.Update(gameTime, Keyboard.GetState(), this);
            }
            ParallaxRect = new Rectangle(new Point((int)ParallaxPosition.X, (int)ParallaxPosition.Y), new Point(_background.Width, _background.Height));
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            List<Character> entities = new List<Character>();
            foreach (var n in NPCs)
            {
                entities.Add(n);
            }
            entities.Add(Player);
            List<Character> sorted_entities = entities.OrderBy(ent => ent.Position.Y).ToList();

            spriteBatch.Draw(_parallaxBackground, ParallaxRect, Color.White);
            spriteBatch.Draw(_background, BackgroundRect, Color.White);
            //Player.Draw(gameTime, spriteBatch);

            foreach (var e in sorted_entities)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }
    }
}
