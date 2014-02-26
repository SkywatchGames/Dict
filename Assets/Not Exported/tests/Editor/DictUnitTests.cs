using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;


[TestFixture]
public class DictUnitTests {

    Dict d;
    object[] testObjs;

    [SetUp]
    public void Setup()
    {
        d = Dict.CreateInstance<Dict>();
        d.Clear();

        testObjs = new object[] { 1, 2.3f, "abc", Color.white, Camera.main };
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
        Assert.Catch(delegate()
        {
            d.Keys<Camera>();
        });
        Assert.AreNotEqual(0, d.KeyCount);

        IEnumerator<Object> enumerator = d.Keys<Object>().GetEnumerator();
        enumerator.MoveNext();
        Assert.NotNull(enumerator.Current as Camera);
    }

    [Test]
    public void CountTest()
    {
        d.KeyType = Dict.Type.FLOAT;
        d.ValueType = Dict.Type.STRING;

        d.Set(1, "abc");
        d.Set(2.3f, "def");

        

        Assert.AreEqual(2, d.KeyCount);
    }

    [Test]
    public void RemoveTest()
    {
        d.KeyType = Dict.Type.INTEGER;
        d.ValueType = Dict.Type.FLOAT;
        d.Clear();

        for (int i = 0; i < 10; i++)
            d.Set(i, 2 * i);

        Assert.True(d.Remove(2));
        Assert.False(d.Remove(2));
        Assert.False(d.Remove(49));
        Assert.AreEqual(9, d.KeyCount);
    }

    [Test]
    public void IntTypeTest()
    {
        d.KeyType = Dict.Type.INTEGER;
        d.ValueType = Dict.Type.INTEGER;
        
        d.Clear();
        TestKeyTypes(0, 23);
        d.Clear();
        TestValueTypes(0, 23);
    }

    [Test]
    public void FloatTypeTest()
    {
        d.KeyType = Dict.Type.FLOAT;
        d.ValueType = Dict.Type.FLOAT;
        object [] testObjs = new object[] { 1, 2.3f, "abc", Color.white, Camera.main };

        //testar keys
        d.Clear();
        for (int i = 0; i < testObjs.Length; i++)
        {
            object key = testObjs[i];
            if (i == 0 || i == 1)
                Assert.DoesNotThrow(delegate()
                {
                    d.Set(key, 276.0102f);
                });
            else
                Assert.Catch(delegate()
                {
                    d.Set(key, 276.0102f);
                });
        }

        //testar values
        d.Clear();
        for (int i = 0; i < testObjs.Length; i++)
        {
            object value = testObjs[i];
            if (i == 0 || i == 1)
                Assert.DoesNotThrow(delegate()
                {
                    d.Set(276.0102f, value);
                });
            else
                Assert.Catch(delegate()
                {
                    d.Set(276.0102f, value);
                });
        }
    }

    [Test]
    public void StringTypeTest()
    {
        d.KeyType = Dict.Type.STRING;
        d.ValueType = Dict.Type.STRING;

        d.Clear();
        TestKeyTypes(2, "abc");
        d.Clear();
        TestValueTypes(2, "def");
    }

    [Test]
    public void ColorTypeTest()
    {
        d.KeyType = Dict.Type.COLOR;
        d.ValueType = Dict.Type.COLOR;

        d.Clear();
        TestKeyTypes(3, Color.white);
        d.Clear();
        TestValueTypes(3, Color.black);
    }

    [Test]
    public void ObjectTypeTest()
    {
        d.KeyType = Dict.Type.OBJECT;
        d.ValueType = Dict.Type.OBJECT;

        d.Clear();
        TestKeyTypes(4, Camera.main);
        d.Clear();
        TestValueTypes(4, Camera.main);
    }

    [Test]
    public void NullTest()
    {
        d.KeyType = Dict.Type.OBJECT;
        d.ValueType = Dict.Type.OBJECT;

        d.Clear();
        Assert.DoesNotThrow(delegate()
        {
            d.Set(Camera.main, null);
        });
        Assert.AreEqual(1, d.KeyCount);
        Assert.AreEqual(null, d.Get<Object>(Camera.main));
    }

    private void TestKeyTypes(int successfulKeyIndex, object value)
    {
        for (int i = 0; i < testObjs.Length; i++)
        {
            object key = testObjs[i];
            if (i == successfulKeyIndex)
                Assert.DoesNotThrow(delegate()
                {
                    d.Set(key, value);
                });
            else
                Assert.Catch(delegate()
                {
                    d.Set(key, value);
                });
        }
    }

    private void TestValueTypes(int successfulKeyIndex, object key)
    {
        for (int i = 0; i < testObjs.Length; i++)
        {
            object value = testObjs[i];
            if (i == successfulKeyIndex)
                Assert.DoesNotThrow(delegate()
                {
                    d.Set(key, value);
                });
            else
                Assert.Catch(delegate()
                {
                    d.Set(key, value);
                });
        }
    }
}
