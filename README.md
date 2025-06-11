# Book12

Book12 is a small WinForms prototype that demonstrates procedural map generation using simplex noise. The solution contains two projects:

- **Book12** – the Windows Forms application responsible for the UI and rendering logic.
- **Engine** – a utility library providing random number helpers and the simplex noise implementation used for terrain creation.

`MapRenderer` generates a set of bitmap layers (base map, land mask, city map) and stores them in a static dictionary so they can be displayed in `MainScreen`. The rendering routine now employs `Bitmap.LockBits` for faster pixel writes compared to the original `SetPixel` loops.

Build the solution with `dotnet build` or open `Book12.sln` in Visual Studio. Running the program opens `MainScreen` where you can view the generated map layers.
