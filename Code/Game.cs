using System;
using JamUtilities;
using JamUtilities.Particles;
using JamUtilities.ScreenEffects;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;

namespace JamTemplate
{
    class Game
    {

        #region Fields

        private State _gameState;

        World _myWorld;
        Score _gameStats;
        float _timeTilNextInput = 0.0f;

        public int EvolutionPoints { get; private set; }

        private Music _bgm;

        private enum eMenuState
        {
            MS_START,
            MS_WORLD,
            MS_TRIBE
        }

        eMenuState _menuState;

        WorldInterfaces.cWorldProperties _worldProperties;
        WorldInterfaces.AnimalProperties _tribeProperties;

        #endregion Fields

        #region Methods

        public Game()
        {
            _bgm = new Music("../SFX/bgm.ogg");
            _bgm.Loop = true;
            _bgm.Play();
            // Predefine game state to menu
            _gameState = State.Menu;
            _menuState = eMenuState.MS_START;

            //TODO  Default values, replace with correct ones !
            SmartSprite._scaleVector = new Vector2f(2.0f, 2.0f);
            ScreenEffects.Init(new Vector2u(800, 600));
            ParticleManager.SetPositionRect(new FloatRect(-500, 0, 1400, 600));
            //ParticleManager.Gravity = GameProperties.GravitationalAcceleration;


            ResetCreationParameters();


            try
            {
                SmartText._font = new Font("../GFX/font.ttf");

                SmartText._lineLengthInChars = 18;
                SmartText._lineSpread = 1.2f;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }

        private void ResetCreationParameters()
        {
            _worldProperties = new WorldInterfaces.cWorldProperties();
            _worldProperties.HeightMapNoiseLength = 20.0f;
            _worldProperties.WorldSizeInTiles = new SFML.Window.Vector2i(GameProperties.WorldSizeInTiles.X, GameProperties.WorldSizeInTiles.Y);

            _worldProperties.MaxHeightInMeter = 100.0f;
            _worldProperties.AtmosphericHeatOutFluxPerSecond = 3.4f/1000.0f;
            _worldProperties.SunHeatInfluxPerSecond = 1.0f;
            _worldProperties.DayNightCycleFrequency = 0.11f;
            _worldProperties.SunLightIntensityFactor = 0.75f * _worldProperties.DayNightCycleFrequency;

            _worldProperties.TileTemperatureChangeMaximum = 3;
            _worldProperties.TileTemperatureExchangeAmplification = 0.5f;

            _worldProperties.MountainHeight = 70;
            _worldProperties.WaterFreezingTemperature = 283.5f;
            _worldProperties.DesertGrassTransitionAtHeightZero = 304.0f;
            _worldProperties.DesertGrassTransitionAtMountainHeight = 298.50f;
            _worldProperties.WaterGrassTransitionAtHeightZero = 304.50f;
            _worldProperties.WaterGrassTransitionHeightAtWaterFreezingPoint = 30.0f;

            _worldProperties.RainWaterAmount = 1.0f;
            _worldProperties.PlantGrowthWaterAmount = 3.0f;
            _worldProperties.PlantGrowthRate = 5.0f;
            _worldProperties.CloudNumber = 1;

            _worldProperties.TileTemperatureIntegrationTimer = 1.5f;

            _worldProperties.WorldSizeInTiles = GameProperties.WorldSizeInTiles;



            _tribeProperties = new WorldInterfaces.AnimalProperties();

            _tribeProperties.Agility = 1;
            _tribeProperties.Diet = WorldInterfaces.AnimalProperties.DietType.CARNIVORE;
            _tribeProperties.GroupBehaviour = 1;
            _tribeProperties.PreferredAltitude = 50;
            _tribeProperties.PreferredTemperature = 300;
            _tribeProperties.PreferredTerrain = WorldInterfaces.AnimalProperties.TerrainType.LAND;
            _tribeProperties.Stamina = 1;
            _tribeProperties.Strength = 1;
            _tribeProperties.NumberOfAnimals = 300;

            EvolutionPoints = GameProperties.EvolutionPointsStart;
        }

        public void GetInput()
        {
            if (_timeTilNextInput < 0.0f)
            {
                if (_gameState == State.Menu)
                {
                    GetInputMenu();
                }
                else if (_gameState == State.Game)
                {
                    _myWorld.GetInput();
                }
                else if (_gameState == State.Credits || _gameState == State.Score)
                {
                    GetInputCreditsScore();
                }
            }
        }

        private void GetInputMenu()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
            {
                
                if (_menuState == eMenuState.MS_START)
                {
                    _menuState = eMenuState.MS_WORLD;
                    _timeTilNextInput = 0.5f;
                }
                else if (_menuState == eMenuState.MS_WORLD)
                {
                    if (EvolutionPoints >= GameProperties.EvolutionPointsStart - GameProperties.EvolutionPointsWorldMax)
                    {
                        _menuState = eMenuState.MS_TRIBE;
                        _timeTilNextInput = 0.5f;
                    }
                }
                else if (_menuState == eMenuState.MS_TRIBE)
                {
                    if (EvolutionPoints >= 0)
                    {
                        StartGame();
                    }
                }
            }
            else if (Keyboard.IsKeyPressed(Keyboard.Key.Back))
            {
                if (_menuState == eMenuState.MS_START)
                {
                    // Nothingness
                }
                else if (_menuState == eMenuState.MS_WORLD)
                {
                    //Nothingness squared
                }
                else if (_menuState == eMenuState.MS_TRIBE)
                {
                    _menuState = eMenuState.MS_WORLD;
                    _timeTilNextInput = 0.5f;
                }
            }

             
            if (_menuState == eMenuState.MS_WORLD)
            {
                GetInputWorldCreation();
            }
            else if (_menuState == eMenuState.MS_TRIBE)
            {
                GetInputTribeCreation();
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.C))
            {
                ChangeGameState(State.Credits);
            }

        }

