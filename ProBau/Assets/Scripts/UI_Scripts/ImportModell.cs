using UnityEngine;
using System.IO;
using UnityEditor;
using ImporterObj;

// source: https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
// obj importer: http://wiki.unity3d.com/index.php?title=ObjImporter
// found also another obj importer (optimized for blender modells and faster): http://wiki.unity3d.com/index.php/FastObjImporter

public class ImportModell : MonoBehaviour
{
    private GameObject modelsContainer = GameObject.Find("ModelsContainer");
    private GameObject importedModell = GameObject.Find("ImportedModell");
    public ObjImporter objImporter;

    // Open file system dialog to import own 3D modell
    public void OpenFilePanel()
    {
        // destroy game object with imported modell if there is one (from former import)
        if (importedModell) {
            Destroy(importedModell);
        }

        string meshPath = EditorUtility.OpenFilePanel("Importiere dein 3D Modell", "", "obj");

        if (meshPath.Length != 0)
        {
            // deactivate default modell
            modelsContainer.SetActive(false);

            // Approach 1 -> TODO convert byte[] to modell/game object
            var newModell = File.ReadAllBytes(meshPath);
            // idea: add modell to list and display it via select methXboxOneDeployMethod of ChangeModell
            // models.Add(newModell);
            // Select(models.IndexOf(newModell));

            // Approach 2 -> TODO use obj importer (mesh on gameobject)
            // importedModell.AddComponent<MeshFilter>();
            // Mesh importedMesh = new Mesh();
            // importedMesh = objImporter.ImportFile(meshPath);
            // importedModell.GetComponent<MeshFilter>().mesh = importedMesh;
        }
    }
}
