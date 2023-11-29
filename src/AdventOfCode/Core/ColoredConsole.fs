module ColoredConsole

type Color = { R: int; G: int; B: int }

let printColoredText text color =
    let textColorCode = sprintf "\x1b[38;2;%d;%d;%dm" color.R color.G color.B
    textColorCode + text + "\x1b[0m" |> printf "%s"

let redColor = { R = 255; G = 0; B = 0 }
let greenColor = { R = 0; G = 255; B = 0 }
let yellowColor = { R = 255; G = 255; B = 0 }
let darkGrayColor = { R = 169; G = 169; B = 169 }

let getColorForElapsedTime =
    function
    | n when n < 25L -> greenColor
    | n when n < 50L -> yellowColor
    | _ -> redColor
