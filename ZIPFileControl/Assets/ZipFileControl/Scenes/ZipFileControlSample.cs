using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZipFileControl;

public class ZipFileControlSample : MonoBehaviour {

	// Use this for initialization
	void Start () {
        var path = FileIOControl.LocalFolderPath + "\\logo.zip";
        var coroutine = StartCoroutine(ZipControl.OpenFile(path, FileIOControl.LocalFolderPath, completed));
    }

    private void completed()
    {
        var dic = Directory.GetDirectories(FileIOControl.LocalFolderPath);
        if (dic.Length > 0) StartCoroutine(ZipControl.CreateFile(dic[0], FileIOControl.LocalFolderPath + "\\logo_.zip", null));
    }
}
