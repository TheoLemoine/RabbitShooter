using System;
using TMPro;
using UnityEngine;

namespace Radio
{
    [RequireComponent(typeof(TextMeshPro))]
    public class ClipNameToText : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        private TextMeshPro _textMesh;

        private void Start()
        {
            _textMesh = GetComponent<TextMeshPro>();
            _textMesh.text = source.clip.name;
        }
    }
}