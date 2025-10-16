// TripRecordMap.cs
using CsvHelper.Configuration;
using ParserService.Models; // Убедись, что пространство имён для TripRecord верное
using System.Globalization;

public sealed class TripRecordMap : ClassMap<TripRecord>
{
    public TripRecordMap()
    {
        var culture = CultureInfo.InvariantCulture;
        
        Map(m => m.PickupDatetime).Name("tpep_pickup_datetime");
        Map(m => m.DropoffDatetime).Name("tpep_dropoff_datetime");
        
        Map(m => m.PassengerCount).Name("passenger_count").Default(0);
        Map(m => m.TripDistance).Name("trip_distance").Default(0.0).TypeConverterOption.CultureInfo(culture);
        Map(m => m.PULocationID).Name("PULocationID").Default(0);
        Map(m => m.DOLocationID).Name("DOLocationID").Default(0);
        Map(m => m.FareAmount).Name("fare_amount").Default(0.0m).TypeConverterOption.CultureInfo(culture);
        Map(m => m.TipAmount).Name("tip_amount").Default(0.0m).TypeConverterOption.CultureInfo(culture);
        
        Map(m => m.StoreAndFwdFlag).Name("store_and_fwd_flag").Convert(args =>
        {
            var flag = args.Row.GetField("store_and_fwd_flag")?.Trim().ToUpper();
            return flag switch
            {
                "Y" => "Yes",
                "N" => "No",
                _ => "Unknown" 
            };
        });
    }
}