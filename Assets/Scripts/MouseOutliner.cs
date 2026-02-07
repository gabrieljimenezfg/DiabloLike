using UnityEngine;

public class MouseOutliner : MonoBehaviour
{
    private MeshRenderer currentRenderer;
    private Material[] originalMaterials;

    [SerializeField] private Material outlineMaterial;

    void Update()
    {
        if (MouseWorldUtils.TryGetFirstHighlightable(out var highlightable))
        {
            var foundRenderer = highlightable.Renderer;
            if (currentRenderer == foundRenderer) return;

            DeleteOutliner();
            currentRenderer = foundRenderer;
            SetOutlineMaterial();
        }
        else
        {
            DeleteOutliner();
        }
    }

    private void SetOutlineMaterial()
    {
        originalMaterials = currentRenderer.materials;
        Material[] newMats = new Material[originalMaterials.Length + 1];
        originalMaterials.CopyTo(newMats, 0);
        newMats[^1] = outlineMaterial;
        currentRenderer.materials = newMats;
    }

    private void DeleteOutliner()
    {
        if (!currentRenderer) return;

        currentRenderer.materials = originalMaterials;
        currentRenderer = null;
        originalMaterials = null;
    }
}