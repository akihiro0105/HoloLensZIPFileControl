using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
#if UNITY_UWP
using System.Threading.Tasks;
using System.IO.Compression;
#elif UNITY_EDITOR || UNITY_STANDALONE
using System.Threading;
using Ionic.Zip;
#endif

namespace ZipFileControl
{
    public class ZipControl
    {
        /// <summary>
        /// Zipファイルを展開
        /// </summary>
        /// <param name="name"></param>
        /// <param name="filePath"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerator OpenZip(string name,string filePath,Action action=null)
        {
#if UNITY_UWP
            var task = Task.Run(() =>
            {
                if (File.Exists(name))
                {
                    var directory = Path.GetDirectoryName(name);
                    if (Directory.Exists(directory)) Directory.Delete(directory);
                    ZipFile.ExtractToDirectory(name, directory);
                }
            });
            yield return new WaitWhile(() => task.IsCompleted == false);
#elif UNITY_EDITOR || UNITY_STANDALONE
            var thread = new Thread(()=>
            {
                using (var zip = ZipFile.Read(name)) zip.ExtractAll(filePath);
            });
            thread.Start();
            yield return new WaitWhile(() => thread.IsAlive == true);
#endif
            yield return null;
            if (action != null) action.Invoke();
        }

        /// <summary>
        /// Zipファイルを作成
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="filePath"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IEnumerator CreateZip(string directory,string filePath,Action action)
        {
#if UNITY_UWP
            var task = Task.Run(() =>
            {
                if (File.Exists(filePath)) File.Delete(filePath);
                ZipFile.CreateFromDirectory(directory, filePath, CompressionLevel.NoCompression, true);
            });
            yield return new WaitWhile(() => task.IsCompleted == false);
#elif UNITY_EDITOR || UNITY_STANDALONE
            var thread = new Thread(() => {
                using (var zip=new ZipFile())
                {
                    zip.AddDirectory(directory, Path.GetFileName(directory));
                    zip.Save(filePath);
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
