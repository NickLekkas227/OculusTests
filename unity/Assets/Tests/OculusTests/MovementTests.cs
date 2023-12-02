using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class MovementTests : BaseTests
{
    [UnityTest]
    [Order(0)]
    public IEnumerator MoveTest()
    {
        yield return new WaitForFixedUpdate();

        Assert.AreEqual(player.transform.position, Vector3.zero);

        playerController.Move(Vector3.right);

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(player.transform.position, Vector3.right);
    }

    [UnityTest]
    [Order(1)]
    public IEnumerator JumpTest()
    {
        Assert.IsTrue(playerController.isGrounded, "Expected player to be isGrounded to be true");
        Assert.AreEqual(playerController.transform.position.y, 0, "Expected y coordinate to be zero for grounded player");
        playerController.Jump();
        yield return new WaitForSeconds(0.1f);
        Assert.IsFalse(playerController.isGrounded, "Expected player to be isGrounded to be false");
        Assert.AreNotEqual(playerController.transform.position.y, 0, "Expected y coordinate to be a non-zero value for grounded player");
    }
}
