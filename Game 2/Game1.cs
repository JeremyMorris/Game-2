using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;

namespace Game_2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Song song;
        Player player;
        BoomerangManager boomerangManager;
        Texture2D background;
        SpriteFont font;
        Texture2D victory;
        Texture2D loss;
        bool gameWon;
        bool gameLost;
        bool songPlaying;
        float friction;
        float gravity;

        public int Score { get; set; }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "Boomerangs?!";

            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 300;
            graphics.ApplyChanges();

            friction = 0.04f;
            gravity = 0.06f;
            gameLost = false;
            gameWon = false;

            player = new Player(new Vector2((GraphicsDevice.Viewport.Width / 2) - 200, GraphicsDevice.Viewport.Height - 64), this, friction, gravity);
            boomerangManager = new BoomerangManager(new System.Random(), this, ref player);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.2f;
            songPlaying = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load assets
            victory = Content.Load<Texture2D>("Victory");
            loss = Content.Load<Texture2D>("Loss");
            font = Content.Load<SpriteFont>("Score");
            background = Content.Load<Texture2D>("Game2-Background");
            song = Content.Load<Song>("CISGame2");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // Remove marked objects
            boomerangManager.RemoveBoomerangs();

            // Play music
            if (!songPlaying)
            {
                MediaPlayer.Play(song);
                songPlaying = true;
            }

            // Update game objects
            player.Update(gameTime);
            boomerangManager.Update(gameTime);

            // Detect ending of game
            if (player.FatalCollision) gameLost = true;

            // Fade music out if game is lost
            if (gameLost && MediaPlayer.Volume > 0)
            {
                MediaPlayer.Volume -= 0.02f;
            }

            // Handle game restart
            if (gameLost && Keyboard.GetState().IsKeyDown(Keys.R))
            {
                MediaPlayer.Volume = 0.2f;
                MediaPlayer.Play(song);
                Score = 0;
                player.FatalCollision = false;
                player.SetX((GraphicsDevice.Viewport.Width / 2) - 200);
                boomerangManager._spawnInterval = 2000;
                boomerangManager._boomerangList.Clear();
                gameLost = false;
            }


            // update animations
            player.UpdateAnimation(gameTime);
            boomerangManager.UpdateAnimation(gameTime);


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // spriteBatch Begin arguments from Stack Overflow post https://stackoverflow.com/questions/25145377/xna-blurred-sprites-when-scaled
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            // Draw background
            spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            // Draw player
            player.Draw(spriteBatch);
            boomerangManager.Draw(spriteBatch);

            // Draw text
            if (gameLost)
            {
                spriteBatch.Draw(loss, new Rectangle((GraphicsDevice.Viewport.Width - 800) / 2, (GraphicsDevice.Viewport.Height - 700) / 2, 800, 700), Color.White);
                spriteBatch.DrawString(font, "PRESS 'R' TO RESTART", new Vector2(380, 200), Color.DarkRed);
            }

            spriteBatch.DrawString(font, "Score: " + Score, new Vector2(10, 10), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
