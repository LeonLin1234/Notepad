using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notepad
{
    public partial class Notepad : Form
    {
        string fname = "*.txt";
        string path = "";
        FontDialog fd = new FontDialog();

        public Notepad()
        {
            InitializeComponent();
            rthBox.AutoWordSelection = false;
        }
        private void Notepad_Load(object sender, EventArgs e)
        {
            undoToolStripMenuItem1.Enabled = false;
            undoToolStripMenuItem2.Enabled = false;
            cutToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem1.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem1.Enabled = false;
            pasteToolStripMenuItem.Enabled = false;
            pasteToolStripMenuItem1.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem1.Enabled = false;
            selectAllToolStripMenuItem.Enabled = false;
            selectAllToolStripMenuItem1.Enabled = false;

            if (Clipboard.GetText() != "")
            {
                pasteToolStripMenuItem.Enabled = true;
                pasteToolStripMenuItem1.Enabled = true;
            }

            rthBox.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
            rthBox.WordWrap = wordWrapToolStripMenuItem.Checked;
            if (statusBarToolStripMenuItem.Checked)
                statusStrip1.Visible = true;
            else statusStrip1.Visible = false;
            fd.Font = rthBox.Font;

        }

        private void OpenFile()
        {
            OpenFileDialog dia = new OpenFileDialog();
            dia.Filter = "Text Documents (*.txt)|*.txt;|All Files(*.*)|*.*";
            if (dia.ShowDialog() == DialogResult.OK)
            {
                path = dia.FileName;
                //MessageBox.Show(path);
                rthBox.LoadFile(path);
                fname = Path.GetFileName(path);
                Text = fname + " - Notepad";
                Show();
            }
        }

        private void SaveFile()
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "Text Documents (*.txt)|*.txt|All Files(*.*)|*.*";
            dia.AddExtension = true;
            dia.FileName = fname;
            if (dia.ShowDialog() == DialogResult.OK)
            {
                rthBox.SaveFile(dia.FileName);
                path = dia.FileName;
                fname = Path.GetFileName(path);
                Text = fname + " - Notepad";
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fname = "*.txt";
            path = "";
            if (Text.Contains('*'))
            {
                CustomDia cdia = new CustomDia();
                if (!Text.Contains("Untitled - Notepad"))
                    cdia.lblText.Text = "Do you want to save changes to\n" + path + "?";
                DialogResult result = cdia.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem.PerformClick();
                    Text = "Untitled - Notepad";
                    rthBox.Clear();
                }
                else if (result == DialogResult.No)
                {
                    Text = "Untitled - Notepad";
                    rthBox.Clear();
                }
            }
            else
            {
                Text = "Untitled - Notepad";
                rthBox.Clear();
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Text.Contains("*"))
            {
                CustomDia cdia = new CustomDia();
                DialogResult result = cdia.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem.PerformClick();
                    OpenFile();
                }
                else if (result == DialogResult.No)
                    OpenFile();
            }
            else
                OpenFile();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (Text.Trim('*') != "Untitled - Notepad")
            {
                Text = Text.Trim('*');
                File.WriteAllText(fname, rthBox.Text);
                rthBox.SaveFile(path);
                Show();
            }
            else
                SaveFile();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            dia.AddExtension = true;
            dia.FileName = fname;

            if (dia.ShowDialog() == DialogResult.OK)
            {
                rthBox.SaveFile(dia.FileName);
                path = dia.FileName;
                fname = Path.GetFileName(path);
                Text = fname + " - Notepad";
                Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Text.Contains("*"))
            {
                CustomDia cdia = new CustomDia();
                if (!Text.Contains("Untitled - Notepad"))
                    cdia.lblText.Text = "Do you want to save changes to\n" + path + "?";
                DialogResult result = cdia.ShowDialog();
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem.PerformClick();
                    Close();
                }
                else if (result == DialogResult.No)
                    Close();
            }
            else Close();
        }

        private void Notepad_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Text.Contains("*"))
            {
                CustomDia cdia = new CustomDia();
                if (!Text.Contains("Untitled - Notepad"))
                    cdia.lblText.Text = "Do you want to save changes to\n" + path + "?";
                DialogResult result = cdia.ShowDialog();
                if (result == DialogResult.Yes)
                    saveToolStripMenuItem.PerformClick();
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fd.ShowDialog() == DialogResult.OK)
                rthBox.Font = fd.Font;
        }

        private void undoToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            rthBox.Undo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem1.Enabled = true;
            rthBox.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pasteToolStripMenuItem.Enabled = true;
            pasteToolStripMenuItem1.Enabled = true;
            rthBox.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int st = rthBox.SelectionStart;
            rthBox.Text = rthBox.Text.Insert(st, Clipboard.GetText());
            rthBox.SelectionStart = st + Clipboard.GetText().Length;
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rthBox.SelectedText = "";
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rthBox.SelectAll();
        }

        private void timeDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = DateTime.Now.ToShortTimeString() + " " + DateTime.Now.ToShortDateString();

            if (rthBox.SelectedText != null)
                rthBox.SelectedText = s;
            else
                rthBox.Text = rthBox.Text.Insert(rthBox.SelectionStart, s);
        }

        private void wordWrapToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            rthBox.WordWrap = wordWrapToolStripMenuItem.Checked;
        }

        private void statusBarToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            statusStrip1.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void rthBox_TextChanged(object sender, EventArgs e)
        {
            Text = "*" + Text.Trim('*');
            if (string.IsNullOrEmpty(rthBox.Text) && Text == "*Untitled - Notepad")
                Text = "Untitled - Notepad";
            if (!string.IsNullOrEmpty(rthBox.Text))
            {
                selectAllToolStripMenuItem.Enabled = true;
                selectAllToolStripMenuItem1.Enabled = true;
            }
            else
            {
                selectAllToolStripMenuItem.Enabled = false;
                selectAllToolStripMenuItem1.Enabled = false;
            }
        }

        private void rthBox_FontChanged(object sender, EventArgs e)
        {
            if (!Text.Contains("Untitled - Notepad"))
                Text = Text.Trim('*');
        }

        private void rthBox_SelectionChanged(object sender, EventArgs e)
        {
            undoToolStripMenuItem1.Enabled = true;
            undoToolStripMenuItem2.Enabled = true;
            int pos = rthBox.SelectionStart;
            int line = rthBox.GetLineFromCharIndex(pos) + 1;
            int col = pos - rthBox.GetFirstCharIndexOfCurrentLine() + 1;
            toolStripStatusLabel1.Text = "Ln " + line + ", Col " + col;

            if (rthBox.SelectedText.Length > 0)
            {
                cutToolStripMenuItem.Enabled = true;
                cutToolStripMenuItem1.Enabled = true;
                copyToolStripMenuItem.Enabled = true;
                copyToolStripMenuItem1.Enabled = true;
                deleteToolStripMenuItem.Enabled = true;
                deleteToolStripMenuItem1.Enabled = true;

                    Regex regExp = new Regex("\b(For|Next|If|Then)\b");

                    foreach (Match match in regExp.Matches(rthBox.Text))
                    {
                        rthBox.Select(match.Index, match.Length);
                        rthBox.SelectionColor = Color.Blue;
                    }

               // toolStripStatusLabel1.Text = "Ln " + line + ", Col " + (col + rthBox.SelectedText.Length);
            }
            else
            {
                cutToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem1.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem1.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem1.Enabled = false;
            }
        }
    }
}
