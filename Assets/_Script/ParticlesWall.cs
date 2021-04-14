using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesWall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("DestroyParticula", 1f);
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void DestroyParticula()
    {
        Destroy(this.gameObject);

    }

}
