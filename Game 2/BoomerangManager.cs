using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_2
{
    public class BoomerangManager
    {
        public List<Boomerang> _boomerangList = new List<Boomerang>();
        private SoundEffect _throwSFX;
        private Random _random;
        private Game1 _game;
        private Player _player;
        public int _spawnInterval;
        private double _timeAtLastSpawn;
        private bool _spawnRight;

        public BoomerangManager(Random r, Game1 game, ref Player player)
        {
            _random = r;
            _game = game;
            _player = player;

            BoomerangModel.SetValues(ref game, ref player, LoadAnimations(game.Content));

            _spawnInterval = 2000;
        }

        public void Update(GameTime gameTime)
        {
            if (gameTime.TotalGameTime.TotalMilliseconds - _timeAtLastSpawn > _spawnInterval && !_player.FatalCollision)
            {
                Vector2 position;
                float turningPoint;

                if (_spawnRight)
                {
                    if (!_player.Airborne) position = new Vector2(_game.GraphicsDevice.Viewport.Width, _random.Next(150, _game.GraphicsDevice.Viewport.Height - 24));
                    else position = new Vector2(_game.GraphicsDevice.Viewport.Width, _random.Next(100,_game.GraphicsDevice.Viewport.Height - 24));
                    turningPoint = _random.Next(200, 500);
                }
                else
                {
                    if (!_player.Airborne) position = new Vector2(0, _random.Next(150, _game.GraphicsDevice.Viewport.Height - 24));
                    else position = new Vector2(0, _random.Next(100, _game.GraphicsDevice.Viewport.Height - 24));
                    turningPoint = _random.Next(500, 800);
                }

                Boomerang newBoomerang = new Boomerang(position, _spawnRight, turningPoint);
                _boomerangList.Add(newBoomerang);
                _spawnRight = !_spawnRight;

                _timeAtLastSpawn = gameTime.TotalGameTime.TotalMilliseconds;
                if (_spawnInterval > 500) _spawnInterval -= 50;

                _throwSFX.Play(0.125f, 0, (position.X * 2) / _game.GraphicsDevice.Viewport.Width - 1);
            }

            // TODO: Pull out boomerang collision into separate function and only call in on boomerangs within player adjacent partitions
            int playerPartition = _player.Partition;

            foreach (Boomerang boomerang in _boomerangList)
            {
                boomerang.Update(gameTime);

                if (boomerang.Partition == playerPartition - 1 || boomerang.Partition == playerPartition || boomerang.Partition == playerPartition + 1)
                {
                    boomerang.DetectCollisionWithPlayer();
                }
            }
        }

        public void RemoveBoomerangs()
        {
            List<Boomerang> removeList = new List<Boomerang>();

            foreach (Boomerang boomerang in _boomerangList)
            {
                if (boomerang.MarkedForRemoval) removeList.Add(boomerang);
            }

            int removeCount = removeList.Count();

            if (removeCount > 0)
            {
                _boomerangList = _boomerangList.Except(removeList).ToList();
                if (!_player.FatalCollision)
                {
                    _game.Score += removeCount;
                    if (_game.Score > _game.HighScore) _game.HighScore = _game.Score;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Boomerang boomerang in _boomerangList)
            {
                boomerang.Draw(spriteBatch);
            }
        }

        public void UpdateAnimation(GameTime gameTime)
        {
            foreach (Boomerang boomerang in _boomerangList)
            {
                boomerang.UpdateAnimation(gameTime);
            }
        }

        public Dictionary<string, Animation> LoadAnimations(ContentManager content)
        {
            // Load SFX
            _throwSFX = content.Load<SoundEffect>("SoundEffects/Throw");

            Dictionary<string, Animation> _animations = new Dictionary<string, Animation>()
            {
                { "Left", new Animation(content.Load<Texture2D>("Boomerang/Boomerang-Left"), 8) },
                { "Right", new Animation(content.Load<Texture2D>("Boomerang/Boomerang-Right"), 8) }
            };

            // Correct frame speeds
            _animations["Right"].FrameSpeed = 60f;
            _animations["Left"].FrameSpeed = 60f;

            return _animations;
        }
    }
}
