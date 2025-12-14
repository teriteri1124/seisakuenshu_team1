using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCtrl : MonoBehaviour
{
    CharacterController charCtrl;
    Animator animCtrl;
    [SerializeField] float speed = 4;
    [SerializeField] float jumppow = 7;
    [SerializeField] float fallspd = 2.0f;

    [SerializeField] bool useCameraDir = true;
    [SerializeField] float movedirOffset = 0;

    [SerializeField] bool zEnable = true;
    [SerializeField] bool xEnable = true;
    [SerializeField] bool canJump = true;
    [SerializeField] bool FPSMove = false;

    [SerializeField] bool useAnimationRotate = false;

    Vector3 forwardVec;
    Vector3 rightVec;

    void Start()
    {
        charCtrl = GetComponent<CharacterController>();
        animCtrl = GetComponent<Animator>();

        var angles = new Vector3(0, movedirOffset, 0);
        forwardVec = Quaternion.Euler(angles) * Vector3.forward;
        rightVec = Quaternion.Euler(angles) * Vector3.right;
    }

    float fallpow = -2.0f;
    Transform groundobj = null;
    Transform beforeGroundObj = null;
    Vector3 beforePos;
    Vector3 floorOffset;

    void Update()
    {
        var moveRate = 0.5f;
        if (Input.GetButton("Fire3"))
        {
            moveRate = 1.0f;
        }

        float xaxis = Input.GetAxis("Horizontal") * moveRate;
        float yaxis = Input.GetAxis("Vertical") * moveRate;

        Vector3 cameraFwdVec = forwardVec;
        Vector3 cameraRightVec = rightVec;
        if (useCameraDir)
        {
            cameraFwdVec = Camera.main.transform.TransformDirection(forwardVec);
            cameraFwdVec.Scale(new Vector3(1, 0, 1));
            cameraFwdVec.Normalize();

            cameraRightVec = Camera.main.transform.TransformDirection(rightVec);
            cameraRightVec.Scale(new Vector3(1, 0, 1));
            cameraRightVec.Normalize();
        }

        var movementxaxis = xaxis;
        var movementyaxis = yaxis;

        if (!zEnable) movementyaxis = 0;
        if (!xEnable) movementxaxis = 0;

        Vector3 moveDir = cameraFwdVec * movementyaxis + cameraRightVec * movementxaxis;
        moveDir = Vector3.ClampMagnitude(moveDir, moveRate);

        if (charCtrl.isGrounded)
        {
            fallpow = -2f;
            if (canJump && Input.GetButtonDown("Jump"))
            {
                fallpow = jumppow;
            }
            animCtrl.SetBool("isJumping", false);
            animCtrl.SetFloat("Speed", moveDir.magnitude);
        }
        else
        {
            if (fallpow > -2.0f || fallpow < -2.0f - fallspd)
            {
                animCtrl.SetBool("isJumping", true);
            }
            fallpow += Physics.gravity.y * Time.deltaTime * fallspd;
        }

        if (groundobj)
        {
            if (groundobj == beforeGroundObj)
            {
                floorOffset = beforePos - groundobj.position;
            }
            else
            {
                beforeGroundObj = groundobj;
                floorOffset = Vector3.zero;
            }
            beforePos = groundobj.position;
            beforeGroundObj = groundobj;
        }

        charCtrl.Move(((new Vector3(0, fallpow, 0) + (moveDir * speed)) * Time.deltaTime) - floorOffset);

        if (FPSMove)
        {
            transform.eulerAngles = new Vector3(
                0, Vector3.SignedAngle(Vector3.forward, cameraFwdVec, Vector3.up), 0);
        }
        else
        {
            var rotdir = cameraFwdVec * yaxis + cameraRightVec * xaxis;
            if (rotdir.magnitude > 0)
            {
                if (useAnimationRotate)
                {
                    animCtrl.SetFloat("X", xaxis);
                    animCtrl.SetFloat("Y", yaxis);
                }
                else
                {
                    transform.eulerAngles = new Vector3(
                        0, Vector3.SignedAngle(Vector3.forward, rotdir, Vector3.up), 0);
                }
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject != gameObject && !hit.collider.isTrigger)
            groundobj = hit.transform;
    }

    // ワープ用メソッド
    public void WarpTo(Vector3 pos, Quaternion rot)
    {
        charCtrl.enabled = false;
        transform.position = pos;
        transform.rotation = rot;
        charCtrl.enabled = true;
    }
}
