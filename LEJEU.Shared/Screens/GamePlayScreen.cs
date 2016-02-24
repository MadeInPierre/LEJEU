using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace LEJEU.Shared
{
	class GamePlayScreen : GameScreen
	{
		Level level;
        Boolean pause = false;

		public override void Initialize()
		{
			level = new Level();
			level.Initialize();
		}

		public override void LoadContent(ContentManager Content, InputManager input)
		{
			base.LoadContent(Content, input);
			level.LoadContent(Content);
		}

		public override void UnloadContent()
		{
			base.UnloadContent();
			level.UnloadContent();
		}

		public override void Update(GameTime gameTime, InputManager input)
		{
			//if(pause) menuPause();
            if (input.KeyPressed(Keys.Escape)) pause = !pause;
			if(!pause) level.Update(gameTime, input);

		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
			level.Draw(sb);
            if(pause) level.drawPause(sb);
		}


	}
}