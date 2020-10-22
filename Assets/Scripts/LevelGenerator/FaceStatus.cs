using System;

namespace LevelGenerator
{
    [Serializable]
    public struct FaceStatus
    {
        public NeededFace nextNeededFace;
        public Face currentFace;
    }

    public enum NeededFace
    {
        Any, // Does not matter what is in front of this face
        Open, // This face leads to somewhere and need to have an open face in front of it
        Close, // This face needs to be closed (end of level for example)
    }

    public enum Face
    {
        Opened, // this face is opened
        Closed, // this face is closed
    }
}