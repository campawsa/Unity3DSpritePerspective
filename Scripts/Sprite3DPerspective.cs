using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite3DPerspective : MonoBehaviour
{
    //camera
    private Transform _camera;

    //sprite
    [SerializeField]
    private Transform _sprite;
    private SpriteRenderer _renderer;
    [SerializeField]
    private Sprite[] _perspectiveSprites;

    void Start()
    {
        _camera = Camera.main.transform;
        _renderer = _sprite.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        UpdateSpritePerspective();
    }

    public int CameraPerspective(float yAngle)
    {
        //0: facing camera
        //1: facing right
        //2: facing away
        //3: facing left
        if (yAngle >= -45 && yAngle < 45) return 0;
        else if (yAngle >= 45 && yAngle < 135) return 1;
        else if (yAngle >= 135 || yAngle <= -135) return 2;
        else if (yAngle <= -45 && yAngle > -135) return 3;
        else return 0;
    }

    public void UpdateSpritePerspective()
    {
        //calculate angle to camera + set sprite based on cam perspective
        Vector3 directionToCamera = transform.InverseTransformPoint(_camera.position);
        float cameraAngle = Mathf.Atan2(directionToCamera.x, directionToCamera.z) * Mathf.Rad2Deg;
        _renderer.sprite = _perspectiveSprites[CameraPerspective(cameraAngle)];

        //rotate sprite toward camera
        _sprite.eulerAngles = new Vector3(0, _camera.eulerAngles.y, 0);
    }
}
