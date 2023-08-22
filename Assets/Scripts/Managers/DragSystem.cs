using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class DragSystem : MonoBehaviour
{
    public static DragSystem instance;

    public GameObject selectedObject;

    Vector3 startingPos;
    private void Awake() 
    {
        if(instance == null)
        {
            instance = this;
        }    
    }
    void Update()
    {
        RaycastHit hit = CastRay();

        if(hit.collider == null)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            
            if(hit.collider == null)
            {
                Debug.Log("wtf");
                return;
            }

            if(hit.collider != null && selectedObject == null )
            {   

                if(hit.collider.CompareTag("UpgradeCard"))
                {
                    selectedObject = hit.collider.gameObject;
                    Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,
                        Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);

                    startingPos = selectedObject.transform.position;
                    selectedObject.GetComponent<UpgradeCard>().upgradeCardChooserCollider.SetActive(true);
                    selectedObject.transform.position = new Vector3(worldPos.x,GameManager.instance.selectedCardYAxis,worldPos.z);
                }
            }
            if(selectedObject != null)
            {   
                Vector3 position = new Vector3(Input.mousePosition.x,Input.mousePosition.y,
                        Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(position);

                    selectedObject.transform.position = new Vector3(worldPos.x,GameManager.instance.selectedCardYAxis,worldPos.z);
            }
        }
        else if(Input.GetMouseButtonUp(0))
        {
            if(selectedObject != null)
            {   
                if(selectedObject.GetComponentInChildren<UpgradeCardChooserCollider>().gunsInCollider.Count > 0)
                {
                    selectedObject.GetComponentInChildren<UpgradeCardChooserCollider>().UpgradeAction();
                }
                else
                {
                    selectedObject.GetComponent<UpgradeCard>().upgradeCardChooserCollider.SetActive(false);
                    selectedObject.transform.position = startingPos;
                    selectedObject = null;
                }
            }
        }
    }
    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }
}
