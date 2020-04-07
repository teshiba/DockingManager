using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using WeifenLuo.WinFormsUI.Docking;
using System.Reflection;
using System.IO;
using System.Collections.ObjectModel;

namespace DockingManager.Tests
{
    [TestClass]
    public class DockPanelManagerTests
    {

        [TestMethod]
        public void DockPanelManagerTest()
        {
            // Arrange
            var mdiForm = new TestMDIForm();
            var testClass = new DockPanelManager(mdiForm);
            var expectedValue = mdiForm.TestDockPanel;

            // Act
            var bindingFlag = BindingFlags.NonPublic | BindingFlags.Instance;
            var dockPlanel = testClass.GetType().GetField("dockPanel", bindingFlag);
            var actualValue = (DockPanel)(dockPlanel.GetValue(testClass));

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void DockPanelManagerTestExceptionNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _ = new DockPanelManager(null);
            });
        }

        [TestMethod]
        public void ShowTest()
        {
            // Arrange
            var form = new TestMultiForm("formId-123");
            var testClass = new DockPanelManager(new TestMDIForm());
            var expectedValue = DockState.Document;

            // Act
            testClass.Show(form);
            var actualValue = form.VisibleState;


            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void ShowTestNull()
        {
            var testClass = new DockPanelManager(new TestMDIForm());
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                testClass.Show(null);
            });
        }

        [TestMethod]
        public void SaveWindowStateTest()
        {
            // Arrange
            var form = new TestMultiForm("formId-123");
            var testClass = new DockPanelManager(new TestMDIForm());
            testClass.Show(form);
            testClass.SaveWindowState();
            var bindingFlag = BindingFlags.NonPublic | BindingFlags.Static;
            var xmlName = testClass.GetType().GetField("XmlName", bindingFlag);

            // Act
            var actualValue = File.Exists((string)(xmlName.GetValue(testClass)));

            // Assert
            Assert.IsTrue(actualValue);
        }

        [TestMethod]
        public void RestorWindowStateTest()
        {
            // Arrange
            using (var mdiFormBefore = new TestMDIForm()) {
                var formBefore1 = new TestMultiForm("formId-1");
                var formBefore2 = new TestForm1();
                var testClassBefore = new DockPanelManager(mdiFormBefore);
                testClassBefore.Show(formBefore1);
                testClassBefore.Show(formBefore2);
                testClassBefore.SaveWindowState();
            }

            var expectedValue = DockState.Document;

            var mdiForm = new TestMDIForm();
            var testClass = new DockPanelManager(mdiForm);

            SetEntryAssembly();

            // Act
            testClass.RestorWindowState();
            var form = (TestMultiForm)mdiForm.GetDockPanel().Contents[0];
            var actualValue = form.VisibleState;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void RestorWindowStateTestWithArgs()
        {
            // Arrange
            using (var mdiFormBefore = new TestMDIForm()) {
                var testClassBefore = new DockPanelManager(mdiFormBefore);
                testClassBefore.Show(new TestMultiForm("formId-1"));
                testClassBefore.Show(new TestMultiForm("formId-2"));
                testClassBefore.Show(new TestMultiForm("formId-3"));
                testClassBefore.Show(new TestForm1());
                testClassBefore.Show(new TestForm2());
                testClassBefore.SaveWindowState();
            }

            var mdiForm = new TestMDIForm();
            var testClass = new DockPanelManager(mdiForm);

            SetEntryAssembly();

            // Act
            var form1 = new TestForm1();
            var form2 = new TestForm2();
            var multiDockContents = testClass.RestorWindowState(form1, form2);

            // Assert
            Assert.AreEqual(DockState.Document, form1.VisibleState);
            Assert.AreEqual(DockState.Document, form2.VisibleState);
            Assert.AreEqual(multiDockContents[0].FormId, "formId-1");
            Assert.AreEqual(multiDockContents[1].FormId, "formId-2");
            Assert.AreEqual(multiDockContents[2].FormId, "formId-3");
            Assert.AreEqual(multiDockContents[0].GetType(), typeof(TestMultiForm));
            Assert.AreEqual(multiDockContents[1].GetType(), typeof(TestMultiForm));
            Assert.AreEqual(multiDockContents[2].GetType(), typeof(TestMultiForm));
        }


        [TestMethod]
        public void RestorWindowStateTestNoFile()
        {
            // Arrange
            var expectedValue = 0;

            var mdiForm = new TestMDIForm();
            var testClass = new DockPanelManager(mdiForm);

            var bindingFlag = BindingFlags.NonPublic | BindingFlags.Static;
            var xmlName = testClass.GetType().GetField("XmlName", bindingFlag);
            var configFile = (string)(xmlName.GetValue(testClass));

            if (File.Exists(configFile)) {
                File.Delete(configFile);
            }

            SetEntryAssembly();

            // Act
            testClass.RestorWindowState();
            var contents = mdiForm.GetDockPanel().Contents;
            var actualValue = contents.Count;

            // Assert
            Assert.AreEqual(expectedValue, actualValue);
        }

        public static void SetEntryAssembly()
        {
            Assembly assembly = Assembly.GetCallingAssembly();
            AppDomainManager manager = new AppDomainManager();
            var flag = BindingFlags.Instance | BindingFlags.NonPublic;

            FieldInfo entryAssemblyfield = manager.GetType().GetField("m_entryAssembly", flag);
            entryAssemblyfield.SetValue(manager, assembly);

            AppDomain domain = AppDomain.CurrentDomain;
            FieldInfo domainManagerField = domain.GetType().GetField("_domainManager", flag);
            domainManagerField.SetValue(domain, manager);
        }
    }
}