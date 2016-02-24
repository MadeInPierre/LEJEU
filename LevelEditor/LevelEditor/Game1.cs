using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using FarseerPhysics;

namespace LevelEditor
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private IntPtr drawSurface;
        public static MainForm form;

        public static GraphicsDeviceManager graphics;
        public static GraphicsDevice graphicsDevice;
        SpriteBatch sb;

        InputManager input;
        public static Editor editor;

        public static Vector2 Dimensions;

        public Game1(string LoadPath)
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            form = new MainForm(LoadPath);
            form.Icon = System.Drawing.Icon.ExtractAssociatedIcon(@"Icon.ico");
            form.ShowIcon = true;
            form.Show();

            
            drawSurface = form.getDrawSurface();
            graphics.PreparingDeviceSettings +=
                new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            System.Windows.Forms.Control.FromHandle((this.Window.Handle)).VisibleChanged +=
                new EventHandler(Game1_VisibleChanged);

            form.XNAWindow.Resize += new EventHandler(resChanged);

            input = new InputManager();
            editor = new Editor();
            Dimensions = new Vector2((int)form.XNAWindowDimensions.X, (int)form.XNAWindowDimensions.Y);
        }

        /// <summary>
        /// Draws the xna window in the designated drawSurface panel.
        /// </summary>
        ///
        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.DeviceWindowHandle =
                drawSurface;
        }

        /// <summary>
        /// Puts the original XNA Form invisible.
        /// </summary>
        private void Game1_VisibleChanged(object sender, EventArgs e)
        {
            if (System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible == true)
                System.Windows.Forms.Control.FromHandle((this.Window.Handle)).Visible = false;
        }

        private void resChanged(object sender, EventArgs e)
        {
            graphics.PreferredBackBufferWidth = (int)form.XNAWindowDimensions.X;
            graphics.PreferredBackBufferHeight = (int)form.XNAWindowDimensions.Y;
            graphics.ApplyChanges();

            Dimensions = new Vector2((int)form.XNAWindowDimensions.X, (int)form.XNAWindowDimensions.Y);
            editor.camera.UpdateScale(Dimensions);
        }

        protected override void Initialize()
        {
            LevelProperties.Initialize();
            EditorVariables.Initialize();
            editor.Initialize();
            editor.camera.UpdateScale(Dimensions);
            base.Initialize();
            form.Initialize();

            ConvertUnits.SetDisplayUnitToSimUnitRatio(42.2f);
        }


        protected override void LoadContent()
        {
            resChanged(null, null);
            // Create a new SpriteBatch, which can be used to draw textures.
            graphicsDevice = GraphicsDevice;
            sb = new SpriteBatch(GraphicsDevice);
            editor.LoadContent(Content);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            input.Update();
            if (input.KeyPressed(Keys.Escape)) this.Exit();

            editor.Update(gameTime, input);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //sb.Begin();
            editor.Draw(sb);
            //sb.End();            
            base.Draw(gameTime);
        }
    }
}
