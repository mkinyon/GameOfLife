using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using GameOfLife.Content;

namespace GameOfLife
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameOfLife : Game
    {
        //must be power of 2
        const int renderScale = 8;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Board board;
        Matrix matrix;
        bool isPaused = false;

        public GameOfLife()
        {
            graphics = new GraphicsDeviceManager( this );
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// </summary>
        protected override void Initialize()
        {
            this.IsMouseVisible = true;

            matrix = Matrix.CreateScale( renderScale, renderScale, 1.0f );
            graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            board = new Board( Content, GraphicsDevice.DisplayMode.Width / renderScale, GraphicsDevice.DisplayMode.Height / renderScale );
            board.PopulateBoard();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch( GraphicsDevice );
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update( GameTime gameTime )
        {
            var state = Mouse.GetState();

            if ( GamePad.GetState( PlayerIndex.One ).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown( Keys.Escape ) )
                Exit();

            if ( state.RightButton == ButtonState.Pressed )
            {
                board.PopulateBoard();
            }

            if ( state.LeftButton == ButtonState.Pressed )
            {
                board.AddCellToBoard( state.X / renderScale, state.Y / renderScale );
            }

            if ( state.MiddleButton == ButtonState.Pressed )
            {
                isPaused = !isPaused;
            }

            if ( !isPaused )
            {
                board.Update( gameTime );
            }

            base.Update( gameTime );
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw( GameTime gameTime )
        {
            GraphicsDevice.Clear( Color.DarkSlateGray );

            spriteBatch.Begin( transformMatrix: matrix );
            board.Draw( spriteBatch );
            spriteBatch.End();

            base.Draw( gameTime );
        }
    }
}
