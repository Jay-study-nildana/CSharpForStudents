using EFSQLiteDemo.Enums;
using EFSQLiteDemo.Interfaces;
using EFSQLiteDemo.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFSQLiteDemo.POCO
{

    public class GetKiteOne : IGetKite
    {
        //TODO - update this to use randomly generated values for color, height, weight and width
        public KiteUpdateOctober4th GiveMeAKite()
        {
            var response = new KiteUpdateOctober4th();
            response.KiteColor = ColorEnum.Blue;
            response.KiteDesigner = "Some Guy";
            response.KiteHeight = 20;
            response.KiteWeight = 59.99;
            response.KiteWidth = 30;

            return response;
        }
    }
}
