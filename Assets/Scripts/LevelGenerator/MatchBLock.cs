namespace LevelGenerator
{
    /*
     * A MatchBlock contains all constraints for selecting a block
     */
    public class MatchBlock
    {
        public NeededFace[] topFaces = new NeededFace[4];
        public NeededFace[] botFaces = new NeededFace[4];
        public NeededFace[] sideFaces = new NeededFace[8];

        public bool Match(LevelBlock block)
        {
            for (uint i = 0; i < 4; i++)
            {
                // if any of the face does not match, the whole block does not match
                if (!(
                    MatchFace(block.botFaces[i].currentFace, botFaces[i])
                    && MatchFace(block.topFaces[i].currentFace, topFaces[i])
                    && MatchFace(block.sideFaces[i].currentFace, sideFaces[i])
                    && MatchFace(block.sideFaces[i + 4].currentFace, sideFaces[i + 4])
                )) return false;
            }
            
            return true;
        }

        private static bool MatchFace(Face current, NeededFace needed)
        {
            return needed == NeededFace.Any // whatever, it's good to go
                   || (current == Face.Opened && needed == NeededFace.Open) // Open in both case
                   || (current == Face.Closed && needed == NeededFace.Close); // Closed in both cases
        }
    }
}