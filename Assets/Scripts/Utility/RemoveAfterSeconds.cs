using System;
using System.Collections;
using UnityEngine;

namespace Utility
{
    public class RemoveAfterSeconds : MonoBehaviour
    {
        [SerializeField] private float timeBeforeRemove = 0f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(timeBeforeRemove);
            Destroy(gameObject);
        }
    }
}