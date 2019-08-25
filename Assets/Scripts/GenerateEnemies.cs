using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int xPos, zPos;
    [SerializeField] private int enemyCount;

    private void Start()
    {
        StartCoroutine(EnemyDrop());

    }

    private IEnumerator EnemyDrop()
    {
        while (enemyCount < 10)
        {
            xPos = Random.Range(-40, 43);
            zPos = Random.Range(-40, 43);
            Instantiate(enemy, new Vector3 (xPos, 4, zPos), transform.rotation);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }

}
