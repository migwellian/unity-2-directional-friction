using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class FrictionTest
{
    [UnitySetUp]
    public IEnumerator LoadScene()
    {
        SceneManager.LoadScene("SampleScene");
        yield return null;
    }

    [UnityTest]
    public IEnumerator DynamicFrictionCoefficientIsUsed()
    {
        var roughCube = GameObject.Find("Rough Cube");
        var smoothCube = GameObject.Find("Smooth Cube");

        float startPosition = roughCube.transform.position.z;
        float startHeight = roughCube.transform.position.y;

        Assert.AreEqual(roughCube.transform.position.z, smoothCube.transform.position.z, 1e-4f, "Both cubes should start at the same z-position");
        Assert.AreEqual(startHeight, roughCube.transform.position.y, 1e-2f, "Rough cube should be on the ground");
        Assert.AreEqual(startHeight, smoothCube.transform.position.y, 1e-2f, "Smooth cube should be on the ground");
        Assert.AreEqual(1, roughCube.transform.forward.z, 0.1f, "Rough cube should be facing forwards");
        Assert.AreEqual(1, smoothCube.transform.forward.z, 0.1f, "smooth cube should be facing forwards");

        Vector3 force = Vector3.forward * 10;
        for (int frame = 0; frame < 2 * 50; frame++)
        {
            roughCube.GetComponent<Rigidbody>().AddForce(force);
            smoothCube.GetComponent<Rigidbody>().AddForce(force);
            Debug.Log("Rough cube position = \t" + roughCube.transform.position.z);
            Debug.Log("Smooth cube position = \t" + smoothCube.transform.position.z);
            yield return new WaitForFixedUpdate();
        }

        Assert.Greater(roughCube.transform.position.z - startPosition, 1, "Both cubes should move");
        Assert.Greater(smoothCube.transform.position.z - roughCube.transform.position.z, 1, "Smooth cube should move further than rough cube");
        Assert.AreEqual(startHeight, roughCube.transform.position.y, 1e-2f, "Rough cube should stay on the ground");
        Assert.AreEqual(startHeight, smoothCube.transform.position.y, 1e-2f, "Smooth cube should stay on the ground");
        Assert.AreEqual(1, roughCube.transform.forward.z, 0.1f, "Rough cube should not roll");
        Assert.AreEqual(1, smoothCube.transform.forward.z, 0.1f, "Smooth cube should not roll");
    }
}
