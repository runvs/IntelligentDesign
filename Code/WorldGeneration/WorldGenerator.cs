using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamUtilities;
using SFML.Window;
using WorldEvolver;
using WorldInterfaces;

namespace WorldGeneration
{
    public static class WorldGenerator
    {

        public static void CreateWorld(ref IWorldInCreation  world, cWorldProperties properties)
        {
            // do all the fancy shit here

            int sizeX = properties.WorldSizeInTiles.X;
            int sizeY = properties.WorldSizeInTiles.Y;

            world.SetWorldProperties(properties);
            cTileSetter.SetWorldProperties(properties);

            float heightMapNoiseFrequency = properties.HeightMapNoiseFrequency;
            float heightMapMaxHeightInMeter = properties.MaxHeightInMeter;

            float[,] heightMap = new float[sizeX, sizeY];
            float[,] temperattureMap = new float[sizeX, sizeY];

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    heightMap[i, j] = Math.Abs(Noise.Generate(i / heightMapNoiseFrequency, j / heightMapNoiseFrequency) * heightMapMaxHeightInMeter);
                    Console.WriteLine(heightMap[i, j]);
                }
            }

            // ToDo: Overlap on the edges Blend it here.


            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    cTileProperties tileprops = new cTileProperties();
                    tileprops.HeightInMeters = heightMap[i,j];
                    
                    // dummy value
                    tileprops.TemperatureInKelvin = 300.0f;

                    cTile tile = new cTile(new Vector2i(i, j), tileprops);
                    world.AddTille(tile);
                }
            }

        }

        
    }
}
