using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game_2
{
    public class Player
    {
        private Game1 game;
        private float _maxSpeed = 0.6f;
        private float _acceleration = 0.04f;
        private Rectangle _animRectangle;
        private Rectangle _collisionBox;
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;

        public float X { get; set; }

        public float Y { get; set; }

        public float Speed { get; set; }

        public float MaxSpeed { get { return _maxSpeed; } }

        public bool FacingRight { get; set; }

        public bool FatalCollision { get; set; }

        public float Acceleration { get { return _acceleration; } }

        public Rectangle CollisionBox { get { return _collisionBox; } }

        public Player(Vector2 playerPosition, Game1 game)
        {
            this.game = game;

            X = playerPosition.X;
            Y = playerPosition.Y;
            Speed = 0;
            _collisionBox = new Rectangle((int)X, (int)Y, 100, 100);

            LoadContent(game.Content);

            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public void LoadContent(ContentManager content)
        {
            // Load player animations
            var animations = new Dictionary<string, Animation>()
            {
                { "IdleRight", new Animation(content.Load<Texture2D>("Player/Guy-IdleRight"), 4) },
                { "IdleLeft", new Animation(content.Load<Texture2D>("Player/Guy-IdleLeft"), 4) },
                { "RunRight", new Animation(content.Load<Texture2D>("Player/Guy-RunRight"), 10) },
                { "RunLeft", new Animation(content.Load<Texture2D>("Player/Guy-RunLeft"), 10) },
                { "JumpRight", new Animation(content.Load<Texture2D>("Player/Guy-JumpRight"), 3, false) },
                { "JumpLeft", new Animation(content.Load<Texture2D>("Player/Guy-JumpLeft"), 3, false) }
            };
            _animations = animations;
        }

        public void SetX(int x)
        {
            _collisionBox.X = x;
            _animRectangle.X = x;
            this.X = x;
        }

        public void SetY(int y)
        {
            _collisionBox.Y = y;
            _animRectangle.Y = y;
            this.Y = y;
        }

        // Draw the player
        public void Draw(SpriteBatch spriteBatch)
        {
            _animationManager.Draw(spriteBatch, _animRectangle);
        }

        // Change the currently playing animation based on the player's state
        public void UpdateAnimation(GameTime gameTime)
        {
            _animationManager.Update(gameTime);
            /*
            if (FacingRight)
            {
                if (FatalCollision) { _animationManager.Play(_animations["DyingRight"]); }
                else if (Speed == 0) { _animationManager.Play(_animations["IdleRight"]); }
                else { _animationManager.Play(_animations["RunRight"]); }
            }
            else
            {
                if (FatalCollision) { _animationManager.Play(_animations["DyingLeft"]); }
                else if (Speed == 0) { _animationManager.Play(_animations["IdleLeft"]); }
                else { _animationManager.Play(_animations["RunRight"]); }
            }
            */
            _animationManager.Play(_animations["RunRight"]);
        }
    }
}
