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
using ZombieRogue.FX;
using ZombieRogue.Items;

namespace ZombieRogue.Objects
{
    public class NonPlayableCharacter : Character
    {
        private ContentManager Content;

        private List<SpecialEffect> FXs = new List<SpecialEffect>();

        public Vector2 PreviousMovement = new Vector2(1,1);

        public Vector2 NewDirection = new Vector2();

        public float MoveSpeed = 0.5f;

        public bool IsAttacking = false;
        public bool IsDamaged = false;
        public bool IsAlive = true;

        public NonPlayableCharacter(ContentManager content, Vector2 position, int[] skin_args) : base(content, position, skin_args)
        {
            Content = content;
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

            foreach (var fx in FXs)
            {
                if (fx.HasEnded.Equals(false))
                {
                    fx.Update(gameTime, keyboardState);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (var fx in FXs)
            {
                if(fx.HasEnded.Equals(false))
                    fx.Draw(gameTime, spriteBatch);
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
                    IsFlipped = true;
                }
                else if (NewDirection.X < 0)
                {
                    IsFlipped = false;
                }
            }

            Sprite.PlayAnimation(Spr_Walk);

            if (NewDirection != Vector2.Zero)
            {
                Sprite.Animation.IsStill = false;
                Sprite.Animation.IsLooping = true;
            }
            else
            {
                Sprite.FrameIndex = 1;
                Sprite.Animation.IsStill = true;
                Sprite.Animation.IsLooping = false;
            }
        }

        public void ApplyPhysics(Map map)
        {
            Vector2 PreviousPosition = Position;
            Position += NewDirection * MoveSpeed;

            // check new position for clearance
            foreach(var ent in map.NPCs)
            {
                Console.WriteLine("NPC movement blocked by other NPC's hitbox.");
                if (object.ReferenceEquals(ent, this))
                    continue;

                if (ent.Hitbox.Intersects(Hitbox) && TestRange((int)Position.Y, (int)ent.Position.Y - 5, (int)ent.Position.Y + 5))
                {
                    Position = PreviousPosition;
                }

            }
        }

        private bool TestRange(int numberToCheck, int bottom, int top)
        {
            return (numberToCheck >= bottom && numberToCheck <= top);
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

        public async void TakeDamage()
        {
            Console.WriteLine("NPC took damage!");
            IsDamaged = true;

            //Sprite.PlayAnimation(Spr_);
            //Sprite.Animation.IsLooping = false;

            var effect = new SpecialEffect(Content, new Vector2(Position.X, Position.Y - 18), "Kapow", 0.0f);
            FXs.Add(effect);

            TimeSpan t = new TimeSpan(25000000);
            await Task.Delay(t);

            IsDamaged = false;
            Sprite.PlayAnimation(Spr_Walk);
            Sprite.Animation.IsLooping = true;

            return;
        }


    }
}
