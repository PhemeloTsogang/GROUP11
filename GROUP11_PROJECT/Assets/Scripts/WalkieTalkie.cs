using UnityEngine;
using UnityEngine.InputSystem;

public class WalkieTalkie : MonoBehaviour
{
    public bool inStunRange;
    public EnemyAI monster;
    public float batteryCount;

    private void Awake()
    {
        inStunRange = false;
        batteryCount = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            inStunRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            inStunRange = false;
        }
    }

    public void onStun(InputAction.CallbackContext context)
    {
        if (inStunRange && context.performed && batteryCount >= 1)
        {
            Debug.Log("STUNNED!");
            //code to stun monster
        }
    }
}
