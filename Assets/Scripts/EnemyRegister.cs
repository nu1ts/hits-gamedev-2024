using UnityEngine;

public class EnemyRegister : MonoBehaviour
{
    void Start()
    {
        Debug.Log("REGISTER");
        EnemyManager.instance.RegisterEnemy(gameObject);
    }

    // void OnDestroy()
    // {
    //     if (EnemyManager.instance != null)
    //     {
    //         EnemyManager.instance.UnregisterEnemy(gameObject);
    //     }
    // }
}
