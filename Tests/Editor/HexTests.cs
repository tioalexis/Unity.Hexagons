using NUnit.Framework;
using UnityEngine;

namespace Cyl.Hexagons.Tests.Editor
{
    public class HexTests
    {
        [Test]
        public void Constructor_CubeCoordinates_ShouldSetProperly()
        {
            var hex = Hex.CreateFromCube(1, -1, 0);
            var hex2 = Hex.CreateFromCube(1, -1);
            Assert.AreEqual(0, hex.Q + hex.R + hex.S);
            Assert.AreEqual(0, hex2.Q + hex2.R + hex2.S);
        }

        [Test]
        public void Constructor_InvalidCubeCoordinates_ShouldThrow()
        {
            Assert.Throws<System.ArgumentException>(() => Hex.CreateFromCube(1, 1, 1));
        }

        [Test]
        public void OffsetConversion_ShouldBeConsistent()
        {
            var original = Hex.CreateFromOffset(2, 3);
            var converted = Hex.CreateFromOffset(original.Col, original.Row);
            Assert.AreEqual(original, converted);
        }

        [Test]
        public void Operators_AdditionSubtractionMultiplication_WorkCorrectly()
        {
            var a = Hex.CreateFromCube(1, -1, 0);
            var b = Hex.CreateFromCube(0, 1, -1);

            var sum = a + b;
            Assert.AreEqual(Hex.CreateFromCube(1, 0, -1), sum);

            var diff = a - b;
            Assert.AreEqual(Hex.CreateFromCube(1, -2, 1), diff);

            var mul = a * 2;
            Assert.AreEqual(Hex.CreateFromCube(2, -2, 0), mul);
        }

        [Test]
        public void RotateLeftRight_ShouldWork()
        {
            var hex = Hex.CreateFromCube(1, -1, 0);
            var left = hex.RotateLeft();
            var right = hex.RotateRight();

            Assert.AreEqual(Hex.CreateFromCube(1, 0, -1), left);
            Assert.AreEqual(Hex.CreateFromCube(0, -1, 1), right);
            Assert.AreEqual(hex, right.RotateLeft()); // Should return to original
            Assert.AreEqual(hex, left.RotateRight()); // Should return to original
        }

        [Test]
        public void GetDistance_ShouldBeCorrect()
        {
            var a = Hex.CreateFromCube(0, 0, 0);
            var b = Hex.CreateFromCube(2, -1, -1);
            Assert.AreEqual(2, a.GetDistance(b));
        }

        [Test]
        public void ImplicitConversion_Vector2Int()
        {
            var hex = Hex.CreateFromOffset(3, 4);
            Vector2Int vec = hex;
            Hex hex2 = vec;
            Assert.AreEqual(hex.Col, vec.x);
            Assert.AreEqual(hex.Row, vec.y);
            Assert.AreEqual(hex, hex2);
        }

        [Test]
        public void EqualsAndHashCode_ShouldWork()
        {
            var a = Hex.CreateFromCube(1, 2, -3);
            var b = Hex.CreateFromCube(1, 2, -3);
            var c = Hex.CreateFromCube(0, 0, 0);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(a.Equals((object)b));
            Assert.IsFalse(a.Equals(c));

            Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }
    }
}
