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

            float heightMapNoiseFrequency = properties.HeightMapNoiseLength;
            float heightMapMaxHeightInMeter = properties.MaxHeightInMeter;

            float[,] heightMap1 = new float[sizeX, sizeY];
            float[,] heightMap2 = new float[sizeX, sizeY];
            float[,] temperattureMap = new float[sizeX, sizeY];

            float height1HighestValue = -1;
            float height1LowestValue = 1;

            float height2HighestValue = -1;
            float height2LowestValue = 1;

            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    heightMap1[i, j] = ( Noise.Generate(i / heightMapNoiseFrequency, j / heightMapNoiseFrequency));
                    if (heightMap1[i, j] >= height1HighestValue)
                    {
                        height1HighestValue = heightMap1[i, j];
                    }
                    if (heightMap1[i, j] < height1LowestValue)
                    {
                        height1LowestValue = heightMap1[i, j];
                    }

                    heightMap2[i, j] = (Noise.Generate(i / (heightMapNoiseFrequency*10.0f), j / (heightMapNoiseFrequency*10.0f)));
                    if (heightMap2[i, j] >= height2HighestValue)
                    {
                        height2HighestValue = heightMap2[i, j];
                    }
                    if (heightMap2[i, j] < height2LowestValue)
                    {
                        height2LowestValue = heightMap2[i, j];
                    }
                    
                }
            }
            for (int i = 0; i < sizeX; i++)
            {
                for (int j = 0; j < sizeY; j++)
                {
                    heightMap2[i, j] = (heightMap2[i, j] - height2LowestValue) / (height2HighestValue - height2LowestValue) * 0.5f + 0.5f;
                    heightMap1[i, j] = (heightMap1[i, j] - height1LowestValue) / (height1HighestValue - height1LowestValue)* heightMapMaxHeightInMeter * heightMap2[i, j];
                   //Console.WriteLine(heightMap1[i, j]);

                    cTileProperties tileprops = new cTileProperties();
                    tileprops.HeightInMeters = heightMap1[i, j];

                    // TODO Do Something about the ugly World Cast
                    cTile tile = new cTile(new Vector2i(i, j), tileprops, world as cWorld);
                    tile.GetTileProperties().TemperatureInKelvin = properties.DesiredTemperature +(float)(RandomGenerator.Random.NextDouble() * 25.0 - 12.0);
                    world.AddTille(tile);
                }
            }

            // ToDo: Overlap on the edges Blend it here.

            System.Console.WriteLine("Building Tile Neighbour Lists");
            world.BuildTileNeighbourLists();
            world.CreateClouds();

        }

        
    }
}
