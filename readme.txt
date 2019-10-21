The entire solution is in the final soln folder, otherwise just run the exe and see what you think.

Things I learned:

	-Polymorphism is so damn important. Duplicate/Similar code sucks, especially for the bounds logic of the shapes.
	 I really would like to refactor it before turning it in, but time is an issue.

	-Try/catches are expensive. I used them around my index checking from time to time, and noticed the 
	 slow down in my canvas redrawing.

	-I need to bring my shape class to a higher level of abstraction.

Left Control rotates the shape, Left and Right arrows move the shape, and enter pushes it to the bottom.
The logic around the losing isn't the best, I checked the highest block coordinates and whether the shape's bottomed out. If so, you lose.
The behavior is weird from time to time. 
 
The score reads / writes to hiscore.txt, adjacent to the .exe file.

I didn't implement a save game feature, but if I did, I'd save the 2d array that acts as the grid into a text file, and to load it, I'd just
read in the 2d array.
