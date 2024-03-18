using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TictactoE : MonoBehaviour
{
    public Button[] buttons;
    public TextMeshProUGUI[] buttonTexts;
    public TextMeshProUGUI endGameText;

    private string[,] board;
    private string currentPlayer;
    private string ai = "O";
    private string human = "X";

    private void Start()
    {
        currentPlayer = ai; // AI starts first
        InitializeBoard();
        AIMove(); // AI makes the first move
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => ButtonClicked(button));
        }
    }

    private void InitializeBoard()
    {
        board = new string[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                board[i, j] = "";
            }
        }
    }

    private void ButtonClicked(Button button)
    {
        if (button.interactable)
        {
            int index = System.Array.IndexOf(buttons, button);
            int row = index / 3;
            int col = index % 3;
            button.interactable = false;
            buttonTexts[index].text = human;
            board[row, col] = human;
            string winner = CheckWinner();
            if (winner != null)
            {
                GameOver(winner + " Wins!");
                return;
            }
            currentPlayer = ai;
            Invoke("AIMove", 0.5f);
        }
    }

    private void AIMove()
    {
        int[] move = BestMove();
        int row = move[0];
        int col = move[1];
        int index = row * 3 + col;
        buttons[index].interactable = false;
        buttonTexts[index].text = ai;
        board[row, col] = ai;
        string winner = CheckWinner();
        if (winner != null)
        {
            GameOver(winner + " Wins!");
            return;
        }
        currentPlayer = human;
    }

    private int[] BestMove()
    {
        int bestScore = int.MinValue;
        int[] move = new int[2];

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == "")
                {
                    board[i, j] = ai;
                    int score = Minimax(board, 0, false);
                    board[i, j] = "";
                    if (score > bestScore)
                    {
                        bestScore = score;
                        move[0] = i;
                        move[1] = j;
                    }
                }
            }
        }
        return move;
    }

    private int Minimax(string[,] board, int depth, bool isMaximizing)
    {
        string result = CheckWinner();
        if (result != null)
        {
            if (result == ai)
                return 10 - depth; // Adjust the score based on depth
            else if (result == human)
                return depth - 10; // Adjust the score based on depth
            else
                return 0;
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = ai;
                        int score = Minimax(board, depth + 1, false);
                        board[i, j] = "";
                        bestScore = Mathf.Max(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[i, j] == "")
                    {
                        board[i, j] = human;
                        int score = Minimax(board, depth + 1, true);
                        board[i, j] = "";
                        bestScore = Mathf.Min(bestScore, score);
                    }
                }
            }
            return bestScore;
        }
    }

    private string CheckWinner()
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] != "" && board[i, 0] == board[i, 1] && board[i, 0] == board[i, 2])
                return board[i, 0]; // Return the symbol of the winner
        }

        // Check columns
        for (int i = 0; i < 3; i++)
        {
            if (board[0, i] != "" && board[0, i] == board[1, i] && board[0, i] == board[2, i])
                return board[0, i]; // Return the symbol of the winner
        }

        // Check diagonals
        if (board[0, 0] != "" && board[0, 0] == board[1, 1] && board[0, 0] == board[2, 2])
            return board[0, 0]; // Return the symbol of the winner
        if (board[0, 2] != "" && board[0, 2] == board[1, 1] && board[0, 2] == board[2, 0])
            return board[0, 2]; // Return the symbol of the winner

        // Check for a tie
        bool isTie = true;
        foreach (var cell in board)
        {
            if (cell == "")
            {
                isTie = false;
                break;
            }
        }
        if (isTie)
            return "tie";

        return null; // No winner yet
    }



    private void GameOver(string result)
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
        endGameText.text = result;
    }
}
