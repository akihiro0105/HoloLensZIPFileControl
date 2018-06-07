using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.IO;
#if UNITY_UWP
using System.Threading.Tasks;
using System.IO.Compression;
#elif UNITY_EDITOR || UNITY_STANDALONE
using Ionic.Zip;
#endif

namespace ZipFileControl
{
    public class ZipControl
    {
        public static IEnumerator OpenFile(string name,string outputpath,Action action=null)
        {
#if UNITY_UWP
            Task task = Task.Run(() =>
            {
                if (File.Exists(name))
                {
                    var directory = Path.GetDirectoryName(name);
                    if (Directory.Exists(directory))
                    {
                        Directory.Delete(directory);
                    }
                    ZipFile.ExtractToDirectory(name, directory);
                }
            });
            yield return new WaitWhile(() => task.IsCompleted == false);
#elif UNITY_EDITOR || UNITY_STANDALONE
            Thread thread = new Thread(()=> {
                using (ZipFile zip=ZipFile.Read(name))
                {
                    zip.ExtractAll(outputpath);
                }
            });
            thread.Start();
            yield return new WaitWhile(() => thread.IsAlive == true);
#endif
            yield return null;
            if (action != null) action.Invoke();
        }

        public static IEnumerator CreateFile(string directory,string outputfile,Action action)
        {
#if UNITY_UWP
            Task task = Task.Run(() =>
            {
                if (File.Exists(outputfile))
                {
                    File.Delete(outputfile);
                }
                ZipFile.CreateFromDirectory(directory, outputfile, CompressionLevel.NoCompression, true);
            });
            yield return new WaitWhile(() => task.IsCompleted == false);
#elif UNITY_EDITOR || UNITY_STANDALONE
            Thread thread = new Thread(() => {
                using (ZipFile zip=new ZipFile())
                {
                    var path = Path.GetFileName(directory);
                    zip.AddDirectory(directory, path);
                    zip.Save(outputfile);
                }
            });
            thread.Start();
            yield return new WaitWhile(() => thread.IsAlive == true);
#endif
            yield return null;
            if (action != null) action.Invoke();
        }
    }
}
