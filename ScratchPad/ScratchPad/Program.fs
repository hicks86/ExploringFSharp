open System
open Aether
open Aether.Operators

type AddressLine =
    { 
        Line : string
    }
    static member Line_ =
        (fun l -> l.Line), (fun value line -> {line with Line = value})

type PersonDetails =
    { 
        Name: string; 
        Address: AddressLine;
        DoB: string
    }
    override this.ToString() = sprintf "Hello %s (DoB %s) who lives in %s" this.Name this.DoB this.Address.Line
    static member Address_ = 
        (fun p -> p.Address), (fun add p -> {p with Address = add})


let addressValue_ =
    //Compose.lens PersonDetails.Address_ AddressLine.Line_
    PersonDetails.Address_ >-> AddressLine.Line_

let GetPersonDetails : PersonDetails =
    printfn "Hello, what is your name?"
    let name = Console.ReadLine()
    //printfn "Hi %s" name
    printfn "Whats your location?"
    let address = Console.ReadLine();
    //printfn "Ah %s lives in %s " name address
    printfn "Whats your DOB?"
    let dob = Console.ReadLine()
    //printfn "Thanks your DOB is %s" dob

    { Name = name; Address = { Line = address}; DoB = dob }



[<EntryPoint>]
let main argv =
    let details = GetPersonDetails

    details.ToString()
    |> printfn "%s"

    let opticValue = Optic.get addressValue_ details //was d.Address.Line
    printfn "So your current location is %s" opticValue

    printfn "Whats your new location?"
    let newAddress = Console.ReadLine()
    
    let d =  Optic.set addressValue_ newAddress details //was { details with Address = { Line = newAddress }}
    let o = Optic.get addressValue_ d
    printfn "Original details %A" details

    printfn "Success! Your new location is %s" o //was d.Address.Line
    0 // return an integer exit code


