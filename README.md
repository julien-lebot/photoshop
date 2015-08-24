# photoshop
A very simple paint program.

This project started as a coding dojo with some colleagues on a Thursday afternoon at 4pm.
The goal was to explore the use of the command pattern with undo / redo support.

After hitting a roadbloack trying to implement a simple image rotation, we postponed to the next week.
I, however, couldn't sleep. So I worked on it until the next day at 7:45am.

The resulting project is not pretty, but it works quite well. It supports:
 - creation of images of various sizes
 - drawing with the mouse
 - inversion of colours
 - filling a region with a colour
 - undo / redo for *all* the commands
 - saving to PNG
 - pan and zoom
 - very slow updates on large images
 
Todo: rotation of the picture (!). It's in there in the Unit Test though.
