namespace SingingFinder.Core

open System

type Month =
    | All=0
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

[<System.FlagsAttribute>]
type Book =
    | ``All``                   =0b000000000000000000000000
    | ``1991 Edition``          =0b000000000000000000000001
    | ``Cooper Edition``        =0b000000000000000000000010
    | ``Shenandoah Harmony``    =0b000000000000000000000100
    | ``Georgian Harmony``      =0b000000000000000000001000
    | ``Christian Harmony``     =0b000000000000000000010000
    | ``Missouri Harmony``      =0b000000000000000000100000
    | ``Southern Harmony``      =0b000000000000000001000000
    | ``Eclectic Harmony``      =0b000000000000000010000000
    | ``JL White Edition``      =0b000000000000000100000000
    | ``New Compositions``      =0b000000000000001000000000
    | ``Colored Sacred Harp``   =0b000000000000010000000000
    | ``Social Harp``           =0b000000000000100000000000
    | ``American Vocalist``     =0b000000000001000000000000
    | ``Harmonia Sacra``        =0b000000000010000000000000
    | ``New Harp of Columbia``  =0b000000000100000000000000
    // 
    | ``Four-Shape``            =0b000000000001111111101111
    | ``Seven-Shape``           =0b000000000110000000010000

type SingingType =
    | All=0
    | Annual=1
    | Regular=2

type Location =
  { Name:string;
    Address:string;
    City:string;
    StateProvince:string;
    Country:string; 
    Latitude: float;
    Longitude: float;
    MapsUrl: string; }

type Singing = 
  { Month:Month; 
    Day:string; 
    Name:string; 
    Location: Location; 
    Info: string;
    SingingUrl: string;
    Book: Book;
    Type: SingingType }

type Days = 
  { Start: System.DateTime; 
    End: System.DateTime }

type Event =
  { Singing: Singing;
    Days: Days list }

type Cardinality = First | Second | Third | Fourth | Fifth | Last

type SingingDay =
    // 25                           -> 25
    | DayOfMonth of int                    
    // Every Monday                 -> Monday
    | Every of DayOfWeek
    // 1st Monday                   -> [First] * Monday * [Monday]
    // 2nd and 4th Tuesday          -> [Second, Fourth] * Tuesday * [Tuesday]
    | Regular of Cardinality list * DayOfWeek
    // Fri and Sat before 2nd Sun   -> Second * Sunday * [Friday, Saturday]
    // 2nd Sun and Sat Before       -> Second * Sunday * [Saturday, Sunday]
    // Sat before 2nd Tuesday       -> Second * Tuesday * [Saturday]
    | OnOrBefore of Cardinality list * DayOfWeek * DayOfWeek list                       