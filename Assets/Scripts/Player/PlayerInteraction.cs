using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 1.5f;
    [SerializeField] private GameObject panelCloud;
    [SerializeField] private GameObject panelTalk;

    private NPC currentNpc;
    public AnimatorStateInfo animatorStateInfo;
    private bool isTalkingWithNpc = false; 

    private GameObject npcInTrigger;
    private bool isInteractPressed = false;

    private void OnEnable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteract += HandleInteractInput;
        }
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.OnInteract -= HandleInteractInput;
        }
    }

    private void Update()
    {
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        PlayerAnimator playerAnim = GetComponent<PlayerAnimator>();

        Ray ray = new Ray(playerMovement.target.transform.position, playerMovement.target.transform.forward);
        RaycastHit hit;

        bool hasHit = Physics.Raycast(ray, out hit, raycastDistance);
        Debug.DrawRay(ray.origin, ray.direction * raycastDistance, hasHit ? Color.red : Color.green);

        if (hasHit)
        {
            npcInTrigger = hit.collider.gameObject;
            currentNpc = hit.collider.GetComponent<NPC>();

            if (isInteractPressed && currentNpc != null)
            {
                isInteractPressed = false;

                Animator npcAnim = currentNpc.animator;
                if (npcAnim != null && panelCloud != null && panelTalk != null)
                {
                    if (!isTalkingWithNpc)
                    {
                        isTalkingWithNpc = true;
                        ChangePanel(true);

                        animatorStateInfo = playerAnim.anim.GetCurrentAnimatorStateInfo(0);

                        if (animatorStateInfo.IsName("IdleFront"))
                        {
                            currentNpc.ResetAllIdleBools();
                            currentNpc.animator.SetBool("Idle_Back", true);
                            currentNpc.parentAnimator.SetBool("IsTalk", true);
                        }
                        else if (animatorStateInfo.IsName("IdleLeft"))
                        {
                            currentNpc.ResetAllIdleBools();
                            currentNpc.animator.SetBool("Idle_Right", true);
                            currentNpc.parentAnimator.SetBool("IsTalk", true);
                        }
                        else if (animatorStateInfo.IsName("IdleBack"))
                        {
                            currentNpc.ResetAllIdleBools();
                            currentNpc.animator.SetBool("Idle_Front", true);
                            currentNpc.parentAnimator.SetBool("IsTalk", true);
                        }
                    }
                    else
                    {
                        isTalkingWithNpc = false;
                        currentNpc.ResetParentAnim();
                        ChangePanel(false);
                    }
                }
                return;
            }
        }
        else
        {
            if (isTalkingWithNpc)
            {
                isTalkingWithNpc = false;
                if (currentNpc != null) currentNpc.ResetParentAnim();
                ChangePanel(false);
            }
        }

        if (isInteractPressed)
        {
            isInteractPressed = false;
        }
    }

    private void HandleInteractInput()
    {
        isInteractPressed = true;
    }

    private void ChangePanel(bool active)
    {
        panelCloud.SetActive(!active);
        panelTalk.SetActive(active);
    }
}