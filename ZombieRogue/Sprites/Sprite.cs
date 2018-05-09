using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombieRogue.Sprites
{
    public class Sprite
    {
        // ========================================================================
        // Basic Object Properties
        // ========================================================================
        private Texture2D _texture;
        public Texture2D Texture
        {
            get
            {
                return _texture;
            }
        }

        private Vector2 _position;
        public Vector2 Position
        {
            get
            {
                return _position;
            }
        }

        public float Rotation;
        public bool Flipped;

        private Vector2 _origin;
        public Vector2 Origin
        {
            get
            {
                return _origin;
            }
        }

        private Rectangle _rect;
        public Rectangle Rect
        {
            get
            {
                return _rect;
            }
        }

        private Vector2 _scaleFactor;
        public Vector2 ScaleFactor
        {
            get
            {
                return _scaleFactor;
            }
        }

        // ========================================================================
        // Animation Properties
        // ========================================================================
        private float _time;
        public int FrameDimension
        {
            get
            {
                return _texture.Height;
            }
        }

        public int FrameCount
        {
            get
            {
                return _texture.Width / FrameDimension;
            }
        }

        public int FrameIndex;

        public float FrameTime = 0.15f;
        public bool IsStill = false;
        public bool IsLooping = false;

        // ========================================================================
        // Animation Methods
        // ========================================================================
        public void PlayAnimation()
        {
            FrameIndex = 0;
            _time = 0.0f;
        }

        // ========================================================================
        // Creates a new sprite using the given texture
        // ========================================================================
        public Sprite(Texture2D texture)
        {
            _texture = texture;
            _position = Vector2.Zero;
            Rotation = 0.0f;
            Flipped = false;
            _origin = new Vector2(FrameDimension / 2.0f, FrameDimension);
            _rect = new Rectangle(0, 0, _texture.Width, _texture.Height);
            _scaleFactor = Vector2.One;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

            // process passing time
            _time += (float)gameTime.ElapsedGameTime.TotalSeconds;
            while (_time > FrameTime)
            {
                _time -= FrameTime;

                // advance frame index; looping or clamping as appropriate
                if (IsStill.Equals(false))
                {
                    if (IsLooping.Equals(true))
                    {
                        FrameIndex = (FrameIndex + 1) % FrameCount;
                    } else
                    {
                        FrameIndex = Math.Min(FrameIndex + 1, FrameCount - 1);
                    }
                }
            }

            Rectangle source = new Rectangle(FrameIndex * Texture.Height, 0, Texture.Height, Texture.Height);

            spriteBatch.Draw(Texture, _position, source, Color.White, Rotation, _origin, _scaleFactor, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
        }

        // ========================================================================
        // Position Accessors/Modifiers
        // ========================================================================
        public void SetPosition(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public void SetPosition(Vector2 pos) { _position = pos; }

        public void Move(float deltaX, float deltaY)
        {
            _position.X += deltaX;
            _position.Y += deltaY;
        }

        public void Move(Vector2 deltaPos) { _position += deltaPos; }

        // ========================================================================
        // Origin Accessors/Modifiers
        // ========================================================================
        public void SetOrigin(float x, float y)
        {
            _origin.X = x;
            _origin.Y = y;
        }

        public void SetOrigin(Vector2 origin) { _origin = origin; }

        // ========================================================================
        // Rect Accessors/Modifiers
        // ========================================================================
        public void SetRect(int x, int y, int width, int height)
        {
            _rect.X = x;
            _rect.Y = y;
            _rect.Width = width;
            _rect.Height = height;
        }

        public void SetRect(Rectangle newRect) { _rect = newRect; }

        // ========================================================================
        // Scale Accessors/Modifiers
        // ========================================================================
        public void SetScale (float x, float y)
        {
            _scaleFactor.X = x;
            _scaleFactor.Y = y;
        }

        public void SetScale (float xy) { _scaleFactor.X = _scaleFactor.Y = xy; }

        public void SetScale(Vector2 scale) { _scaleFactor = scale; }

        public void Scale( float x, float y)
        {
            _scaleFactor.X *= x;
            _scaleFactor.Y *= y;
        }

        public void Scale(float xy)
        {
            _scaleFactor.X *= xy;
            _scaleFactor.Y *= xy;
        }

        public void Scale(Vector2 scale) { _scaleFactor *= scale; }
    }
}
