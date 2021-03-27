using UnityEngine;
using NUnit.Framework;
using Utils;


[TestOf(typeof(MathUtils))]
public class MathUtilsTests
{
    //x^2 + 2x + 1 = 0 (-1)
    //x^2 - 2x + 1 = 0 (1)
    //x^2 + 2x - 8 = 0 (2, -4)
    [Test, Sequential]
    public void SolveQudraticEquasions(
        [Values(1, 1, 1)] float a, 
        [Values(2, -2, 2)] float b, 
        [Values(1, 1, -8)] float c, 
        [Values(new float[] { -1 }, new float[] { 1 }, new float[] { 2, -4 })] float[] expectedRoots)
    {
        var roots = MathUtils.SolveQuadraticEquasion(a, b, c);

        Assert.AreEqual(roots.Length, expectedRoots.Length);

        if (expectedRoots.Length == 2)
        {
            foreach (var root in roots)
            {
                Assert.IsTrue(Mathf.Approximately(root, expectedRoots[0]) || Mathf.Approximately(root, expectedRoots[1]));
            }
        }
        else
        {
            Assert.IsTrue(Mathf.Approximately(roots[0], expectedRoots[0]));
        }
    }
}
