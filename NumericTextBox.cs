using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver
{
    class NumericTextBox : TextBox
    {
        private bool nonNumberEntered = false;
        public NumericTextBox ()
        {
            this.Width = 20;
            this.Height = 20;
            this.MaxLength = 1;
            this.TextAlign = HorizontalAlignment.Center;
            this.Text = "";
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (nonNumberEntered == true)
            {
                e.Handled = true;
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            nonNumberEntered = false;
            if (e.Shift == true || e.Alt == true)
            {
                nonNumberEntered = true;
                return;
            }
            if (e.KeyCode < Keys.D1 || e.KeyCode > Keys.D9)
            {
                if (e.KeyCode < Keys.NumPad1 || e.KeyCode > Keys.NumPad9)
                {
                    if (e.KeyCode != Keys.Back)
                    {
                        nonNumberEntered = true;
                    }
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);

        }

    }
}
