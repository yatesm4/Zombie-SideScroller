using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ZombieRogue.Items;

namespace ZombieRogue.Objects
{
    public class PlayableCharacter : Character
    {

        public Vector2 PreviousMovement = new Vector2(1, 1);

        private float PreviousDirection = 1;

        public float MoveSpeed = 1.0f;

        public float PreviousScale = 0.0f;

        public bool IsRunning = false;

        public bool IsAttacking = false;

        public bool IsUsingWeapon = false;

        public bool WeaponIsThrown = false;

        public Weapon CurrentWeapon;

        public List<Projectile> Projectiles;

        public Map CurrentMapState;

        //===========================================================================
        // State (Mouse and Keyboard) Variables
        //===========================================================================

        public MouseState PreviousMouseState;
        public KeyboardState PreviousKeyboardState;

        public PlayableCharacter(ContentManager content, Vector2 position, int[] skin_args) : base(content, position, skin_args)
        {
            // construct playable char
            PreviousMouseState = Mouse.GetState();
            Sprite.AnimationEnded += Sprite_AnimationEnded;
            CurrentWeapon = new Hammer(content, Position, "Mjolnir", new int[] { 0, 0, 0, 0, 0, 0 });
            Projectiles = new List<Projectile>();
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, Map map)
        {
            CurrentMapState = map;

            if(IsAttacking.Equals(false))
            {
                MovementInput(keyboardState);
                CombatInput(keyboardState);
                ApplyPhysics(gameTime, map);
            }
            if(CurrentWeapon != null)
            {
                CurrentWeapon.Position = Position;
                CurrentWeapon.IsFlipped = IsFlipped;
                //CurrentWeapon.Update(gameTime, keyboardState, this);
            }

            //Console.WriteLine($"Player Y: {Position.Y}; Scale: {Sprite.Scale}");

            for (int i = 0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Update(gameTime, keyboardState, this);
            }
        }

