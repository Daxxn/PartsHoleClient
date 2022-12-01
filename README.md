# Parts Hole

Where parts go to **stop** dissapearing!

note: this is the client. See the [API Here](https://github.com/Daxxn/PartsHoleAPI)

## The Problem

Thats the hardest problem im dealing with when designing and building PCBs. It can be almost impossible to remember what components you bought already and what package its in. (As in shape and size of the IC, pin count.) Alot of chips come in multiple packages with wildly different sizes and shapes. Ive tried to solve this problem a few way, Spreadsheets can work but its also very hands-on. Ive used DigiKeys order history which works fairly well. Theres a half way decent search feature and links to the components datasheet. The big problem is how slow and buggy thier site is. It will sometimes take multiple tries to open an order. Obviously parts ordered from anywhere else, like Mouser, wont be there.

## My Solution

A complete Inventory Management System with different ways to track all the components used in a project and where to store them. With invoice parsing for components from [DigiKey](https://www.digikey.com/MyDigiKey) and [Mouser](https://www.mouser.com/MyMouser/AccountSummary.aspx). A different way to keep track of passives like Resistors and Capacitors, called Books. Most of the passive components I get are literraly in books, so why not keep them together.

This is specifically designed for electronic component management. Complex components and sub-components are not planned.

## Features

- Full Inventory management, including Quantities, custom part numbers, and location tracking
- Digikey and Mouser invoice parsing
- Project BOM and versioning Management
- KiCAD BOM parser
- Custom Part Number creation tools
- Passive component (Resistors, Capacitors, Inductors) Management page with standardized value tools
