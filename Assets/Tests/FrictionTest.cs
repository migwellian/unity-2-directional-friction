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

        Assert.AreEqual(roughCube.transform.position.z, smoothCube.transform.position.z, 1e-4f);
        Assert.AreEqual(0, roughCube.transform.rotation.eulerAngles.x, 0.1f);
        Assert.AreEqual(0, smoothCube.transform.rotation.eulerAngles.x, 0.1f);
        float startPosition = roughCube.transform.position.z;
        float startHeight = roughCube.transform.position.y;

        Vector3 force = Vector3.forward * 10;
        for (int frame = 0; frame < 50; frame++)
        {
            roughCube.GetComponent<Rigidbody>().AddForce(force);
            smoothCube.GetComponent<Rigidbody>().AddForce(force);
            yield return new WaitForFixedUpdate();
        }

        Assert.Greater(roughCube.transform.position.z - startPosition, 1);
        Assert.Greater(smoothCube.transform.position.z - roughCube.transform.position.z, 1);
        Assert.AreEqual(startHeight, roughCube.transform.position.y, 1e-4f);
        Assert.AreEqual(startHeight, smoothCube.transform.position.y, 1e-4f);
        Assert.AreEqual(0, roughCube.transform.rotation.eulerAngles.x, 0.1f);
        Assert.AreEqual(0, smoothCube.transform.rotation.eulerAngles.x, 0.1f);
    }
}
