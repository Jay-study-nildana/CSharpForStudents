// See https://aka.ms/new-console-template for more information

//Step wise Builder calls

using StepwiseBuilder.POCOs;

//notice that the exact sequence of function calls is maintained
//TypeOfBoardGame can only be called after StartGame
//PlayerCount can only be called after TypeOfBoardGame


var boardgameletsplay = BoardGameBuilder.StartGame().TypeOfBoardGame(BoardGameType.WoodenMaterial).PlayerCount(5).Build();

Console.WriteLine(boardgameletsplay);
