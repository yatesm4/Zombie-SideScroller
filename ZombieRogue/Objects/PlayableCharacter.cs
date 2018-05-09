using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace ZombieRogue.Objects
{
    public class PlayableCharacter : Character
    {
        public float MoveSpeed = 5.0f;

        public PlayableCharacter(ContentManager content, Vector2 position, int[] skin_args) : base(content, position, skin_args)
        {
            // construct playable char
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            GetInput(keyboardState);
            ApplyPhysics(gameTime);
        }

        public void GetInput(KeyboardState keyboardState)
        {
            Movement = new Vector2(0, 0);

            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.D))
            {
                Sprite.Animation.IsStill = false;
                Sprite.PlayAnimation(Spr_Walk);
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    Movement.X = -MoveSpeed;
                    IsFlipped = true;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    Movement.X = MoveSpeed;
                    IsFlipped = false;
                }

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    Movement.Y = -MoveSpeed;
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    Movement.Y = MoveSpeed;
                }
                Sprite.Animation.IsLooping = true;

                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    Movement = Movement * 2;
                }
            }
            else
            {
                Sprite.PlayAnimation(Spr_Idle);
                Sprite.Animation.IsLooping = false;
                Sprite.Animation.IsStill = true;
            }
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if(Movement != Vector2.Zero)
            {
                Console.WriteLine("Movement detected");
            }

            Vector2 previousPosition = Position;

            Velocity.X += Movement.X * MoveAcceleration * elapsed;
            Velocity.Y += Movement.Y * MoveAcceleration * elapsed;

            Velocity.X *= GroundDragFactor;
            Velocity.Y *= GroundDragFactor;

            Velocity.X = MathHelper.Clamp(Velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
            Velocity.Y = MathHelper.Clamp(Velocity.Y, -MaxMoveSpeed, MaxMoveSpeed);

            Position += Velocity * elapsed;
            //Position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            Console.WriteLine($"Position: {Position}");
            Console.WriteLine($"Velocity: {Velocity}");

            //HandleCollisions();

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                Velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                Velocity.Y = 0;
        }
    }
}
