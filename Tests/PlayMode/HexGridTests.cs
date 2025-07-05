using NUnit.Framework;
using UnityEngine;

namespace Cyl.Hexagons.Tests.PlayMode
{
    public class MockHexGridElement : IHexGridElement
    {
        public Hex GridPosition { get; set; }
        public void OnAddedToGrid(HexGrid<IHexGridElement> grid) { }
    }

    public class TestHexGrid : HexGrid<IHexGridElement>
    {
        private readonly IHexGridElement[] _elements;
        public override IHexGridElement[] Elements => _elements;

        public override int Width { get; } = 5;
        public override int Height { get; } = 5;
        public override float CellUnitScale { get; } = 1f;

        public TestHexGrid()
        {
            _elements = new IHexGridElement[Width * Height];
        }
    }

    public class HexGridTests
    {
        private TestHexGrid _grid;

        [SetUp]
        public void Setup()
        {
            var go = new GameObject();
            _grid = go.AddComponent<TestHexGrid>();
        }

        [Test]
        public void IsValidPosition_Works()
        {
            Assert.IsTrue(_grid.IsValidPosition(0, 0));
            Assert.IsFalse(_grid.IsValidPosition(-1, 0));
            Assert.IsFalse(_grid.IsValidPosition(5, 5));
        }

        [Test]
        public void AddAndRemoveElement_Works()
        {
            var element = new MockHexGridElement();
            var added = _grid.AddElement(element, 2, 2);
            Assert.IsTrue(added);
            Assert.IsNotNull(_grid.GetElement(2, 2));

            var removed = _grid.RemoveElement(new Hex(2, 2));
            Assert.IsTrue(removed);
            Assert.IsNull(_grid.GetElement(2, 2));
        }

        [Test]
        public void CountNeighbors_ShouldReturnCorrectCount()
        {
            var center = new MockHexGridElement();
            var neighbor = new MockHexGridElement();

            _grid.AddElement(center, 2, 2);
            _grid.AddElement(neighbor, 2, 3);

            var count = _grid.CountNeighbors(new Hex(2, 2));
            Assert.GreaterOrEqual(count, 1);
        }

        [Test]
        public void CalculateGridSize_ReturnsNonZero()
        {
            var size = _grid.CalculateGridSize();
            Assert.Greater(size.x, 0);
            Assert.Greater(size.y, 0);
        }

        [Test]
        public void GetWorldPosition_ReturnsVector()
        {
            var pos = _grid.GetWorldPosition(new Hex(1, 1));
            Assert.IsInstanceOf<Vector3>(pos);
        }

        [Test]
        public void FindNearestUnoccupiedGridPosition_Works()
        {
            var found = _grid.FindNearestUnoccupiedGridPosition(Vector3.zero, out var hex);
            Assert.IsTrue(found);
            Assert.IsTrue(_grid.IsValidPosition(hex));
        }
        
        [Test]
        public void FindBottomMostOccupiedRow_WorksCorrectly()
        {
            // No elements yet: should return -1
            Assert.AreEqual(-1, _grid.FindBottomMostOccupiedRow());

            // Add one element at row 0 (bottom)
            var elementBottom = new MockHexGridElement();
            _grid.AddElement(elementBottom, 1, 0);

            Assert.AreEqual(0, _grid.FindBottomMostOccupiedRow());

            // Add another element higher up
            var elementTop = new MockHexGridElement();
            _grid.AddElement(elementTop, 2, 4);

            // Bottom-most should still be 0
            Assert.AreEqual(0, _grid.FindBottomMostOccupiedRow());
        }

        [Test]
        public void FindTopMostOccupiedRow_WorksCorrectly()
        {
            // No elements yet: should return -1
            Assert.AreEqual(-1, _grid.FindTopMostOccupiedRow());

            // Add one element at row 0 (bottom)
            var elementBottom = new MockHexGridElement();
            _grid.AddElement(elementBottom, 1, 0);

            Assert.AreEqual(0, _grid.FindTopMostOccupiedRow());

            // Add another element higher up
            var elementTop = new MockHexGridElement();
            _grid.AddElement(elementTop, 2, 4);

            // Top-most should now be 4
            Assert.AreEqual(4, _grid.FindTopMostOccupiedRow());
        }

    }
}
