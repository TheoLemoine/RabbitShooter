using UnityEngine;

namespace LevelGenerator
{
    [CreateAssetMenu(fileName = "new_block", menuName = "Level Generator/Level Block", order = 0)]
    public class LevelBlock : ScriptableObject
    {
        public GameObject blockPrefab;
        
        // rotation to apply to the prefab on instanciation
        [HideInInspector] public uint currentRotation = 0;
        private const uint NbPossibleRotation = 4;
        
        // faces count goes counter-clock wise starting on left facing forward
        //
        // all array values must not be changed since they are by conception
        // linked to the way prefabs are built but we cant enforce that in
        // unity inspector or C#
        public FaceStatus[] topFaces = new FaceStatus[4];
        public FaceStatus[] botFaces = new FaceStatus[4];
        public FaceStatus[] sideFaces = new FaceStatus[8];

        public LevelBlock GetNextRotation()
        {
            var nextRotatedBlock = CreateInstance<LevelBlock>();
            nextRotatedBlock.blockPrefab = blockPrefab;
            nextRotatedBlock.currentRotation = (currentRotation + 1) % NbPossibleRotation;

            for (int i = 0; i < 4; i++)
            {
                nextRotatedBlock.botFaces[(i + 1) % 4] = botFaces[i];
                nextRotatedBlock.topFaces[(i + 1) % 4] = topFaces[i];
                
                // rotate 2 values at the time
                nextRotatedBlock.sideFaces[(i + 2) % 8] = sideFaces[i];
                nextRotatedBlock.sideFaces[(i + 4 + 2) % 8] = sideFaces[(i + 4) % 8]; 
            }

            return nextRotatedBlock;
        }
    }
}