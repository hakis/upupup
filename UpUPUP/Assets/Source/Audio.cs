using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Audio : MonoBehaviour
{
    struct Key
    {
        public string label;
        public string file;
        public bool loop;
    }

    public string root;

    List<Key> keys = new List<Key>();
    // Start is called before the first frame update
    void Start()
    {
        root = Application.persistentDataPath;
        StartCoroutine(GetText("list.txt", () =>
         {
             Play("Bg", () =>
             {
             });
         }));
    }

    public void Play(string label, System.Action done)
    {
        foreach (Key key in keys)
        {
            if (key.label == label)
            {
                StartCoroutine(Load(key, done));
            }
        }
    }

    void Update()
    {
    }

    IEnumerator Load(Key key, System.Action done)
    {
        UnityWebRequest req = UnityWebRequestMultimedia.GetAudioClip("file:///" + root + "/audio/" + key.file, AudioType.MPEG);
        yield return req.SendWebRequest();

        AudioSource[] sources = GetComponents<AudioSource>();
        foreach (AudioSource source in sources)
        {
            if (!source.isPlaying)
            {
                source.clip = DownloadHandlerAudioClip.GetContent(req);
                source.Play();
                source.loop = key.loop;
                break;
            }
        }

        done();
    }

    IEnumerator GetText(string file, System.Action done)
    {
        UnityWebRequest www = UnityWebRequest.Get("file:///" + root + "/" + file);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            string fs = www.downloadHandler.text;
            string[] lines = Regex.Split(fs, "\n|\r|\r\n");

            foreach (string line in lines)
            {

                string[] tmp = line.Split(':');

                Key key = new Key();
                key.label = tmp[0];
                key.file = tmp[1];
                key.loop = tmp[2] == "1" ? true : false;

                keys.Add(key);
            }

            byte[] results = www.downloadHandler.data;

            done();
        }
    }
}
