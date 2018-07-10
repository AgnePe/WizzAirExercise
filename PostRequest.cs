using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace TestExercise {

    public class PostRequest {

        // returns all flights from->to for specific date
        // use default user agent if not given otherwise
        public static List<Flight> GetFlightsForDate(string from, string to, DateTime flightDate, bool useMACOSUserAgent = false) {
            return GetFlightsForDates(from, to, flightDate, flightDate, useMACOSUserAgent);
        }

        // returns all flights from->to for date range fromDate-toDate
        // use default user agent if not given otherwise
        // Will thow exception bad request is sent. Possible errors:
        // - invalid departure/arrival station (entered code does not exist)
        // - invalid time/date range (exceeds 43 days (API response))
        public static List<Flight> GetFlightsForDates(string from, string to, DateTime fromDate, DateTime toDate, bool useMACOSUserAgent = false) {

            dynamic JSONResponse = null;

            List<Flight> allFlights = new List<Flight>(); 

            string wizzAirUrl = "https://be.wizzair.com/8.1.1/Api/search/timetable";

            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(wizzAirUrl);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            if (useMACOSUserAgent) {
                // user agent list found on https://developers.whatismybrowser.com/useragents/explore/operating_system_name/mac-os-x/
                httpWebRequest.UserAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_11_6) AppleWebKit/601.7.7 (KHTML, like Gecko) Version/9.1.2 Safari/601.7.7";
            }
            httpWebRequest.Method = "POST";

            // create a stream of information to POST to server
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream())) {

                string fromDateString = fromDate.ToString("yyyy-MM-dd");
                string toDateString = toDate.ToString("yyyy-MM-dd");

                // formating data for upload
                string payload = "{\"flightList\":[{" +
                                        "\"departureStation\":\"" + from + "\"," +
                                        "\"arrivalStation\":\"" + to + "\"," +
                                        "\"from\":\"" + fromDateString + "\"," +
                                        "\"to\":\"" + toDateString + "\"}]," +
                                  "\"priceType\":\"regular\"," +
                                  "\"adultCount\":1," +
                                  "\"childCount\":0," +
                                  "\"infantCount\":0}";
                streamWriter.Write(payload);
                streamWriter.Flush();
                streamWriter.Close();
            }

            // POST data
            using (var response = httpWebRequest.GetResponse() as HttpWebResponse) {
                if (httpWebRequest.HaveResponse && response != null) {
                    using (var reader = new StreamReader(response.GetResponseStream())) {
                        JSONResponse = JObject.Parse(reader.ReadToEnd());
                    }
                }
            }

            // return list of flights from the response
            for (int f = 0; f < JSONResponse.outboundFlights.Count; f++) {
                DateTime departureDate = JSONResponse.outboundFlights[f].departureDate;
                float p = JSONResponse.outboundFlights[f].price.amount;
                string c = JSONResponse.outboundFlights[f].price.currencyCode;
                Price flightPrice = new Price(p, c);
                Flight flight = new Flight(departureDate, flightPrice);
                allFlights.Add(flight);
            }

            return allFlights;
        }

    }
}