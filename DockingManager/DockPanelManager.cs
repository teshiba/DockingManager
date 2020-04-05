using System;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using WeifenLuo.WinFormsUI.Docking;

namespace DockingManager
{
    /// <summary>
    /// DockPanel manager class.
    /// </summary>
    public class DockPanelManager
    {
        private const string XmlName = "dockpanelconfig.xml";

        private readonly DockPanel dockPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="DockPanelManager"/> class.
        /// </summary>
        /// <param name="parentForm">parent form with DocPanel.</param>
        public DockPanelManager(IDockPanel parentForm)
        {
            if (parentForm is null) {
                throw new ArgumentNullException(nameof(parentForm));
            }

            dockPanel = parentForm.GetDockPanel();
        }

        /// <summary>
        /// Shows given form.
        /// </summary>
        /// <param name="form">DockContent form.</param>
        public void Show(DockContent form)
        {
            if (form is null) {
                throw new ArgumentNullException(nameof(form));
            }

            form.Show(dockPanel);
        }

        /// <summary>
        /// Saves dock contents window state.
        /// </summary>
        public void SaveWindowState()
        {
            dockPanel.SaveAsXml(XmlName);
        }

        /// <summary>
        /// Restores window state from setting file.
        /// </summary>
        public void RestorWindowState()
        {
            if (File.Exists(XmlName)) {
                dockPanel.LoadFromXml(XmlName, DeserializeContent);
            }
        }

        /// <summary>
        /// Persist string is Parsed by delimiter.
        /// </summary>
        /// <param name="persistString">Persist string.</param>
        /// <param name="contentsType">Contents type.</param>
        /// <param name="fileName">file name.</param>
        private static void ParsePersistString(string persistString, out string contentsType, out string fileName)
        {
            var item = Regex.Split(persistString, MultiDockContent.PersistStringDelimiter);
            fileName = string.Empty;

            contentsType = item[0];

            if (item.Length == 2) {
                fileName = item[1];
            }
        }

        private IDockContent DeserializeContent(string persistString)
        {
            ParsePersistString(persistString, out var contentsType, out var formId);

            var type = Assembly.GetEntryAssembly().GetType(contentsType);

            DockContent dockContent;
            if (string.IsNullOrEmpty(formId)) {
                dockContent = (DockContent)Activator.CreateInstance(type);
            } else {
                dockContent = (MultiDockContent)Activator.CreateInstance(type, formId);
            }

            return dockContent;
        }
    }
}