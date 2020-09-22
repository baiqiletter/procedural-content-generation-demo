using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRise : MonoBehaviour
{
    private bool isPlayerNear;
    private bool isPlat;
    private Enemy enemyScript;
    private Transform fallCheck;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerNear = false;
        fallCheck = transform.Find("FallCheck");
        enemyScript = GetComponent<Enemy>();
        enemyScript.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!enemyScript.isActiveAndEnabled)
        {
            isPlat = Physics2D.OverlapCircle(fallCheck.position, .2f, 1 << LayerMask.NameToLayer("Default"));

            Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, 10f);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject.tag == "Player")
                {
                    StartCoroutine(Rise());
                }
            }
        }
    }

    IEnumerator Rise()
    {
        isPlayerNear = true;

        float riseTime = Random.Range(0.2f, 0.5f);
        yield return new WaitForSeconds(riseTime);

        transform.GetComponent<Animator>().SetBool("Rise", true);

        float waitTime = Random.Range(1f, 2f);
        yield return new WaitForSeconds(waitTime);

        if (!enemyScript.isActiveAndEnabled)
            enemyScript.enabled = true;
    }
}
