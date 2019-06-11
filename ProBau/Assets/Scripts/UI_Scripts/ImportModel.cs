using UnityEngine;
using System.IO;
using UnityEditor;
using ImporterObj;

/* source: https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
 * obj importer: http://wiki.unity3d.com/index.php?title=ObjImporter
 * found also another obj importer (optimized for blender modells and faster):
 * http://wiki.unity3d.com/index.php/FastObjImporter */

public class ImportModel : MonoBehaviour
{
    private GameObject modelsContainer;
    private GameObject importedModel;
    public ObjImporter objImporter;

    public void Start()
    {
        modelsContainer = GameObject.Find("ModelsContainer");
        importedModel = GameObject.Find("ImportedModel");
        objImporter = new ObjImporter();
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

            importedModel.AddComponent<MeshFilter>();
            importedModel.GetComponent<MeshFilter>().mesh = objImporter.ImportFile(meshPath);

            importedModel.AddComponent<MeshRenderer>();

            foreach (Transform t in transform)
            {
                t.gameObject.SetActive(false);
            }

            importedModel.SetActive(true);
        }
    }
}
