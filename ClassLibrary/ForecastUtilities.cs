using System;


namespace ClassLibrary
{
    public class ForecastUtilities
    {
        public class Weather
        {
            private double degreesCelsious;
            private double humidity;
            private double windSpeed;

            public Weather()
            {
                this.degreesCelsious = 0;
                this.windSpeed = 0;
                this.humidity = 0;
            }
            public Weather(double DegreesCelsious, double Humility, double WindSpeed)
            {
                this.degreesCelsious = DegreesCelsious;
                this.windSpeed = WindSpeed;
                this.humidity = Humility;
            }

            public double GetTemperature()
            {
                return degreesCelsious;
            }

            public void SetTemperature(double degreesCelsious)
            {
                this.degreesCelsious = degreesCelsious;
            }

            public double GetHumidity()
            {
                return humidity;
            }

            public void SetHumidity(double humidity)
            {
                this.humidity = humidity;
            }

            public double GetWindSpeed()
            {
                return windSpeed;
            }

            public void SetWindSpeed(double windSpeed)
            {
                this.windSpeed = windSpeed;
            }

            public double CalculateFeelsLikeTemperature()
            {
                double c1 = -8.78469475556;
                double c2 = 1.61139411;
                double c3 = 2.33854883889;
                double c4 = -0.14611605;
                double c5 = -0.012308094;
                double c6 = -0.0164248277778;
                double c7 = 0.002211732;
                double c8 = 0.00072546;
                double c9 = -0.000003582;
                double HI;

                HI = c1 + c2 * degreesCelsious + c3 * humidity + c4 * degreesCelsious * humidity
                    + c5 * Math.Pow(degreesCelsious, 2) + c6 * Math.Pow(humidity, 2) + c7 * Math.Pow(degreesCelsious, 2) * humidity
                    + c8 * degreesCelsious * Math.Pow(humidity, 2) + c9 * Math.Pow(degreesCelsious, 2) * Math.Pow(humidity, 2);

                return HI;
            }


            public double CalculateWindChill()
            {
                if (windSpeed > 4.8 && degreesCelsious <= 10)
                {

                    return (13.12 + 0.6215 * degreesCelsious - 11.37 * Math.Pow(windSpeed, 0.16) + 0.3965 * (degreesCelsious) * (Math.Pow(windSpeed, 0.16)));

                }
                else
                {
                    return 0;
                }


            }

            public string GetAsString()
            {
                return $"T={this.degreesCelsious}°C, w={this.windSpeed}km/h, h={this.humidity}%";
            }

        }
        public static Weather FindWeatherWithLargestWindchill(Weather[] weathers)
        {
            Weather max = new Weather(0, 0, 0);
            for (int i = 0; i < weathers.Length; i++)
            {
                if (weathers[i].CalculateWindChill() > max.CalculateWindChill())
                {
                    max = weathers[i];
                }

            }
            return max;
        }

        public class DailyForecast
        {
            private DateTime date;
            private Weather weather;

            public DailyForecast(DateTime Date, Weather w)
            {
                this.date = Date;
                this.weather = w;
            }
            public Weather GetWeather()
            {
                return weather;
            }

            public string GetAsString()
            {
                return $"{this.date} {this.weather.GetAsString()}";
            }

            public static bool operator <(DailyForecast d1, DailyForecast d2)
            {
                if (d1.GetWeather().GetTemperature() < d2.GetWeather().GetTemperature())
                {
                    return true;
                }
                return false;
            }
            public static bool operator >(DailyForecast d1, DailyForecast d2)
            {
                if (d1.GetWeather().GetTemperature() > d2.GetWeather().GetTemperature())
                {
                    return true;
                }
                return false;

            }

        }

        public static DailyForecast Parse(string s)
        {
            string[] part = s.Split(",");
            DateTime date = DateTime.ParseExact(part[0], "dd/MM/yyyy HH:mm:ss", null);

            Weather weather = new Weather();
            weather.SetTemperature(double.Parse(part[1]));
            weather.SetWindSpeed(double.Parse(part[2]));
            weather.SetHumidity(double.Parse(part[3]));

            return new DailyForecast(date, weather);

        }

        public class WeeklyForecast
        {
            private DailyForecast[] dailyForecasts;

            public WeeklyForecast(DailyForecast[] DForecasts)
            {
                this.dailyForecasts = DForecasts;
            }

            public DailyForecast this[int index]
            {
                get { return this.dailyForecasts[index]; }
            }


            public string GetAsString()
            {
                string a = "";
                for (int i = 0; i < dailyForecasts.Length; i++)
                {
                    a += this.dailyForecasts[i].GetAsString();
                    a += "\n";
                }
                return $"{a}";

            }

            public double GetMaxTemperature()
            {
                DailyForecast max = dailyForecasts[0];
                for (int i = 0; i < dailyForecasts.Length; i++)
                {
                    if (max < dailyForecasts[i])
                    {
                        max = dailyForecasts[i];
                    }

                }
                return max.GetWeather().GetTemperature();
            }


        }





    }
}
