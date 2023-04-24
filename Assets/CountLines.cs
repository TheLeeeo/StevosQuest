using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CountLines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int lines = 0;
        int numChars = 0;
        ProcessFiles("/Users/04lefo01/Desktop/Programmering/Unity/UnityProjects/Steveo's Quest/Assets", ref lines, ref numChars);
        Debug.Log("the project has " + lines.ToString() + " of code");
    }

    private static void ProcessFiles(string dir, ref int numLines, ref int numChars)
    {
        var files = System.IO.Directory.GetFiles(dir);
        foreach (var file in files)
        {
            var ext = System.IO.Path.GetExtension(file);
            if (ext == ".cs")
            {
                var lines = System.IO.File.ReadAllLines(file);
                /*foreach (var line in lines)
                    numChars += line.Trim().Length;*/

                numLines += lines.Length;

                //Console.WriteLine("Scanned " + Path.GetFileNameWithoutExtension(file));
            }
        }

        var dirs = System.IO.Directory.GetDirectories(dir);
        foreach (var d in dirs)
            ProcessFiles(d, ref numLines, ref numChars);
    }
}