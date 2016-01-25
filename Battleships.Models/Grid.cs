using System;
using System.Collections.Generic;
using Battleships.Models.Interfaces;

namespace Battleships.Models
{
    public class Grid : IGrid
    {
        private readonly IConfigurationManager _configurationManager;

        public Grid(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;

            CoordinatesShotAt = new List<Coordinate>();

            ConfigureGridSettings();
        }

        public int MaxXAxis { get; set; }

        public int MaxYAxis { get; set; }

        public List<Ship> Ships { get; set; }

        public List<Coordinate> CoordinatesShotAt { get; set; }

        private void ConfigureGridSettings()
        {
            Ships = new List<Ship>();

            MaxXAxis = Convert.ToInt32(_configurationManager.AppSettings["MaxXAxis"]);
            MaxYAxis = Convert.ToInt32(_configurationManager.AppSettings["MaxYAxis"]);

            for (var i = 0; i < Convert.ToInt32(_configurationManager.AppSettings["TotalDestroyers"]); i++)
            {
                Ships.Add(new Destroyer());
            }

            for (var i = 0; i < Convert.ToInt32(_configurationManager.AppSettings["TotalBattleships"]); i++)
            {
                Ships.Add(new Battleship());
            }
        }

        public bool IsValidCoordinate(string input)
        {
            var maxYAxis = Convert.ToInt32(_configurationManager.AppSettings["MaxYAxis"]);
            var maxXAxis = Convert.ToInt32(_configurationManager.AppSettings["MaxXAxis"]);

            var validXCoordinates = BuildValidXAxisValues(maxXAxis);

            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            if (input.Length < 2)
            {
                return false;
            }

            var xAxis = input.Substring(0, 1);

            if (!validXCoordinates.Contains(xAxis.ToLower()))
            {
                return false;
            }

            int yAxis;

            if (!int.TryParse(input.Substring(1), out yAxis) || yAxis <= 0 || yAxis > maxYAxis)
            {
                return false;
            }

            return true;
        }

        private List<string> BuildValidXAxisValues(int maxXAxis)
        {
            var validXAxisValues = new List<string>();
        
            for (var c = 'a'; c <= 'z'; c++)
            {
                if (maxXAxis > 0)
                {
                    validXAxisValues.Add(c.ToString());

                    maxXAxis--;
                }
            } 

            return validXAxisValues;
        }
    }
}
