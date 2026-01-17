using UnityEngine;
using UnityEngine.UIElements;

public class NPC : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform npc;
    [SerializeField] protected GameObject interactionMarker;

    [SerializeField] private float floatSpeed = 2;
    [SerializeField] private float floatRange = 0.2f;
    private Vector3 startPosition;
    private bool facingRight = true;

    protected Player player;
    protected UI ui;

    protected void Awake()
    {
        ui = FindAnyObjectByType<UI>();
        startPosition = interactionMarker.transform.position;
        interactionMarker.SetActive(false);
    }

    protected void Update()
    {
        // 悬浮效果
        FloatEffect();
        HandleNPCFlipped();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player = collision.GetComponent<Player>();
        interactionMarker.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactionMarker.SetActive(false);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        npc.Rotate(0, 180, 0);
    }

    private void HandleNPCFlipped()
    {
        if (player == null) return;

        if (player.transform.position.x > transform.position.x && !facingRight
    || player.transform.position.x < transform.position.x && facingRight)
            Flip();
    }

    private void FloatEffect()
    {
        float yOffset = Mathf.Sin(floatSpeed * Time.time) * floatRange;
        interactionMarker.transform.position = startPosition + new Vector3(0, yOffset, 0);
    }

    public virtual void Interact()
    {
        if (interactionMarker.activeSelf)
        {
            Debug.Log($"你在和{gameObject.name}进行交互");
            ui.dialogueUI.gameObject.SetActive(true);
        }
    }
}
