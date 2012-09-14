using System;
using System.Windows.Forms;

namespace ChangeDetectorGUI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void chkHashingAlgorithm_CheckedChanged(object sender, EventArgs e)
        {
            comboHashingAlgorithm.Enabled = chkHashingAlgorithm.Checked;
        }

        private void btnBrowseFSSnapshotOne_Click(object sender, EventArgs e)
        {
            var results = openFileDialog1.ShowDialog();

            if (results == DialogResult.OK)
                txtSnapshotFile1.Text = openFileDialog1.FileName;
        }

        private void btnBrowseFSSnapshotTwo_Click(object sender, EventArgs e)
        {
            var results = openFileDialog1.ShowDialog();

            if (results == DialogResult.OK)
                txtSnapshotFile2.Text = openFileDialog1.FileName;
        }
    }
}
