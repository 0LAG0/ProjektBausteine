using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

/* Source:
 * https://docs.unity3d.com/ScriptReference/EditorUtility.OpenFilePanel.html
 */

public class FileExplorer : EditorWindow {

    [MenuItem("Example/Overwrite Texture")]

    public static void Apply()
    {
        Texture2D texture = Selection.activeObject as Texture2D;

        if (texture == null)
        {
            EditorUtility.DisplayDialog("Select Texture", "You must select a texture first!", "OK");
            return;
        }

        if (path.Length != 0)
        {
            var fileContent = File.ReadAllBytes(path);
            texture.LoadImage(fileContent);
        }
    }
}