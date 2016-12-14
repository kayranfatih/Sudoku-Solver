using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SudokuSolver
{
    public partial class SSolver : Form
    {
        #region defineParam
        NumericTextBox[,] Cells = new NumericTextBox[9, 9];
        string[,] strArray = new string[9, 9];
        bool control = true;
        bool solver = false;
        #endregion
        public SSolver()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            #region btnClear
            Button btnClear = new Button();
            btnClear.Location = new Point(130, 220);
            btnClear.AutoSize = true;
            btnClear.Text = "Clear";
            btnClear.Click += btnClear_Click;
            this.Height = 350;
            this.Width = 350;
            this.Controls.Add(btnClear);
            #endregion
            #region btnSolve
            Button btnSolve = new Button();
            btnSolve.Location = new Point(130,245);
            btnSolve.AutoSize = true;
            btnSolve.Text = "Solve";
            btnSolve.Click += btnSolve_Click;
            this.Height = 350;
            this.Width = 350;
            this.Controls.Add(btnSolve);
            #endregion
            #region addNumericTextBox
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                    Cells[j, i] = new NumericTextBox();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    int xLong = 0, yLong = 0;
                    if (i > 2)
                        xLong = 5;
                    if (i > 5)
                        xLong = 10;
                    if (j > 2)
                        yLong = 5;
                    if (j > 5)
                        yLong = 10;

                    Cells[i, j].Location = new Point(70 + i * 20 + xLong, yLong + j * 20 + 10);
                    Cells[i, j].TextChanged += new EventHandler(Cells_TextChanged);
                    this.Controls.Add(Cells[j, i]);
                }
            }
            #endregion
        }
        #region btnClickMethods
        private void btnClear_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 9; i++)
                for (int j = 0; j < 9; j++)
                {
                    Cells[i, j].Text = "";
                    strArray[i, j] = "";
                }
        }
        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (control)
            {
                solver = true;
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        strArray[i, j] = Cells[i, j].Text;
                SudokuSolve(0, 0);
                for (int i = 0; i < 9; i++)
                    for (int j = 0; j < 9; j++)
                        Cells[i, j].Text = strArray[i,j];
                solver = false;
            }
            else
            {
                MessageBox.Show("Sudoku Can't Be Solved Because Of Collison(s).");
            }
        }
        #endregion

        #region SudokuControls
        public bool RowCheck(int x, int y)
        {
            for (int i = 0; i < 9; i++)
            {
                if (strArray[i, y] != "")
                {
                    if (i != x)
                    {
                        if (strArray[i, y] == strArray[x, y])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        public bool ColumnCheck(int x, int y)
        {
            for (int j = 0; j < 9; j++)
            {
                if (strArray[x, j] != "")
                {
                    if (j != y)
                    {
                        if (strArray[x, j] == strArray[x, y])
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        public bool BoxCheck(int x, int y)
        {
            int box_X = (x/3)*3;
            int box_Y = (y/3)*3;

            for (int i = box_X; i < box_X + 3; i++)
            {
                for (int j = box_Y; j < box_Y + 3; j++)
                {
                    if (strArray[i,j] != "")
                    {
                        if (i != x && j != y)
                        {
                            if (strArray[i, j] == strArray[x, y])
                                return false;
                        }
                    }
                    
                }
                
            }
            return true;
        }
        public bool SudokuSolve(int x, int y)
        {
            if (x == 9)
            {
                x = 0;
                y++;
                if (y == 9)
                    return true;
            }
            if (strArray[x, y] != "")
                return SudokuSolve(x + 1, y);
            for (int i = 1; i <= 9; i++)
            {
                if (AllCheck(x, y, i))
                {
                    strArray[x, y] = i.ToString();
                    if (SudokuSolve(x + 1, y))
                        return true;
                }
            }
            strArray[x, y] = "";
            return false;
        }
        public bool AllCheck(int x, int y, int val)
        {
            for (int i = 0; i < 9; ++i)
                if (val.ToString() == strArray[i, y])
                    return false;
            for (int i = 0; i < 9; ++i)
                if (val.ToString() == strArray[x, i])
                    return false;
            int box_X = (x / 3) * 3;
            int box_Y = (y / 3) * 3;
            for (int i = 0; i < 3; ++i)
                for (int j = 0; j < 3; ++j)
                    if (val.ToString() == strArray[box_X + i, box_Y + j])
                        return false;
            return true;
        }
        private void Cells_TextChanged(object sender, EventArgs e)
        {
            if (solver)
                return;
            control = true;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    strArray[i, j] = Cells[i, j].Text;
                    Cells[i, j].ForeColor = Color.Black;
                }
            }

            for (int j = 0; j < 9; j++)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (RowCheck(i,j))
                    {
                        if (ColumnCheck(i,j))
                        {
                            if (!BoxCheck(i, j))
                            {
                                Cells[i, j].ForeColor = Color.Red;
                                control = false;
                            }
                        }
                        else
                        {
                            Cells[i, j].ForeColor = Color.Red;
                            control = false;
                        }
                        
                    }
                    else
                    {
                        Cells[i, j].ForeColor = Color.Red;
                        control = false;
                    }
                         
                }
            }
        }
        #endregion
       

    }
}

