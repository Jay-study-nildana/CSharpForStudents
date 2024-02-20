using StepwiseBuilder.POCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepwiseBuilder.Interfaces
{
    public interface IChooseBoardGameType
    {
        public IChooseNumberOfPlayers TypeOfBoardGame(BoardGameType boardGameType);
    }
}
