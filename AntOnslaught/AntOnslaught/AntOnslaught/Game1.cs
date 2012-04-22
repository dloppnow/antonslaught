using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace AntOnslaught
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        AudioManager audioManager;
        Renderer rend;
        Map map;
        List<MovableObject> movableObjs;
        Menu menu;
        KeyboardState keyState;
        MouseState mouseState;
        Vector2 sizeOfScreen;
        Vector2 currentMapLoc; //in pixels
        Vector2 mouseLeftPressed;
        bool leftReleased = true;
        bool leftPressed = false;
        List<Ant> selectedAnts;
        //GUI members
        SpriteFont font;
        Rectangle background;
        Rectangle workerButton;
        Rectangle soldierButton;
        Texture2D dummyTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            sizeOfScreen.Y = graphics.PreferredBackBufferHeight;
            sizeOfScreen.X = graphics.PreferredBackBufferWidth;
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
            // TODO: Add your initialization logic here
            movableObjs = new List<MovableObject>();
            selectedAnts = new List<Ant>();
            //movableObjs.Add(new WorkerAnt(new Vector2(0, 0), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            //movableObjs.Add(new WorkerAnt(new Vector2(3, 3), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            //movableObjs.Add(new WorkerAnt(new Vector2(3, 1), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            //movableObjs.Add(new WorkerAnt(new Vector2(1, 3), new SpriteAnimation(Content.Load<Texture2D>("worker_sprite_sheet"), 32, 32, 100)));
            base.Initialize();
            this.IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            map = new Map(Content);
            movableObjs.AddRange(map.getNewObjects());
            map.setTexture(Content.Load<Texture2D>("tile_sheet"));
            rend = new Renderer(spriteBatch, GraphicsDevice.Viewport, new Vector2(15, 15), Content);
            audioManager = new AudioManager(Content);
			menu = new Menu(spriteBatch, Content);
			currentMapLoc = sizeOfScreen / 2 + rend.getViewCenter() * 32;
            currentMapLoc.X -= 16;
            currentMapLoc.Y -= 16;

            //Initialize GUI members
            font = Content.Load<SpriteFont>("Font");
            dummyTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            dummyTexture.SetData(new Color[] { Color.White });
            background = new Rectangle(0, GraphicsDevice.Viewport.Height - 50, 150, 50);
            workerButton = new Rectangle(0, GraphicsDevice.Viewport.Height - 50, 150, 50);
            soldierButton = new Rectangle();
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

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();

            // TODO: Add your update logic here
            base.Update(gameTime);
            updateMenuState(gameTime);
            if (menu.shouldQuit())
                this.Exit();
            if (!menu.isPaused())
                updateGameState(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            if (menu.isPaused())
            {
                drawMenuState();
            }
            else
            {
                drawGameState();
            }
            base.Draw(gameTime);
        }

        public void updateGameState(GameTime gameTime)
        {
            foreach (MovableObject obj in movableObjs)
            {
                if(!obj.hasPath() && obj.geta)
                {

                }
                if (!obj.updateMovement(gameTime))
                {
                    if (obj.getGoalCell().passable)
                    {
                        obj.setPath(map.getPath(obj.getCurrentCell(), obj.getGoalCell()));
                    }
                }
                if (obj is Ant)
                {
                    if (obj is QueenAnt)
                    {
                        QueenAnt ant = (QueenAnt)obj;
                        ant.update(gameTime);
                    }
                    else
                    {
                        Ant ant = (Ant)obj;
                        ant.update(gameTime);
                    }
                }
            }
            
            if ( mouseState.RightButton == ButtonState.Pressed )
            {
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);
                if ((mousePos.X > currentMapLoc.X) && (mousePos.Y > currentMapLoc.Y) &&
                    (mousePos.X < currentMapLoc.X + map.getWidth() * 32) && (mousePos.Y < currentMapLoc.Y + map.getHeight() * 32))
                {
                    Vector2 mapMousePos = new Vector2(mouseState.X - currentMapLoc.X, mouseState.Y - currentMapLoc.Y);
                    foreach (Ant ant in selectedAnts)
                    {
                        //Cell c = map.getCell((int)mapMousePos.X / 32, (int)mapMousePos.Y / 32);
                        if (ant.getGoalCell() != null)
                        {
                            ant.getGoalCell().occupied = false;
                        }
                        Cell desCell = map.getCell((int)mapMousePos.X / 32, (int)mapMousePos.Y / 32);
                        if (desCell.occupied && desCell.passable || desCell.food != null)
                        {
                            desCell = map.findUnoccupiedClosestCell(desCell);
                        }
                        if (desCell != null && desCell.passable)
                        {
                            ant.setGoalCell(desCell);
                            ant.getGoalCell().occupied = true;
                            if (ant.getCurrentCell() == null)
                            {
                                ant.setCurrentCell(map.getCell((int)ant.getPosition().X / 32, (int)ant.getPosition().Y / 32));
                            }
                            ant.setPath(map.getPath(ant.getCurrentCell(), ant.getGoalCell()));
                        }
                    }
                }
            }
            if (mouseState.LeftButton == ButtonState.Pressed && leftReleased == true)
            {
                mouseLeftPressed = new Vector2(mouseState.X, mouseState.Y);
                leftPressed = true;
                leftReleased = false;
            }
            if (mouseState.LeftButton == ButtonState.Released && leftPressed == true)
            {
                leftPressed = false;
                leftReleased = true;
                selectedAnts.Clear();
                Vector2 mousePos = new Vector2(mouseState.X, mouseState.Y);

                Vector2 mapMousePos = new Vector2(mouseState.X - currentMapLoc.X, mouseState.Y - currentMapLoc.Y);
                Vector2 mapMouseOldPos = new Vector2(mouseLeftPressed.X - currentMapLoc.X, mouseLeftPressed.Y - currentMapLoc.Y);
                Rectangle mouseSelectBox;
                //if (mapMouseOldPos.X > mapMousePos.X && mapMouseOldPos.Y > mapMousePos.Y)
                //{
                int recX = 0;
                int recY = 0;
                int recWidth = 0;
                int recHeight = 0;
                if (mapMouseOldPos.X - (int)mapMousePos.X < 0)
                {
                    recX = (int)mapMouseOldPos.X;
                    recWidth = (int)mapMousePos.X - (int)mapMouseOldPos.X;

                }
                else
                {
                    recX = (int)mapMousePos.X;
                    recWidth = (int)mapMouseOldPos.X - (int)mapMousePos.X;
                }
                if (mapMouseOldPos.Y - (int)mapMousePos.Y < 0)
                {
                    recY = (int)mapMouseOldPos.Y;
                    recHeight = (int)mapMousePos.Y - (int)mapMouseOldPos.Y;

                }
                else
                {
                    recY = (int)mapMousePos.Y;
                    recHeight = (int)mapMouseOldPos.Y - (int)mapMousePos.Y;
                }
                mouseSelectBox = new Rectangle(recX, recY, recWidth, recHeight);
                foreach (Ant ant in movableObjs)
                {
                    Rectangle bounding = new Rectangle((int)ant.getPosition().X, (int)ant.getPosition().Y, 32, 32);
                    if (mouseSelectBox.Width == 0 && mouseSelectBox.Height == 0)
                    {
                        if (bounding.Contains(mouseSelectBox.Center))
                        {
                            selectedAnts.Add(ant);
                        }
                    }
                    else if (mouseSelectBox.Contains(bounding.Center))
                    {
                        selectedAnts.Add(ant);
                    }
                }
            }
            //}            //Move the map around
            if (keyState.IsKeyDown(Keys.Left))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X - 1, vec.Y));
                currentMapLoc.X += 32;
            }
            if (keyState.IsKeyDown(Keys.Right))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X + 1, vec.Y));
                currentMapLoc.X -= 32;
            }
            if (keyState.IsKeyDown(Keys.Up))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X, vec.Y - 1));
                currentMapLoc.Y += 32;
            }
            if (keyState.IsKeyDown(Keys.Down))
            {
                Vector2 vec = rend.getViewCenter();
                rend.setViewCenter(new Vector2(vec.X, vec.Y + 1));
                currentMapLoc.Y -= 32;
            }
            if (keyState.IsKeyDown(Keys.D1))
            {
                audioManager.queueEffect(AudioManager.Effect.Blip);
            }
        }

        public void drawGameState()
        {
            audioManager.playEffects();
            rend.Draw(map);
            rend.DrawSelectionCircles(selectedAnts);
            foreach (MovableObject obj in movableObjs)
            {
                rend.Draw(obj);
            }
        }

        public void updateMenuState(GameTime gameTime)
        {
            menu.update(gameTime, keyState, mouseState);
        }

        public void drawMenuState()
        {
            menu.draw();
        }

        public void updateGUI(GameTime gameTime)
        {

        }

        public void drawGUI()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(dummyTexture, quitButton, textBackColor);
            spriteBatch.End();
        }
    }
}
