using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static EnemyAI;

public class WalkieTalkie : MonoBehaviour
{
    public float stunRange;
    public bool inStunRange;
    public Transform monster, player, tutMonster;
    public LayerMask ignore;
    public GameObject stunText, monst, tutMonst;
    public FPController batteryCount;

    private void Awake()
    {
        inStunRange = false;
    }

    private void Update()
     { 
        if (monst.activeInHierarchy)
        {
            Vector3 direction = (player.transform.position - monster.position).normalized;
            Ray ray = new Ray(monster.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, stunRange, ignore))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    batteryCount.stun = this;
                    inStunRange = true;
                    if (batteryCount.batteryCount == 1)
                    {
                        stunText.SetActive(true);
                    }
                }
                else
                {
                    inStunRange = false;
                    stunText.SetActive(false);
                }
            }
            else
            {
               /* if (batteryCount != null && batteryCount.stun == this)
                {
                    batteryCount.stun = null;
                } */

                inStunRange = false;
                stunText.SetActive(false);
            }
        }
        else if (tutMonst.activeInHierarchy)
        {
            Vector3 direction = (player.transform.position - tutMonster.position).normalized;
            Ray ray = new Ray(tutMonster.position, direction);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, stunRange, ignore))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    batteryCount.stun = this;
                    inStunRange = true;
                    if (batteryCount.batteryCount == 1)
                    {
                        stunText.SetActive(true);
                    }
                }
                else
                {
                    inStunRange = false;
                    stunText.SetActive(false);
                }
            }
            else
            {
                /*if (batteryCount != null && batteryCount.stun == this)
                {
                    batteryCount.stun = null;
                }*/
                inStunRange = false;
                stunText.SetActive(false);
            }
        }
    }
}
