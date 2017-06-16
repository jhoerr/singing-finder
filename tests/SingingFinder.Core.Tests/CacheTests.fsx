#r "../../packages/FSharp.Data.2.3.3/lib/net40/FSharp.Data.dll"

open System
open FSharp.Data.Runtime.Caching

let shortCache = createInMemoryCache (TimeSpan.FromSeconds(1.0))

shortCache.Set ("key",1)

shortCache.TryRetrieve "key"

