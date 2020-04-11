using System;
using System.Text;
using System.IO;

class MainClass {

  public static void PrintBoard() { //creates and then prints a completed minesweeper board
    int numTests = Convert.ToInt32(Console.ReadLine());
    for (int t = 0; t < numTests; t++){ //while there are still cases to print
      string[] nums = Console.ReadLine().Split(' '); //starts creating array, has 3 diff array values, r, c, m
      int numRows = Convert.ToInt32(nums[0]); //slot 0 is r
      int numCols = Convert.ToInt32(nums[1]); //slot 1 is c
      int numMines = Convert.ToInt32(nums[2]); //slot 2 is m
      // TODO: create the board
      String [,] board = new String[numRows,numCols];
      for (int r = 0; r <numRows; r++)
        for (int c = 0; c <numCols; c++)
          board[r,c] = "0";
      for (int m = 0; m < numMines; m++){
        string[] coords = Console.ReadLine().Split(' '); //this is taking the location of the mines
        int row = Convert.ToInt32(coords[0]); //this gets the row of mine
        int col = Convert.ToInt32(coords[1]); //this gets the col of mine
        // TODO: add mine at row/col to the board
        board[row,col] = "M";
      }
      //TODO: make the values around M the number they should be
      for(int r = 0; r<numRows; r++)
        for (int c = 0; c<numCols; c++){ //to do that, check if any value around the array is M, if so, add 1
          int count = 0;
          if (!(board[r,c].Equals("M"))){ //if this slot is not a mine
            for (int lr = -1; lr<2; lr++) //lr means localRow compared to the current row and col
              for (int lc = -1; lc<2; lc++) 
                try{ //look at all values around, if it doesnt work (out of bounds or whatever), try the next value
                  if (board[(r+lr),(c+lc)] == "M") //if one of the slots around this specific piece is "M"
                    count++; 
                 }catch(Exception p){} //end of lr lc call

                  //btw I know this is def not how I should do the board call, its basically just a try catch designed to ignore all of my problems and failures as a cs student but I love it because it's so friggin effective and try catches are unkowingly overpowered so I'm sticking with it

                  //I also like it because Jaime said that he had one of the *very few* cases where you can ignore so I wanted to make another one

            board[r,c] = count.ToString(); //set it equal to the amount of mines adjacent to the r,c
        } //end of "if this slot is not a mine" call
      } //end of a single r,c call
      // TODO: print the board created above
      printBoard(board); //private method below
      } 
      
  }

  public static void PlayGame() {
      //READ: for notes on how this code works, look at PrintBoard()
      string[] nums = Console.ReadLine().Split(' '); 
      int numRows = Convert.ToInt32(nums[0]);
      int numCols = Convert.ToInt32(nums[1]); 
      int numMines = Convert.ToInt32(nums[2]);
      String [,] board = new String[numRows,numCols]; //final board (unseen by player)
      for (int r = 0; r <numRows; r++)
        for (int c = 0; c <numCols; c++)
          board[r,c] = "0";
      for (int m = 0; m < numMines; m++){
        string[] coords = Console.ReadLine().Split(' ');
        int row = Convert.ToInt32(coords[0]); 
        int col = Convert.ToInt32(coords[1]);
        board[row,col] = "M";
      }
      for(int r = 0; r<numRows; r++)
        for (int c = 0; c<numCols; c++){
          int count = 0;
          if (!(board[r,c].Equals("M"))){
            for (int lr = -1; lr<2; lr++) 
              for (int lc = -1; lc<2; lc++) 
                try{ 
                  if (board[(r+lr),(c+lc)] == "M")
                    count++; 
                 }catch(Exception p){}
            board[r,c] = count.ToString(); 
        } 
      } 

      //creating player board
      string[,] playerBoard = new String[numRows,numCols]; //board that player sees
      for (int r = 0; r <numRows; r++)
        for (int c = 0; c <numCols; c++)
          playerBoard[r,c] = "-";
      printBoard(playerBoard); //private method below used to print out boards
      
      //start of inputs into players board
      int inputTimes = Convert.ToInt32(Console.ReadLine()); //how many times will you make moves
      for (int inputs = 0; inputs < inputTimes; inputs++){
        string[] call = Console.ReadLine().Split(' '); //gets the 3 pieces of data split up
        if (call[0].Equals("R")){ //if they are looking to reveal
          Console.WriteLine("{0} {1} {2}", call[0], call[1], call[2]); 
          if (board[Int32.Parse(call[1]),Int32.Parse(call[2])].Equals("M")){ //if the revealed slot is a mine
            Console.WriteLine("GAME OVER"); //game over
            break; //end game
          }else{ //if the revealed slot is NOT a mine
            playerBoard = RecurseBoard(board, playerBoard, Int32.Parse(call[1]), Int32.Parse(call[2])); //private method below
            printBoard(playerBoard);
          }
          //end of call R
        }else{ //if they are not looking to reveal, then assume they want to mark a mine
          Console.WriteLine("{0} {1} {2}", call[0], call[1], call[2]);
          playerBoard[Int32.Parse(call[1]), Int32.Parse(call[2])] = "M";
          printBoard(playerBoard);
        }
      }

    
  }

  private static void printBoard(string[,] board){ //overloaded method that prints a given boards matrix
    for(int r = 0; r<board.GetLength(0); r++){ //get the row length
        for (int c = 0; c<board.GetLength(1); c++) // get the col length
          Console.Write(board[r,c] + " "); //and print the matrix
        Console.WriteLine();
      }
  }

  private static string[,] RecurseBoard(string[,] finalBoard, string[,] playerBoard, int row, int col){
    if(finalBoard[row,col].Equals("0") && playerBoard[row,col].Equals("-")){ //if the revealed slot is 0 and player DNK this
    playerBoard[row,col] = "0";
    for(int r = -1; r < 2; r++)
      for(int c = -1; c<2; c++) //check all values around that 0
        try{ 
          if (finalBoard[(row+r),(col+c)].Equals("0") && playerBoard[(row+r),(col+c)].Equals("-")) //if slot is 0 and player DNK
          playerBoard = RecurseBoard(finalBoard, playerBoard, (row+r), (col+c));
          else
            playerBoard[(row+r),(col+c)] = finalBoard[(row+r),(col+c)];  
                 }catch(Exception p){} //end of lr lc call
    }else //if the slot is either known or not a 0
      playerBoard[row,col] = finalBoard[row,col];  
    return playerBoard;
  }
        

    public static void Main(string[] args){
      // This redirects the program to read standard in (i.e. the Console, 
      // which is equivalent to System.in in Java) from the given file.
      // This is one of a *very few* places where it's actually OK to catch
      // and ignore an exception
      try {
        Console.SetIn(new StreamReader("playgame1.txt"));
        //Console.SetIn(new StreamReader("minegame1.txt"));
      } catch (Exception e) {
      // ignore!
      // We will just read from standard in if the file cannot be found and read
      }

      string inputType = Console.ReadLine();
      if (inputType.Equals("print_board")) {
        PrintBoard();
      } else if (inputType.Equals("play_game")){
        PlayGame();
      } else {
        throw new System.InvalidOperationException($"{inputType} is not a valid Minesweeper input file type");
      }
      
  }
}