using UnityEngine;
using System.Linq;
using SFB;
using System.IO;
using Dummiesman;
//using UnityEngine.UI;

/* source: https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
 * obj importer: https://assetstore.unity.com/packages/tools/modeling/runtime-obj-importer-49547
 * found also another obj importer (optimized for blender modells and faster):
 * http://wiki.unity3d.com/index.php/FastObjImporter */

public class ImportModel : MonoBehaviour
{
    public Material defaultMaterial;
    public ChangeModel changeModel;
    private GameObject modelsContainer;
    private GameObject importedModel;
    //private Slider scaleSlider;

    public void Start()
    {
        modelsContainer = GameObject.Find("ModelsContainer");
        importedModel = GameObject.Find("ImportedModel");
        //scaleSlider = Slider.FindObjectOfType<Slider>();
    }

    // Open file system dialog to import own 3D modell
    public void ImportObj()
    {
        // destroy imported modell if there is one (from former import)
        if (importedModel) {
            Destroy(importedModel);
        }

        string meshPath = StandaloneFileBrowser.OpenFilePanel("Importiere dein 3D-Modell", "", "obj", false).FirstOrDefault();

        if (meshPath.Length != 0)
        {
            //importedModel = new GameObject("ImportedModel");
            
            
            if (File.Exists(meshPath))
            {
                importedModel = new OBJLoader().Load(meshPath);
            }

            importedModel.transform.parent = modelsContainer.transform;
            importedModel.tag = "model";
            var meshFilter = importedModel.GetComponentInChildren<MeshFilter>();
            var mesh = meshFilter.mesh;
            mesh = MeshUtils.RescaleAndCenterPivot(mesh, 50);
            mesh.RecalculateBounds();
            meshFilter.mesh=mesh;
            
            var renderer = importedModel.GetComponentInChildren<MeshRenderer>();
            renderer.material = defaultMaterial;
            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }

            changeModel.SetImportedActive(importedModel);
        }
    }
}
