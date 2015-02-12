using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class PlayerControl : MonoBehaviour
{

    public int PlayerNum = 1;
    public int ControllerNum = 1;
    public int Speed = 5;
  
    public Vector2 aimVector;

    //public ControlSet CustomControls = ControlSet.MouseKeyboardControlSet();
    public ControlSet CustomControls = ControlSet.DefaultControlSet();

    public Transform PointerTransform;
    public SpriteRenderer ButtonHelpSprite;

    public float interactDist = 1.5f;

    public System.Collections.Generic.List<Item> ItemList;

    public Color color;

    //network shit

    private bool receivedOnePacket = false;
    private float lastSyncTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    private Quaternion syncStartRotation;
    private Quaternion syncEndRotation;

	void Start()
	{
        ItemList = new System.Collections.Generic.List<Item>();
        ItemList.Add(new Weapon(this, "Prefabs/Bullets/Bullet"));
	}

    [RPC]
    public void PrintText(NetworkMessageInfo info)
    {
        Debug.Log(info.sender.ipAddress.ToString() + " ran Start for Player Number " + this.PlayerNum.ToString());
    }

    void Update()
    {
        if (networkView.isMine)
        {
            UpdatePlayerMovement();
            UpdatePointer();
            UpdatePlayerActions();
            UpdateInteractables();
        }
        else
        {
            if (receivedOnePacket)
            {
                UpdateNetworkPosition();
            }
        }
        
    }

    void UpdateNetworkPosition()
    {
        syncTime += Time.deltaTime;
        transform.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime/syncDelay);
        transform.rotation = Quaternion.Lerp(syncStartRotation, syncEndRotation, syncTime/syncDelay);
    }

    [RPC]
    public void InitializePlayer(int playerNum, int controllerNum, Vector3 vecColor)
    {
        PlayerNum = playerNum;
        ControllerNum = controllerNum;
        SetColor(new Color(vecColor.x, vecColor.y, vecColor.z, 1));

        CustomControls = ControlSet.MouseKeyboardControlSet();
        
    }

    public void SetColor(Color setColor)
    {
        GameObject playerSkin = this.transform.FindChild("PlayerSkin").gameObject;
        (playerSkin.renderer as SpriteRenderer).color = setColor;
        GetComponent<Damageable>()._startColor = setColor;
    }

	void FixedUpdate()
	{
	    foreach (Item item in ItemList)
	    {
	        item.OnUpdate();
	    }

        
	}

    void UpdatePlayerMovement()
    {
        float moveX = InputHandler.GetPlayerAxis(CustomControls.MOVE_X, ControllerNum) * Speed;
        float moveY = InputHandler.GetPlayerAxis(CustomControls.MOVE_Y, ControllerNum) * Speed;

        Vector2 movementVector = new Vector2(moveX, moveY);

        rigidbody2D.velocity = movementVector;
    }

    void UpdatePlayerActions()
    {
        //Primary weapon
        if (InputHandler.GetPlayerAxis(CustomControls.SHOOT1, ControllerNum) > .2)
        {
            if (ItemList[0] != null)
            {
                ItemList[0].OnUse();
            }
        }

        //Secondary weapon
        if (InputHandler.GetPlayerAxis(CustomControls.SHOOT2, ControllerNum) > .2)
        {
            if (ItemList[1] != null)
            {
                ItemList[1].OnUse();
            }
        }
    }

    void UpdateInteractables()
    {
        GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
        GameObject closeInteractable = null;

        foreach (GameObject interactable in interactables)
        {
            float dist = Vector2.Distance(transform.position, interactable.transform.position);
            if ( dist < interactDist)
            {
                closeInteractable = interactable;
                break;
            }
        }

        if (closeInteractable != null)
        {
            ButtonHelpSprite.enabled = true;
            if (InputHandler.GetPlayerButton(CustomControls.INTERACT, ControllerNum))
            {
                closeInteractable.SendMessage("Activate");
            }


        }
        else
        {
            ButtonHelpSprite.enabled = false;
        }


    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo messageInfo)
    {
        Vector3 syncPosition = Vector3.zero;
        Quaternion aimRotation = new Quaternion(); 
        if (stream.isWriting)
        {
            syncPosition = transform.position;
            stream.Serialize(ref syncPosition);

            aimRotation = PointerTransform.rotation;
            stream.Serialize(ref aimRotation);

        }
        else
        {
            stream.Serialize(ref syncPosition);
            syncTime = 0f;
            syncDelay = Time.time - lastSyncTime;
            lastSyncTime = Time.time;

            syncStartPosition = transform.position;
            syncEndPosition = syncPosition;

            stream.Serialize(ref aimRotation);
            syncStartRotation = transform.rotation;
            syncEndRotation = aimRotation;

            receivedOnePacket = true;
        }
    }

    private void UpdatePointer()
    {
        if (CustomControls.MOUSEAIM)
        {
            //get mouse position in world space

            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //Debug.Log(mousePos);

            float aimX = mousePos.x - transform.position.x;
            float aimY = mousePos.y - transform.position.y;
            aimVector = new Vector2(aimX, aimY).normalized;

            //Don't know why this needs to be -aimVector... probably some right-hand rule thing or something
            Quaternion newRotation = Quaternion.LookRotation(-aimVector, Vector3.forward);
            newRotation.x = 0;
            newRotation.y = 0;

            PointerTransform.rotation = newRotation;

        }
        else
        {
            float aimX = InputHandler.GetPlayerAxis(CustomControls.AIM_X, ControllerNum);
            float aimY = InputHandler.GetPlayerAxis(CustomControls.AIM_Y, ControllerNum);

            //Avoid making the aim go crazy when you release the stick, just keep aiming where you were
            if (Mathf.Abs(aimX) + Mathf.Abs(aimY) > .25)
            {
                aimVector = new Vector2(aimX, aimY).normalized;
                Vector3 target = new Vector3(this.transform.position.x + aimX, this.transform.position.y + aimY, 0);


                Quaternion newRotation = Quaternion.LookRotation(transform.position - target, Vector3.forward);
                newRotation.x = 0;
                newRotation.y = 0;

                PointerTransform.rotation = newRotation;
            }
        }
    }

    


}