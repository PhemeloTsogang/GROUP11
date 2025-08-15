using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using static EnemyAI;

public class WalkieTalkie : MonoBehaviour
{
    public float stunRange;
    public bool inStunRange;
    public Transform monster, player;
    public LayerMask ignore;
    public GameObject stunText;

    private void Awake()
    {
        inStunRange = false;
    }

    private void Update()
     {
        Vector3 direction = (player.transform.position - monster.position).normalized;
        Ray ray = new Ray(monster.position, direction);
        RaycastHit hit;

        // If the ray hits something
        if (Physics.Raycast(ray, out hit, stunRange, ignore))
        {
            if(hit.collider.CompareTag("Player"))
            {
                inStunRange = true;
                stunText.SetActive(true);
                
            }
            else
            {
                inStunRange = false;
                stunText.SetActive(false);
            }
        }
        else
        {
            inStunRange = false;
            stunText.SetActive(false);
        }
    }
}
