using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionRange = 3f; // Max distance for interaction
    public LayerMask interactableLayer; // Assign Interactable layer in Inspector
    public Material highlightMaterial; 
    private Material originalMaterial;
    private Renderer lastHighlightedRenderer;

    private IInteractable nearestInteractable;
    //public Transform playerBody;

    void Update()
    {
        DetectInteractable();
        if (nearestInteractable != null && Input.GetKeyDown(KeyCode.E))
        {
            nearestInteractable.Interact();
        }
    }

    void Highlighting(Renderer objRenderer)
    {
        if (objRenderer != null)
        {
            // Restore the previous highlighted object if it's different
            if (lastHighlightedRenderer != null && lastHighlightedRenderer != objRenderer)
            {
                lastHighlightedRenderer.material = originalMaterial;
            }

            // Store original material & apply highlight
            if (lastHighlightedRenderer != objRenderer)
            {
                originalMaterial = objRenderer.material;
            }
            objRenderer.material = highlightMaterial;
            lastHighlightedRenderer = objRenderer;
            return;
        }

        // Restore the material if no object is highlighted
        if (lastHighlightedRenderer != null)
        {
            lastHighlightedRenderer.material = originalMaterial;
            lastHighlightedRenderer = null;
        }
    }

    void DetectInteractable()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, interactionRange, interactableLayer);
        float minDistance = float.MaxValue;
        IInteractable closest = null;
        Renderer closestRenderer = null;

        foreach (var hit in hits)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            Renderer renderer = hit.collider.GetComponent<Renderer>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(transform.position, hit.point);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = interactable;
                    closestRenderer = renderer;
                }
            }
        }

        nearestInteractable = closest;
        Highlighting(closestRenderer);
    }
}
