using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace LevelGenerator
{
    public class LevelGenerator : MonoBehaviour
    {
        [SerializeField] private LevelBlock[] blocks;
        [SerializeField] private AdditionalAsset[] additionalAssets;

        [SerializeField] private int width = 10;
        [SerializeField] private int lenght = 10;
        [SerializeField] private int height = 3;

        // 3 dimensional array containing chosen blocks for level
        private LevelBlock[,,] _level;
        
        public void GenerateLevel()
        {
            for (int i = transform.childCount - 1; i >= 0 ; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
            
            _level = new LevelBlock[width, lenght, height];

            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < lenght; z++) // following unity, z is depth of the level
                {
                    for (var y = 0; y < height; y++) // and y is height
                    {
                        // get constraints at the current point
                        var match = ComputeMatchBlockAtCoord(x, z, y);

                        // get all block supporting the constraints
                        var matchedBlocks = GetMatchingBlocks(match);
                        // if none exist, skip
                        if (matchedBlocks.Count == 0)
                        {
                            Debug.LogWarning("No matching block found");
                            continue;
                        }
                        
                        // choose one randomly
                        var chosen = matchedBlocks[Random.Range(0, matchedBlocks.Count)];
                        _level[x, z, y] = chosen;

                        Instantiate(
                            chosen.blockPrefab,
                            new Vector3(x * 12 - width * 6 ,  y * 5 - height * 2.5f,  z * 12 - lenght * 6),
                            Quaternion.Euler(0, -90 * chosen.currentRotation, 0), 
                            transform);
                    }
                }
            }
            
            // put random assets in the level
            foreach (var additionalAsset in additionalAssets)
            {
                for (var i = 0; i < additionalAsset.amount; i++)
                {
                    Instantiate(
                        additionalAsset.prefab, 
                        new Vector3(
                                Random.Range(-width * 6, width * 6),
                                Random.Range(-height * 2.5f, height * 2.5f),
                                Random.Range(-lenght * 6, lenght * 6)
                            ),
                        Quaternion.Euler(0, Random.Range(0, 360), 0),
                        transform
                    );
                }
            }

            // regenerate navmesh
            UnityEditor.AI.NavMeshBuilder.BuildNavMesh();
        }

        // yeah... this is quite dirty code, but I did not find any other way, we need to manually check
        // all corresponding side to their opposite
        private MatchBlock ComputeMatchBlockAtCoord(int x, int z, int y)
        {
            
            var match = new MatchBlock();
            
            if (y + 1 >= height) // if upper is outside of bounds, we need to close
            {
                for (var i = 0; i < 4; i++) match.topFaces[i] = NeededFace.Any;
            }
            else if (_level[x, z, y + 1] == null) // if no block is there yet, we might put anything
            {
                for (var i = 0; i < 4; i++) match.topFaces[i] = NeededFace.Any;
            }
            else // if there is a block we get what it needs
            {
                var upperBlock = _level[x, z, y + 1];
                for (var i = 0; i < 4; i++)
                {
                    match.topFaces[i] = upperBlock.botFaces[i].nextNeededFace;
                }
            }
            
            // same but opposite (lower block to bottom)
            if (y - 1 < 0)
            {
                for (var i = 0; i < match.botFaces.Length; i++) match.botFaces[i] = NeededFace.Close;
            }
            else if(_level[x, z, y - 1] == null)
            {
                for (var i = 0; i < match.botFaces.Length; i++) match.botFaces[i] = NeededFace.Any;
            }
            else
            {
                var lowerBlock = _level[x, z, y - 1];
                for (var i = 0; i < 4; i++)
                {
                    match.botFaces[i] = lowerBlock.topFaces[i].nextNeededFace;
                }
            }
            
            // same but on each side :
            // right
            if (x + 1 >= width)
            {
                match.sideFaces[6] = NeededFace.Close;
                match.sideFaces[7] = NeededFace.Close;
            }
            else if(_level[x + 1, z, y] == null)
            {
                match.sideFaces[6] = NeededFace.Any;
                match.sideFaces[7] = NeededFace.Any;
            }
            else
            {
                var sideBlock = _level[x + 1, z, y];
                match.sideFaces[6] = sideBlock.sideFaces[3].nextNeededFace;
                match.sideFaces[7] = sideBlock.sideFaces[2].nextNeededFace;
            }
            
            // left
            if (x - 1 < 0)
            {
                match.sideFaces[2] = NeededFace.Close;
                match.sideFaces[3] = NeededFace.Close;
            }
            else if(_level[x - 1, z, y] == null)
            {
                match.sideFaces[2] = NeededFace.Any;
                match.sideFaces[3] = NeededFace.Any;
            }
            else
            {
                var sideBlock = _level[x - 1, z, y];
                match.sideFaces[2] = sideBlock.sideFaces[7].nextNeededFace;
                match.sideFaces[3] = sideBlock.sideFaces[6].nextNeededFace;
            }
            
            // front
            if (z + 1 >= lenght)
            {
                match.sideFaces[0] = NeededFace.Close;
                match.sideFaces[1] = NeededFace.Close;
            }
            else if(_level[x, z + 1, y] == null)
            {
                match.sideFaces[0] = NeededFace.Any;
                match.sideFaces[1] = NeededFace.Any;
            }
            else
            {
                var sideBlock = _level[x, z + 1, y];
                match.sideFaces[0] = sideBlock.sideFaces[5].nextNeededFace;
                match.sideFaces[1] = sideBlock.sideFaces[4].nextNeededFace;
            }
            
            // back
            if (z - 1 < 0)
            {
                match.sideFaces[4] = NeededFace.Close;
                match.sideFaces[5] = NeededFace.Close;
            }
            else if(_level[x, z - 1, y] == null)
            {
                match.sideFaces[4] = NeededFace.Any;
                match.sideFaces[5] = NeededFace.Any;
            }
            else
            {
                var sideBlock = _level[x, z - 1, y];
                match.sideFaces[4] = sideBlock.sideFaces[1].nextNeededFace;
                match.sideFaces[5] = sideBlock.sideFaces[0].nextNeededFace;
            }

            return match;
        }

        private List<LevelBlock> GetMatchingBlocks(MatchBlock matchBlock)
        {
            var result = new List<LevelBlock>();

            foreach (var block in blocks)
            {
                var currentBlock = block;
                
                do
                {
                    if (matchBlock.Match(currentBlock))
                    {
                        result.Add(currentBlock);
                    }

                    currentBlock = currentBlock.GetNextRotation();

                } while (currentBlock.currentRotation != 0); // we are back at starting rotation
                
            }
            
            return result;
        }
        
    }
}