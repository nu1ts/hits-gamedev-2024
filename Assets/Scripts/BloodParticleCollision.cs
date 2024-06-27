using System.Collections.Generic;
using UnityEngine;

public class BloodParticleCollision : MonoBehaviour
{
    ParticleSystem particle;
    public GameObject splatPrefab;
    public Transform splatHolder;
    public List<Sprite> splatSprites; // Список спрайтов для выбора
    private List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = particle.GetCollisionEvents(other, collisionEvents);

        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 collisionPos = collisionEvents[i].intersection;

            // Случайный поворот по оси Z
            float randomZRotation = Random.Range(0f, 360f);
            Quaternion rotation = Quaternion.Euler(0f, 0f, randomZRotation);

            GameObject splatInstance = Instantiate(splatPrefab, collisionPos, rotation, splatHolder);

            // Выбор случайного спрайта
            Sprite randomSprite = splatSprites[Random.Range(0, splatSprites.Count)];
            splatInstance.GetComponent<SpriteRenderer>().sprite = randomSprite;
        }
    }
}
