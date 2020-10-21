using UnityEngine;

namespace Player
{
    public interface IShootTarget
    {
        void GetShot(Vector3 shotFrom, Vector3 shotPoint);
    }
}