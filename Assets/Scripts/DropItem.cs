using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int index = Random.Range(0, UnitManager.Instance.pickup.Length);
        Instantiate(UnitManager.Instance.pickup[index], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
