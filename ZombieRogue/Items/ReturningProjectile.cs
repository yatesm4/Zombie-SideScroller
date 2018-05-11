using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using ZombieRogue.Objects;

namespace ZombieRogue.Items
{
    public class ReturningProjectile : Projectile
    {
        public bool IsReturning = false;

        public event EventHandler Returned;
        public bool IsReturned = false;

        public Vector2 Origin;
        public float Radius;
        public double Angle;

        public ReturningProjectile(ContentManager content, Vector2 position, string weapon_name, int[] skin_args, Vector2 direction) : base(content, position, weapon_name, skin_args, direction)
        {
            InitialPosition = position;
            Trajectory = direction;
            Angle = 45;
            Radius = 10;
            LoadContent(content, weapon_name, skin_args);
            Reset(position);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, Character owner)
        {
            if (IsReturned.Equals(false))
            {
                if (IsReturning.Equals(false))
                {
                    Position += new Vector2(Trajectory.X * ProjectileSpeed, 0);
                    if (Sprite.Rotation < 5f)
                    {
                        Sprite.Rotation += 0.1f;
                    }
                    else
                    {
                        IsReturning = true;
                    }
                }
                else
                {
                    Position -= new Vector2(Trajectory.X * ProjectileSpeed, 0);
                    if (Sprite.Rotation > 0)
                    {
                        Sprite.Rotation -= 0.1f;
                    }
                    else
                    {
                        Sprite.Rotation = 0.0f;
                    }
                    if (Position.Equals(owner.Position))
                    {
                        Console.WriteLine("Hammer returned to initial position");
                        Sprite.Rotation = 0.0f;
                        IsReturned = true;
                        IsReturning = false;
                        Returned?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        public Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsReturned.Equals(false))
            {
                Sprite.Draw(gameTime, spriteBatch, Position, IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);
            } else
            {
                Console.WriteLine("Not drawing projectile");
            }
        }
    }
}
