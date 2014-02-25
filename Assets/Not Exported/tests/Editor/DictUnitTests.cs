using UnityEngine;
using System.Collections;
using NUnit.Framework;


[TestFixture]
public class DictUnitTests {

    Dict d;
    [SetUp]
    public void Setup()
    {
        d = Dict.CreateInstance<Dict>();
        d.Clear();
    }

    [Test]
    public void ExceptionTest()
    {
        d.KeyType = Dict.Type.INTEGER;
        d.ValueType = Dict.Type.COLOR;

        Assert.Catch(delegate() {
            d.Set(0, "zero");
        });
    }

    [Test]
    public void GetValueTest()
    {
        d.KeyType = Dict.Type.INTEGER;
        d.ValueType = Dict.Type.FLOAT;

        d.Set(0, 1f);
        Assert.AreEqual(1, d.Get<float>(0));
        d.Set(10, 20);
        Assert.AreEqual(20, d.Get<float>(10));

        Assert.Catch(delegate()
        {
            d.Get<float>(5);
        });
    }

    [Test]
    public void TypeTests()
    {
        d.KeyType = Dict.Type.INTEGER;
        d.ValueType = Dict.Type.FLOAT;

        d.Set(2, 10);

        Assert.Catch(delegate()
        {
            d.Get<string>(2);
        });
        Assert.Catch(delegate()
        {
            d.Set("oi", 4f);
        });
        Assert.Catch(delegate()
        {
            d.Set(1, "oi");
        });
        Assert.Catch(delegate()
        {
            d.Keys<Color>();
        });
        Assert.Catch(delegate()
        {
            d.Values<Color>();
        });
        Assert.Catch(delegate()
        {
            d.GetEnumerator<Color, int>();
        });
        Assert.DoesNotThrow(delegate()
        {
            d.GetEnumerator<int, float>();
        });


        d.KeyType = Dict.Type.OBJECT;
        d.Set(Camera.main, 0);
        Assert.DoesNotThrow(delegate()
        {
            d.Keys<Camera>();
        });
        Assert.NotNull(d.Keys<Camera>().GetEnumerator().Current);
    }
}
