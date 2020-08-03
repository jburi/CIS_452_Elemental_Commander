using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    AISpawner aISpawner;

    RaycastHit hit;
    public List<AI> selectedAI = new List<AI>();
    bool isDragging = false;

    //State Booleans
    public bool M_attackState = false;
    public bool U_attackState = false;
    public bool healState = false;

    //State Spites
    public Sprite M_attackStateOn;
    public Sprite M_attackStateOff;
    public Sprite U_attackStateOn;
    public Sprite U_attackStateOff;
    public Sprite healStateOn;
    public Sprite healStateOff;

    //UI Buttons
    public Button m_Attack;
    public Button u_Attack;
    public Button move;
    public Button heal;

    Vector3 mousePosition;

    private void OnGUI()
    {
        if (isDragging)
        {
            var rect = ScreenHelper.GetScreenRect(mousePosition, Input.mousePosition);
            ScreenHelper.DrawScreenRect(rect, new Color(0.5f, 0.5f, 0.6f, 0.15f));
            ScreenHelper.DrawScreenRectBorder(rect, 1.5f, Color.Lerp(Color.grey, Color.white, 0.3f));

        }

        
    }

    private void Start()
    {
        aISpawner = gameObject.GetComponent<AISpawner>();
        MouseAttackState();

        //Set State Sprites
        /*
        M_attackStateOn = Resources.Load("M_AttackOn") as Sprite;
        M_attackStateOff = Resources.Load("M_AttackOff") as Sprite;
        U_attackStateOn = Resources.Load("U_AttackOn") as Sprite;
        U_attackStateOff = Resources.Load("M_AttackOff") as Sprite;
        moveStateOn = Resources.Load("Moving") as Sprite;
        moveStateOff = Resources.Load("NotMoving") as Sprite;
        healStateOn = Resources.Load("HealOn") as Sprite;
        healStateOff = Resources.Load("HealOff") as Sprite;
        */
    }

    // Update is called once per frame
    void Update()
    {
        //Selecting units by clicking
        if (Input.GetMouseButtonDown(0))
        {
            mousePosition = Input.mousePosition;

            var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(camRay, out hit))
            {
                Debug.Log(hit.transform.tag);
                if (hit.transform.CompareTag("Player"))
                {
                    SelectUnit(hit.transform.GetComponent<AI>(), Input.GetKey(KeyCode.LeftShift));
                }
                else
                {
                    isDragging = true;
                }

                /*
                else if (hit.transform.CompareTag("Ground"))
                {
                    DeselectUnits();
                }
                */
            }
        }
        //Select units by dragging the mouse with left click
        if (Input.GetMouseButtonUp(0))
        {
            //aISpawner = gameObject.GetComponent<AISpawner>();
            if(isDragging)
            {
                DeselectUnits();

                foreach (GameObject selectableObject in aISpawner.currentAllies)
                {
                    if (IsWithinSelectionBounds(selectableObject.transform))
                    {
                        SelectUnit(selectableObject.gameObject.GetComponent<AI>(), true);
                    }
                }
            }

            //Reset
            isDragging = false;
        }


        if (selectedAI.Contains(null))
        {
            selectedAI.Clear();
        }

        //Tell your allies to attack or move
        if (Input.GetKeyDown("q"))
        {
            MouseAttackState();
        }

        if (Input.GetKeyDown("w"))
        {
            UnitAttackState();
        }

        if (Input.GetKeyDown("e"))
        {
            HealState();
        }


        //What to do in each state
        if (M_attackState)
        {
            //Right Click to hit enemies
            if (Input.GetMouseButtonDown(1))
            {
                var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(camRay, out hit))
                {
                    Debug.Log(hit.transform.tag);

                    if (hit.transform.CompareTag("EnemyUnit"))
                    {
                        Debug.Log("Giving them a slap");
                        hit.transform.GetComponent<AI>().health -= 2;
                    }
                }
            }
        }

        if(U_attackState)
        {
            //Right Click to attack with units
            if (Input.GetMouseButtonDown(1) && selectedAI.Count > 0)
            {
                var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(camRay, out hit))
                {
                    Debug.Log(hit.transform.tag);
                    
                    if (hit.transform.CompareTag("EnemyUnit"))
                    {
                        foreach (AI thisAI in selectedAI)
                        {
                            thisAI.currentTarget = hit.transform;
                            thisAI.gameObject.GetComponent<AI>().StartCoroutine("ContinueAttacking");
                            

                        }
                    }

                    if (hit.transform.CompareTag("Ground"))
                    {
                        foreach (AI thisAI in selectedAI)
                        {
                            thisAI.currentTarget = null;
                            thisAI.navAgent.destination = hit.point;
                            thisAI.StopCoroutine("ContinueAttacking");
                        }
                    }
                }
            }
        }
        
        
        if (healState)
        {
            //Right Click a thisAI to heal them for a cost
            if (Input.GetMouseButtonDown(0) && selectedAI.Count > 0)
            {
                var camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(camRay, out hit))
                {
                    Debug.Log(hit.transform.tag);

                    if (hit.transform.CompareTag("Player"))
                    {
                        Debug.Log("Healing");
                        //hit.health += healStrength;
                    }
                }
            }
        }

    }


    #region UIStateToggles
    //Methods to change the AI state using bools
    public void ResetState()
    {
        M_attackState = false;
        U_attackState = false;
        healState = false;

        m_Attack.image.sprite = M_attackStateOff;
        u_Attack.image.sprite = U_attackStateOff;
        heal.image.sprite = healStateOff;
    }

	
	public void MouseAttackState()
    {
        ResetState();

        M_attackState = true;
        m_Attack.image.sprite = M_attackStateOn;
    }

    public void UnitAttackState()
    {
        ResetState();

        U_attackState = true;
        u_Attack.image.sprite = U_attackStateOn;
    }

    public void HealState()
    {
        ResetState();

        healState = true;
        heal.image.sprite = healStateOn;
    }
	#endregion



	private void SelectUnit(AI thisAI, bool isMultiSelect = false)
    {
        if (!isMultiSelect)
        {
            DeselectUnits();
        }

        if (!selectedAI.Contains(thisAI.gameObject.GetComponent<AI>()))
        {
            selectedAI.Add(thisAI);
        }
        else if (selectedAI.Contains(thisAI) && isMultiSelect)
        {
            selectedAI.Remove(thisAI);
            thisAI.SetSelected(false);
        }

        thisAI.SetSelected(true);
    }

    private void DeselectUnits()
    {
        
        foreach (AI thisAI in selectedAI)
        {
            thisAI.SetSelected(false);
        }
        /*
        for (int i = 0; i < selectedAI.Count; i++)
        {
            selectedAI[i].Find("Highlight").gameObject.SetActive(false);
        }
        */

        selectedAI.Clear();
        
    }

    private bool IsWithinSelectionBounds(Transform transform)
    {
        if (!isDragging)
        {
            return false;
        }

        var cam = Camera.main;
        var viewportBounds = ScreenHelper.GetViewportBounds(cam, mousePosition, Input.mousePosition);
        return viewportBounds.Contains(cam.WorldToViewportPoint(transform.position));

    }

}
