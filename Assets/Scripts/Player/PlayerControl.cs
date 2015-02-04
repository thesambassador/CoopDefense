using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PlayerControl : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public int PlayerNum = 1;
    public int Speed = 5;
  
    public Vector2 aimVector;

    //public ControlSet CustomControls = ControlSet.MouseKeyboardControlSet();
    public ControlSet CustomControls = ControlSet.DefaultControlSet();

    public Transform PointerTransform;

    public List<Item> ItemList;

    public int curCooldown;

    public Color color;

	void Start()
	{
        ItemList = new List<Item>();
        ItemList.Add(new Weapon(this, "Prefabs/Bullets/Bullet"));

	    GameObject playerSkin = this.transform.FindChild("PlayerSkin").gameObject;
	    (playerSkin.renderer as SpriteRenderer).color = color;
	}

    void Update()
    {
        UpdatePlayerMovement();
        UpdatePointer();
        UpdatePlayerActions();
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
        float moveX = InputHandler.GetPlayerAxis(CustomControls.MOVE_X, PlayerNum) * Speed;
        float moveY = InputHandler.GetPlayerAxis(CustomControls.MOVE_Y, PlayerNum) * Speed;

        Vector2 movementVector = new Vector2(moveX, moveY);

        rigidbody2D.velocity = movementVector;
    }

    void UpdatePlayerActions()
    {
        //Primary weapon
        if (InputHandler.GetPlayerAxis(CustomControls.SHOOT1, PlayerNum) > .2)
        {
            if (ItemList[0] != null)
            {
                ItemList[0].OnUse();
            }
        }

        //Secondary weapon
        if (InputHandler.GetPlayerAxis(CustomControls.SHOOT2, PlayerNum) > .2)
        {
            if (ItemList[1] != null)
            {
                ItemList[1].OnUse();
            }
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

            Vector3 target = new Vector3(this.transform.position.x + mousePos.x, this.transform.position.y + mousePos.y, 0);
            

            Quaternion newRotation = Quaternion.LookRotation(transform.position - target, Vector3.forward);
            newRotation.x = 0;
            newRotation.y = 0;

            PointerTransform.rotation = newRotation;

        }
        else
        {
            float aimX = InputHandler.GetPlayerAxis(CustomControls.AIM_X, PlayerNum);
            float aimY = InputHandler.GetPlayerAxis(CustomControls.AIM_Y, PlayerNum);

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