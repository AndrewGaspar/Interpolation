#load "Interpolation.fs"
open NumericalAnalysis.Interpolation

let part2Data = 
    [   
        [0.0;   0.0;    75.0]; 
        [3.0;   225.0;  77.0]; 
        [5.0;   383.0;  80.0];
        [8.0;   623.0;  74.0];
        [13.0;  993.0;  72.0]
    ]   
    |> Seq.map List.toSeq;
    
part2Data    
|> hermiteCoefficients
|> Seq.iteri (printfn "a%d = %f") 

part2Data
|> hermiteEquation 
|> printfn "%s"

let func =
    part2Data
    |> hermite

func 10.0 |> printfn "%f"

let part2CData = 
    [   
        (0.0,   75.0); 
        (3.0,   77.0); 
        (5.0,   80.0);
        (8.0,   74.0);
        (13.0,  72.0)
    ]   
    |> Seq.map (fun (x,v) -> (x,v*3600.0/5280.0));

let v = part2CData |> interpolate