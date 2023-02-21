using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Api : MonoBehaviour
{
    private string api = "https://rickandmortyapi.com/api/character/";
    private string apiFalsa = "https://my-json-server.typicode.com/Rolosky69/Entrega1Sistemas/db";
    [SerializeField] private RawImage[] images;
    private string[] baraja1, baraja2, baraja3;
    private int count;

    void Start()
    {
        User();
    }
    void User()
    {
        StartCoroutine(GetUsers());
    }
    public void Baraja(int index)
    {
        string[] revision = new string[5];

        switch (index)
        {
            case 0:
                revision = baraja1;
                break;
            case 1:
                revision = baraja2;
                break;
            case 2:
                revision = baraja3;
                break;
            default:
                return;
        }

        count = 0;

        for (int i = 0; i < revision.Length; i++)
        {
            StartCoroutine(GetPersonaje(revision[i]));
        }
    }
    
    
    IEnumerator GetPersonaje(string index)
    {
        UnityWebRequest www = UnityWebRequest.Get(api + index);
        yield return www.SendWebRequest();
 
        if(www.result != UnityWebRequest.Result.Success) 
        {
            Debug.Log("Connection Error: " + www.error);
        }
        else 
        {
            if (www.responseCode == 200)
            {
                string responseText = www.downloadHandler.text;

                Personaje personaje = JsonUtility.FromJson<Personaje>(responseText);

                StartCoroutine(DownloadImage(personaje.image,images[count]));
                count++;
            }
            else
            {
                string message = "Status: " + www.responseCode;
                message += "\nContent-type: " + www.GetResponseHeader("content-type");
                message += "\nError: " + www.error;
            }
        }
    }
    IEnumerator DownloadImage(string image, RawImage textureIm)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(image);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
        }
        else
        {
            textureIm.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
    IEnumerator GetUsers()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiFalsa);
        yield return www.SendWebRequest();
 
        if(www.result != UnityWebRequest.Result.Success) 
        {
            Debug.Log("Connection Error: " + www.error);
        }
        else 
        {
            if (www.responseCode == 200)
            {
                string responseText = www.downloadHandler.text;

                print(responseText);
                
                UserList usuarios = JsonUtility.FromJson<UserList>(responseText);

                baraja1 = usuarios.users[0].baraja.Split(",");
                baraja2 = usuarios.users[1].baraja.Split(",");
                baraja3 = usuarios.users[2].baraja.Split(",");

            }
            else
            {
                string message = "Status: " + www.responseCode;
                message += "\nContent-type: " + www.GetResponseHeader("content-type");
                message += "\nError: " + www.error;
            }
        }
    }
}

[System.Serializable]
public class Personaje
{
    public int id;
    public string image;
}

[System.Serializable]
public class UserList
{
    public Usuario[] users;
}

[System.Serializable]
public class Usuario
{
    public int id;
    public string nombre;
    public string baraja;
}
