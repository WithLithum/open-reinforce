# Developer guide

This document is a guide to how to work with Open Reinforce project in principle.

## Project structure

This solution is consisted of three projects:

- `FrReader`: A command line utility for prototyping LSPDFR XML file reader logic.
- `OpenReinforce`: The main LSPDFR plugin.
- `OpenReinforce.TestPlugin`: A RPH plugin for rapid prototyping and iteration.

All of the above projects consists of minimal code on their own to get themselves to work.
`FrReader` additional have all the CLI UI code.

## Building

To build the `OpenReinforce` LSPDFR plugin, you will need the LSPD First Response mod plugin file.

This file cannot be redistributed and is _not_ available on NuGet.

If you don't have the mod installed, you can get the mod archive from
[its homepage](https://www.lcpdfr.com/files/file/7792-lspd-first-response) on `LCPDFR.com`. Find
the plugin file within the installation archive.

If you have it already installed, the plugin file can be found under the `plugins`, named
`LSPD First Response.dll`.

Once you have the plugin file, copy it into the `References` directory that exists beside the
solution file.