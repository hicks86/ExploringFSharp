module FSharpLoopSumFunction
    let RunLoop (range: int list) =
        range |> List.fold(fun (accSum, accIndex) item ->
                accSum + accIndex + item, accIndex + 1) (0, 0)


            

