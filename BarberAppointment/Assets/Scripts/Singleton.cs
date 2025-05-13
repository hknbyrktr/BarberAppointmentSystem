using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    private static readonly object _lock = new object();

    public static T Instance
    {
        get
        {

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = (T)FindObjectOfType(typeof(T));

                    if (FindObjectsOfType(typeof(T)).Length > 1)
                    {
                        Debug.LogError("[Singleton] Birseyler ters gitti. " + " - asla birden fazla singleton olmamalidir!" + "Sahneyi yeniden acmak sorunu cozebilir.");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<T>();
                        singleton.name = "(singleton) " + typeof(T).ToString();

                        DontDestroyOnLoad(singleton);

                        Debug.Log("[Singleton] ornegi: " + typeof(T) + " sahnede ihtiyac var, bu yuzden '" + singleton + "'olusturuldu.");
                    }
                    else
                    {
                        Debug.Log("[Singleton] Zaten olusturulmus ornek kullaniliyor: " + _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }
    }


}
