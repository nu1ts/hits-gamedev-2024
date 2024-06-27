using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(EnemyManager.instance.AreAllEnemiesDead())
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
