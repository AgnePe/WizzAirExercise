using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace TestExercise {

    public class Flight {

        public DateTime DepartureDate { get; }
        public Price FlightPrice { get; }

        public Flight (DateTime departureDate, Price price) {
            this.DepartureDate = departureDate;
            this.FlightPrice = price;
        }

        // compare two flights by price
        public bool IsCheaperThan (Flight flight) {
            return (this.FlightPrice.Amount < flight.FlightPrice.Amount);
        }

    }
}
