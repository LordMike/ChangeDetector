using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace ChangeDetectorGUI
{
    public enum ViewMode
    {
        Files,
        Registry
    }

    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    public partial class SnapshotLayout : UserControl
    {
        public SnapshotLayout()
        {
            InitializeComponent();
        }

        [Category("Behavior"), Description("Specifies the view mode")]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ViewMode ViewMode { get; set; }

        private void btnAdd_Click(object sender, System.EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
                listPaths.Items.Add(folderBrowserDialog1.SelectedPath);
        }

        private void btnRemove_Click(object sender, System.EventArgs e)
        {
            if (listPaths.SelectedIndices.Count >= 1)
            {
                for (int i = listPaths.SelectedIndices.Count - 1; i >= 0; i--)
                {
                    int index = listPaths.SelectedIndices[i];
                    listPaths.Items.RemoveAt(index);
                }
            }
        }

        private void btnStartSnapshot_Click(object sender, System.EventArgs e)
        {
            switch (ViewMode)
            {
                case ViewMode.Files:
                    break;
                case ViewMode.Registry:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
