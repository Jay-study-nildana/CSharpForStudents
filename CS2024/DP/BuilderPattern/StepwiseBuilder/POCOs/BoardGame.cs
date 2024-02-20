using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StepwiseBuilder.POCOs
{
    public class BoardGame
    {
        public BoardGameType BoardGameType;
        public int NumberOfPlayers;

        public override string ToString()
        {
            return $"{nameof(BoardGameType)}: {BoardGameType}, {nameof(NumberOfPlayers)}: {NumberOfPlayers}";
        }

    }


}