        public void MovementInput(KeyboardState keyboardState)
        {
            if (Movement != Vector2.Zero)
                PreviousMovement = Movement;

            if (Movement.X != 0)
                PreviousDirection = Movement.X;

            Movement = new Vector2(0, 0);

            PreviousScale = Sprite.Scale;


            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.D))
            {
                Sprite.Animation.IsStill = false;
                if (keyboardState.IsKeyDown(Keys.A))
                {
                    Movement.X = -MoveSpeed;
                    IsFlipped = false;
                }
                else if (keyboardState.IsKeyDown(Keys.D))
                {
                    Movement.X = MoveSpeed;
                    IsFlipped = true;
                }

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    Movement.Y = -(MoveSpeed / 2.25f);
                    /*
                    if(Sprite.Scale > 0.850)
                        Sprite.Scale -= 0.002f;
                    */
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    Movement.Y = MoveSpeed / 2.25f;
                    /*
                    if(Sprite.Scale < 1.040)
                        Sprite.Scale += 0.002f;
                    */
                }
                Sprite.Animation.IsLooping = true;
                if (keyboardState.IsKeyDown(Keys.LeftShift) || keyboardState.IsKeyDown(Keys.RightShift))
                {
                    IsRunning = true;
                    Movement = Movement * 2f;
                    if(Sprite.Animation != Spr_Run)
                    {
                        Sprite.PlayAnimation(Spr_Run);
                    }
                } else
                {
                    IsRunning = false;
                    if (Sprite.Animation != Spr_Walk)
                        Sprite.PlayAnimation(Spr_Walk);
                }
            }
            else
            {
                if (Sprite.Animation != Spr_Walk)
                    Sprite.PlayAnimation(Spr_Walk);
                Sprite.Animation.IsLooping = false;
                Sprite.Animation.IsStill = true;
            }
        }

        public void CombatInput(KeyboardState keyboardState)
        {
            var mouseState = Mouse.GetState();
            
            if (PreviousKeyboardState.IsKeyDown(Keys.X) && keyboardState.IsKeyUp(Keys.X))
            {
                IsAttacking = true;
                Sprite.PlayAnimation(Spr_Punch);
                Console.WriteLine($"Punch!");
            }
            else if (PreviousKeyboardState.IsKeyDown(Keys.Z) && keyboardState.IsKeyUp(Keys.Z))
            {
                /*
                IsAttacking = true;
                IsUsingWeapon = true;
                WeaponIsThrown = true;
                Sprite.PlayAnimation(Spr_Punch);
                HandleWeaponAnimations(1);
                Console.WriteLine($"Swing!");
                */
                IsAttacking = true;
                Sprite.PlayAnimation(Spr_Kick);
                Console.WriteLine($"Kick!");

            }
            else if (PreviousKeyboardState.IsKeyDown(Keys.C) && keyboardState.IsKeyUp(Keys.C))
            {
                IsAttacking = true;
                Sprite.PlayAnimation(Spr_Uppercut);
                Console.WriteLine($"Uppercut!");
            }
            else
            {
                CurrentWeapon.Sprite.PlayAnimation(CurrentWeapon.Spr_Idle);
            }

            if (IsAttacking.Equals(true))
            {
                Sprite.Animation.IsStill = false;
                Sprite.Animation.IsLooping = false;

                foreach (var ent in CurrentMapState.NPCs)
                {
                    if (Hitbox.Intersects(ent.Hitbox) && ent.IsDamaged.Equals(false))
                    {
                        if (IsFlipped.Equals(false))
                        {
                            // facing left
                            if(ent.Position.X < Position.X)
                            {
                                // player is facing entity
                                ent.TakeDamage();
                            }
                        } else
                        {
                            // facing right
                            if (ent.Position.X > Position.X)
                            {
                                // player is facing entity
                                ent.TakeDamage();
                            }
                        }
                    }
                }
            }

            PreviousMouseState = mouseState;
            PreviousKeyboardState = keyboardState;
        }

        public void HandleWeaponAnimations(int attack_id)
        {
            if(CurrentWeapon != null)
            {
                switch (attack_id)
                {
                    case 0:
                        CurrentWeapon.Sprite.PlayAnimation(CurrentWeapon.Spr_Swing);
                        break;
                    case 1:
                        CurrentWeapon.Sprite.PlayAnimation(CurrentWeapon.Spr_Swing_Throw);
                        ReturningProjectile p = new ReturningProjectile(Content, Position - new Vector2(0, 32), "Mjolnir", new int[] { 0 }, new Vector2(PreviousDirection, 0))
                        {
                            GraphDevice = this.GraphDevice,
                            IsFlipped = this.IsFlipped,
                            Entities = CurrentMapState.NPCs
                        };
                        Projectiles.Add(p);
                        p.Returned += delegate
                        {
                            Console.WriteLine("Hammer Returned!");
                            Projectiles.RemoveAll(s => s.Equals(p));
                            IsUsingWeapon = false;
                            WeaponIsThrown = false;
                        };
                        break;
                }
            }
        }

        public void Sprite_AnimationEnded(object sender, EventArgs e)
        {
            if (IsAttacking.Equals(true))
            {
                Thread cooldown = new Thread(AttackCooldown);
                cooldown.Start();
            }
        }
        
        public async void AttackCooldown()
        {
            TimeSpan t = new TimeSpan((int)((Sprite.Animation.FrameTime * Sprite.Animation.FrameCount) * 1000));
            await Task.Delay(t);
            IsAttacking = false;
            return;
        }

        public void ApplyPhysics(GameTime gameTime, Map map)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 previousPosition = Position;

            Velocity.X += Movement.X * MoveAcceleration * elapsed;
            Velocity.Y += Movement.Y * MoveAcceleration * elapsed;

            Velocity.X *= GroundDragFactor;
            Velocity.Y *= GroundDragFactor;

            Velocity.X = MathHelper.Clamp(Velocity.X, -MaxMoveSpeed, MaxMoveSpeed);
            Velocity.Y = MathHelper.Clamp(Velocity.Y, -MaxMoveSpeed, MaxMoveSpeed);

            Position += Velocity * elapsed;

            HandleCollisions(Position, previousPosition, map);

            // If the collision stopped us from moving, reset the velocity to zero.
            if (Position.X == previousPosition.X)
                Velocity.X = 0;

            if (Position.Y == previousPosition.Y)
                Velocity.Y = 0;
        }
        
        public void HandleCollisions(Vector2 position, Vector2 previousPosition, Map map)
        {
            if ((position.X <= map.BackgroundRect.X) || (position.X >= map.BackgroundRect.X + map.BackgroundRect.Width))
                Position = new Vector2(previousPosition.X, Position.Y);

            if ((position.Y <= map.BackgroundRect.Y + 115) || (position.Y >= map.BackgroundRect.Y + map.BackgroundRect.Height + 15))
            {
                Position = new Vector2(Position.X, previousPosition.Y);
                Sprite.Scale = PreviousScale;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            /*
            if (IsUsingWeapon.Equals(true) && WeaponIsThrown.Equals(false))
            {
                base.Draw(gameTime, spriteBatch);
                //CurrentWeapon.Draw(gameTime, spriteBatch);
            } else
            {
                if(WeaponIsThrown.Equals(false))
                    //CurrentWeapon.Draw(gameTime, spriteBatch);
                base.Draw(gameTime, spriteBatch);
            }
            */

            base.Draw(gameTime, spriteBatch);


            for (int i=0; i < Projectiles.Count; i++)
            {
                Projectiles[i].Draw(gameTime, spriteBatch);
            }
        }
    }
}
