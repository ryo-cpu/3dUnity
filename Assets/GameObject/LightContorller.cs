using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightContorller : MonoBehaviour
{
    public GameObject Player;
    
  
    // Start is called before the first frame update
    void Start()
    {
    }
    void Deleetion()
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Player.transform.position;
    }
}
