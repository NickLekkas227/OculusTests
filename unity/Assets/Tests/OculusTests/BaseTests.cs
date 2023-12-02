using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class BaseTests
{
    protected GameObject player;
    protected PlayerController playerController;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        player = new GameObject("Manager");
        GameObject cameraRigPref = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Oculus/VR/Prefabs/OVRCameraRig.prefab");
        GameObject cameraRigGO = GameObject.Instantiate(cameraRigPref, player.transform);

        yield return new WaitForFixedUpdate();

        OVRCameraRig cameraRig = cameraRigGO.GetComponent<OVRCameraRig>();
        Assert.IsNotNull(cameraRig, "Failed to initialize camera rig");

        CharacterController characterController = player.AddComponent<CharacterController>();
        playerController = player.AddComponent<PlayerController>();
        playerController.useProfileData = false;

        GameObject forward = new GameObject("ForwardDirection");
        forward.transform.parent = player.transform;

        yield return new WaitForFixedUpdate();

        Assert.IsNotNull(characterController, "Failed to initialize character controller");
        Assert.IsNotNull(playerController, "Failed to initialize player controller");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        GameObject.Destroy(player);
        yield return new WaitForFixedUpdate();
    }
}
