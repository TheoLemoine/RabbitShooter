﻿using System.Collections;
using System.Linq;
using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(Renderer))]
    public class ScaleWithSound : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private float updateFrequency = 6;
        [SerializeField] private float minScale = 0.8f;
        [SerializeField] private float growFactor = 2f;

        private Transform _transform;
        private Renderer _renderer;

        private float _updatePeriod;
        private float[] _clipSampleData;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _renderer = GetComponent<Renderer>();
            
            _updatePeriod = 1 / updateFrequency;
            _clipSampleData = new float[1024];

            StartCoroutine(UpdateScale());
        }

        private IEnumerator UpdateScale()
        {
            for (;;)
            {
                // do not make theses calculation for nothing, averaging 1024 sample is kinda heavy
                if (_renderer.isVisible)
                {
                    source.clip.GetData(_clipSampleData, source.timeSamples);
                    _transform.localScale = Vector3.one * (minScale + _clipSampleData.Average() * growFactor);
                }
            
                yield return new WaitForSeconds(_updatePeriod);
            }
        }
    }
}