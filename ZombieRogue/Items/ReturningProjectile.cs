using System;
using System.Collections;
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
        public GraphicsDevice GraphDevice;

        public bool IsReturning = false;

        public event EventHandler Returned;
        public bool IsReturned = false;

        public bool IsDestroyed = false;

        public Vector2 Origin;
        public float Radius;
        public double Angle;

        public List<NonPlayableCharacter> Entities = new List<NonPlayableCharacter>();

        public Rectangle Hitbox
        {
            get
            {
                return new Rectangle((int)Position.X - 16, (int)Position.Y - 16, 16, 16);
            }
        }

        public Rectangle RotatedHitbox
        {
            get
            {
                var x1 = (Position.X * Math.Cos(Sprite.Rotation)) - (Position.Y * Math.Sin(Sprite.Rotation));
                var y1 = (Position.X * Math.Sin(Sprite.Rotation)) + (Position.Y * Math.Cos(Sprite.Rotation));

                var x2 = (((Position.X + 16) * Math.Cos(Sprite.Rotation)) -
                          ((Position.Y + 16) * Math.Sin(Sprite.Rotation)));

                var y2 = (((Position.X + 16) * Math.Sin(Sprite.Rotation)) +
                          ((Position.Y + 16) * Math.Cos(Sprite.Rotation)));

                return new Rectangle(Convert.ToInt32(x1), Convert.ToInt32(y1), Convert.ToInt32(x2), Convert.ToInt32(y2));
            }
        }

        public Vector2 HitboxOrigin = new Vector2(8, 24);

        public ReturningProjectile(ContentManager content, Vector2 position, string weapon_name, int[] skin_args, Vector2 direction) : base(content, position, weapon_name, skin_args, direction)
        {
            InitialPosition = position;
            Trajectory = direction;
            Console.WriteLine($"{Trajectory.ToString()}");
            Angle = 45;
            Radius = 10;
            LoadContent(content, weapon_name, skin_args);
            Reset(position);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, Character owner)
        {
            var crossedReturningPoint = false;
            if (IsDestroyed.Equals(false))
            {
                if (IsReturned.Equals(false))
                {
                    Sprite.Rotation += 0.1f;
                    if (IsReturning.Equals(false))
                    {
                        Position += new Vector2(Trajectory.X * ProjectileSpeed, 0);
                        if (Sprite.Rotation > 5f)
                        {
                            IsReturning = true;
                        }
                    }
                    else
                    {
                        Position -= new Vector2(Trajectory.X * ProjectileSpeed, 0);
                        if (Trajectory.X > 0)
                        {
                            // thrown to right
                            if (Position.X <= InitialPosition.X)
                            {
                                crossedReturningPoint = true;
                            }
                        }
                        else
                        {
                            // thrown to left
                            if (Position.X >= InitialPosition.X)
                            {
                                crossedReturningPoint = true;
                            }
                        }
                        if (IsReturning.Equals(true) && crossedReturningPoint.Equals(true))
                        {
                            Console.WriteLine("Hammer returned to initial position");
                            Sprite.Rotation = 0.0f;
                            IsReturned = true;
                            IsReturning = false;
                            Returned?.Invoke(this, new EventArgs());
                        }
                    }
                }

                foreach (var e in Entities)
                {
                    if (e.Hitbox.Intersects(Hitbox))
                    {
                        Console.WriteLine("Intersected with NPC");
                    }
                }
            }
        }

        public Vector2 RotateAboutOrigin(Vector2 point, Vector2 origin, float rotation)
        {
            return Vector2.Transform(point - origin, Matrix.CreateRotationZ(rotation)) + origin;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsDestroyed.Equals(false))
            {
                if (IsReturned.Equals(false))
                {
                    Sprite.Draw(gameTime, spriteBatch, Position, IsFlipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None);

                    // Draw projectile hitbox
                    DrawDebugRect(Hitbox, Color.Red, spriteBatch);
                }
                else
                {
                    //Console.WriteLine("Not drawing projectile");
                }
            }
        }

        public async void EnsureDestroy(int seconds)
        {
            int timer = DateTime.Now.Second + seconds;
            while (DateTime.Now.Second <= timer)
            {
                // do nothing
                await Task.Delay(1);
            }

            Console.WriteLine("DEBUG: Destroyed projectile");
            IsDestroyed = true;
        }

        public void DrawDebugRect(Rectangle coords, Color color, SpriteBatch spriteBatch)
        {
            //Console.WriteLine("Drawing debug rect");

            var Debug_Rect = new Texture2D(GraphDevice, 1, 1);
            Debug_Rect.SetData(new[] { Color.Red });

            //spriteBatch.Draw(Debug_Rect, coords, new Color(color, 0.25f));
            spriteBatch.Draw(Debug_Rect, new Vector2(Position.X, Position.Y), coords, new Color(color, 0.25f), Sprite.Rotation, HitboxOrigin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
