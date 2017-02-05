using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLife.Content
{
    public class Board
    {
        private int gridWidth;
        private int gridHeight;
        private bool[,] board;
        private Random rand = new Random();
        private ContentManager content;
        private Texture2D texture;
        Vector2 origin;
        float timeSinceLastTick;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="content"></param>
        /// <param name="gridWidth"></param>
        /// <param name="gridHeight"></param>
        public Board( ContentManager content, int gridWidth, int gridHeight )
        {
            this.content = content;
            texture = content.Load<Texture2D>( "Colors" );
            origin = new Vector2( texture.Width / 2, texture.Height / 2 );

            this.gridWidth = gridWidth;
            this.gridHeight = gridHeight;

            board = new bool[gridWidth + 1, gridHeight + 1];
        }

        /// <summary>
        /// Gets a total count of adjacent neighbors
        /// </summary>
        /// <param name="xPos">The X position of the cell to check.</param>
        /// <param name="yPos">The Y position of the cell to check.</param>
        /// <returns></returns>
        public int GetNeighborCount( int xPos, int yPos )
        {
            int count = 0;

            for ( int x = xPos - 1; x <= xPos + 1 && xPos > 0 && xPos <= gridWidth; x++ )
            {
                for ( int y = yPos - 1; y <= yPos + 1 && yPos > 0 && yPos <= gridHeight; y++ )
                {
                    count += board[x, y] ? 1 : 0;
                }
            }

            return count - ( board[xPos, yPos] ? 1 : 0 );
        }

        /// <summary>
        /// Populates the board with a random amount of cells
        /// </summary>
        public void PopulateBoard()
        {
            int popCount = 0;

            for ( int x = 0; x < gridWidth; x++ )
            {
                for ( int y = 0; y < gridHeight; y++ )
                {
                    if ( true )
                    {
                        var temp = rand.Next( -1, 2 );
                        if( temp == 1 )
                        {
                            board[x, y] = true;
                        }
                        popCount++;
                    }
                }
            }
        }

        /// <summary>
        /// Adds a cell to the board
        /// </summary>
        /// <param name="x">The X position of the placement.</param>
        /// <param name="y">The Y position of the placement.</param>
        public void AddCellToBoard( int x, int y )
        {
            if ( x > 0 && y > 0 && x <= gridWidth && y <= gridHeight )
            {
                board[x, y] = true;
            }
        }

        /// <summary>
        /// Processes the simulation
        /// </summary>
        public void ProcessBoard()
        {
            // create a board to temporarily store the results
            bool[,] tempBoard = new bool[gridWidth + 1, gridHeight + 1];
            int neighborCounts = 0;
            bool isAlive = false;

            // Flag cells as either alive or dead
            for ( int x = 0; x < gridWidth; x++ )
            {
                for ( int y = 0; y < gridHeight; y++ )
                {
                    // get the number of neighbors
                    neighborCounts = GetNeighborCount( x, y );

                    // check if this cell is alive
                    isAlive = board[x, y];

                    // Process this cells results based on the game of life rules.
                    tempBoard[x, y] = isAlive && ( neighborCounts == 2 || neighborCounts == 3 ) || !isAlive && neighborCounts == 3; 
                }
            }

            board = tempBoard;
        }

        /// <summary>
        /// Updates the board during the game tick
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update( GameTime gameTime )
        {
            timeSinceLastTick += ( float ) gameTime.ElapsedGameTime.TotalSeconds;

            if ( timeSinceLastTick > .05f )
            {
                ProcessBoard();
                timeSinceLastTick = 0f;
            }
        }

        /// <summary>
        /// Draws the cells to the screen
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw( SpriteBatch spriteBatch )
        {
            for ( int x = 0; x < gridWidth; x++ )
            {
                for ( int y = 0; y < gridHeight; y++ )
                {
                    if ( board[x,y] == true )
                    {
                        spriteBatch.Draw( texture, new Vector2( x + 2, y + 2 ), new Rectangle( 3, 3, 1, 1 ), Color.Yellow, 0f, origin, 1f, SpriteEffects.None, 0f );
                    }
                }
            }
        }
    }
}
