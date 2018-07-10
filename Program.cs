using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;

namespace TestExercise {

    class Program {

        public static Flight GetCheapestFlight(List<Flight> allFlights) {
            Flight cheapestFlight = allFlights[0];
            foreach (Flight tmpFlight in allFlights) {
                if (!cheapestFlight.IsCheaperThan(tmpFlight)) {
                    cheapestFlight = tmpFlight;
                }
            }
            return cheapestFlight;
        }

        public static string ReturnFlightInfoAsString(Flight flight) {
            return "Departure Date: " + flight.DepartureDate + "\tPrice: " + flight.FlightPrice.Amount + flight.FlightPrice.CurrencyCode;
        }
                 
        static void Main(string[] args) {

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            List<Flight> flightsOfDay = PostRequest.GetFlightsForDate("VNO", "BVA", new DateTime(2018, 08, 03));
            // to use Mac OS User Agent:
            // List<Flight> flightsOfDay = PostRequest.GetFlightsForDate("VNO", "BVA", new DateTime(2018, 08, 03), true);

            Flight cheapestDayFlight = GetCheapestFlight(flightsOfDay);
            Console.WriteLine(ReturnFlightInfoAsString(cheapestDayFlight));

            List<Flight> flightsOfMonth = PostRequest.GetFlightsForDates("VNO", "BVA", new DateTime(2018, 08, 01), new DateTime(2018, 10, 30));
            // to use Mac OS User Agent:
            // List<Flight> flightsOfMonth = PostRequest.GetFlightsForDate("VNO", "BVA", new DateTime(2018, 08, 01), new DateTime(2018, 08, 31), true);

            Flight cheapestMonthFlight = GetCheapestFlight(flightsOfMonth);
            Console.WriteLine(ReturnFlightInfoAsString(cheapestMonthFlight));

            Console.ReadKey();
        }

    }
}
