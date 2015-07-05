# Space-Station-X1 [![Build Status](https://travis-ci.org/AlmostPete/Space-Station-X1.svg?branch=master)](https://travis-ci.org/AlmostPete/Space-Station-X1)
Faithful remake of the cult classic Space Station 13, running on a new framework. The goal of this project is to update, instead of completely recreate the game, like so many other remakes are trying to do. The game is popular as is, it only needs some improvements to performance, interface, and compatibility.

This project is written in C# on the .NET framework, using the Monogame graphics library. Thanks to OpenGL and the Mono framework, this game will be fully cross platform, with Windows and Linux under primary development, and MacOS expected soon.

### Command Line Arguments
The following is a list of command line arguments that affect the core library, and application. Custom command line arguments will not break the application, and custom content packs can process command line arguments as well when they start up. Optional arguments are denoted with `<>`.

* **Core**
  * `-nf`, `--no-file`: Keeps the logger from outputting to a file.
  * `-nt`, `--no-thread`: Stops the logger from using a separate thread.
