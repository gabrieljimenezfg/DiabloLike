using UnityEngine;

public class MouseOutliner : MonoBehaviour
{
    private GameObject currentHoveredEnemy;
    private Material[] originalMaterials;

    [SerializeField] private Material outlineMaterial;

    void Update()
    {
        if (MouseWorldUtils.TryGetMousePositionOnTargetLayer(MouseRayTargetLayer.Enemy, out var enemyHit))
        {
            var enemyObj = enemyHit.collider.gameObject;
            if (currentHoveredEnemy != enemyObj)
            {
                DeleteOutliner();
                currentHoveredEnemy = enemyObj;
                SetOutlineMaterial();
            }
        }
        else
        {
            DeleteOutliner();
        }
    }

    private void SetOutlineMaterial()
    {
        MeshRenderer meshRenderer = currentHoveredEnemy.GetComponent<MeshRenderer>();
        originalMaterials = meshRenderer.materials;
        Material[] newMats = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(newMats, 0);
        newMats[^1] = outlineMaterial;
        meshRenderer.materials = newMats;
    }

    private void DeleteOutliner()
    {
        if (!currentHoveredEnemy) return;

        if (currentHoveredEnemy.TryGetComponent(out MeshRenderer meshRenderer))
        {
            meshRenderer.materials = originalMaterials;
        }

        currentHoveredEnemy = null;
        originalMaterials = null;
    }
}