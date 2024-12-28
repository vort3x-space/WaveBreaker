using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;         // Oyuncunun Transform referansı
    public Vector3 offset;           // Kameranın oyuncuyla arasındaki sabit mesafe
    public float smoothSpeed = 0.125f; // Kameranın yumuşak hareket hızı

    private void LateUpdate()
    {
        // Hedef pozisyon: Oyuncu pozisyonu + ofset
        Vector3 desiredPosition = player.position + offset;

        // Kamerayı hedef pozisyona yumuşak bir şekilde taşı
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Kameranın rotasyonunu sabit tut 
        transform.rotation = Quaternion.Euler(50f, 67.8f, 0f);
    }
}
