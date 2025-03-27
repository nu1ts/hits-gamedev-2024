using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (SceneManager.GetActiveScene().buildIndex == 3)
            {
                LevelController.instance.GoToMenu();
            }
            else if (EnemyManager.instance.AreAllEnemiesDead())
            {
                LevelController.instance.NextLevel();
            }
            else
            {
                LevelController.instance.NeedToKillAll();
            }
        }
    }
}