        private void GetInputTribeCreation()
        {
         
            //SmartText.DrawText("Pref. Terrain " + _tribeProperties.PreferredTerrain + " [R, F]", TextAlignment.LEFT, new Vector2f(200.0f, 350.0f), 0.65f, rw);
            //SmartText.DrawText("Pref Altitude " + _tribeProperties.PreferredAltitude + " [T, G]", TextAlignment.LEFT, new Vector2f(200.0f, 375.0f), 0.65f, rw);
            //SmartText.DrawText("Pref Temperature " + _tribeProperties.PreferredTemperature + " [Z, H]", TextAlignment.LEFT, new Vector2f(200.0f, 400.0f), 0.65f, rw);

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                _tribeProperties.Agility -= 1.0f;
                if (_tribeProperties.Agility <= 1.0f)
                {
                    _tribeProperties.Agility = 1.0f;
                }

                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                _tribeProperties.Agility += 1.0f;
                if (_tribeProperties.Agility >= 10.0f)
                {
                    _tribeProperties.Agility = 10.0f;
                }
                _timeTilNextInput = 0.25f;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                _tribeProperties.Stamina -= 1.0f;
                if (_tribeProperties.Stamina <= 1.0f)
                {
                    _tribeProperties.Stamina = 1.0f;
                }
                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                _tribeProperties.Stamina += 1.0f;
                if (_tribeProperties.Stamina >= 10.0f)
                {
                    _tribeProperties.Stamina = 10.0f;
                }
                _timeTilNextInput = 0.25f;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                _tribeProperties.Strength -= 1.0f;
                if (_tribeProperties.Strength <= 1.0f)
                {
                    _tribeProperties.Strength = 1.0f;
                }

                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.E))
            {
                _tribeProperties.Strength += 1.0f;
                if (_tribeProperties.Strength >= 10.0f)
                {
                    _tribeProperties.Strength = 10.0f;
                }

            
                _timeTilNextInput = 0.25f;
            }

            //if (Keyboard.IsKeyPressed(Keyboard.Key.F))
            //{
            //    _worldProperties.SunLightIntensityFactor -= 0.25f / 100.0f;
            //    if (_worldProperties.SunLightIntensityFactor <= 5.0f / 100.0f)
            //    {
            //        _worldProperties.SunLightIntensityFactor = 5.0f / 100.0f;
            //    }

            //    _timeTilNextInput = 0.25f;
            //}
            //if (Keyboard.IsKeyPressed(Keyboard.Key.R))
            //{
            //    _worldProperties.SunLightIntensityFactor += 0.25f / 100.0f;
            //    if (_worldProperties.SunLightIntensityFactor >= 10.0f / 100.0f)
            //    {
            //        _worldProperties.SunLightIntensityFactor = 10.0f / 100.0f;
            //    }

            //    _timeTilNextInput = 0.25f;
            //}

            if (Keyboard.IsKeyPressed(Keyboard.Key.H))
            {
                _tribeProperties.PreferredTemperature -= 0.5f;
                if (_tribeProperties.PreferredTemperature <= 250.0f )
                {
                    _tribeProperties.PreferredTemperature = 250.0f;
                }

                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Z))
            {
                _tribeProperties.PreferredTemperature += 0.5f;
                if (_tribeProperties.PreferredTemperature >= 350.0f)
                {
                    _tribeProperties.PreferredTemperature = 350.0f;
                }

                _timeTilNextInput = 0.25f;
            }



            //if (Keyboard.IsKeyPressed(Keyboard.Key.G))
            //{
            //    _worldProperties.AtmosphericHeatOutFluxPerSecond -= 0.001f / 1000.0f;
            //    if (_worldProperties.AtmosphericHeatOutFluxPerSecond <= 0.32f / 1000.0f)
            //    {
            //        _worldProperties.AtmosphericHeatOutFluxPerSecond = 0.32f / 1000.0f;
            //    }

            //    _timeTilNextInput = 0.25f;
            //}
            //if (Keyboard.IsKeyPressed(Keyboard.Key.T))
            //{
            //    _worldProperties.AtmosphericHeatOutFluxPerSecond += 0.001f / 1000.0f;
            //    if (_worldProperties.AtmosphericHeatOutFluxPerSecond >= 0.36f / 1000.0f)
            //    {
            //        _worldProperties.AtmosphericHeatOutFluxPerSecond = 0.36f / 1000.0f;
            //    }

            //    _timeTilNextInput = 0.25f;
            //}

            if (Keyboard.IsKeyPressed(Keyboard.Key.U))
            {
                if (_tribeProperties.Diet == WorldInterfaces.AnimalProperties.DietType.CARNIVORE)
                {
                    _tribeProperties.Diet = WorldInterfaces.AnimalProperties.DietType.OMNIVORE;
                }
                else if (_tribeProperties.Diet == WorldInterfaces.AnimalProperties.DietType.OMNIVORE)
                {
                    _tribeProperties.Diet = WorldInterfaces.AnimalProperties.DietType.HERBIVORE;
                }
                else if (_tribeProperties.Diet == WorldInterfaces.AnimalProperties.DietType.HERBIVORE)
                {
                    _tribeProperties.Diet = WorldInterfaces.AnimalProperties.DietType.CARNIVORE;
                }

                _timeTilNextInput = 0.25f;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.K))
            {
                _tribeProperties.GroupBehaviour -= 1.0f;
                if (_tribeProperties.GroupBehaviour <= 1.0f)
                {
                    _tribeProperties.GroupBehaviour = 1.0f;
                }

                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.I))
            {
                _tribeProperties.GroupBehaviour += 1.0f;
                if (_tribeProperties.GroupBehaviour >= 10.0f)
                {
                    _tribeProperties.GroupBehaviour = 10.0f;
                }

                _timeTilNextInput = 0.25f;
            }



        }





        private void GetInputWorldCreation()
        {

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                _worldProperties.DayNightCycleFrequency -= 0.01f;
                if (_worldProperties.DayNightCycleFrequency <= 0.0f)
                {
                    _worldProperties.DayNightCycleFrequency = 0.0f;
                }

                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Q))
            {
                _worldProperties.DayNightCycleFrequency += 0.01f;
                if (_worldProperties.DayNightCycleFrequency >= 2.0f)
                {
                    _worldProperties.DayNightCycleFrequency = 2.0f;
                }
                _timeTilNextInput = 0.25f;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                _worldProperties.MaxHeightInMeter -= 1.0f;
                if (_worldProperties.MaxHeightInMeter<= 50.0f)
                {
                    _worldProperties.MaxHeightInMeter = 50.0f;
                }
                if (_worldProperties.MountainHeight >= _worldProperties.MaxHeightInMeter)
                {
                    _worldProperties.MountainHeight = _worldProperties.MaxHeightInMeter;
                }
                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                _worldProperties.MaxHeightInMeter += 1.0f;
                if (_worldProperties.MaxHeightInMeter>= 200.0f)
                {
                    _worldProperties.MaxHeightInMeter = 200.0f;
                }
                _timeTilNextInput = 0.25f;
            }


            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                _worldProperties.MountainHeight -= 1.0f;
                if (_worldProperties.MountainHeight <= 50.0f)
                {
                    _worldProperties.MountainHeight = 50.0f;
                }
            
                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.E))
            {
                _worldProperties.MountainHeight += 1.0f;
                if (_worldProperties.MountainHeight >= 200.0f)
                {
                    _worldProperties.MountainHeight = 200.0f;
                }

                if (_worldProperties.MaxHeightInMeter <= _worldProperties.MountainHeight)
                {
                    _worldProperties.MaxHeightInMeter = _worldProperties.MountainHeight;
                }

                _timeTilNextInput = 0.25f;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.F))
            {
                _worldProperties.SunLightIntensityFactor -= 0.25f / 100.0f;
                if (_worldProperties.SunLightIntensityFactor <= 5.0f / 100.0f)
                {
                    _worldProperties.SunLightIntensityFactor = 5.0f / 100.0f;
                }
            
                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.R))
            {
                _worldProperties.SunLightIntensityFactor += 0.25f / 100.0f;
                if (_worldProperties.SunLightIntensityFactor >= 10.0f / 100.0f)
                {
                    _worldProperties.SunLightIntensityFactor = 10.0f / 100.0f;
                }

                _timeTilNextInput = 0.25f;
            }

            if (Keyboard.IsKeyPressed(Keyboard.Key.G))
            {
                _worldProperties.AtmosphericHeatOutFluxPerSecond -= 0.001f / 1000.0f;
                if (_worldProperties.AtmosphericHeatOutFluxPerSecond <= 0.32f / 1000.0f)
                {
                    _worldProperties.AtmosphericHeatOutFluxPerSecond = 0.32f / 1000.0f;
                }

                _timeTilNextInput = 0.25f;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.T))
            {
                _worldProperties.AtmosphericHeatOutFluxPerSecond += 0.001f / 1000.0f;
                if (_worldProperties.AtmosphericHeatOutFluxPerSecond >= 0.36f / 1000.0f)
                {
                    _worldProperties.AtmosphericHeatOutFluxPerSecond = 0.36f / 1000.0f;
                }

                _timeTilNextInput = 0.25f;
            }


        }

        private void GetInputCreditsScore()
        {
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) || Keyboard.IsKeyPressed(Keyboard.Key.Return) || Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                ChangeGameState(State.Menu, 1.0f);
            }
        }

        public void Update(float deltaT)
        {
            if (_timeTilNextInput >= 0.0f)
            {
                _timeTilNextInput -= deltaT;
            }


            if (_worldProperties != null && _tribeProperties != null)
            {
                EvolutionPoints = GameProperties.EvolutionPointsStart - _worldProperties.GetPropertyCosts() - _tribeProperties.GetPropertyCosts();
            }
            else
            {
                EvolutionPoints = GameProperties.EvolutionPointsStart;
            }

            CanBeQuit = false;
            if (_gameState == State.Game)
            {
                _myWorld.Update(Timing.Update(deltaT));

               // Game End Condition

            }
            else if (_gameState == State.Menu && this._timeTilNextInput <= 0.0f)
            {
                CanBeQuit = true;
            }

        }


        public void Draw(RenderWindow rw)
        {
            rw.Clear();
            if (_gameState == State.Menu)
            {
                DrawMenu(rw);
            }
            else if (_gameState == State.Game)
            {
                _myWorld.Draw(rw);
            }
            else if (_gameState == State.Credits)
            {
                DrawCredits(rw);
            }
            else if (_gameState == State.Score)
            {
                _gameStats.Draw(rw);
            }
        }

        private void DrawMenu(RenderWindow rw)
        {
            SmartText.DrawText("Intelligent Design", TextAlignment.MID, new Vector2f(400.0f, 150.0f), 1.5f, rw);

            if (_menuState == eMenuState.MS_START)
            {
                SmartText.DrawText("Create World [Return]", TextAlignment.MID, new Vector2f(400.0f, 250.0f), rw);
            }
            else if (_menuState == eMenuState.MS_WORLD)
            {
                SmartText.DrawText("Creating World", TextAlignment.MID, new Vector2f(300.0f, 200.0f), rw);
                if (EvolutionPoints > GameProperties.EvolutionPointsStart - GameProperties.EvolutionPointsWorldMax)
                {
                    SmartText.DrawText("Evo Points " + EvolutionPoints, TextAlignment.MID, new Vector2f(650.0f, 200.0f), rw);
                }
                else
                {
                    SmartText.DrawText("Evo Points " + EvolutionPoints, TextAlignment.MID, new Vector2f(650.0f, 200.0f), Color.Red, rw);
                }

                SmartText.DrawText("Day Night Frequency " + _worldProperties.DayNightCycleFrequency  + " [Q, A]", TextAlignment.LEFT, new Vector2f(200.0f, 250.0f), 0.65f, rw);

                SmartText.DrawText("Max Height " + _worldProperties.MaxHeightInMeter  + " [W, S]", TextAlignment.LEFT, new Vector2f(200.0f, 275.0f), 0.65f, rw);
                SmartText.DrawText("Mountain Height " + _worldProperties.MountainHeight + " [E, D]", TextAlignment.LEFT, new Vector2f(200.0f, 300.0f), 0.65f, rw);

                SmartText.DrawText("Sunlight Intensity " + _worldProperties.SunLightIntensityFactor* 100 + " [R, F]", TextAlignment.LEFT, new Vector2f(200.0f, 425.0f), 0.65f, rw);
                SmartText.DrawText("Atmospheric Heat Flux " + _worldProperties.AtmosphericHeatOutFluxPerSecond * 1000.0f  + " [T, G]", TextAlignment.LEFT, new Vector2f(200.0f, 450.0f), 0.65f, rw);
                    

                SmartText.DrawText("Create Tribe [Return]", TextAlignment.MID, new Vector2f(400.0f, 500.0f), rw);
            }
            else if (_menuState == eMenuState.MS_TRIBE)
            {
                SmartText.DrawText("Selecting Tribe", TextAlignment.MID, new Vector2f(300.0f, 200.0f), rw);
                if (EvolutionPoints >= 0)
                {
                    SmartText.DrawText("Evo Points " + EvolutionPoints, TextAlignment.MID, new Vector2f(650.0f, 200.0f), rw);
                }
                else
                {
                    SmartText.DrawText("Evo Points " + EvolutionPoints, TextAlignment.MID, new Vector2f(650.0f, 200.0f), Color.Red, rw);
                }

                SmartText.DrawText("Agility " + _tribeProperties.Agility + " [Q, A]", TextAlignment.LEFT, new Vector2f(200.0f, 250.0f), 0.65f, rw);
                SmartText.DrawText("Stamina " + _tribeProperties.Stamina + " [W, S]", TextAlignment.LEFT, new Vector2f(200.0f, 275.0f), 0.65f, rw);
                SmartText.DrawText("Strength " + _tribeProperties.Strength + " [E, D]", TextAlignment.LEFT, new Vector2f(200.0f, 300.0f), 0.65f, rw);


                SmartText.DrawText("Pref. Terrain " + _tribeProperties.PreferredTerrain + " [R, F]", TextAlignment.LEFT, new Vector2f(200.0f, 350.0f), 0.65f, rw);
                SmartText.DrawText("Pref Altitude " + _tribeProperties.PreferredAltitude + " [T, G]", TextAlignment.LEFT, new Vector2f(200.0f, 375.0f), 0.65f, rw);
                SmartText.DrawText("Pref Temperature " + _tribeProperties.PreferredTemperature + " [Z, H]", TextAlignment.LEFT, new Vector2f(200.0f, 400.0f), 0.65f, rw);

                SmartText.DrawText("Diet " + _tribeProperties.Diet + " [U, J]", TextAlignment.LEFT, new Vector2f(200.0f, 450.0f), 0.65f, rw);
                SmartText.DrawText("Group Behaviour " + _tribeProperties.GroupBehaviour + " [I, K]", TextAlignment.LEFT, new Vector2f(200.0f, 475.0f), 0.65f, rw);
                


                SmartText.DrawText("Start Game [Return]", TextAlignment.MID, new Vector2f(400.0f, 500.0f), rw);
            }


            SmartText.DrawText("[C]redits", TextAlignment.LEFT, new Vector2f(30.0f, 550.0f), rw);
            ScreenEffects.GetStaticEffect("vignette").Draw(rw);
        }

        private void DrawCredits(RenderWindow rw)
        {

            SmartText.DrawText("$GameTitle$", TextAlignment.MID, new Vector2f(400.0f, 20.0f), 1.5f, rw);

            SmartText.DrawText("A Game by", TextAlignment.MID, new Vector2f(400.0f, 100.0f), 0.75f, rw);
            SmartText.DrawText("$DeveloperNames$", TextAlignment.MID, new Vector2f(400.0f, 135.0f), rw);

            SmartText.DrawText("Visual Studio 2012 \t C#", TextAlignment.MID, new Vector2f(400, 170), 0.75f, rw);
            SmartText.DrawText("aseprite \t SFML.NET 2.1", TextAlignment.MID, new Vector2f(400, 200), 0.75f, rw);
            SmartText.DrawText("Cubase 5 \t SFXR", TextAlignment.MID, new Vector2f(400, 230), 0.75f, rw);

            SmartText.DrawText("Thanks to", TextAlignment.MID, new Vector2f(400, 350), 0.75f, rw);
            SmartText.DrawText("Families & Friends for their great support", TextAlignment.MID, new Vector2f(400, 375), 0.75f, rw);

            SmartText.DrawText("Created $Date$", TextAlignment.MID, new Vector2f(400.0f, 500.0f), 0.75f, rw);
            ScreenEffects.GetStaticEffect("vignette").Draw(rw);
        }

        private void ChangeGameState(State newState, float inputdeadTime = 0.5f)
        {
            this._gameState = newState;
            _timeTilNextInput = inputdeadTime;
        }


        private void StartGame()
        {
            _myWorld = new World();
            _myWorld.GameWorldCreationProperties = _worldProperties;
            
            _myWorld.InitWorld();
            _myWorld.SpawnTribe(_tribeProperties);
            _myWorld.CreateRandomTribes(GameProperties.NumberOfEnemyTribes);
            ChangeGameState(State.Game, 0.1f);
        }


        #endregion Methods

        #region Subclasses/Enums

        private enum State
        {
            Menu,
            Game,
            Score,
            Credits
        }

        #endregion Subclasses/Enums


        public bool CanBeQuit { get; set; }
    }
}
