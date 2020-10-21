using System.Collections;
using System.Linq;
using UnityEngine;

namespace Utility
{
    public class ScaleWithSound : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private float updateFrequency = 6;
        [SerializeField] private float minScale = 0.8f;
        [SerializeField] private float growFactor = 2f;

        private Transform _transform;

        private float _updatePeriod;
        private float[] _clipSampleData;

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _updatePeriod = 1 / updateFrequency;
            _clipSampleData = new float[1024];

            StartCoroutine(UpdateScale());
        }

        private IEnumerator UpdateScale()
        {
            for (;;)
            {
                source.clip.GetData(_clipSampleData, source.timeSamples);

                _transform.localScale = Vector3.one * (minScale + _clipSampleData.Average() * growFactor);
            
                yield return new WaitForSeconds(_updatePeriod);
            }
        }
    }
}