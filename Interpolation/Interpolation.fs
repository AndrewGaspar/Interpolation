namespace NumericalAnalysis

module Interpolation =
    let interpolate (dataPoints: seq<float * float>) =
        let data = dataPoints |> Seq.toArray

        let dividedDifferences: float option [] [] 
            =   Array.init data.Length 
                    (fun i -> Array.create (data.Length-i) None);

        let rec dividedDifference i k = 
            let j = k-i;
            match dividedDifferences.[i].[j] with
            | Some(coefficient) -> coefficient
            | None ->
                dividedDifferences.[i].[j] <- Some (
                    if i = k then snd data.[i];
                    else 
                        (((dividedDifference (i+1) k) - (dividedDifference i (k-1))) 
                            / (fst data.[k] - fst data.[i]));
                    )
                dividedDifferences.[i].[j].Value
                        

        let a n = dividedDifference 0 n
        
        let rec p n =
            if n = data.Length - 1 then (fun x -> a n)
            else (fun x -> (x - fst data.[n])*(p (n+1) x) + a n)

        p 0