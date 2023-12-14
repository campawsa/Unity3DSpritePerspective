using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    //main refs
    private Animator _animator;
    [SerializeField]
    private Transform _sprite;
    private SpriteRenderer _renderer;

    [SerializeField]
    private int _cameraAngleIndex;
    private float _previousAngle;

    //dead zones prevent float weirdness when walking toward/away from camera, and when walking diagonally
    private float _mirrorDeadZone = 4f; //dead zone in degrees
    private float _angleDeadZone = 4f; //degrees
    
    void Start()
    {
        _animator = _sprite.GetComponent<Animator>();
        _renderer = _sprite.GetComponent<SpriteRenderer>();
    }

    private float CameraAngle(Transform camera)
    {
        //calculate rotation of the camera around the player
        Vector3 directionToCamera = transform.InverseTransformPoint(camera.position);
        return Mathf.Atan2(directionToCamera.x, directionToCamera.z) * Mathf.Rad2Deg;
    }

    private int CameraPerspective(float yAngle)
    {
        //0: player facing camera
        //1: player facing right
        //2: player facing away

        float yAngleAbs = Mathf.Abs(yAngle);

        if (yAngleAbs < 45) return 0;
        else if (yAngleAbs >= 45 && yAngleAbs < 135) return 1;
        else return 2;
    }

    private int CameraPerspectiveWithDeadZones(float yAngle, float lastAngle)
    {
        //updates camera perspective index if angle is outside dead zones
        //within deadzones, angle only updates if current angle has a difference of [threshold] from previous frame, in degrees
        //hopefully prevents float weirdness :3

        float yAngleAbs = Mathf.Abs(yAngle);

        if (IsPerspectiveUpdateRange(yAngleAbs, 0, 45)) _cameraAngleIndex = 0;
        else if (IsPerspectiveUpdateRange(yAngleAbs, 45, 135)) _cameraAngleIndex = 1;
        else if (IsPerspectiveUpdateRange(yAngleAbs, 135, 180)) _cameraAngleIndex = 2;
        else
        {
            float lastAngleAbs = Mathf.Abs(lastAngle);
            float deadZoneDifferenceThreshold = _angleDeadZone + 1;
            if (Mathf.Abs(yAngleAbs - lastAngleAbs) > deadZoneDifferenceThreshold)
            {
                _cameraAngleIndex = CameraPerspective(yAngle);
            }
        }
        return _cameraAngleIndex;
    }

    private bool IsPerspectiveUpdateRange(float angle, float rangeMin, float rangeMax)
    {
        //true = camera rotation is in a perspective update zone, false = dead zone
        float halfDeadZone = _angleDeadZone / 2;
        return angle >= rangeMin + halfDeadZone && angle < rangeMax - halfDeadZone;
    }

    private void FlipSpriteX(float cameraAngle)
    {
        //flip sprite if camera rotation is negative (with dead zone)
        float halfDeadZone = _mirrorDeadZone / 2;
        if (cameraAngle <= 0 - halfDeadZone && cameraAngle >= -180 + halfDeadZone) _renderer.flipX = true;
        else if (cameraAngle >= 0 + halfDeadZone && cameraAngle <= 180 - halfDeadZone) _renderer.flipX = false;
    }

    public IEnumerator InitSpritePerspectiveOnFirstFrame(Transform camera)
    {
        //set sprite perspective (ignoring deadzones) on frame 1
        yield return new WaitForEndOfFrame();

        float initAngle = CameraAngle(camera);
        _cameraAngleIndex = CameraPerspective(initAngle);
        _animator.SetFloat("Perspective", _cameraAngleIndex);
        FlipSpriteX(initAngle);
        _previousAngle = initAngle;

        Debug.Log("player sprite perspective initialized: angle index: " + _cameraAngleIndex + ", flip sprite?: " + _renderer.flipX);
    }

    public void UpdateSpritePerspective(Transform camera)
    {
        float currentCameraAngle = CameraAngle(camera);
        _animator.SetFloat("Perspective", CameraPerspectiveWithDeadZones(currentCameraAngle, _previousAngle));

        //cache current camera angle for use in next frame
        StartCoroutine(CacheCurrentCameraAngleForNextFrame(currentCameraAngle));

        FlipSpriteX(currentCameraAngle);

        //rotate sprite transform toward camera
        _sprite.eulerAngles = new Vector3(0, camera.eulerAngles.y, 0);
    }

    private IEnumerator CacheCurrentCameraAngleForNextFrame(float currentAngle)
    {
        yield return new WaitForEndOfFrame();
        _previousAngle = currentAngle;
    }

    public void PlayWalkAnimation(float playerForwardVelocity)
    {
        //play walk animation? uses player forward velocity
        int isWalking;
        if (playerForwardVelocity > 0.2) isWalking = 1;
        else isWalking = 0;
        _animator.SetFloat("Velocity", isWalking);
    }
}
