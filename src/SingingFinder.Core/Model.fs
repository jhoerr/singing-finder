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
    | ``All``                   =0
    | ``Denson``                =1
    | ``Cooper``                =2
    | ``Shenandoah Harmony``    =4
    | ``Georgian Harmony``      =8
    | ``Christian Harmony``     =16
    | ``Missouri Harmony``      =32
    | ``Southern Harmony``      =64
    | ``Eclectic Harmony``      =128
    | ``JL White``              =256
    | ``New Compositions``      =512
    | ``Colored Sacred Harp``   =1024
    | ``Social Harp``           =2048
    | ``American Vocalist``     =4096
    | ``Harmonia Sacra``        =8192


type Singing = 
  { Month:Month; 
    Day:string; 
    Name:string; 
    Location: string; 
    Latitude: float; 
    Longitude: float; 
    Info: string;
    SingingUrl: string;
    LocationUrl: string;
    Book: Book }

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