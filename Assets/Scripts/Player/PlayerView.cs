using UnityEngine;

public class PlayerView : MonoBehaviour
{
    [SerializeField] Transform cameraAxis;
    Vector2 direction;
    [SerializeField] float rotateSensitive = 15f;
    [SerializeField] Vector2 clampForFirstPerson;
    float camRotateX = 0f;

    public bool cursorIsLocked = false;


    void Start()
    {

        PlayerInputController inputController = CharacterManager.Instance.Player.inputController;
        inputController.OnLookEvent += OnLook;
        UIManager.Instance.OnMouseLock += Toggle;
        // inputController.OnToggleSettingEvent += Toggle;
        // inputController.OnToggleInventoryEvent += Toggle;

        camRotateX = cameraAxis.localEulerAngles.x;

        cursorIsLocked = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        if(!cursorIsLocked) return;

        Look();
    }


    void OnLook(Vector2 mouseDelta)
    {
        direction = mouseDelta;
    }

    void Look()
    {
        float speed = rotateSensitive * Time.deltaTime;

        transform.Rotate(Vector3.up * direction.x * speed);

        camRotateX += speed * -direction.y;
        camRotateX = Mathf.Clamp(camRotateX, clampForFirstPerson.x, clampForFirstPerson.y);

        cameraAxis.localEulerAngles = new Vector3(camRotateX, 0f, 0f);
    }   

    void Toggle(bool isLocked)
    {
        cursorIsLocked = !isLocked;

        if (cursorIsLocked)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;
    }
}