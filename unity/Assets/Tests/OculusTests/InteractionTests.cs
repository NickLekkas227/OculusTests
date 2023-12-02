using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class InteractionTests : BaseTests
{
    private GameObject hand;
    private GameObject cube;

    [UnitySetUp]
    public IEnumerator CreateInteractiveElements()
    {
        hand = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        hand.transform.localScale = Vector3.one * 0.1f;
        hand.transform.parent = player.transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
        hand.transform.localPosition = (Vector3.forward + Vector3.right) * 0.3f;
        Rigidbody handRb = hand.AddComponent<Rigidbody>();
        handRb.isKinematic = true;
        handRb.useGravity = false;
        hand.AddComponent<PlayerGrab>();
        hand.GetComponent<SphereCollider>().isTrigger = true;

        GameObject grabbableCubePref = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/GrabbableCube.prefab");
        cube = GameObject.Instantiate(grabbableCubePref);

        yield return new WaitForFixedUpdate();
    }

    private static readonly Dictionary<GrabbablePosition, Vector3> grabbablePositions = new Dictionary<GrabbablePosition, Vector3>()
    {
        { GrabbablePosition.Unreachable, Vector3.zero },
        { GrabbablePosition.Reachable, new Vector3(0.3f, 0, 0.35f) }
    };

    [UnityTest]
    [Order(0)]
    public IEnumerator InteractWithGrabbable([ValueSource(nameof(grabbablePositions))] KeyValuePair<GrabbablePosition, Vector3>  grabbablePosition)
    {
        cube.transform.position = grabbablePosition.Value;
        yield return new WaitForFixedUpdate();

        if (grabbablePosition.Key == GrabbablePosition.Unreachable)
        {
            hand.GetComponent<PlayerGrab>().Grab();
            yield return new WaitForFixedUpdate();
            Assert.IsFalse(cube.GetComponent<OVRGrabbable>().isGrabbed, $"Expected player to be unable to grab unreachable object. Player postition: {hand.transform.position}, Object Position: {cube.transform.position}");
        }
        else if(grabbablePosition.Key == GrabbablePosition.Reachable)
        {
            hand.GetComponent<PlayerGrab>().Grab();
            yield return new WaitForFixedUpdate();
            Assert.IsTrue(cube.GetComponent<OVRGrabbable>().isGrabbed, $"Expected player to be able to grab reachable object. Player postition: {hand.transform.position}, Object Position: {cube.transform.position}");
        }

        yield return new WaitForFixedUpdate();
    }

    [UnityTest]
    [Order(1)]
    public IEnumerator GrabAndThrow()
    {
        cube.transform.position = new Vector3(0.3f, 0, 0.35f);
        yield return new WaitForFixedUpdate();

        hand.GetComponent<PlayerGrab>().Grab();
        yield return new WaitForFixedUpdate();

        Assert.IsTrue(cube.GetComponent<OVRGrabbable>().isGrabbed, $"Expected player to be able to grab reachable object. Player postition: {hand.transform.position}, Object Position: {cube.transform.position}");
        Assert.IsNotNull(hand.GetComponent<PlayerGrab>().grabbedObject);

        Vector3 lastPos = cube.transform.position;
        hand.GetComponent<PlayerGrab>().Throw();
        yield return new WaitForSeconds(0.2f);
        Assert.IsFalse(cube.GetComponent<OVRGrabbable>().isGrabbed, $"Expected object to have been released");
        Assert.AreNotEqual(lastPos, cube.transform.position, $"Expected object to move forward from throw");

        yield return new WaitForFixedUpdate();
    }

    [UnityTearDown]
    public IEnumerator DestroyInteractiveElements()
    {
        GameObject.Destroy(hand);
        GameObject.Destroy(cube);

        yield return new WaitForFixedUpdate();
    }

    public enum GrabbablePosition
    {
        Unreachable,
        Reachable
    }
}
