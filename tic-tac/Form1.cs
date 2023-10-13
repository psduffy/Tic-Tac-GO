using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tic_tac
{
    public partial class Form1 : Form
    {
        public enum CellState
        {
            Empty,
            BlackStone,
            WhiteStone
        }


        // Class Variables

        private CellState[,] board;

        private int gridSize = 9;  // Making this a class-level variable
        private int cellSize;  // Making this a class-level variable too

        private int hoveredRow = -1;
        private int hoveredCol = -1;


        public Form1()
        {
            InitializeComponent();

            cellSize = pictureBox1.Width / gridSize;  // Initializing cellSize here

            board = new CellState[gridSize, gridSize];
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    board[i, j] = CellState.Empty;
                }
            }

            pictureBox1.MouseMove += PictureBox1_MouseMove;
            pictureBox1.MouseClick += PictureBox1_MouseClick;

        }

        private Random random = new Random();  // Create a random number generator.


        // Computer Move - method definition
        private void MakeComputerMove()
        {
            // Get a list of all empty spots on the board.
            List<Point> emptySpots = new List<Point>();
            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    if (board[i, j] == CellState.Empty)
                    {
                        emptySpots.Add(new Point(i, j));
                    }
                }
            }

            // If there are no empty spots, just return.
            if (emptySpots.Count == 0) return;

            // Randomly select one.
            Point selectedSpot = emptySpots[random.Next(emptySpots.Count)];

            // Place the computer's stone on that spot.
            board[selectedSpot.X, selectedSpot.Y] = CellState.WhiteStone;

            // Refresh the board.
            pictureBox1.Invalidate();
        }



        private void PictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            hoveredRow = (e.Y + cellSize / 2) / cellSize;
            hoveredCol = (e.X + cellSize / 2) / cellSize;

            if (hoveredRow < 0 || hoveredRow >= gridSize || hoveredCol < 0 || hoveredCol >= gridSize || board[hoveredCol, hoveredRow] != CellState.Empty)
            {
                hoveredRow = -1;
                hoveredCol = -1;
            }

            pictureBox1.Invalidate();  // Redraw the board
        }



        // Mouse click event - method definition
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int row = (e.Y + cellSize / 2) / cellSize;
            int col = (e.X + cellSize / 2) / cellSize;

            if (row >= 0 && row < gridSize && col >= 0 && col < gridSize && board[col, row] == CellState.Empty)
            {
                board[col, row] = CellState.BlackStone;
                pictureBox1.Invalidate();
            }

            // After the player makes a move, let the computer make its move.
            MakeComputerMove();
 }




        // Make grid - method definition
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias; // Enable anti-aliasing

            for (int i = 0; i < gridSize; i++)
            {
                for (int j = 0; j < gridSize; j++)
                {
                    e.Graphics.DrawRectangle(Pens.Black, i * cellSize, j * cellSize, cellSize, cellSize);

                    // Check the state of the cell and draw the appropriate stone
                    switch (board[i, j])
                    {
                        case CellState.BlackStone:
                            e.Graphics.FillEllipse(Brushes.Black, (i * cellSize) - (cellSize / 2), (j * cellSize) - (cellSize / 2), cellSize, cellSize);
                            break;
                        case CellState.WhiteStone:
                            e.Graphics.FillEllipse(Brushes.White, (i * cellSize) - (cellSize / 2), (j * cellSize) - (cellSize / 2), cellSize, cellSize);
                            break;
                    }

                    // Drawing the dotted circle for hovered cell
                    if (i == hoveredCol && j == hoveredRow)
                    {
                        Pen dottedPen = new Pen(Color.Black) { DashStyle = System.Drawing.Drawing2D.DashStyle.Dot };
                        e.Graphics.DrawEllipse(dottedPen, (i * cellSize) - (cellSize / 2), (j * cellSize) - (cellSize / 2), cellSize, cellSize);
                    }
                }
            }
        }
    }
}