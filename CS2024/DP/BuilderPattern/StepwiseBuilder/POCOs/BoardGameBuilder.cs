using StepwiseBuilder.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepwiseBuilder.POCOs
{
    public class BoardGameBuilder
    {
        public static IChooseBoardGameType StartGame()
        {
            return new BoardGameImplementation();
        }

        private class BoardGameImplementation : IChooseBoardGameType, IChooseNumberOfPlayers, IStartBoardGame
        {
            private BoardGame BoardGame = new BoardGame();
            public BoardGame Build()
            {
                return BoardGame;
            }

            public IStartBoardGame PlayerCount(int playerCount)
            {
                BoardGame.NumberOfPlayers = playerCount;
                return this;
            }

            public IChooseNumberOfPlayers TypeOfBoardGame(BoardGameType boardGameType)
            {
                BoardGame.BoardGameType = boardGameType;
                return this;
            }
        }
    }
}
