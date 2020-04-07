using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace DockingManager.Tests
{
    class TestMultiForm : MultiDockContent
    {
        public string PersistString { get; }

        public TestMultiForm(string formId) : base(formId)
        {
            PersistString = GetPersistString();
        }
    }

    class TestForm1 : DockContent
    {
        public string PersistString { get; }

        public TestForm1()
        {
            PersistString = GetPersistString();
        }
    }

    class TestForm2 : DockContent
    {
        public string PersistString { get; }

        public TestForm2()
        {
            PersistString = GetPersistString();
        }
    }

    class TestMDIForm : Form, IDockPanel
    {
        public DockPanel TestDockPanel { get; }

        public TestMDIForm()
        {
            TestDockPanel = new DockPanel {
                Dock = DockStyle.Fill,
                DocumentStyle = DocumentStyle.DockingWindow,
                Name = "TestDockPanel",
            };
            Controls.Add(TestDockPanel);
        }

        public DockPanel GetDockPanel()
        {
            return TestDockPanel;
        }
    }
}