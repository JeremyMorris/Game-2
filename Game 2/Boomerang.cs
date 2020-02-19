using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_2
{
    public class Boomerang
    {
        protected AnimationManager _animationManager;
        private Rectangle _animRectangle;
        private CollisionManager _collisionManager;

        private float _maxSpeed = 0.5f;
        private bool _turn = false;
        private float _framesToTurn = 40;
        
        public Vector2 Center { get; set; }

        public float X { get; set; }

        public float Y { get; set; }

        public float TurningPoint { get; set; }

        public bool FromRight { get; set; }

        public float HorizontalSpeed { get; set; }

        public float VerticalSpeed { get; set; }

        public bool PlayerCollision { get; set; }

        public bool MarkedForRemoval { get; set; }

        public int Partition { get; set; }

        public Boomerang(Vector2 origin, bool right, float turningPoint)
        {
            PlayerCollision = false;
            FromRight = right;
            TurningPoint = turningPoint;

            Center = new Vector2(origin.X + 12, origin.Y + 12);
            _animRectangle = new Rectangle((int)origin.X, (int)origin.Y, 24, 24);

            SetX(origin.X);
            SetY(origin.Y);

            _animationManager = new AnimationManager(BoomerangModel.animations.First().Value);
            _collisionManager = new CollisionManager();

            if (FromRight) HorizontalSpeed = -_maxSpeed;
            else HorizontalSpeed = _maxSpeed;
        }

        public void Update(GameTime gameTime)
        {
            // Detect turning point
            if (FromRight)
            {
                if (Center.X <= TurningPoint) _turn = true;
            }
            else
            {
                if (Center.X >= TurningPoint) _turn = true;
            }

            // Change speed if turning
            if (_turn)
            {
                if (FromRight && HorizontalSpeed <= _maxSpeed)
                {
                    HorizontalSpeed += _maxSpeed / _framesToTurn;
                }
                else if (!FromRight && HorizontalSpeed >= -_maxSpeed)
                {
                    HorizontalSpeed -= _maxSpeed / _framesToTurn;
                }
            }

            // Move boomerang
            SetY((int)(Y + VerticalSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds));
            SetX((int)(X + HorizontalSpeed * (float)gameTime.ElapsedGameTime.TotalMilliseconds));

            // Detect collision
            if (DetectCollisionWithPlayer())
            {
                PlayerCollision = true;
                BoomerangModel.player.FatalCollision = true;
            }

            // Mark for removal
            if (FromRight)
            {
                if (Center.X > BoomerangModel.game.GraphicsDevice.Viewport.Width + _animRectangle.Width) MarkedForRemoval = true;
            }
            else
            {
                if (Center.X < -_animRectangle.Width) MarkedForRemoval = true;
            }
        }

        public bool DetectCollisionWithPlayer()
        {
            return _collisionManager.IsWithinRange(Center, BoomerangModel.player.CollisionBox, 8);
        }

        public void SetX(float x)
        {
            Center = new Vector2(x + 12, this.Center.Y);
            X = x;
            _animRectangle.X = (int)x;

            Partition = BoomerangModel.game.GraphicsDevice.Viewport.Width / (int)Center.X;
        }

        public void SetY(float y)
        {
            Center = new Vector2(this.Center.X, y + 12);
            Y = y;
            _animRectangle.Y = (int)y;
        }

        // Draw the boomerang
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, _animRectangle);
        }

        // Change the currently playing animation based on the boomerang's state
        public void UpdateAnimation(GameTime gameTime)
        {
            _animationManager.Update(gameTime);

            if (FromRight)
            {
                _animationManager.Play(BoomerangModel.animations["Left"]);
            }
            else
            {
                _animationManager.Play(BoomerangModel.animations["Right"]);
            }
        }
    }
}
