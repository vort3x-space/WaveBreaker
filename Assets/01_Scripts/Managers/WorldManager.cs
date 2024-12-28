using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public GameObject[] worldPrefabs; // Sahneye yerleştirilmiş tüm world prefab'ları

    public void LoadWorld(int index)
    {
        for (int i = 0; i < worldPrefabs.Length; i++)
        {
            // Sadece istenen indexteki prefab'ı aktif et
            worldPrefabs[i].SetActive(i == index);
        }
    }
}
