using System;
using System.Collections.Generic;
using MinistryPlatform.Translation.Extensions;
using NUnit.Framework;

namespace MinistryPlatform.Translation.Test.Extensions
{
    [TestFixture]
    public class DictionaryExtensionTests
    {
        private Dictionary<string, object> _mockDictionary;

        [SetUp]
        public void SetUp()
        {
            _mockDictionary = new Dictionary<string, object>
            {
                {"Int", 456},
                {"Not_Nullable_Int", null},
                {"Int_Not_A_Number", "abc"},
                {"Nullable_Int", 987},
                {"Null_Nullable_Int", null},
                {"String", "abc"},
                {"Null_String", null},
                {"Date_As_String", "01/01/2007"},
                {"Null_Date_As_String", null},
                {"Invalid_Date_As_String", "NotADate"},
                {"Bool", true},
                {"NullBool", null}
            };
        }

        [Test]
        public void Null_Date_As_String_Success()
        {
            Assert.AreEqual(string.Empty, _mockDictionary.ToDateAsString("Null_Date_As_String"));
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void Null_Date_As_String_Failure()
        {
            Assert.AreEqual(string.Empty, _mockDictionary.ToDateAsString("Null_Date_As_String", true));
        }

        [Test]
        public void StringToBool_Success()
        {
            Assert.AreEqual(true, _mockDictionary.ToBool("Bool"));
        }

        [Test]
        public void NullStringToBool_Success()
        {
            Assert.AreEqual(false, _mockDictionary.ToBool("NullBool"));
        }

        [Test]
        public void StringToInt_Success()
        {
            Assert.AreEqual(456, _mockDictionary.ToInt("Int"));
        }

        [Test]
        public void NullStringToString_Success()
        {
            Assert.AreEqual(null, _mockDictionary.ToString("Null_String"));
        }

        [Test]
        public void StringToNullableInt_Success()
        {
            Assert.AreEqual(987, _mockDictionary.ToNullableInt("Nullable_Int"));
        }

        [Test]
        public void NullStringToNullableInt_Success()
        {
            Assert.AreEqual(null, _mockDictionary.ToNullableInt("Null_Nullable_Int"));
        }

        [Test]
        public void NullStringToIntNotNullable_Success()
        {
            Assert.AreEqual(0, _mockDictionary.ToInt("Not_Nullable_Int", false));
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void NullStringToIntNotNullable_Failure()
        {
            Assert.AreEqual(0, _mockDictionary.ToInt("Not_Nullable_Int", true));
        }

        [Test]
        public void StringToInt_NotANumber()
        {
            Assert.AreEqual(0, _mockDictionary.ToInt("Int_Not_A_Number"));
        }

        [Test]
        [ExpectedException(typeof (FormatException))]
        public void StringToInt_NotANumber_ThrowsError()
        {
            Assert.AreEqual(0, _mockDictionary.ToInt("Int_Not_A_Number", true));
        }

        [Test]
        public void StringToDateString_Success()
        {
            Assert.AreEqual("01/01/2007", _mockDictionary.ToDateAsString("Date_As_String"));
        }

        [Test]
        [ExpectedException(typeof(FormatException))]
        public void StringToNullableDate_InvalidDate_ThrowsError()
        {
            _mockDictionary.ToNullableDate("Invalid_Date_As_String", true);
        }

        [Test]
        public void StringToNullableDate_ValidDate_Success()
        {
            Assert.AreEqual(new DateTime(2007, 1, 1), _mockDictionary.ToNullableDate("Date_As_String"));
        }

        [Test]
        public void StringToNullableDate_NullDate_Success()
        {
            Assert.AreEqual(null, _mockDictionary.ToNullableDate("Null_Date_As_String"));
        }

        [Test]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void StringToNullableDate_ColumnDoesNotExistInDictionarThrowsError()
        {
            _mockDictionary.ToNullableDate("Fake_Column");
        }
    }
}