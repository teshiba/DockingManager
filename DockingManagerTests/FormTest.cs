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

    class TestForm : DockContent
    {
        public string PersistString { get; }

        public TestForm()
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