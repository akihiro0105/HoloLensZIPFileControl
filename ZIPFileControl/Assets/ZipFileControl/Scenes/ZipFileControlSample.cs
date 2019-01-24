using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZipFileControl;
using HoloLensModule.Environment;

/// <summary>
/// Zipファイルの展開と作成を行うサンプル
/// </summary>
public class ZipFileControlSample : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        // Zipファイル展開後に作成を行う
        StartCoroutine(ZipControl.OpenZip(FileIOControl.LocalFolderPath + "\\logo.zip", FileIOControl.LocalFolderPath,
            () =>
            {
                // Zipファイルの作成を行う
                var dic = Directory.GetDirectories(FileIOControl.LocalFolderPath);
                if (dic.Length > 0)
                    StartCoroutine(ZipControl.CreateZip(dic[0], FileIOControl.LocalFolderPath + "\\logo_.zip",
                        () => Debug.Log("Create Zip")));
            }));
    }
}
