﻿namespace BitfinexAPI.Resources
{
    public class Alerts
    {
        public int Operative { get; set; }
    }

    public class Alert
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Symbol { get; set; }
        public double Price { get; set; }
        public int Unknown { get; set; }
    }
}