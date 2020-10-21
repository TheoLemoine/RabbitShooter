using System.Collections;
using UnityEngine;
using UnityEngine.Accessibility;
using Random = UnityEngine.Random;

namespace Rabbit
{
    public class RabbitGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject rabbitPrefab;
        [SerializeField] private uint nbRabbit = 5;
        [SerializeField] private Transform spawnCenter;
        [SerializeField] private float spawnRange = 3;
    
        private void Start()
        {
            StartCoroutine(SpawnRabbits());
        }

        private IEnumerator SpawnRabbits()
        {
            // generating colors for each rabbit
            Color[] palette = new Color[nbRabbit];
            VisionUtility.GetColorBlindSafePalette(palette, 0.5f, 1.0f);
        
            for (uint i = 0; i < nbRabbit; i++)
            {
                // instantiating rabbit in circle
                var randomCircle = Random.insideUnitCircle;

                var rabbit = Instantiate(
                    rabbitPrefab, 
                    spawnCenter.TransformPoint(new Vector3(randomCircle.x, 0, randomCircle.y) * spawnRange),
                    Quaternion.Euler(0, Random.Range(0, 360), 0)
                );

                // setting scale of rabbit
                rabbit.transform.localScale = Vector3.one * Random.Range(0.8f, 1.4f);

                // setting color of rabbit
                foreach (var rabbitRenderer in rabbit.GetComponentsInChildren<Renderer>())
                {
                    if(rabbitRenderer.material.color == Color.black
                       || rabbitRenderer.material.color == Color.white) continue;
                
                    rabbitRenderer.material.color = palette[i];
                }
            
                // waiting before next rabbit
                yield return new WaitForSeconds(Random.Range(0.5f, 3f));
            }
        }
    }
}