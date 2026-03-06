using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class DoorLock : MonoBehaviour
{
    [SerializeField]
    private int keyNum;
    [SerializeField]
    private List<GameObject> DoorListA = new List<GameObject>();
    [SerializeField]
    private List<GameObject> DoorListB = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            for (int i = 0; i < other.gameObject.GetComponent<Player>().keyList.Count; i++)
            {
                if (keyNum == other.gameObject.GetComponent<Player>().keyList[i])
                {
                    foreach (GameObject door in DoorListA)
                    {
                        door.GetComponent<Animator>().SetTrigger("Door1");
                    }
                    foreach (GameObject door in DoorListB)
                    {
                        door.GetComponent<Animator>().SetTrigger("Door2");
                    }
                }
            }
        }
    }
}
