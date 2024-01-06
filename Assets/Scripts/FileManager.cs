using AnotherFileBrowser.Windows;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    [SerializeField]
    private Piano piano;

    public void OpenFileBrowser()
    {
        var bp = new BrowserProperties();
        bp.filter = "Sound files | *.wav; *.mp3";
        bp.filterIndex = 0;

        new FileBrowser().OpenFileBrowser(bp, path =>
        {
            StartCoroutine(LoadClip(path));
        });
    }

    IEnumerator LoadClip(string path)
    {
        string ext = Path.GetExtension(path);
        AudioType audioType;
        if (ext.Contains("wav"))
        {
            audioType = AudioType.WAV;
        }
        else
        {
            audioType = AudioType.MPEG;
        }

        using (UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(path, audioType))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(uwr.error);
            } else
            {
                AudioClip clip = DownloadHandlerAudioClip.GetContent(uwr);
                piano.SetClip(clip);
            }
        }
    }
}
