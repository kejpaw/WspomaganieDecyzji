using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using TransportApp.Models;


namespace TransportApp.Helpers
{
    public class NavigationLogger
    {

        public static string BuildNavigationMessage(List<Orders> orders)
        {

            var result = new StringBuilder();

            try
            {
                var startOrder = orders.Where(x => x.Request).Single();
                var waypoints = orders.Where(x => x.IsInProgress);

            }
            catch (Exception ex)
            {
                return "Brak poprawnie zapisanej trasy";
            }
            
            var stringResult = result.ToString();
            return stringResult == string.Empty ? "Brak wskazówek dotarcia" : stringResult;
        }


        private static string Go(string from, string to, bool onFoot)
        {

            return "";
        }


    }
}
