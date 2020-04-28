# Webcam Settings Tool

This is a little utility for forcing webcam settings. It's a command-line tool that reads in settings via XML, an adaption from a lifecam utility by marcelray. 

In Windows 10 there are no drivers for Windows LifeCam, so this fork has been changed to support web cams generically. 

I have personally tested it with the following cameras:

- Microsoft LifeCam Studio 1080p HD

TODO:

- Add command line options, making the xml optional and specified via the command line
- Convert to a tooltray utility


A couple notes:

- This has been forked from a project by Marcel Ray (github.com/marcelray)
- The project I forked targeted .NET 3.5. I've not changed this as it works as is on my system. This most prob. should be retargeted to a later version of .NET
- Note I am not a .NET developer, so it will be rough around the edges. Feel free to do with this source what you will.

