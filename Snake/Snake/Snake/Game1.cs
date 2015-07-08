using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Snake
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public const int EMPTY = 0;
        public const int SNAKE = 1;
        public const int FOOD = 2;

        public const int ColLength = 50;

        public const int RowLength=50;
        int score = 0;
        int passed = 150;
        int[,] grid;
        List<int> snakex;
        List<int>snakey;
        Boolean alive = true;
        Vector2 textPos;
        Color textC;
        Texture2D blue;
        Texture2D red;
        Texture2D white;
        Texture2D temp;
        string state;
        KeyboardState key;
        SpriteFont font1;

        int velox=0;
        int veloy=-1;

        public String wherego = "up";

        public int foodx;
        public int foody;
        Random dice;
        int time = 0;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            grid = new int[50, 50];
            snakex=new List<int>();
            snakey = new List<int>();
            dice = new Random();
            state = "score : " + score;
            textC = Color.Black;
            graphics.PreferredBackBufferWidth = 500;
            graphics.PreferredBackBufferHeight = 500;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            snakex.Cast<int>();
            snakey.Cast<int>();
            snakex.Add(RowLength/2);
            snakey.Add(ColLength/2);
            grid[RowLength / 2, ColLength / 2] = 1;
            grid = Setfood(grid);
            textPos = new Vector2();
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

            blue = new Texture2D(GraphicsDevice, 10, 10);
            red = new Texture2D(GraphicsDevice, 10, 10);
            white = new Texture2D(GraphicsDevice, 10, 10);
            font1 = Content.Load<SpriteFont>("SpriteFont1");

            temp = white;
            Color[] a = new Color[red.Width * red.Height];
            for(int i=0;i<a.Length;i++){
                a[i] = Color.Red;
            }
            red.SetData<Color>(a);
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = Color.Blue;
            }
            blue.SetData<Color>(a);
            for (int i = 0; i < a.Length; i++)
            {
                a[i] = Color.White;
            }
            white.SetData<Color>(a);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }
        public int[,] Setfood(int[,] grid)
        {
            List<Vector2> b = new List<Vector2>();
            for (int i = 0; i < ColLength; i++)
            {
                for (int j = 0; j < RowLength; j++)
                {
                    if (grid[i, j] == 0)
                        b.Add(new Vector2(i, j));
                }
            }
            int bla = dice.Next(b.Count);
            int rx = (int)b[bla].X;
            int ry = (int)b[bla].Y;
            foodx = (int)(b[bla].X);
            foody = (int)(b[bla].Y);
            grid[foodx, foody] = 2;
            return grid;
        }

        
        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            key = Keyboard.GetState();
            if (alive)
            {
                textPos = Vector2.Zero;
                textC = Color.Black;
                state = "score : " + score;
                time += gameTime.ElapsedGameTime.Milliseconds;

                if (time >= passed)
                {
                    time = 0;

                    int nx = (int)(snakex.ElementAt(0));
                    int ny = (int)(snakey.ElementAt(0));
                    nx += velox;
                    ny += veloy;
                    if (ny >= ColLength)
                    {
                        ny = 0;
                    }
                    if (ny < 0)
                    {
                        ny = ColLength - 1;
                    }
                    if (nx >= RowLength)
                    {
                        nx = 0;
                    }
                    if (nx < 0)
                    {
                        nx = RowLength - 1;
                    }

                    if (grid[nx, ny] == 1)
                    {
                        alive = false;

                    }
                    if (grid[nx, ny] == 2)
                    {
                        grid = Setfood(grid);
                        score++;
                    }
                    else
                    {
                        int tailx = (int)(snakex.ElementAt(snakex.Count - 1));
                        snakex.RemoveAt(snakex.Count - 1);
                        int taily = (int)(snakey.ElementAt(snakey.Count - 1));
                        snakey.RemoveAt(snakey.Count - 1);
                        grid[tailx, taily] = 0;
                    }
                    grid[nx, ny] = 1;
                    snakex.Insert(0, nx);
                    snakey.Insert(0, ny);

                    
                }


                if (key.IsKeyDown(Keys.Right) && !wherego.Equals("left"))
                {
                    velox = 1;
                    veloy = 0;
                    wherego = "right";
                }
                else if (key.IsKeyDown(Keys.Left) && !wherego.Equals("right"))
                {
                    velox = -1;
                    veloy = 0;
                    wherego = "left";
                }
                else if (key.IsKeyDown(Keys.Up) && !wherego.Equals("down"))
                {
                    velox = 0;
                    veloy = -1;
                    wherego = "up";
                }
                else if (key.IsKeyDown(Keys.Down) && !wherego.Equals("up"))
                {
                    velox = 0;
                    veloy = 1;
                    wherego = "down";
                }
                // TODO: Add your update logic here

                passed = key.IsKeyDown(Keys.LeftShift) ? 10 : 40;
            }
            else
            {
                textPos = new Vector2(RowLength / 2, ColLength / 2);
                textC = Color.Red;
                state = "you lose : clicked r to restart";
                if (key.IsKeyDown(Keys.R))
                {
                    for (int i = 0; i < ColLength; i++)
                    {
                        for (int j = 0; j < RowLength; j++)
                        {
                            grid[i, j] = 0;
                        }
                    }
                        score = 0;
                    alive = true;
                    snakex.Clear();
                    snakey.Clear();
                    snakex.Add(RowLength / 2);
                    snakey.Add(ColLength / 2);
                    grid[RowLength / 2, ColLength / 2] = 1;
                    velox=0;
                    veloy=-1;
                    wherego = "up";
                    grid = Setfood(grid);
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = 0; i < ColLength; i++)
            {
                for (int j = 0; j < RowLength; j++)
                {
                    switch (grid[i, j])
                    {
                        case 0:
                            {
                                temp = white;
                                break;
                            }
                        case 1:
                            {
                                temp = blue;
                                break;
                            }
                        case 2:
                            {
                                temp = red;
                                break;
                            }
                    }
                    
                    spriteBatch.Draw(temp, new Vector2(10 * i, 10 * j), Color.White);
                }
            }
            spriteBatch.DrawString(font1, state, textPos, textC);
                spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
