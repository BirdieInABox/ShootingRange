using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionVFX : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyMe", 1);
    }

    private void DestroyMe()
    {
        Destroy(this.gameObject);
    }
}
