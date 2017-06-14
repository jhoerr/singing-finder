namespace SingingFinder.Core

type Month =
    | January=1
    | February=2
    | March=3
    | April=4
    | May=5
    | June=6
    | July=7
    | August=8
    | September=9
    | October=10
    | November=11
    | December=12

type Singing = 
  { Month:Month; 
    Day:string; 
    Name:string; 
    Location: string; 
    Latitude: float; 
    Longitude: float; 
    Info: string; 
    LocationUrl: string;
    Book: Book }

type Days = 
  { Start: System.DateTime; 
    End: System.DateTime }

type Event =
  { Singing: Singing;
    Days: Days }

type SingingDays = 
    | Sunday
    | Saturday
    | SaturdayAndSunday
    | FridayAndSaturday

