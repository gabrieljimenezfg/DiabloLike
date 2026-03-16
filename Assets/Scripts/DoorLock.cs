using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class DoorLock : MonoBehaviour
{
    [SerializeField]
    private int keyNum;
    [SerializeField]
    private List<GameObject> DoorListA = new List<GameObject>();
    [SerializeField]
    private List<GameObject> DoorListB = new List<GameObject>();
    [SerializeField]
    private List<GameObject> DoorListC = new List<GameObject>();
    [SerializeField]
    private List<GameObject> DoorListD = new List<GameObject>();
    [SerializeField] private bool isBossBattleDoor;
    [SerializeField] private bool isFinalLevel1Door;
    [SerializeField] private bool isFinalLevel2Door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(Player.Instance.keyList.Count == 0)
            {
                Player.Instance.ShowMessage("I need a key to open this...");
            }
            else
            {
                for (int i = 0; i < other.gameObject.GetComponent<Player>().keyList.Count; i++)
                {
                    if (keyNum == other.gameObject.GetComponent<Player>().keyList[i])
                    {
                        if (isBossBattleDoor)
                        {
                            if (Player.Instance.HasStaff)
                            {
                                foreach (GameObject door in DoorListA)
                                {
                                    door.GetComponent<Animator>().SetTrigger("Door1");
                                }
                                foreach (GameObject door in DoorListB)
                                {
                                    door.GetComponent<Animator>().SetTrigger("Door2");
                                }
                                foreach (GameObject door in DoorListC)
                                {
                                    door.GetComponent<Animator>().SetTrigger("Door3");
                                }
                                foreach (GameObject door in DoorListD)
                                {
                                    door.GetComponent<Animator>().SetTrigger("Door4");
                                }
                                Player.Instance.HideMessage();
                                break;
                            }
                            else
                            {
                                Player.Instance.ShowMessage("I need my staff first...");
                            }
                        }
                        else if (isFinalLevel1Door)
                        {
                            Player.Instance.HideMessage();
                            SceneManagerScript.instance.LoadLevel2();
                        }
                        else if (isFinalLevel2Door)
                        {
                            Player.Instance.HideMessage();
                            SceneManagerScript.instance.LoadLevel3();
                        }
                        else
                        {
                            foreach (GameObject door in DoorListA)
                            {
                                door.GetComponent<Animator>().SetTrigger("Door1");
                            }
                            foreach (GameObject door in DoorListB)
                            {
                                door.GetComponent<Animator>().SetTrigger("Door2");
                            }
                            foreach (GameObject door in DoorListC)
                            {
                                door.GetComponent<Animator>().SetTrigger("Door3");
                            }
                            foreach (GameObject door in DoorListD)
                            {
                                door.GetComponent<Animator>().SetTrigger("Door4");
                            }
                            Player.Instance.HideMessage();
                            break;
                        }
                    }
                    else
                    {
                        Player.Instance.ShowMessage("I don't have the key for this...");
                    }
                }
            }
        }
    }
}
