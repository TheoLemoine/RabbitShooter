using UnityEngine;
using Random = UnityEngine.Random;

namespace Radio
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioSource : MonoBehaviour
    {
        [SerializeField] private AudioClip[] clips;

        private AudioSource _source;
        
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            _source.clip = clips[Random.Range(0, clips.Length)];
            _source.Play();
        }
    }
}