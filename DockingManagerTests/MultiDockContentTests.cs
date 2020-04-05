using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DockingManager.Tests
{
    [TestClass]
    public class MultiDockContentTests
    {
        [TestMethod]
        public void MultiDockContentTest()
        {
            // Arrange
            var expectedValue = "formId-123";
            var testClass = new TestMultiForm(expectedValue);

            // Act
            var actualValue = testClass.FormId;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void MultiDockContentTestGetPersistString()
        {
            // Arrange
            var formId = "formId-123";
            var expectedValue = "DockingManager.Tests.TestMultiForm::" + formId;
            var testClass = new TestMultiForm(formId);

            // Act
            var actualValue = testClass.PersistString;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

    }
}