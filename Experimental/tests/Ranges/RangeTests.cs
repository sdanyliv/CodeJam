using System;
using System.Linq;

using NUnit.Framework;

namespace CodeJam.Ranges
{
    [TestFixture]
    public class RangeTests
    {
        [Test]
        public void Contains()
        {
            var range = Range.Create(0, 2);

            Assert.IsTrue(range.Contains(0));
            Assert.IsTrue(range.Contains(1));
            Assert.IsTrue(range.Contains(2));

			range = Range.Create(0, 2, false);

            Assert.IsFalse(range.Contains(0));
            Assert.IsTrue(range.Contains(1));
            Assert.IsFalse(range.Contains(2));

            Assert.IsFalse(Range.Empty<int>().Contains(10));
        }

        [Test]
        public void Intersects()
        {
            var range1 = Range.Create(0, 3);

            Assert.IsTrue(range1.Intersects(Range.Create(1, 5)));
            Assert.IsTrue(range1.Intersects(Range.Create(1, 3)));
            Assert.IsTrue(range1.Intersects(Range.Create(3, 5)));
            Assert.IsTrue(range1.Intersects(Range.Create(0, 0)));
            Assert.IsFalse(range1.Intersects(Range.Create(-2, -1)));


            range1 = Range.Create(0, 3, false);

            Assert.IsTrue(range1.Intersects(Range.Create(1, 5, false)));
            Assert.IsTrue(range1.Intersects(Range.Create(1, 3, false)));
            Assert.IsFalse(range1.Intersects(Range.Create(3, 5, false)));
            Assert.IsFalse(range1.Intersects(Range.Create(0, 0, false)));
            Assert.IsFalse(range1.Intersects(Range.Create(-2, -1, false)));

            Assert.IsFalse(range1.Intersects(Range.Empty<int>()));
            Assert.IsTrue(Range.StartsWith(0, false).Intersects(Range.StartsWith(-2, false)));
        }

        [Test]
        public void Intersect()
        {
            CheckIntersect(Range.Full<int>(), Range.Full<int>(), Range.Full<int>());
            CheckIntersect(Range.Full<int>(), Range.Empty<int>(), Range.Empty<int>());
            CheckIntersect(Range.Full<int>(), Range.StartsWith(10), Range.StartsWith(10));
            CheckIntersect(Range.Empty<int>(), Range.Empty<int>(), Range.Empty<int>());

            
            CheckIntersect(Range.EndsWith(10, true), Range.Create(0, 10, true, true), Range.Create(0, 10, true));
            CheckIntersect(Range.EndsWith(10, false), Range.Create(0, 10, true, true), Range.Create(0, 10, true, false));

            CheckIntersect(Range.EndsWith(10, false), Range.StartsWith(10, true));
            CheckIntersect(Range.EndsWith(10, false), Range.StartsWith(10, false));

            CheckIntersect(Range.EndsWith(10, true), Range.StartsWith(10, true), Range.Simple(10));

            CheckIntersect(Range.EndsWith(10, true), Range.Create(0, 9, true), Range.Create(0, 9, true));
            CheckIntersect(Range.EndsWith(10, true), Range.Create(0, 9, true, false), Range.Create(0, 9, true, false));

            CheckIntersect(Range.Create(0, 10, true), Range.Create(0, 2, false, false), Range.Create(0, 2, false, false));
        }

        [Test]
        public void Exclude()
        {
            var range1 = Range.Simple("Germany");
            var range2 = Range.Simple("Germany");

            Assert.AreEqual(0, range1.Exclude(range2).Count());
        }

        private void CheckIntersect<TValue>(Range<TValue> range1, Range<TValue> range2, Range<TValue> expected)
            where TValue : IComparable<TValue>
        {
            var intesection = range1.Intersect(range2).ToArray();
            Assert.AreEqual(1, intesection.Length, "Intesection should have only one element");
            Assert.AreEqual(expected, intesection[0]);

            intesection = range2.Intersect(range1).ToArray();
            Assert.AreEqual(1, intesection.Length, "Intesection should have only one element");
            Assert.AreEqual(expected, intesection[0]);
        }

        private void CheckIntersect<TValue>(Range<TValue> range1, Range<TValue> range2)
            where TValue : IComparable<TValue>
        {
            var intesection = range1.Intersect(range2).ToArray();
            Assert.AreEqual(0, intesection.Length, "Intesection should be empty");

            intesection = range2.Intersect(range1).ToArray();
            Assert.AreEqual(0, intesection.Length, "Intesection should be empty");
        }


    }
}