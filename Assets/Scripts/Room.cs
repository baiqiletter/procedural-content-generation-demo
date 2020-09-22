using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // entry flag
    public bool entry_up = false;
    public bool entry_dn = false;
    public bool entry_lt = false;
    public bool entry_rt = false;

    // location
    public int x = 0;
    public int y = 0;

    // type flag
    public bool isSpecial = false;
    public bool isEnd = false;

}
