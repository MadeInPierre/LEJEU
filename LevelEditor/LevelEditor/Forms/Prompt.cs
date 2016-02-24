using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LevelEditor
{
    public static class Prompt
    {
        public static string ShowDialog(string caption)
        {
            Form prompt = new Form();
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.Text = caption;
            prompt.ClientSize = new System.Drawing.Size(399, 54);

            TextBox textBox = new TextBox();
            textBox.Location = new System.Drawing.Point(12, 18);
            textBox.Name = "textBox1";
            textBox.Size = new System.Drawing.Size(244, 40);
            textBox.TabIndex = 1;

            Button confirmation = new Button();
            confirmation.Location = new System.Drawing.Point(262, 12);
            confirmation.Name = "Done";
            confirmation.Size = new System.Drawing.Size(126, 28);
            confirmation.TabIndex = 0;
            confirmation.Text = "Done";

            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;
            prompt.TopMost = true;
            prompt.ShowDialog();
            return textBox.Text;
        }
    }
}