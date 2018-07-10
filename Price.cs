using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace TestExercise {

    public class Price {

        public float Amount { get; }
        public string CurrencyCode { get; }

        public Price(float amount, string currencyCode) {
            this.Amount = amount;
            this.CurrencyCode = currencyCode;
        }

        public string PriceAsString() {
            return this.Amount + this.CurrencyCode;
        }
    }
}
