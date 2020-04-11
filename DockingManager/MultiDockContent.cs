using WeifenLuo.WinFormsUI.Docking;

namespace DockingManager
{
    /// <summary>
    /// Manages multiple instance which is created from one class derived <see cref="DockContent"/>.
    /// </summary>
    public class MultiDockContent : DockContent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDockContent"/> class.
        /// </summary>
        /// <param name = "formId">Instance identifier of one class derived <see cref="DockContent"/>.</param>
        public MultiDockContent(string formId)
        {
            FormId = formId;
            Text = formId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiDockContent"/> class.
        /// This constructor is only used in form designer.
        /// </summary>
        private MultiDockContent()
        {
        }

        /// <summary>
        /// Gets delimiter string.
        /// </summary>
        public static string PersistStringDelimiter => "::";

        /// <summary>
        /// Gets or sets Instance identifier.
        /// </summary>
        public string FormId { get; set; }

        /// <summary>
        /// Get PersisitString of this instance.
        /// </summary>
        /// <returns>PersistString identified by file name.</returns>
        protected override string GetPersistString()
        {
            return base.GetPersistString() + PersistStringDelimiter + FormId;
        }
    }
}