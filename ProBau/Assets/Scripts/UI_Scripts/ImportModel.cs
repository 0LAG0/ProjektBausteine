using UnityEngine;
using UnityEditor;
using ImporterObj;
//using UnityEngine.UI;

/* source: https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
 * obj importer: http://wiki.unity3d.com/index.php?title=ObjImporter
 * found also another obj importer (optimized for blender modells and faster):
 * http://wiki.unity3d.com/index.php/FastObjImporter */

public class ImportModel : MonoBehaviour
{
    public Material defaultMaterial;
    public ChangeModel changeModel;
    private GameObject modelsContainer;
    private GameObject importedModel;
    private ObjImporter objImporter;
    //private Slider scaleSlider;

    public void Start()
    {
        modelsContainer = GameObject.Find("ModelsContainer");
        importedModel = GameObject.Find("ImportedModel");
        objImporter = new ObjImporter();
        //scaleSlider = Slider.FindObjectOfType<Slider>();
    }

    // Open file system dialog to import own 3D modell
    public void ImportObj()
    {
        // destroy imported modell if there is one (from former import)
        if (importedModel) {
            Destroy(importedModel);
        }

        string meshPath = EditorUtility.OpenFilePanel("Importiere dein 3D-Modell", "", "obj");

        if (meshPath.Length != 0)
        {
            importedModel = new GameObject("ImportedModel");
            importedModel.transform.parent = modelsContainer.transform;
            importedModel.tag = "model";

            var meshFilter = importedModel.AddComponent<MeshFilter>();
            var mesh = objImporter.ImportFile(meshPath);

            mesh = MeshUtils.RescaleAndCenterPivot(mesh, 50);
            mesh.RecalculateBounds();
            meshFilter.mesh=mesh;

            var renderer = importedModel.AddComponent<MeshRenderer>();
            renderer.material = defaultMaterial;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }


            changeModel.SetImportedActive(importedModel);
        }
    }
}
