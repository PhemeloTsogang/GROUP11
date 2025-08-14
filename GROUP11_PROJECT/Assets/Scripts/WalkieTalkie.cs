using UnityEngine;
using UnityEngine.InputSystem;

public class WalkieTalkie : MonoBehaviour
{
    public bool inStunRange;
    public EnemyAI monster;
    public float batteryCount;
    public Hide lockerPlayer, ventPlayer;

    private void Awake()
    {
        inStunRange = false;
        batteryCount = 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") && !lockerPlayer.isHiding && !ventPlayer.isHiding) //check this
        {
            inStunRange = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster") && !lockerPlayer.isHiding && !ventPlayer.isHiding)
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
