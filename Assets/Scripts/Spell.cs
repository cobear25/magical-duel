using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public int playerNumber = 0;

    void OnBecameInvisible() {
        Destroy(gameObject);
    }
}
