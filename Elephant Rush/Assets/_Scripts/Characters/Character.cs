using UnityEngine;
using System;
using System.Diagnostics;

/// <summary>
/// Mainly used as a data container to define a character. This script is attached to the prefab
/// (found in the Bundles/Characters folder) and is to define all data related to the character.
/// </summary>
public class Character : MonoBehaviour
{
    public string characterName;
    public int cost;
	public int premiumCost;

	public CharacterAccessories[] accessories;

    public Animator animator;
	public Sprite icon;

	[Header("Sound")]
	public AudioClip jumpSound;
	public AudioClip hitSound;
	public AudioClip deathSound;

    public float rotationSpeed = 360f;

    private Vector3 initialMousePosition;
    private bool isMouseDown = false;
    private bool shouldRotate = false;

    // Called by the game when an accessory changes, enable/disable the accessories children objects accordingly
    // a value of -1 as parameter disables all accessory.
    public void SetupAccesory(int accessory)
    {
        for (int i = 0; i < accessories.Length; ++i)
        {
            accessories[i].gameObject.SetActive(i == PlayerData.instance.usedAccessory);
        }
    }

    public void ShouldRotate(bool value)
    {
        shouldRotate = value;
    }

    private void Update()
    {
        if (shouldRotate)
        {
            RotateCharacter();
        }
    }

    private void RotateCharacter()
    {
        if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            initialMousePosition = GetInputPosition();
        }
        else if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved))
        {
            Vector2 touchPosition = GetInputPosition();

            float deltaTouchPosition = touchPosition.x - initialMousePosition.x;

            transform.rotation = Quaternion.Euler(0, -deltaTouchPosition * rotationSpeed, 0) * transform.rotation;
        }

#if UNITY_EDITOR     
        if (Input.GetMouseButtonDown(0))
        {
            isMouseDown = true;
            initialMousePosition = GetInputPosition();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isMouseDown = false;
        }

        if (isMouseDown)
        {
            Vector2 mousePosition = GetInputPosition();

            float deltaMousePosition = mousePosition.x - initialMousePosition.x;

            transform.rotation = Quaternion.Euler(0, -deltaMousePosition * rotationSpeed, 0) * transform.rotation;
        }
#endif
    }

    private Vector2 GetInputPosition()
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(0).position;
        }
        else
        {
            return Input.mousePosition;
        }
    }
}
