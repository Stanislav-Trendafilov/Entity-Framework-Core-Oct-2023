using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Common
{
    public static class ValidationConstants
    {
        //team
        public const int TeamNameMaxLength = 50;
        public const int TeamMaxUrlLength = 2048;
        public const int TeamInitialsMaxLength = 4;

        //color
        public const int ColorNameMaxLength = 30;

        //town
        public const int TownNameMaxLength = 58;

        //country
        public const int CountryNameMaxLength = 56;

        //player
        public const int PlayerNameMaxLength = 100;
    }
}
