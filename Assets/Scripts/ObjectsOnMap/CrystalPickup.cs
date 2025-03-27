using UnityEngine;

public class CrystalPickup : MonoBehaviour
{
    public int crystalValue = 10; // Количество кристаллов, которые добавятся игроку при подборе

    private Transform playerTransform; // Ссылка на трансформ игрока

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerTransform = other.transform;
            GetComponent<Collider2D>().enabled = false; // Отключаем коллайдер, чтобы игрок не мог снова подобрать
            Collect();
        }
    }

    public void Collect()
    {
        // Действия при сборе кристалла
        PlayerCrystalController playerController = playerTransform.GetComponentInParent<PlayerCrystalController>();
        if (playerController != null)
        {
            playerController.AddCrystals(crystalValue);
            Destroy(transform.root.gameObject); // Уничтожаем кристалл после подбора
        }
    }
}
