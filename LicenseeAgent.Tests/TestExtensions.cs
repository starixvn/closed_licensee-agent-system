namespace LicenseeAgent.Tests
{
    using System;
    using NUnit.Framework;

    /// <summary>
    /// TestExtensions class.
    /// </summary>
    public static class TestExtensions
    {
        public static void ShouldBeNull(this object obj)
        {
            Assert.IsNull(obj);
        }

        public static T ShouldBeNull<T>(this T obj)
        {
            Assert.IsNull(obj);
            return obj;
        }

        public static T ShouldBeNull<T>(this T obj, string message)
        {
            Assert.IsNull(obj, message);
            return obj;
        }

        public static void ShouldBeNotNull(this object obj)
        {
            Assert.IsNotNull(obj);
        }

        public static T ShouldBeNotNull<T>(this T obj)
        {
            Assert.IsNotNull(obj);
            return obj;
        }

        public static T ShouldBeNotNull<T>(this T obj, string message)
        {
            Assert.IsNotNull(obj, message);
            return obj;
        }

        public static T ShouldEqual<T>(this T actual, object expected)
        {
            Assert.AreEqual(expected, actual);
            return actual;
        }

        public static void ShouldEqual(this object actual, object expected, string message)
        {
            Assert.AreEqual(expected, actual);
        }

        public static void ShouldEqualIgnoreCase(this string actual, string expected)
        {
            if (!string.Equals(actual, expected, StringComparison.InvariantCultureIgnoreCase))
            {
                var message = string.Format("Expected {0} but was {1}", expected, actual);
                throw new AssertionException(message);
            }
        }

        public static void ShouldBe<T>(this object actual)
        {
            Assert.IsInstanceOf<T>(actual);
        }

        public static void ShouldBeTrue(this bool source)
        {
            Assert.IsTrue(source);
        }

        public static void ShouldBeFalse(this bool source)
        {
            Assert.IsFalse(source);
        }

        public static void ShouldBeTheSameAs(this object actual, object expected)
        {
            Assert.AreSame(expected, actual);
        }

        public static void ShouldBeNotBeTheSameAs(this object actual, object expected)
        {
            Assert.AreNotSame(expected, actual);
        }

        public static T CastTo<T>(this object source)
        {
            return (T)source;
        }

        public static Exception ShouldBeThrownBy(this Type exceptionType, TestDelegate testDelegate)
        {
            return Assert.Throws(exceptionType, testDelegate);
        }
    }
}