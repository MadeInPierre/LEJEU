using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;


namespace LEJEU.Shared
{
    public class MapZonesManager
    {
        #region Variables
        ContentManager Content;
        int NZones, NBacks;
        float backgroundSpeed;
        int currentZone, limit1Backs, limit2Backs; //zone actuellement au centre

        public int CurrentZone
        {
            get { return currentZone;  }
            set { currentZone = value; }
        }

        Vector2 spawnPosition;



        Texture2D[] zonePlatforms;
        Texture2D[] zoneForegrounds;
        Texture2D[] backgrounds;

        int currentCamPos;
        #endregion


        public MapZonesManager(LevelProperties LP)
        {
            spawnPosition = LP.playerSpawnPos;
			currentZone = (int)(LP.playerSpawnPos.X / ResolutionManager.GameRes.X);
            NZones = LP.NZones;
            NBacks = LP.Backgrounds.Count;
            backgroundSpeed = 0.5f;
            Console.WriteLine(backgroundSpeed);
            zonePlatforms = new Texture2D[NZones + 1]; //pour aller de 1 a NZones (0 ne sert a rien, zone vide)
            zoneForegrounds = new Texture2D[NZones + 1];

            backgrounds = new Texture2D[NBacks + 1];
        }

        public void LoadContent(ContentManager Content, LevelProperties LP)
        {
            this.Content = Content;
                backgrounds[0] = Content.Load<Texture2D>(LP.Backgrounds[0].path);


                for (int i = 0; i < NZones; i++)
                {
                    zonePlatforms[i] = Content.Load<Texture2D>(LP.Zones[i].platformPath);
                    zonePlatforms[i] = null;
                    zoneForegrounds[i] = Content.Load<Texture2D>(LP.Zones[i].foregroundPath);
                    zoneForegrounds[i] = null;
                }

            
                if (currentZone == 0)
                {
                    loadZone(0, LP); if(NZones != 0) loadZone(1, LP);
                }
                else if (currentZone == NZones)
                {
                    loadZone(currentZone, LP); loadZone(currentZone + 1, LP);
                }
                else
                {
                    loadZone(currentZone - 1, LP); loadZone(currentZone, LP); loadZone(currentZone + 1, LP);
                }
        }

        /// <summary>
        /// ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        public void UnloadContent()
        {
            for (int i = 0; i < NZones; i++)
            {
                try 
                { 
                    zonePlatforms[i].Dispose();
                    zoneForegrounds[i].Dispose();
                    //backgrounds[i].Dispose();
                }
                catch {}
            }
        }
        protected void loadZone(int targetZone, LevelProperties LP)
        {
            if (targetZone >= 0 && targetZone <= NZones)
            {
                zonePlatforms[targetZone] = Content.Load<Texture2D>(LP.Zones[targetZone].platformPath);
                zoneForegrounds[targetZone] = Content.Load<Texture2D>(LP.Zones[targetZone].foregroundPath);
            }
        }
        protected void unloadZone(int targetZone)
        {
            if(targetZone >= 0 && targetZone <= NZones)
            {
                zonePlatforms[targetZone] = null;
                zoneForegrounds[targetZone] = null;
            }
        }

                    protected void loadBack(int targetZone, LevelProperties LP)
                    {
                        //backgrounds[targetZone] = Content.Load<Texture2D>(LP.Backgrounds[targetZone - 1].path);
                    }
                    protected void unloadBack(int targetZone)
                    {
                       // backgrounds[targetZone] = null;
                    }

        public void Update(InputManager input, Camera camera, Player player, LevelProperties LP)
        {

            ///////////Backgrounds////////////
			limit1Backs = (int)((camera.CameraPosition.X * (1 - backgroundSpeed)) / ResolutionManager.GameRes.X);
			limit2Backs = (int)((camera.CameraPosition.X * (1 - backgroundSpeed) + ResolutionManager.ActiveRes.X / ResolutionManager.WindowScale) / ResolutionManager.GameRes.X);

            for(int i = 0; i <= NBacks; i++)
            {
                if (i < limit1Backs || i > limit2Backs)
                    backgrounds[i] = null;
                else backgrounds[i] = Content.Load<Texture2D>(LP.Backgrounds[i].path);
            }


            ///////////Zones////////////
			currentCamPos = (int)(camera.CameraPosition.X + (ResolutionManager.ActiveRes.X / ResolutionManager.InGameScale / 2));

			if (currentCamPos < currentZone * ResolutionManager.GameRes.X && currentZone > 0)
            { //quand la camera passa a gauche de la currentZone
                loadZone(currentZone - 2, LP); //charger la prochaine zone a gauche
                unloadZone(currentZone + 1); //decharger la zone la plus a droite qui est sortie du champ

                currentZone--;
            }
			else if (currentCamPos > (currentZone + 1) * ResolutionManager.GameRes.X && currentZone < NZones)
            {//quand la camera passe dans la zone de droite de currentZone (currentZone + 1)
                loadZone(currentZone + 2, LP);
                unloadZone(currentZone - 1);

                currentZone++;
            }
            
        }

        public void DrawBackGrounds(SpriteBatch sb, Camera camera, float alpha)
        {
            for (int j = limit1Backs; j <= limit2Backs; j++)
            {
                sb.Draw(backgrounds[j],
					new Vector2(j * ResolutionManager.GameRes.X + (camera.CameraPosition.X * backgroundSpeed), 0),
                Color.White * alpha);
            }
        }

        public void DrawPlatforms(SpriteBatch sb, float alpha)
        {
            for (int i = -1; i < 2; i++)
            {
                if (currentZone + i >= 0 && currentZone + i <= NZones && zonePlatforms[currentZone + i] != null)
					sb.Draw(zonePlatforms[currentZone + i], new Vector2((currentZone + i) * ResolutionManager.GameRes.X, 0), Color.White * alpha);
            }
        }

        //                               .    ______._____
        //                                    |          |
        //                 (------------)(----|------)(--|-------)
        //                                    |__________|


        public void DrawForeground(SpriteBatch sb, float alpha)
        {
            for (int i = -1; i < 2; i++)
            {
                if (currentZone + i >= 0 && currentZone + i <= NZones && zoneForegrounds[currentZone + i] != null)
					sb.Draw(zoneForegrounds[currentZone + i], new Vector2((currentZone + i) * ResolutionManager.GameRes.X, 0), Color.White * alpha);
            }
        }



    }
}
