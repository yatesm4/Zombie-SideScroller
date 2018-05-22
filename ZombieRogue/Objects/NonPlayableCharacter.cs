using System;
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
    public class NonPlayableCharacter : Character
    {
        public Vector2 PreviousMovement = new Vector2(1,1);

        public Vector2 NewDirection = new Vector2();

        public float MoveSpeed = 0.5f;

        public bool IsAttacking = false;
        public bool IsDamaged = false;
        public bool IsAlive = true;

        public NonPlayableCharacter(ContentManager content, Vector2 position, int[] skin_args) : base(content, position, skin_args)
        {
            // construct playable char
            Sprite.AnimationEnded += Sprite_AnimationEnded;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboardState, Map map)
        {
            if (IsAlive.Equals(true) && IsDamaged.Equals(false))
            {
                if (IsAttacking.Equals(false))
                {
                    DetermineMovement(map);
                }
                ApplyPhysics(map);
            }
        }

        public void DetermineMovement(Map map)
        {
            NewDirection = Vector2.Zero;

            if ((Position.X < map.Player.Position.X + (localBounds.Width - 8)) && (Position.X > map.Player.Position.X - (localBounds.Width - 8)))
            {
                // do nothing
            }
            else
            {
                NewDirection = map.Player.Position - Position;
                NewDirection.Normalize();
                if (NewDirection.X > 0)
                {
                    IsFlipped = false;
                }
                else if (NewDirection.X < 0)
                {
                    IsFlipped = true;
                }
            }

            if (NewDirection != Vector2.Zero)
            {
                if(!Sprite.Animation.Equals(Spr_Walk))
                    Sprite.PlayAnimation(Spr_Walk);

                Sprite.Animation.IsStill = false;
                Sprite.Animation.IsLooping = true;
            }
            else
            {
                Sprite.PlayAnimation(Spr_Idle);
                Sprite.Animation.IsStill = true;
                Sprite.Animation.IsLooping = false;
            }
        }

        public void ApplyPhysics(Map map)
        {
            Vector2 PreviousPosition = Position;
            Position += NewDirection * MoveSpeed;
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


    }
}
