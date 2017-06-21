namespace SingingFinder.Core 

open System

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
                                            
                                            